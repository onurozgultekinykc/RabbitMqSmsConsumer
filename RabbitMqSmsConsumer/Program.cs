using System;
using System.Threading.Tasks;
using MailConsumerRabbitMQ.Modals;

class Program
{
    static async Task Main(string[] args)
    {
        Console.WriteLine("Sms Consumer başlatılıyor...");

        var smsConsumer = new SmsConsumer();
        await smsConsumer.InitializeAsync(); // Bağlantıyı başlat
        smsConsumer.StartListening(); // Mesajları dinlemeye başla

        await Task.Delay(-1); // Programın sürekli çalışmasını sağla
    }
}
