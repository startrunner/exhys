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

        public static List<TItem> Resize<TItem>(this List<TItem> that, int minLength, TItem item)
        {
            while (that.Count < minLength) that.Add(item);
            return that;
        }

        public static TItem LastByIndex<TItem>(this IList<TItem> that)
        {
            return that[that.Count - 1];
        }

        public static TItem LastByIndexOrDefault<TItem>(this IList<TItem> that)
        {
            if (that.Count == 0) return default(TItem);
            else return that[that.Count - 1];
        }
    }
}