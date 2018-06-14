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
using HT.APPStore.Interfaces;
using Xam.Plugin.WebView.Abstractions;
using Xam.Plugin.WebView.Droid;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

namespace HT.APPStore.Droid.Renderers
{
    public class DetailsPageWebViewRenderer : FormsWebViewRenderer
    {
        public DetailsPageWebViewRenderer()
        {

        }
        protected override void OnElementChanged(ElementChangedEventArgs<FormsWebView> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement != null)
            {
                Control.Settings.JavaScriptEnabled = true;

                if (Build.VERSION.SdkInt < BuildVersionCodes.Lollipop)
                    Control.Settings.UseWideViewPort = true;

                LoadData();
            }
        }
        private void LoadData()
        {
            try
            {
                var chrome = new HtmlWebChromeClient(this);
                Control.SetWebChromeClient(chrome);
            }
            catch (System.Exception)
            {

                throw;
            }
        }

        public class HtmlWebChromeClient : WebChromeClient
        {
            DetailsPageWebViewRenderer customWebViewRenderer;
            internal HtmlWebChromeClient(DetailsPageWebViewRenderer customWebViewRenderer)
            {
                this.customWebViewRenderer = customWebViewRenderer;
            }
            public override void OnProgressChanged(global::Android.Webkit.WebView view, int newProgress)
            {
                if (newProgress == 100)
                {
                    new Android.OS.Handler().PostDelayed(() =>
                    {
                        try
                        {
                            var newContentHeight = view.ContentHeight;

                            if (newContentHeight == 0 || customWebViewRenderer == null) return;
                            var element = customWebViewRenderer.Element;
                            element.HeightRequest = newContentHeight;
                        }
                        catch (System.Exception ex)
                        {
                            DependencyService.Get<ILog>().SaveLog("HtmlWebChromeClient", ex);
                        }
                    }, 225);
                }
                base.OnProgressChanged(view, newProgress);
            }

        }


    }
}