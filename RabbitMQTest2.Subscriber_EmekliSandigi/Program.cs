using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

class Program
{
    static void Main(string[] args)
    {
        var factory = new ConnectionFactory();
        factory.Uri = new Uri("amqps://hlhvpexd:8tyFLTYa1wv1adchaD25UOMlJkS7njzJ@shrimp.rmq.cloudamqp.com/hlhvpexd ");

        using var connection = factory.CreateConnection();

        var channel = connection.CreateModel();


        channel.BasicQos(0, 1, false);
        var consumer = new EventingBasicConsumer(channel);
        Console.WriteLine("Logları dinleniyor...");
        var queueName = $"direct-queue-EmekliSandigi";
        channel.BasicConsume(queueName, false, consumer); /*Kuyruklar oluşturuluyor.*/

        consumer.Received += (object sender, BasicDeliverEventArgs e) =>
        {
            var tc = Encoding.UTF8.GetString(e.Body.ToArray());

            Console.WriteLine(EmekliSandiği(tc));
            channel.BasicAck(e.DeliveryTag, false);
        };
        Console.ReadLine();
    }

    public static string EmekliSandiği(string tc)
    {
        Thread.Sleep(1000);
        return "Emekli Sandığı soybis yapıldı:" + tc;
    }
}