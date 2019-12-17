using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
namespace WebLibrary.Utils
{
    public static class MyHelpers
    {
        //исп-ся в предст. "Detail" 
        public static HtmlString MyPrintHelper(this HtmlHelper helper, string str, int fontSize)
        {
            string innerHtml = $"<p style=\"font-size:{fontSize}px\">{str}</p>";
            return new HtmlString(innerHtml);
        }
        //исп-ся в предст. "Index"(пример надуманный, т.к. представл. привязано к типу List<Books>)
        public static MvcHtmlString CreateTableList(this HtmlHelper helper, List<Books> items, string cssClassName)
        {
            TagBuilder tbl = new TagBuilder("table");
            tbl.AddCssClass(cssClassName);
            TagBuilder rowHeader = new TagBuilder("tr");
            var props = items[0].GetType().GetProperties();  //массив св-в объекта Books
            
            foreach (var i in props)    //шапка табл.
            {
                TagBuilder bodyHeader = new TagBuilder("th");      
                //System.Diagnostics.Debug.WriteLine(i.Name); 
                bodyHeader.SetInnerText(i.Name);     
                rowHeader.InnerHtml += bodyHeader.ToString();
            }            
            tbl.InnerHtml += rowHeader.ToString();

            foreach (var item in items) //ост. строки
            {
                TagBuilder row = new TagBuilder("tr");
                foreach (var i in props)
                {
                    //System.Diagnostics.Debug.WriteLine(i.GetValue(item));

                    TagBuilder body = new TagBuilder("td");
                    body.SetInnerText(i.GetValue(item).ToString());

                    row.InnerHtml += body.ToString();
                }                
                tbl.InnerHtml += row.ToString();
            }
            return new MvcHtmlString(tbl.ToString());
        }
    }
}