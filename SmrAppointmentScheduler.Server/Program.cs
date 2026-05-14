using Microsoft.EntityFrameworkCore;
using SmrAppointmentScheduler.Server.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure EF Core
var connectionString = builder.Configuration.GetConnectionString("Default") ?? "Server=(localdb)\\mssqllocaldb;Database=SmrAppointmentSchedulerDb;Trusted_Connection=True;MultipleActiveResultSets=true";
builder.Services.AddDbContext<SmrAppointmentScheduler.Server.Data.AppDbContext>(options =>
    options.UseSqlServer(connectionString));

// Register services
builder.Services.AddScoped<SmrAppointmentScheduler.Server.Services.ISlotService, SmrAppointmentScheduler.Server.Services.SlotService>();
builder.Services.AddScoped<SmrAppointmentScheduler.Server.Services.IBookingService, SmrAppointmentScheduler.Server.Services.BookingService>();
builder.Services.AddScoped<SmrAppointmentScheduler.Server.Services.IMechanicService, SmrAppointmentScheduler.Server.Services.MechanicService>();

var app = builder.Build();

app.UseDefaultFiles();
app.MapStaticAssets();

// Ensure database is created and apply migrations on startup
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<SmrAppointmentScheduler.Server.Data.AppDbContext>();
    try
    {
        db.Database.Migrate();
    }
    catch
    {
        // If migrations are not available, fall back to ensure database creation
        db.Database.EnsureCreated();
    }

    // Seed initial data if empty
    SmrAppointmentScheduler.Server.Data.SeedData.Seed(db);
}
// Configure the HTTP request pipeline.

// Register exception handling middleware
app.UseMiddleware<SmrAppointmentScheduler.Server.Middleware.ExceptionHandlingMiddleware>();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapFallbackToFile("/index.html");

app.Run();
