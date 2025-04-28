using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMqSmsConsumer.Modals
{
    public class MessageMQSms
    {
        public string baseUrl { get; set; }
        public Dictionary<string, string> data { get; set; }
    }
}
