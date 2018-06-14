using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using HT.APPStore.Droid.Renderers;
using HT.APPStore.Interfaces;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(HT.APPStore.Controls.HtmlTextView), typeof(HtmlTextViewRenderer))]

namespace HT.APPStore.Droid.Renderers
{
    public class HtmlTextViewRenderer : ViewRenderer<HT.APPStore.Controls.HtmlTextView, TextView>
    {
        private Org.Sufficientlysecure.Htmltextview.HtmlTextView htmlTextView;
        public HtmlTextViewRenderer(Context context) : base(context)
        {

        }
        protected override void OnElementChanged(ElementChangedEventArgs<HT.APPStore.Controls.HtmlTextView> e)
        {
            base.OnElementChanged(e);
            if (e.NewElement != null)
            {
                try
                {
                    if (htmlTextView == null)
                        htmlTextView = new Org.Sufficientlysecure.Htmltextview.HtmlTextView(Android.App.Application.Context);
                    if (this.Element.FontSize > 0)
                        htmlTextView.TextSize = float.Parse(this.Element.FontSize.ToString());
                    if (this.Element.TextColor != new Color())
                        htmlTextView.SetTextColor(this.Element.TextColor.ToAndroid());

                    var textView = (HT.APPStore.Controls.HtmlTextView)Element;
                    var lineSpacing = textView.LineSpacing;
                    var maxLines = textView.MaxLines;
                    if (lineSpacing != 1)
                    {
                        htmlTextView.SetLineSpacing(1f, (float)lineSpacing);
                    }
                    if (maxLines > 1)
                    {
                        htmlTextView.SetMaxLines(maxLines);
                        htmlTextView.Ellipsize = global::Android.Text.TextUtils.TruncateAt.End;
                    }
                    SetNativeControl(htmlTextView);
                    this.UpdateNativeControl();
                }
                catch (System.Exception ex)
                {
                    DependencyService.Get<ILog>().SaveLog("HtmlTextViewRenderer", ex);
                }
            }
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            if (e.PropertyName == "Text")
            {
                this.UpdateNativeControl();
            }
        }
        private void UpdateNativeControl()
        {
            if (htmlTextView != null && this.Element.Text != null)
                htmlTextView.SetHtml(this.Element.Text, new Org.Sufficientlysecure.Htmltextview.HtmlHttpImageGetter(htmlTextView));
        }
    }
}