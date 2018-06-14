using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.Widget;
using Android.Views;
//using Android.Widget;
using HT.APPStore.Controls;
using HT.APPStore.Droid.Renderers;
using Xamarin.Forms;

[assembly: ExportRenderer(typeof(LoadMoreListView), typeof(LoadMoreListViewRenderer))]

namespace HT.APPStore.Droid.Renderers
{
    public class LoadMoreListViewRenderer : Xamarin.Forms.Platform.Android.ListViewRenderer
    {
        public LoadMoreListViewRenderer(Context context) : base(context)
        {

        }
        protected override void OnElementChanged(Xamarin.Forms.Platform.Android.ElementChangedEventArgs<ListView> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement != null)
            {
                var aListView = (Android.Widget.ListView)Control;
                var _refresh = (SwipeRefreshLayout)aListView.Parent;
                if (_refresh != null)
                {
                    _refresh.SetColorSchemeResources(Resource.Color.primary);
                }
            }
        }
    }
}