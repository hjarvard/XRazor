using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace XRazor.ViewPages
{
	public abstract class XsltWebViewPage<TModel> : WebViewPage<TModel> where TModel : class
	{
		public XslHelper<TModel> Xsl { get; set; }

		public override void InitHelpers()
		{
			base.InitHelpers();
			Xsl = new XslHelper<TModel>(ViewContext, this);
		}
	}
}
