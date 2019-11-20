using System;
using System.Diagnostics;
using System.Reflection;
using System.Threading.Tasks;

namespace XOutput.Logging
{
    /// <summary>
    /// Logger base class.
    /// </summary>
    public abstract class AbstractLogger : ILogger
    {
        private readonly Type loggerType;
        public Type LoggerType => loggerType;
        private readonly int level;
        public int Level => level;

        protected AbstractLogger(Type loggerType, int level)
        {
            this.loggerType = loggerType;
            this.level = level;
        }

        protected string GetCallerMethodName()
        {
            MethodBase method = new StackTrace().GetFrame(2).GetMethod();
            bool asyncFunction = method.DeclaringType.Name.Contains("<") && method.DeclaringType.Name.Contains(">");
            if (asyncFunction)
            {
                int openIndex = method.DeclaringType.Name.IndexOf("<");
                int closeIndex = method.DeclaringType.Name.IndexOf(">");
                return method.DeclaringType.Name.Substring(openIndex + 1, closeIndex - openIndex - 1);
            }
            else
            {
                return method.Name;
            }
        }

        protected string CreatePrefix(DateTime time, LogLevel loglevel, string classname, string methodname)
        {
            return $"{time.ToString("yyyy-MM-dd HH\\:mm\\:ss.fff zzz")} {loglevel.Text} {classname}.{methodname}: ";
        }

        public void Trace(string log)
        {
            LogCheck(LogLevel.Trace, GetCallerMethodName(), log);
        }

        public void Trace(Func<string> log)
        {
            LogCheck(LogLevel.Trace, GetCallerMethodName(), log);
        }

        public void Debug(string log)
        {
            LogCheck(LogLevel.Debug, GetCallerMethodName(), log);
        }

        public void Debug(Func<string> log)
        {
            LogCheck(LogLevel.Debug, GetCallerMethodName(), log);
        }

        public void Info(string log)
        {
            LogCheck(LogLevel.Info, GetCallerMethodName(), log);
        }

        public void Info(Func<string> log)
        {
            LogCheck(LogLevel.Info, GetCallerMethodName(), log);
        }

        public void Warning(string log)
        {
            LogCheck(LogLevel.Warning, GetCallerMethodName(), log);
        }

        public void Warning(Func<string> log)
        {
            LogCheck(LogLevel.Warning, GetCallerMethodName(), log);
        }

        public void Warning(Exception ex)
        {
            LogCheck(LogLevel.Warning, GetCallerMethodName(), ex.ToString());
        }

        public void Warning(string log, Exception ex)
        {
            LogCheck(LogLevel.Warning, GetCallerMethodName(), log, ex);
        }

        public void Warning(Func<string> log, Exception ex)
        {
            LogCheck(LogLevel.Warning, GetCallerMethodName(), log, ex);
        }

        public void Error(string log)
        {
            LogCheck(LogLevel.Error, GetCallerMethodName(), log);
        }

        public void Error(Func<string> log)
        {
            LogCheck(LogLevel.Error, GetCallerMethodName(), log);
        }

        public void Error(Exception ex)
        {
            LogCheck(LogLevel.Error, GetCallerMethodName(), ex.ToString());
        }

        public void Error(string log, Exception ex)
        {
            LogCheck(LogLevel.Error, GetCallerMethodName(), log, ex);
        }

        public void Error(Func<string> log, Exception ex)
        {
            LogCheck(LogLevel.Error, GetCallerMethodName(), log, ex);
        }

        protected void LogCheck(LogLevel loglevel, string methodName, string log)
        {
            if (loglevel.Level >= Level)
            {
                Log(loglevel, methodName, log);
            }
        }

        protected void LogCheck(LogLevel loglevel, string methodName, string log, Exception ex)
        {
            if (loglevel.Level >= Level)
            {
                Log(loglevel, methodName, log + Environment.NewLine + ex.ToString());
            }
        }

        protected void LogCheck(LogLevel loglevel, string methodName, Func<string> log)
        {
            if (loglevel.Level >= Level)
            {
                Log(loglevel, methodName, log());
            }
        }

        protected void LogCheck(LogLevel loglevel, string methodName, Func<string> log, Exception ex)
        {
            if (loglevel.Level >= Level)
            {
                Log(loglevel, methodName, log() + Environment.NewLine + ex.ToString());
            }
        }

        /// <summary>
        /// Writes the log.
        /// </summary>
        /// <param name="loglevel">loglevel</param>
        /// <param name="methodName">name of the caller method</param>
        /// <param name="log">log text</param>
        /// <returns></returns>
        protected abstract void Log(LogLevel loglevel, string methodName, string log);

        public void SafeCall(Action action)
        {
            SafeCall(action, null, LogLevel.Error, GetCallerMethodName());
        }
        public void SafeCall(Action action, string log)
        {
            SafeCall(action, log, LogLevel.Error, GetCallerMethodName());
        }

        public void SafeCall(Action action, string log, LogLevel level)
        {
            SafeCall(action, log, level, GetCallerMethodName());
        }

        public T SafeCall<T>(Func<T> action)
        {
            return SafeCall(action, null, LogLevel.Error, GetCallerMethodName());
        }

        public T SafeCall<T>(Func<T> action, string log)
        {
            return SafeCall(action, log, LogLevel.Error, GetCallerMethodName());
        }

        public T SafeCall<T>(Func<T> action, string log, LogLevel level)
        {
            return SafeCall(action, log, level, GetCallerMethodName());
        }

        private void SafeCall(Action action, string log, LogLevel level, string methodName)
        {
            SafeCall<object>(() =>
            {
                action();
                return null;
            }, log, level, methodName);
        }

        private T SafeCall<T>(Func<T> action, string log, LogLevel level, string methodName)
        {
            try
            {
                return action();
            }
            catch(Exception ex)
            {
                Log(level, methodName, log == null ? ex.ToString() : log + Environment.NewLine + ex.ToString());
                return default;
            }
        }
    }
}
