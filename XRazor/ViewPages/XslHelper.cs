using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.WebPages;
using System.Xml.XPath;
using sdf.XPath;
using System.IO;

namespace XRazor.ViewPages
{
	public class XslHelper<TModel> where TModel : class
	{
		private ViewContext _viewContext;
		private XsltWebViewPage<TModel> _xsltWebViewPage;

		private Stack<XPathNavigator> _navigatorsStack;

		private List<TemplateRule> _templateRules;

		public XslHelper(ViewContext viewContext, XsltWebViewPage<TModel> xsltWebViewPage)
			: this(viewContext, xsltWebViewPage, null)
		{

		}

		public XslHelper(ViewContext viewContext, XsltWebViewPage<TModel> xsltWebViewPage, XPathNavigator xPathNavigator)
		{
			_navigatorsStack = new Stack<XPathNavigator>();
			_templateRules = new List<TemplateRule>();
			var context = new ObjectXPathContext();
			_viewContext = viewContext;
			_xsltWebViewPage = xsltWebViewPage;
			_navigatorsStack.Push(xPathNavigator ?? context.CreateNavigator(_xsltWebViewPage.Model));
		}

		public HelperResult Match(string xpath, Func<dynamic, HelperResult> template)
		{
			RegisterMatch(xpath, template);
			return new HelperResult(writer => { });
		}

		public HelperResult ApplyTemplates()
		{
			return ApplyTemplates("/");
		}

		public HelperResult ApplyTemplates(string xpath)
		{

			var stringWriter = new StringWriter();
			var nodes = _navigatorsStack.Peek().Select(xpath);

			while (nodes.MoveNext())
			{
				var navigator = nodes.Current;
				_navigatorsStack.Push(navigator);
				foreach (var templateRule in _templateRules)
				{
					if (navigator.Matches(templateRule.XPath))
					{
						templateRule.Template(_xsltWebViewPage.Model).WriteTo(stringWriter);
					}
				}
				_navigatorsStack.Pop();
			}

			return new HelperResult(writer =>
			{
				writer.Write(stringWriter);
			});
		}

		public HelperResult ValueOf(string xpath)
		{
			return new HelperResult(writer =>
										{
											writer.Write(_navigatorsStack.Peek().SelectSingleNode((typeof(TModel).Name) + "/" + xpath).Value);
										});
		}

		private void RegisterMatch(string xpath, Func<dynamic, HelperResult> template)
		{
			_templateRules.Add(new TemplateRule(xpath, template));
		}

	}
}
