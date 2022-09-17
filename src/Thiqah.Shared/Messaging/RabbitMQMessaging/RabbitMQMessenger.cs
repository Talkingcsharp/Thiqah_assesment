using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Threading.Channels;
using System.Threading.Tasks;
using Thiqah.Shared.Messaging.Base;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Thiqah.Shared.Messaging.RabbitMQMessaging
{
    public sealed class RabbitMQMessenger: IMessenger, IDisposable
    {
        private readonly IConfiguration _configuration;
        private ConnectionFactory? _connectionFactory;
        private IConnection? _connection;
        private IModel? _channel;


        public RabbitMQMessenger(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public Task Publish<T>(BaseMessage<T> messageObject)
        {
            SetupConnection();

            if (_channel is null)
            {
                throw new NullReferenceException("No channels were optained");
            }

            if (!string.IsNullOrEmpty(messageObject.Header.Topic))
            {
                DeclareExchange(_channel, messageObject);
                var messagebody = GetMessageBody(messageObject);
                _channel.BasicPublish(messageObject.Header.Topic, "", body: messagebody);
                return Task.CompletedTask;
            }
            var props = _channel.CreateBasicProperties();
            props.ReplyTo = messageObject.Header.ReplyToAddress;
            props.CorrelationId = messageObject.Header.ReplytToCorelationId;
            var messageBody = GetMessageBody(messageObject);
            _channel.BasicPublish("", props.ReplyTo, props, messageBody);
            return Task.CompletedTask;
        }

        public async Task<TResponse> Request<TRequest, TResponse>(BaseMessage<TRequest> messageObject, int waitInSeconds = 5)
        {
            SetupConnection();

            if (_channel is null)
            {
                throw new NullReferenceException("No channels were optained");
            }

            DeclareExchange(_channel, messageObject);

            string responseQueueName = _channel.QueueDeclare(exclusive: false).QueueName;

            var consumer = new EventingBasicConsumer(_channel);

            Channel<TResponse> blocking = Channel.CreateBounded<TResponse>(1);

            consumer.Received += async (source, args) =>
            {
                var messageString = Encoding.UTF8.GetString(args.Body.ToArray());
                var response = JsonSerializer.Deserialize<TResponse>(messageString);
                if (response is not null)
                {
                    await blocking.Writer.WriteAsync(response);
                }
            };

            _channel.BasicConsume(responseQueueName, true, consumer);

            var props = _channel.CreateBasicProperties();

            props.ReplyTo = responseQueueName;
            props.CorrelationId = Guid.NewGuid().ToString();

            messageObject.Header.ReplyToAddress = responseQueueName;
            messageObject.Header.ReplytToCorelationId = props.CorrelationId;

            var messageBody = GetMessageBody(messageObject);

            _channel.BasicPublish(messageObject.Header.Topic, "", props, messageBody);

            var token = new CancellationTokenSource(TimeSpan.FromSeconds(waitInSeconds)).Token;

            await blocking.Reader.WaitToReadAsync(token);

            return await blocking.Reader.ReadAsync();
        }

        public Task Subscribe<T>(BaseMessage<T> messageObject, Func<T, Task> received)
        {
            SetupConnection();
            DeclareExchange(_channel, messageObject);
            DeclareQueue(_channel, messageObject);
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += async (src, ea) =>
            {
                var messageBytes = ea.Body;
                var messageBodyString = Encoding.UTF8.GetString(messageBytes.ToArray());
                var message = JsonSerializer.Deserialize<T>(messageBodyString);
                if (message is not null)
                {
                    await received(message);
                }
            };

            _channel.BasicConsume(messageObject.Header.Queue, true, consumer);
            return Task.CompletedTask;
        }

        private void SetupConnection()
        {
            if (_connectionFactory is null)
            {
                _connectionFactory = new ConnectionFactory() //Thiw woulb be from configurations later
                {
                    HostName = "sparrow-01.rmq.cloudamqp.com",
                    UserName = "cuiezyus",
                    Password = "CAbY4Kf_qaUUWFy9kdFLDtJYZ98FpiNO",
                    Port = 5672,
                    VirtualHost = "cuiezyus"
                };

                _connection = _connectionFactory.CreateConnection();
                _channel = _connection.CreateModel();
            }
        }

        private void DeclareExchange<T>(IModel? channle, BaseMessage<T> messageObject)
        {

            if (!string.IsNullOrEmpty(messageObject.Header.Topic))
            {
                channle.ExchangeDeclare(messageObject.Header.Topic, ExchangeType.Fanout, true, false);
            }
        }

        private void DeclareQueue<T>(IModel? channle, BaseMessage<T> messageObject)
        {
            if (!string.IsNullOrEmpty(messageObject.Header.Queue))
            {
                channle.QueueDeclare(messageObject.Header.Queue, true, false, false);
            }

            if (!string.IsNullOrEmpty(messageObject.Header.Topic))
            {
                channle.ExchangeDeclare(messageObject.Header.Topic, ExchangeType.Fanout, true, false);

                if (!string.IsNullOrEmpty(messageObject.Header.Queue))
                {
                    channle.QueueBind(messageObject.Header.Queue, messageObject.Header.Topic, "");
                }
            }
        }

        private byte[] GetMessageBody(object? input)
        {
            if (input is null)
            {
                return new byte[] { };
            }

            var messageString = JsonSerializer.Serialize(input, new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.IgnoreCycles
            });

            var bytes = Encoding.UTF8.GetBytes(messageString);

            return bytes;
        }

        public void Dispose()
        {
            if (_connectionFactory is not null)
            {
                if (_connection is not null)
                {
                    if (_channel is not null)
                    {
                        _channel.Dispose();
                        _channel = null;
                    }
                    _connection.Dispose();
                    _connection = null;
                }
                _connectionFactory = null;
            }
        }
    }
}
