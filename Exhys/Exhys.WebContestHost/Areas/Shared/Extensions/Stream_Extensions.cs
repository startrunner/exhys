using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace Exhys.WebContestHost.Areas.Shared.Extensions
{
    public static class Stream_Extensions
    {
        public static byte[] ToArray(this Stream that)
        {
            LinkedList<byte> bytes = new LinkedList<byte>();
            for(int b;;)
            {
                b = that.ReadByte();
                if (b != -1) bytes.AddLast((byte)b);
                else break;
            }
            return bytes.ToArray();
        }
    }
}