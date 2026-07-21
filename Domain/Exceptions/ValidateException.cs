using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Domain.Exceptions
{
    public class ValidateException : Exception, IDisposable
    {
        public override string Message => string.Join(", ", Messages.Select(s => s.Message));
        public List<ExceptionViewModel> Messages { get; set; } = new List<ExceptionViewModel>();
        private bool IsThrown { get; set; }

        public ValidateException() { }

        public ValidateException(string message)
        {
            Messages.Add(new ExceptionViewModel()
            {
                Message = message
            });
        }

        public ValidateException(string elementId, string message)
        {
            Messages.Add(new ExceptionViewModel()
            {
                ElementId = elementId,
                Message = message
            });
        }

        public void Add(string message)
        {
            Messages.Add(new ExceptionViewModel()
            {
                Message = message
            });
        }

        public void Add(string elementId, string message)
        {
            Messages.Add(new ExceptionViewModel()
            {
                ElementId = elementId,
                Message = message
            });
        }

        public void Throw()
        {
            if (Messages.Count == 0)
                return;
            else
            {
                IsThrown = true;
                throw this;
            }
        }

        public void Dispose()
        {
            bool thrownFromInside = Marshal.GetExceptionPointers() != IntPtr.Zero;
            if (Messages.Count == 0 || IsThrown || thrownFromInside)
                return;
            throw this;
        }

        public class ExceptionViewModel 
        { 
            public string ElementId { get; set; }
            public string Message { get; set; }
        }
    }
}
