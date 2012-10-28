using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.WebPages;
using System.Xml.XPath;

namespace XRazor.Extensions
{
	public class XRazor
	{
		private Dictionary<string, Func<dynamic, HelperResult>> _matchDictionary;

		public XRazor()
		{
			_matchDictionary = new Dictionary<string, Func<dynamic, HelperResult>>();
		}

		public void Match(string xpath, Func<dynamic, HelperResult> template)
		{
			RegisterMatch(xpath, template);
		}

		private void RegisterMatch(string xpath, Func<dynamic, HelperResult> template)
		{
			if (!_matchDictionary.ContainsKey(xpath))
			{
				_matchDictionary.Add(xpath, template);
			}
		}

		public HelperResult ApplyForModel(object model)
		{
			var navigator = model as XPathNavigator;
			if (navigator == null)
			{
				throw new ArgumentException("Model is not XPathNavigable");
			}

			return new HelperResult(writer => {
				foreach (var item in _matchDictionary)
				{
					var nodes = navigator.Select(item.Key);
					foreach (var node in nodes)
					{
						item.Value(node).WriteTo(writer);
					}
				}
			});
		}
	}
}