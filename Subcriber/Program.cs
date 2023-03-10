using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

var connectionFactory = new ConnectionFactory
{
    HostName = "localhost"
};
var connection = connectionFactory.CreateConnection();
using var channel = connection.CreateModel();
channel.QueueDeclare("orders", exclusive: false);
var consumer = new EventingBasicConsumer(channel);
consumer.Received += (_, eventAgrs) =>
{
    var body = eventAgrs.Body.ToArray();
    var message = Encoding.UTF8.GetString(body);
    Console.WriteLine($"Message received {message}");

};

channel.BasicConsume("orders", true,consumer);
Console.ReadKey();