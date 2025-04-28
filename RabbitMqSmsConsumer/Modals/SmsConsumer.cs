using System.Net.Http;
using System.Text;
using System.Web;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMqSmsConsumer.Modals;

namespace MailConsumerRabbitMQ.Modals
{
    public class SmsConsumer
    {
        private  ConnectionFactory _factory;
        private IConnection _connection;
        private IChannel _channel;

        public async Task InitializeAsync()
        {
            _factory = new ConnectionFactory
            {
                Port = 5672,
                HostName = "c_rabbitmq",
                //HostName = "192.168.1.76",
                UserName = "user",
                Password = "1234567",
            
            };

            // RabbitMQ bağlantısı oluştur (senkron API kullanıldığı için doğrudan çağırılıyor)
            _connection =await _factory.CreateConnectionAsync();
            _channel =await _connection.CreateChannelAsync();

            // Kuyruğu tanımla (Asenkron olmayan metot olduğu için direkt çağırılıyor)
           await _channel.QueueDeclareAsync(queue: "sms_queue",
                                  durable: true,
                                  exclusive: false,
                                  autoDelete: false,
                                  arguments: null);

            await Task.CompletedTask; // Metodun async yapısını korumak için ekledik
        }


        public void StartListening()
        {
            var consumer = new AsyncEventingBasicConsumer(_channel);
            consumer.ReceivedAsync += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                try
                {
                    var mailProperties = JsonConvert.DeserializeObject<MessageMQSms>(message);
                 
                    if (mailProperties != null)
                    {
                        await SendSms(mailProperties);
                        await _channel.BasicAckAsync(ea.DeliveryTag, false); // Mesaj işlendiyse onayla
                    }
                  
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Sms gönderme hatası: {ex.Message}");
                    await _channel.BasicAckAsync(ea.DeliveryTag, false); // Hata olursa mesajı tekrar kuyruğa koy
                }
            };

            _channel.BasicConsumeAsync(queue: "sms_queue",
                                  autoAck: false, // Manuel onaylama
                                  consumer: consumer);

            Console.WriteLine("SmsConsumer çalışıyor, sms kuyruğu dinleniyor...");
        }
        public int sayac = 1;
        private async Task SendSms(MessageMQSms _mail)
        {
            try
            {
                var httpClient = new HttpClient();
                using (var response = await httpClient.PostAsync(_mail.baseUrl, _mail.data))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        Encoding iso = Encoding.GetEncoding("ISO-8859-1");
                        Encoding utf8 = Encoding.UTF8;
                        byte[] isoBytes = Encoding.Convert(utf8, iso, await response.Content.ReadAsByteArrayAsync());
                        string apiResponse = iso.GetString(isoBytes);
                        var queryParams = HttpUtility.ParseQueryString(apiResponse);
                     
                    }
                }
                Console.WriteLine(sayac+") SMS başarıyla gönderildi."+ DateTime.Now.ToString("dd-MMM HH:m:s"));
                sayac++;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Sms gönderme hatası: {ex.Message}");
                throw;
            }
        }

    }
}
