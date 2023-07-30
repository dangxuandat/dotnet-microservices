using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Platform_Service.AsyncDataService;
using Platform_Service.SyncDataServices.Http;
using PlatformService.Data;
using PlatformService.Dtos;
using PlatformService.Models;

namespace Platform_Service.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PlatformsController : ControllerBase
{
    private readonly IPlatformRepo _repository;
    private readonly IMapper _mapper;
    private readonly ICommandDataClient _commandDataClient;
    private readonly IMessageBusClient _messageBusClient;

    public PlatformsController(IPlatformRepo repository, IMapper mapper, ICommandDataClient commandDataClient, IMessageBusClient messageBusClient)
    {
        _repository = repository;
        _mapper = mapper;
        _commandDataClient = commandDataClient;
        _messageBusClient = messageBusClient;
    }

    [HttpGet]
    public ActionResult<IEnumerable<PlatformReadDto>> GetPlatforms()
    {
        var platformItem = _repository.GetAllPlatforms();

        return Ok(_mapper.Map<IEnumerable<PlatformReadDto>>(platformItem));
    }

    [HttpGet("{id}", Name = "GetPlatformById")]
    public IActionResult GetPlatformById(int id)
    {
        var platform = _repository.GetPlatformById(id);
        if (platform is null)
        {
            return NotFound();
        }
        return Ok(_mapper.Map<PlatformReadDto>(platform));
    }

    [HttpPost]
    public async Task<ActionResult<PlatformReadDto>> CreatePlatform(PlatformCreateDto platformCreateDto)
    {
        var platFormModel = _mapper.Map<Platform>(platformCreateDto);
        _repository.CreatePlatform(platFormModel);
        _repository.SaveChanges();

        var platformReadDto = _mapper.Map<PlatformReadDto>(platFormModel);
        //Send Sync Message
        try
        {
            await _commandDataClient.SendPlatformToCommand(platformReadDto);
        }
        catch (Exception e)
        {
            Console.WriteLine($"--> Could not send synchronously {e.Message}");
            throw;
        }
        //Send Async Message
        try
        {
            var platformPublishedDto = _mapper.Map<PlatformPublishDto>(platformReadDto);
            platformPublishedDto.Event = "Platform_Published";
            _messageBusClient.PublishNewPlatform(platformPublishedDto);
        }
        catch (Exception e)
        {
            Console.WriteLine($"--> Could not send Asynchronously: {e.Message}");
            throw;
        }
        
        
        return CreatedAtRoute(nameof(GetPlatformById), new { Id = platformCreateDto }, platformCreateDto);
    }
}