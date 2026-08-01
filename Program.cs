using Assignement;
using Assignement.Services.Events;
using Assignement.Services.Heartbeat;
using Assignement.Services.Machines;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddGrpc();
builder.Services.AddSignalR();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<IMachineRepository, InMemoryMachineRepository>();
builder.Services.AddSingleton<IMachineEventPublisher, MachineEventPublisher>();
builder.Services.AddScoped<IMachineService, MachineService>();
builder.Services.AddScoped<IHeartbeatProcessor, HeartbeatProcessor>();
builder.Services.AddHostedService<OfflineDetectionService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.MapGrpcService<HeartbeatGrpcService>();
app.MapHub<DashboardHub>("/hubs/dashboard");

app.Run();