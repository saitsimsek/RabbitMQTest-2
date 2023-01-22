using RabbitMQ.Client;
using System.Text;

class Program
{
    static void Main(string[] args)
    {
        var factory = new ConnectionFactory();
        factory.Uri = new Uri("amqps://hlhvpexd:8tyFLTYa1wv1adchaD25UOMlJkS7njzJ@shrimp.rmq.cloudamqp.com/hlhvpexd");

        using var connection = factory.CreateConnection();

        var channel = connection.CreateModel();

        channel.ExchangeDeclare("soybis-direct", durable: true, type: ExchangeType.Direct);

        Enum.GetNames(typeof(SoybisType)).ToList().ForEach(x =>
        {
            var routeKey = $"route-{x}";
            var queueName = $"direct-queue-{x}";
            channel.QueueDeclare(queueName, true, false, false);

            channel.QueueBind(queueName, "soybis-direct", routeKey, null);

        });

        Enumerable.Range(1, 1000).ToList().ForEach(x =>
        {
            int tc = 10000 + x;
            Enum.GetNames(typeof(SoybisType)).ToList().ForEach(x =>
            {
                string message = $"{tc}";

                var messageBody = Encoding.UTF8.GetBytes(message);

                var routeKey = $"route-{x}";

                channel.BasicPublish("soybis-direct", routeKey, null, messageBody);

                Console.WriteLine($"Tc Kimlik {x} için gönderilmiştir : {message}");
            });
        });

        Console.ReadLine();
    }
}

public enum SoybisType
{
    Arac = 1,
    Tapu = 2,
    SGK = 3,
    EmekliSandigi = 4
}