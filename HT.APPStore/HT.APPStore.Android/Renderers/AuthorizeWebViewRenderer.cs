using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Webkit;
using Android.Widget;
using HT.APPStore.Controls;
using HT.APPStore.Droid.Renderers;
using Xam.Plugin.WebView.Abstractions;
using Xam.Plugin.WebView.Droid;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly:ExportRenderer(typeof(AuthorizeWebView),typeof(AuthorizeWebViewRenderer))]
namespace HT.APPStore.Droid.Renderers
{
    public class AuthorizeWebViewRenderer : FormsWebViewRenderer
    {
        public AuthorizeWebViewRenderer()
        {

        }
        protected override void OnElementChanged(ElementChangedEventArgs<FormsWebView> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement != null)
            {
                CookieSyncManager cookieSyncManager = CookieSyncManager.CreateInstance(Control.Context);
                CookieManager cookieManager = CookieManager.Instance;
                cookieManager.SetAcceptCookie(true);
                cookieManager.RemoveSessionCookie();
                cookieManager.RemoveAllCookie();

            }
        }
    }
}