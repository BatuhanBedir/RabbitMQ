using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace RabbitMQ.Subscriber;

internal class Program
{
    static void Main(string[] args)
    {
        var factory = new ConnectionFactory();
        factory.Uri = new Uri("amqps://dqbsddti:ma5R1xsLza84BdJZR_p3kx7OG4lq_uIS@woodpecker.rmq.cloudamqp.com/dqbsddti");

        using var connection = factory.CreateConnection();

        var channel = connection.CreateModel();

        channel.BasicQos(0, 1, false);
        //herbir subs. bir mesaj gönderecek.

        var consumer = new EventingBasicConsumer(channel);

        channel.BasicConsume("hello-queue", false, consumer);

        consumer.Received += (sender,e) =>
        {
            var message = Encoding.UTF8.GetString(e.Body.ToArray());

            Thread.Sleep(1500);

            Console.WriteLine("Gelen Mesaj: " + message);

            channel.BasicAck(e.DeliveryTag, false);
        };

        Console.ReadLine();
    }

}