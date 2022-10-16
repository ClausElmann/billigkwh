using HtmlAgilityPack;
using System.Linq;

namespace BilligKwhWebApp.Extensions
{
    public static class CommonExtensions
	{
		public static string RemoveScripts(this string objHTMLdoc)
		{
			HtmlDocument doc = new HtmlDocument();
			if (string.IsNullOrEmpty(objHTMLdoc))
				return objHTMLdoc;

			doc.LoadHtml(objHTMLdoc);

			doc.DocumentNode.Descendants()
							.Where(n => n.Name == "script" || n.Name == "style")
							.ToList()
							.ForEach(n => n.Remove());
			return doc.DocumentNode.OuterHtml;
		}
	}
}
