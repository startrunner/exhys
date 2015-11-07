using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;

namespace Exhys.WebContestHost.Areas.Shared
{
    public static class ExhysHelpers
    {
        public static MvcHtmlString FixedDropDownListFor<TModel, TProperty>
            (this HtmlHelper<TModel> helper, Expression<Func<TModel, TProperty>> expression, IEnumerable<SelectListItem> options, string selectedValue, bool allowNull, object htmlAttributes=null)
        {
            if (selectedValue == null && helper.ViewData.Model != null)
                selectedValue = expression.Compile()(helper.ViewData.Model).ToString();

            string inputName = ExpressionHelper.GetExpressionText(expression);

            return helper.FixedDropDownListFor(inputName, options, selectedValue, allowNull, htmlAttributes);
        }

        public static MvcHtmlString FixedDropDownListFor<TModel, TProperty>
            (this HtmlHelper<TModel> helper, Expression<Func<TModel, TProperty>> expression, IEnumerable<SelectListItem> options, object htmlAttributes = null)
        {
            return helper.FixedDropDownListFor(expression, options, null, false, htmlAttributes);
        }
		public static MvcHtmlString FixedDropDownListFor
			( this HtmlHelper helper, string name, IEnumerable<SelectListItem> options, object htmlAttributes = null )
		{
			return helper.FixedDropDownListFor(name, options, null, false, htmlAttributes);
		}

		public static MvcHtmlString FixedDropDownListFor
            (this HtmlHelper helper, string inputName, IEnumerable<SelectListItem> options, string selectedValue = null, bool allowNull=false, object htmlAttributes=null)
        {
            TagBuilder selectTb = new TagBuilder("select");
            selectTb.Attributes.Add("id", inputName);
            selectTb.Attributes.Add("name", inputName);
            foreach (var attr in new RouteValueDictionary(htmlAttributes))
            {
                selectTb.Attributes.Add(attr.Key, attr.Value.ToString());
            }

            if(allowNull)
            {
                string optionTxt = "<option value=\"\">[NULL]</option>";
                selectTb.InnerHtml += optionTxt;
            }

            foreach (var v in options)
            {
                string optionTxt = string.Format("<option value=\"{0}\" {1}>{2}</option>",
                    v.Value,
                    v.Value == selectedValue ? "selected" : "",
                    v.Text != null ? v.Text : v.Value);
                selectTb.InnerHtml += optionTxt;
            }

            return new MvcHtmlString(selectTb.ToString());
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