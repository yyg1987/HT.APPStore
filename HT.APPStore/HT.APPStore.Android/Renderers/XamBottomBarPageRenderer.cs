using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using BottomBar.XamarinForms;
using BottomNavigationBar.Listeners;
using HT.APPStore.Controls;
using HT.APPStore.Droid.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(XamBottomBarPage), typeof(XamBottomBarPageRenderer))]

namespace HT.APPStore.Droid.Renderers
{
    public class XamBottomBarPageRenderer : BottomBar.Droid.Renderers.BottomBarPageRenderer, IOnTabClickListener
    {
        BottomNavigationBar.BottomBar _bottomBar;
        public XamBottomBarPageRenderer()
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<BottomBarPage> e)
        {
            base.OnElementChanged(e);
            if (e.NewElement != null)
            {
                _bottomBar = (BottomNavigationBar.BottomBar)this.GetChildAt(0);
                _bottomBar.SetOnTabClickListener(this);

            }
        }
        public new void OnTabSelected(int position)
        {
            var bottomBarPage = Element as XamBottomBarPage;
            bottomBarPage.CurrentPage = Element.Children[position];
            Element.Title = Element.Children[position].Title;
        }

        public new void OnTabReSelected(int position)
        {
        }
    }
}