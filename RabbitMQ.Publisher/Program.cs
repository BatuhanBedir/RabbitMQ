using RabbitMQ.Client;
using System.Text;

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

        channel.ExchangeDeclare("logs-topic", durable: true, type: ExchangeType.Topic);


        Random rnd = new Random();
        Enumerable.Range(1, 50).ToList().ForEach(x =>
        {
            LogNames log1 = (LogNames)rnd.Next(1, 5);
            LogNames log2 = (LogNames)rnd.Next(1, 5);
            LogNames log3 = (LogNames)rnd.Next(1, 5);

            var routeKey = $"{log1}.{log2}.{log3}";

            string message = $"log-type : {log1}-{log2}-{log3}";

            var messageBody = Encoding.UTF8.GetBytes(message);

            channel.BasicPublish("logs-topic", routeKey, null, messageBody);

            Console.WriteLine($"Mesaj gönderilmiştir : {message}");

        });

        Console.ReadLine();
    }
}