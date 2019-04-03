using System;
using System.Text;

namespace Base.Service.Log
{
    public static class LogServiceExtensions
    {
        public static void Log(this ILogService logger, Exception exception, string message = null)
        {
            StringBuilder sb = new StringBuilder();

            WriteException(sb,exception);

            if (message != null)
            {
                sb.Append(" (");
                sb.Append(message);
                sb.Append(")");
            }
            logger.Log(sb.ToString());
        }

        private static void WriteException(StringBuilder sb, Exception exception)
        {
            sb.Append(exception.GetType().Name);
            sb.Append(exception.Message);
            sb.Append(exception.StackTrace);

            var aggregate_exception = exception as AggregateException;

            if (aggregate_exception != null)
            {
                foreach (var ex in aggregate_exception.InnerExceptions)
                {
                    sb.Append(" -> ");
                    WriteException(sb, ex);
                }

            }
            else if (exception.InnerException != null)
            {
                sb.Append(" -> ");

                WriteException(sb, exception.InnerException);
            }
        }
    }
}