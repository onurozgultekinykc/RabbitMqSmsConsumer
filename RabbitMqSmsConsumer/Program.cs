using System;
using System.Threading.Tasks;
using MailConsumerRabbitMQ.Modals;

class Program
{
    static async Task Main(string[] args)
    {
        Console.WriteLine("Sms Consumer başlatılıyor...");

        var smsConsumer = new SmsConsumer();
        var hostName = Environment.GetEnvironmentVariable("HOST_NAME") ?? "192.168.1.76";
        await smsConsumer.InitializeAsync(hostName); // Bağlantıyı başlat
        smsConsumer.StartListening(); // Mesajları dinlemeye başla

        await Task.Delay(-1); // Programın sürekli çalışmasını sağla
    }
}
