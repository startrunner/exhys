using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace Exhys.WebContestHost.Areas.Shared.Extensions
{
    public static class HttpPostedFileBase_Extensions
    { 
        private static string tmpRoot = Path.GetTempPath();
        public static string ReadContents(this HttpPostedFileBase that)
        {
            //string fileName = string.Format("{0}{1}_{2}.tmp", tmpRoot, "readhttp", Guid.NewGuid().ToString().ToLower());
            //that.SaveAs(fileName);
            //return File.ReadAllText(fileName);


            LinkedList<byte> bytes = new LinkedList<byte>();
            for(int i=that.InputStream.ReadByte();i!=-1;i=that.InputStream.ReadByte())
            {
                bytes.AddLast((byte)i);
            }
            return Encoding.Default.GetString(bytes.ToArray());
        }
    }
}