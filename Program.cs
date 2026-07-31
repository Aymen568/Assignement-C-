using Assignement;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddGrpc();
builder.Services.AddSignalR();
builder.Services.AddSingleton<IMachineRepository, InMemoryMachineRepository>();
builder.Services.AddSingleton<IMachineEventPublisher, MachineEventPublisher>();
builder.Services.AddScoped<IMachineService, MachineService>();
builder.Services.AddScoped<IHeartbeatProcessor, HeartbeatProcessor>();

var app = builder.Build();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.MapGrpcService<HeartbeatGrpcService>();
app.MapHub<DashboardHub>("/hubs/dashboard");

app.Run();
