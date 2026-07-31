using Assignement;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddGrpc();
builder.Services.AddSignalR();
builder.Services.AddSingleton<IMachineRepository, InMemoryMachineRepository>();
builder.Services.AddSingleton<IMachineEventPublisher, MachineEventPublisher>();
builder.Services.AddScoped<IMachineService, MachineService>();
builder.Services.AddScoped<IHeartbeatProcessor, HeartbeatProcessor>();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.MapGrpcService<HeartbeatGrpcService>();
app.MapHub<DashboardHub>("/hubs/dashboard");

app.Run();
