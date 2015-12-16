using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Exhys.WebContestHost.Areas.Shared.Extensions
{
    public static class TempDataDictionary_Extensions
    {
        private static class PropertyNames
        {
            public const string FormErrors = "form-errors-278uwdghdwu";
            public const string RedirectsBackTo = "redirects-back-2";
        }
        public static void SetFormErrors (this TempDataDictionary that, IEnumerable<FormErrors.FormError> errors)
        {
            that[PropertyNames.FormErrors] = errors;
        }

        public static void SetRedirectsBackTo(this TempDataDictionary that, RouteData url)
        {
            that[PropertyNames.RedirectsBackTo] = url;
        }

        public static IEnumerable<FormErrors.FormError> GetFormErrors (this TempDataDictionary that)
        {
            return that[PropertyNames.FormErrors] as IEnumerable<FormErrors.FormError>;
        }

        public static RouteData GetRedirectsBackTo(this TempDataDictionary that)
        {
            return that[PropertyNames.RedirectsBackTo] as RouteData;
        }

    }
}