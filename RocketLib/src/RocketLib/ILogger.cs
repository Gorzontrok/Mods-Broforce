using System;

namespace RocketLib
{
    public interface ILogger
    {
        void Log(object message);
        void Log(string message);
        void Error(string message);
        void Exception(Exception exception);
        void Exception(string message, Exception exception);
        void Debug(string message);
        void Warning(string message);
    }
}
