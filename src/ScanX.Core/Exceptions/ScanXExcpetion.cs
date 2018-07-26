using System;
using System.Collections.Generic;
using System.Text;

namespace ScanX.Core.Exceptions
{
    public enum ScanXExceptionCodes
    {
        NoPaper = 300,
        NoDevice = 302,
        DeviceBusy = 304,
        CoverOpen = 305,
        DeviceLocked = 306,
        IconrrectSetting = 307,
        NotSupportedCommand = 308,
        ItemDeleted = 309,
        ScannerLampIsOff = 310,
        ScannerInterupted = 312,
        MultipageFeedCondition = 313,
        DeviceOffline = 320,
        PaperJammed = 321,
        DocumentFeeder = 322,
        DeviceIsWarmpingUp = 323,
        CommunicationWithDeviceFailed = 400,
        DeviceDriverError = 500,
        DeviceDriverInvlid = 501,
        UnkownError = 100,

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

        public ScanXException(string message, Exception inner,ScanXExceptionCodes code) : base(message, inner)
        {
            this.Code = code;
        }

        protected ScanXException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }

}
