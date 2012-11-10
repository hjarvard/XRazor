using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.XPath;
using System.Web.WebPages;

namespace XRazor.ViewPages
{
	public class TemplateRule
	{
		public XPathExpression XPath { get; set; }
		public Func<dynamic, HelperResult> Template{ get; set; }
		public decimal Priority { get; set; }

		public TemplateRule(string xpath, Func<dynamic, HelperResult> template)
		{
			XPath = XPathExpression.Compile(xpath);
			Template = template;
		}
	}
}