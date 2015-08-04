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
            (this HtmlHelper<TModel> helper, Expression<Func<TModel, TProperty>> expression, IEnumerable<SelectListItem> options, string selectedValue = null)
        {
            if (selectedValue == null && helper.ViewData.Model != null)
                selectedValue = expression.Compile()(helper.ViewData.Model).ToString();

            string inputName = ExpressionHelper.GetExpressionText(expression);

            return helper.FixedDropDownListFor(inputName, options, selectedValue);
        }

        public static MvcHtmlString FixedDropDownListFor 
            (this HtmlHelper helper, string inputName, IEnumerable<SelectListItem> options, string selectedValue=null)
        {
            StringBuilder rt = new StringBuilder();

            rt.Append(string.Format(@"<select name={0} id={0}>", inputName));

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

        public static TabControlHelper TabControl(this HtmlHelper helper, string[] headers)
        {
            return new TabControlHelper(helper, headers);
        }

        public static TabItemHelper TabItem (this HtmlHelper helper)
        {
            return new TabItemHelper(helper);
        }

        public class TabControlHelper:IDisposable
        {
            public static TabControlHelper Current { get; private set; } = null;

            public int TabCount { get; set; }

            public string Prefix { get; private set; }

            private StringBuilder builder;
            public TabControlHelper (HtmlHelper helper, string[] headers)
            {
                this.Prefix = Guid.NewGuid().ToString().ToLower();

                builder = ((StringWriter)helper.ViewContext.Writer).GetStringBuilder();
                builder.Append(@"<div class='tab-control'>");

                builder.Append(@"<div class='tab-headers'>");
                {
                    int i = 0;
                    foreach (string s in headers)
                    {
                        builder.AppendFormat(@"<a href='#{0}-{1}'>{2}</a>",this.Prefix, (i++).ToString(), s);
                    }
                }
                builder.Append(@"</div>");

                TabCount = 0;

                builder.Append(@"<div class='tab-content'>");
               
                Current = this;
            }

            public void Dispose ()
            {
                builder.Append(@"</div></div>");
                Current = null;
            }
        }

        public class TabItemHelper : IDisposable
        {
            private StringBuilder builder;

            public TabItemHelper(HtmlHelper helper)
            {

                if (TabControlHelper.Current == null) throw new Exception("A tab item must be inside of a tab control.");
                builder = ((StringWriter)helper.ViewContext.Writer).GetStringBuilder();

                builder.AppendFormat(@"<div class='tab-item' id='{0}-{1}'>", TabControlHelper.Current.Prefix, (TabControlHelper.Current.TabCount++).ToString());
            }

            public void Dispose ()
            {
                builder.Append(@"</div>");
            }
        }
    }
}