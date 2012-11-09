using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace XRazor.ViewPages
{
    public class XslHelper<TModel> where TModel : class
    {
        private ViewContext _viewContext;
        private XsltWebViewPage<TModel> _xsltWebViewPage;

        public XslHelper(ViewContext viewContext, XsltWebViewPage<TModel> xsltWebViewPage)
        {
            _viewContext = viewContext;
            _xsltWebViewPage = xsltWebViewPage;
        }
    }
}
