using Microsoft.EntityFrameworkCore;
using Thiqah.Orders.DataAccess.Orders;
using Thiqah.Orders.EntityFramework;
using Thiqah.Shared.Context;
using Thiqah.Shared.Messaging;
using Thiqah.Shared.Messaging.RabbitMQMessaging;
using Thiqah.Shared.MiddleWare;
using Thiqah.Shared.Validation;
using MediatR;
using Thiqah.Orders.Infrastructure.Messages;
using Thiqah.Orders.Infrastructure.Request;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//Register Helpers
builder.Services.AddScoped<ActiveContext>();
builder.Services.AddSingleton<IMessenger, RabbitMQMessenger>();
builder.Services.AddSingleton<FluentValidator>();
builder.Services.AddDbContext<OrdersDbContext>(a => a.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnectionString")));
builder.Services.AddMediatR(typeof(Program));
builder.Services.AddScoped<UserCreateOrderEventPublisher>();
builder.Services.AddScoped<RequestUserEventOperator>();

//RegisterRepos
builder.Services.AddScoped<IOrderRepository, OrdersDbContext>();
builder.Services.AddScoped<IOrderQueryRepository, OrdersDbContext>();


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
