using Microsoft.EntityFrameworkCore;
using Thiqah.Shared.Context;
using Thiqah.Shared.Messaging;
using Thiqah.Shared.Messaging.RabbitMQMessaging;
using Thiqah.Shared.MiddleWare;
using Thiqah.Shared.Validation;
using Thiqah.Users.Application.Users;
using Thiqah.Users.DataAccess.Users;
using Thiqah.Users.EntityFramework;
using Thiqah.Users.Infrastructure.Messages;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//Register Helpers
builder.Services.AddScoped<ActiveContext>();
builder.Services.AddSingleton<IMessenger, RabbitMQMessenger>();
builder.Services.AddSingleton<FluentValidator>();
builder.Services.AddDbContext<UsersDbContext>(a => a.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnectionString")));


//RegisterRepos
builder.Services.AddScoped<IUserQueryRepository, UsersDbContext>();
builder.Services.AddScoped<IUserRepository, UsersDbContext>();

//RegisterServices
builder.Services.AddScoped<IUsersQuery, UsersQuery>();
builder.Services.AddScoped<IUsersCommand, UsersCommand>();

//Add event handlers
builder.Services.AddHostedService<UserCreatedOrderEventHandler>();
builder.Services.AddHostedService<GetUserEventHandler>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseMiddleware<ActiveContextMiddleware>();
app.UseMiddleware<ExceptionHandlerMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
