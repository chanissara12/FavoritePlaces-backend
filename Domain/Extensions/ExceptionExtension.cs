using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Extensions
{
    public static class ExceptionExtension
    {
        public static string AllMessages(this Exception exception, int recursiveAmt = 5)
        {
            if (!string.IsNullOrWhiteSpace(exception?.InnerException?.Message) && recursiveAmt-- > 0)
                return $"{exception.Message} : {exception.InnerException.AllMessages(recursiveAmt)}";
            else
                return exception?.Message ?? "";
        }

        public static string InnermostMessage(this Exception exception)
        {
            if (!string.IsNullOrWhiteSpace(exception?.InnerException?.Message))
                return exception.InnerException.InnermostMessage();
            else
                return exception?.Message ?? "";
        }

        public static string AllStackTrace(this Exception exception)
        {
            if (exception.InnerException == null)
                return exception.StackTrace;
            return $"{exception.InnerException.AllStackTrace()}\r\n{exception.StackTrace}";
        }
    }
}
