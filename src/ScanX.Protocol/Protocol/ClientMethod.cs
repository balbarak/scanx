using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScanX.Protocol.Protocol
{
    public class ClientMethod
    {
        public const string ON_IMAGE_SCANNED = "OnImageScanned";

        public const string ON_ERROR = "OnError";

        public const string ON_SCAN_FINISHED = "OnFinish";

        public const string ON_LOG = "OnLog";
    }
}
