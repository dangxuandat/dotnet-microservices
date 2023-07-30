using Microsoft.EntityFrameworkCore;
using Platform_Service;
using Platform_Service.AsyncDataService;
using Platform_Service.SyncDataServices.Grpc;
using Platform_Service.SyncDataServices.Http;
using PlatformService.Data;

var builder = WebApplication.CreateBuilder(args);

 //Add services to the container.
 if (builder.Environment.IsDevelopment()) 
 {
     Console.WriteLine("--> Using InMem Db");
     builder.Services.AddDbContext<AppDbContext>(options => options.UseInMemoryDatabase("InMem")); 
 }
 else
 {
    Console.WriteLine("--> Using MSSQL Db");
    builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("PlatformsCon")));
 }
builder.Services.AddScoped<IPlatformRepo, PlatformRepo>();
builder.Services.AddHttpClient<ICommandDataClient, HttpCommandDataClient>();
builder.Services.AddSingleton<IMessageBusClient, MessageBusClient>();
builder.Services.AddGrpc();
builder.Services.AddControllers();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

app.UseAuthorization();


app.MapGrpcService<GrpcPlatformService>();
app.MapGet("/protos/platforms.proto", async context =>
{
    await context.Response.WriteAsync(File.ReadAllText("Protos/platforms.proto"));
});
app.MapControllers();
PrepDatabase.PrepPopulation(app, !builder.Environment.IsDevelopment());
app.Run();