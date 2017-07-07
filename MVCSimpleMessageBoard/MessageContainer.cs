using System.Collections.Generic;

namespace MVCSimpleMessageBoard
{
    public class MessageContainer : IMessageContainer
    {
        public List<string> Messages { get; }
        public MessageContainer()
        {
            Messages = new List<string>();
        }
    }
}
