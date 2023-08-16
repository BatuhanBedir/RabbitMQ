using RabbitMQ.Client;
using System.Text;

namespace RabbitMQ.Publisher;

internal class Program
{
    static void Main(string[] args)
    {
        var factory = new ConnectionFactory();

        factory.Uri = new Uri("amqps://dqbsddti:ma5R1xsLza84BdJZR_p3kx7OG4lq_uIS@woodpecker.rmq.cloudamqp.com/dqbsddti");

        using var connection = factory.CreateConnection();

        var channel = connection.CreateModel();

        channel.QueueDeclare("hello-queue", true, false, false);

        string message = "Hello World!";

        var messageBody = Encoding.UTF8.GetBytes(message);
        channel.BasicPublish(string.Empty, "hello-queue", null, messageBody);

        Console.WriteLine("Mesaj gönderilmiştir.");
        Console.ReadLine();
    }
}