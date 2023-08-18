using RabbitMQ.Client;
using Shared;
using System.Text;
using System.Text.Json;

namespace RabbitMQ.Publisher;

internal class Program
{
    public enum LogNames
    {
        Critical = 1,
        Error = 2,
        Warning = 3,
        Info = 4,
    }
    static void Main(string[] args)
    {
        var factory = new ConnectionFactory();

        factory.Uri = new Uri("amqps://dqbsddti:ma5R1xsLza84BdJZR_p3kx7OG4lq_uIS@woodpecker.rmq.cloudamqp.com/dqbsddti");

        using var connection = factory.CreateConnection();

        var channel = connection.CreateModel();

        channel.ExchangeDeclare("header-exchange", durable: true, type: ExchangeType.Headers);

        Dictionary<string, object> headers = new Dictionary<string, object>();
        headers.Add("format", "pdf");
        headers.Add("shape", "a4");

        IBasicProperties properties = channel.CreateBasicProperties();
        properties.Headers = headers;
        properties.Persistent = true; //mesajlar kalıcı hale gelir.

        var product = new Product { Id = 1, Name = "Pencil", Price = 100, Stock = 10 };

        var productJsonString = JsonSerializer.Serialize(product);

        channel.BasicPublish("header-exchange", string.Empty, properties,Encoding.UTF8.GetBytes(productJsonString));

        Console.WriteLine("Message has been sent");

        Console.ReadLine();
    }
}