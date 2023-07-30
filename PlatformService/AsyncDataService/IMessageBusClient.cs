using PlatformService.Dtos;

namespace Platform_Service.AsyncDataService;

public interface IMessageBusClient
{
    void PublishNewPlatform(PlatformPublishDto platformPublishedDto);
    
}