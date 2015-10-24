using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Exhys.WebContestHost.Areas.Shared.Extensions
{
    public static class List_Extensions
    {
        public static List<TTo> Convert<TFrom, TTo>(this IEnumerable<TFrom> that, Func<TFrom, TTo> convert)
        {
            List<TTo> rt = new List<TTo>();
            foreach(var v in that) rt.Add(convert(v));
            return rt;
        }
    }
}