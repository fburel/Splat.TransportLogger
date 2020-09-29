using System;
using System.Diagnostics;

namespace Splat.TransportLogger.Transports
{
    public class DebugOutput : ITransport
    {
        public void OnLogReceived(object message, LogLevel level, Type type = null)
        {
            Debug.WriteLine(message);
        }
    }
}