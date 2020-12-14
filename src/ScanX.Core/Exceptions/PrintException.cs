using System;
using System.Collections.Generic;
using System.Text;

namespace ScanX.Core.Exceptions
{

    [Serializable]
    public class PrintException : Exception
    {

        public PrintException() { }
        public PrintException(string message) : base(message) { }
        public PrintException(string message, Exception inner) : base(message, inner) { }
        protected PrintException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
