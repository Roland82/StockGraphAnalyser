

namespace StockGraphAnalyser.FrontEnd.Infrastructure
{
    using System;
    using System.Text;
    using System.Web.Mvc;

    public static class HtmlHelperExtensions
    {
        public static MvcHtmlString DropdownListFromEnum(this HtmlHelper htmlHelper, string name, Type enumType) {
            var stringBuilder = new StringBuilder(string.Format("<select name='{0}'>", name));
            foreach (var e in Enum.GetValues(enumType))
            {
                stringBuilder.AppendFormat("<option value='{0}'>{1}</option>", e.GetHashCode(), e.ToString());
            }
            stringBuilder.Append("</select>");
            return new MvcHtmlString(stringBuilder.ToString());
        }
    }
}