using System;

namespace Splat.TransportLogger
{
    public interface ITransport
    {
        void OnLogReceived(object message, LogLevel level, Type type = null);
    }
}