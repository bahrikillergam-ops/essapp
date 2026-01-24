using EssTeamApi.Data;
using Microsoft.EntityFrameworkCore;
using Hangfire;
using Hangfire.PostgreSql;
using EssTeamApi.Services;

var builder = WebApplication.CreateBuilder(args);

// 1. Controller setup with JSON cycle handling
builder.Services.AddControllers()
    .AddJsonOptions(options => 
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
    });

// 2. CORS Policy for Frontend/Swagger access
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy => policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});

// 3. Database Connection
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// 4. Inject Services
builder.Services.AddScoped<EmailService>();
builder.Services.AddScoped<SmsService>();

// 5. Configure Hangfire (The Scheduling Engine)
builder.Services.AddHangfire(config => config
    .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
    .UseSimpleAssemblyNameTypeSerializer()
    .UseRecommendedSerializerSettings()
    .UsePostgreSqlStorage(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddHangfireServer(); 

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// 6. Middleware Pipeline
app.UseCors("AllowAll");
app.UseSwagger();
app.UseSwaggerUI();

// 7. Hangfire Dashboard (Access this at /hangfire)
app.UseHangfireDashboard(); 

// 8. Schedule the Daily SMS Reminder
using (var scope = app.Services.CreateScope())
{
    var recurringJobManager = scope.ServiceProvider.GetRequiredService<IRecurringJobManager>();
    var smsService = scope.ServiceProvider.GetRequiredService<SmsService>();

    // This schedules the job in the database
    recurringJobManager.AddOrUpdate(
        "SendDailyReminders", 
        () => smsService.SendScheduledReminders(), 
        Cron.Daily(9)
    );
}


app.MapControllers();
app.Run();