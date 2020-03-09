using System;
using System.Linq;
using HtmlAgilityPack;

namespace BannerWebAPI.Validation
{
    public class ValidateHtml : IValidate
    {
        public string[] Validate(in string html)
        {
            try
            {
                var htmlDoc = new HtmlDocument();
                htmlDoc.LoadHtml(html);


                return htmlDoc.ParseErrors.Select(error => error.Reason)?.ToArray();
            }
            catch (Exception)
            {
                return new[] { $"The field 'html' does not contain valid HTML" };
            }
        }
    }
}
