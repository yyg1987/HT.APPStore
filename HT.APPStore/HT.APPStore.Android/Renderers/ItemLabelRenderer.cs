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
using HT.APPStore.Droid.Helpers;
using HT.APPStore.Droid.Renderers;
using HT.APPStore.Interfaces;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(HT.APPStore.Controls.ItemLabel), typeof(ItemLabelRenderer))]

namespace HT.APPStore.Droid.Renderers
{
    public class ItemLabelRenderer : LabelRenderer
    {
        public ItemLabelRenderer(Context context) : base(context)
        {

        }
        protected override void OnElementChanged(ElementChangedEventArgs<Label> e)
        {
            base.OnElementChanged(e);
            if (e.NewElement != null)
            {
                this.UpdateNativeControl();

                var itemLabel = (HT.APPStore.Controls.ItemLabel)Element;
                var lineSpacing = itemLabel.LineSpacing;
                var maxLines = itemLabel.MaxLines;

                this.Control.SetLineSpacing(1f, (float)lineSpacing);
                if (maxLines > 1)
                {
                    this.Control.SetMaxLines(maxLines);
                    this.Control.Ellipsize = global::Android.Text.TextUtils.TruncateAt.End;
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
            try
            {
                this.Control.SetText(HtmlUtils.GetHtml(Element.Text), Android.Widget.TextView.BufferType.Normal);
            }
            catch (System.Exception ex)
            {
                this.Control.Text = Element.Text;
                DependencyService.Get<ILog>().SaveLog("ItemLabelRenderer", ex);
            }
        }
    }
}