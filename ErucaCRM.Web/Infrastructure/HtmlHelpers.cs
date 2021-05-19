using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc.Html;
using System.Web.Mvc;
using System.Linq.Expressions;
using ErucaCRM.Utility;


namespace ErucaCRM.Web.Infrastructure
{
    public static class HtmlHelpers
    {
        public static MvcHtmlString CultureSpecificLabel<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, object htmlAttributes, string module = "")
        {
            string moduleName = "";

            if (module != "")
            {
                moduleName = module;
            }

            else if (html.ViewBag.ModuleName != null)
            {
                moduleName = html.ViewBag.ModuleName;

            }
            else if (html.ViewData.Model != null)
            {
                var moduelNameAttribute = (html.ViewData.Model.GetType().GetCustomAttributes(false).FirstOrDefault());

                if (moduelNameAttribute is CultureModuleAttribute)
                {
                    moduleName = ((CultureModuleAttribute)moduelNameAttribute).ModuleName;

                }
            }

            if (!string.IsNullOrEmpty(moduleName))
            {

                return LabelExtensions.LabelFor(html, expression, CommonFunctions.GetGlobalizedLabel(moduleName, ((MemberExpression)expression.Body).Member.Name), htmlAttributes);
            }
            else
            {
                return LabelExtensions.LabelFor(html, expression, htmlAttributes);
            }

        }





        public static MvcHtmlString CultureSpecificLabel<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, string module = "")
        {
            string moduleName = "";

            if (module != "")
            {
                moduleName = module;
            }

            else if (html.ViewBag.ModuleName != null)
            {
                moduleName = html.ViewBag.ModuleName;

            }
            else if (html.ViewData.Model != null)
            {
                var moduelNameAttribute = (html.ViewData.Model.GetType().GetCustomAttributes(false).FirstOrDefault());

                if (moduelNameAttribute is CultureModuleAttribute)
                {
                    moduleName = ((CultureModuleAttribute)moduelNameAttribute).ModuleName;

                }
            }

            if (!string.IsNullOrEmpty(moduleName))
            {

                var s = ((MemberExpression)expression.Body).Member;
                var e = s.GetCustomAttributes(false).Where(x => x.GetType() == typeof(System.ComponentModel.DataAnnotations.DisplayAttribute)).FirstOrDefault();
                if (e == null)
                    return LabelExtensions.LabelFor(html, expression, CommonFunctions.GetGlobalizedLabel(moduleName, s.Name));
                else
                {
                    // var x = (System.ComponentModel.DataAnnotations.DisplayAttribute)e;
                    return LabelExtensions.LabelFor(html, expression, CommonFunctions.GetGlobalizedLabel(moduleName, s.Name));
                }

            }
            else
            {
                return LabelExtensions.LabelFor(html, expression);
            }
        }


        public static String CultureSpecificText(string labelText, string moduleName)
        {
            return labelText;
        }


        public static String CultureSpecificDisplay<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, string module = "")
        {
            string moduleName = "";

            if (module != "")
            {
                moduleName = module;
            }

            else if (html.ViewBag.ModuleName != null)
            {
                moduleName = html.ViewBag.ModuleName;

            }
            else if (html.ViewData.Model != null)
            {
                var moduelNameAttribute = (html.ViewData.Model.GetType().GetCustomAttributes(false).FirstOrDefault());

                if (moduelNameAttribute is CultureModuleAttribute)
                {
                    moduleName = ((CultureModuleAttribute)moduelNameAttribute).ModuleName;

                }
            }

            if (!string.IsNullOrEmpty(moduleName))
            {
                return CommonFunctions.GetGlobalizedLabel(moduleName, ((MemberExpression)expression.Body).Member.Name);


            }


            else
            {
                return (((MemberExpression)expression.Body).Member.Name.ToString());
            }


        }


        public static MvcHtmlString CultureSpecificDropDownList<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IEnumerable<SelectListItem> selectList, object htmlAttributes)
        {
            selectList = selectList.Select(x => { 
                x.Text = CommonFunctions.GetGlobalizedLabel(Constants.CULTURE_SPECIFIC_SHEET_DROPDOWNS, x.Text);
                return x; 
            }).ToList();
            return htmlHelper.DropDownListFor(expression, selectList, htmlAttributes);
        }



        public static MvcHtmlString CultureSpecificValidationMessage<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression)
        {

            return ValidationExtensions.ValidationMessageFor(htmlHelper, expression);

        }

        public static string jQueryDatePickerFormat()
        {
            return System.Threading.Thread.CurrentThread.CurrentCulture.DateTimeFormat.ShortDatePattern.Replace("yyyy", "yy").Replace("MM", "mm").ToLower();
        }
    }
}