using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMqSmsConsumer.Modals
{
    public class PostaGuverciniResponse
    {
        public string ErrorNo { get; set; }
        public string ErrText { get; set; }
        public string Status { get; set; }
        public string MessageId { get; set; }
        public string Charge { get; set; }
        public string Credit { get; set; }
    }
}
