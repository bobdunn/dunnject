using System.Collections.Generic;

namespace MVCSimpleMessageBoard
{
    public interface IMessageContainer
    {
        List<string> Messages { get; }
    }
}
