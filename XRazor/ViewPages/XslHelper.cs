using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.WebPages;
using System.Xml.XPath;
using sdf.XPath;

namespace XRazor.ViewPages
{
    public class XslHelper<TModel> where TModel : class
    {
        private ViewContext _viewContext;
        private XsltWebViewPage<TModel> _xsltWebViewPage;
        private Dictionary<string, Func<XslHelper<TModel>, HelperResult>> _matchDictionary;
        private XPathNavigator _xPathNavigator;

        public XslHelper(ViewContext viewContext, XsltWebViewPage<TModel> xsltWebViewPage)
            : this(viewContext, xsltWebViewPage, null)
        {
          
        }

        public XslHelper(ViewContext viewContext, XsltWebViewPage<TModel> xsltWebViewPage, XPathNavigator xPathNavigator)
        {
            var context = new ObjectXPathContext();
            _matchDictionary = new Dictionary<string, Func<XslHelper<TModel>, HelperResult>>();
            _viewContext = viewContext;
            _xsltWebViewPage = xsltWebViewPage;
            _xPathNavigator = xPathNavigator ?? context.CreateNavigator(_xsltWebViewPage.Model);
        } 

        public HelperResult Match(string xpath, Func<XslHelper<TModel>, HelperResult> template)
        {
            RegisterMatch(xpath, template);
            return new HelperResult(writer =>
                                        {
                                            
                                        });
        }

        public HelperResult ApplyTemplates(string xpath)
        {

            return new HelperResult(writer =>
            {
                foreach (var item in _matchDictionary)
                {
                    var nav = _xPathNavigator.Clone();
                    nav.MoveToChild(item.Key, "");
                    var xslHelper = new XslHelper<TModel>(_viewContext, _xsltWebViewPage, nav);
                    item.Value(xslHelper).WriteTo(writer);
                }
            });
        }

        public HelperResult ValueOf(string xpath)
        {
            return new HelperResult(writer =>
                                        {
                                            writer.Write(_xPathNavigator.SelectSingleNode((typeof(TModel).Name) + "/" + xpath).Value);
                                        });
        }

        private void RegisterMatch(string xpath, Func<XslHelper<TModel>, HelperResult> template)
        {
            if (!_matchDictionary.ContainsKey(xpath))
            {
                _matchDictionary.Add(xpath, template);
            }
        }

    }
}
