using Aiursoft.Pylon.Models.Stargate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MessageQueue.Data
{
    public static class MessageQueueMemory
    {
        public static List<Message> Messages { get; set; } = new List<Message>();
    }
}
