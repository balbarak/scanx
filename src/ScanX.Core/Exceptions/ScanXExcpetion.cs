using System;
using System.Collections.Generic;
using System.Text;

namespace ScanX.Core.Exceptions
{
    public enum ScanXExceptionCodes
    {
        NoPaper = 300,
        NoDevice = 302
    }

    [Serializable]
    public class ScanXException : Exception
    {
        public ScanXExceptionCodes Code { get; set; }

        public ScanXException()
        {

        }

        public ScanXException(string message,ScanXExceptionCodes code) : base(message)
        {
            this.Code = code;
        }

        public ScanXException(string message, Exception inner) : base(message, inner) { }
        protected ScanXException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }

}
