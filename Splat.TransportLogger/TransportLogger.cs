using System;
using System.Collections.Generic;

namespace Splat.TransportLogger
{
    public class TransportLogger : ILogger
    {
        private readonly ISet<ITransport> _transports = new HashSet<ITransport>();

        public TransportLogger()
        {
#if DEBUG
            AddTransport(new Splat.TransportLogger.Transports.DebugOutput());
#endif
        }
        
        public TransportLogger AddTransport(ITransport transport)
        {
            _transports.Add(transport);
            return this;
        }
        #region ILLoger
        
        public void Write(string message, LogLevel logLevel)
        {
            Write(message, null, logLevel);
        }
        
        public void Write(Exception exception, string message, LogLevel logLevel)
        {
            Write($"exception: ({message}) : {exception.Message}", logLevel);
        }
        
        public void Write(string message, Type type, LogLevel logLevel)
        {
            foreach (var transport in _transports)
                try
                {
                    transport.OnLogReceived(message, logLevel, type);
                }
                catch
                {
                    // do nothing
                }
        }

        public void Write(Exception exception, string message, Type type, LogLevel logLevel)
        {
            Write($"exception: ({message}) : {exception.Message}", type, logLevel);
        }

        public LogLevel Level { get; private set; }
        
        #endregion
        
        
    }
}