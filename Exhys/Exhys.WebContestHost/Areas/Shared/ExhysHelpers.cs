using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Exhys.WebContestHost.Areas.Shared
{
    public static class ExhysHelpers
    {
        public static MvcHtmlString FixedDropDownListFor<TModel, TProperty>
            (this HtmlHelper<TModel> helper, Expression<Func<TModel, TProperty>> expression, IEnumerable<SelectListItem> options, string selectedValue = null, bool allowNull=false)
        {
            if (selectedValue == null && helper.ViewData.Model != null)
                selectedValue = expression.Compile()(helper.ViewData.Model).ToString();

            string inputName = ExpressionHelper.GetExpressionText(expression);

            return helper.FixedDropDownListFor(inputName, options, selectedValue, allowNull);
        }

        public static MvcHtmlString FixedDropDownListFor
            (this HtmlHelper helper, string inputName, IEnumerable<SelectListItem> options, string selectedValue = null, bool allowNull=false)
        {
            StringBuilder rt = new StringBuilder();

            rt.Append(string.Format(@"<select name={0} id={0}>", inputName));

            if(allowNull)
            {
                rt.Append("<option value=\"\">[NULL]</option>");
            }

            foreach (var v in options)
            {
                rt.Append(
                    String.Format("<option value=\"{0}\" {1}>{2}</option>",
                    v.Value,
                    v.Value == selectedValue ? "selected" : "",
                    v.Text != null ? v.Text : v.Value));
            }

            rt.Append(@"</select>");

            return new MvcHtmlString(rt.ToString());
        }

        public static MvcHtmlString ErrorListFor(this HtmlHelper helper, ViewDataDictionary viewData)
        {
            StringBuilder rt = new StringBuilder();

            rt.Append("<ul class=\"error-list\">");

            foreach(var v in viewData.ModelState.Values)
            {
                foreach(var v1 in v.Errors)
                {
                    rt.AppendFormat("<li>{0}</li>", v1.ErrorMessage); 
                }
            }

            rt.Append("</ul>");

            return new MvcHtmlString(rt.ToString());
        }
    }
}