using System;
using System.Runtime.Serialization;

namespace Exhys.WebContestHost.Areas.Shared.ViewModels
{
    [Serializable]
    internal class WhatTheFuckException : Exception
    {
        public WhatTheFuckException ()
        {
        }

        public WhatTheFuckException (string message) : base(message)
        {
        }

        public WhatTheFuckException (string message, Exception innerException) : base(message, innerException)
        {
        }

        protected WhatTheFuckException (SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}