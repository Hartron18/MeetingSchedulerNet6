using MeetingSchedulerNet6;
using MeetingSchedulerNet6.Models;
using MeetingSchedulerNet6.Repository;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<MeetingContext>(options =>
{
    options.UseInMemoryDatabase("MeetingDb");
});
builder.Services.AddScoped(typeof(IMeetingService<Meeting>), typeof(MeetingService));
//builder.Services.AddScoped<typeof(IMeetingService<Meeting>),typeof(MeetingService)>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
