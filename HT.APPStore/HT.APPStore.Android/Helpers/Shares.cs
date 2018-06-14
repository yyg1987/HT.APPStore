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
using HT.APPStore.Droid.Helpers;
using HT.APPStore.Interfaces;

[assembly: Xamarin.Forms.Dependency(typeof(Shares))]
namespace HT.APPStore.Droid.Helpers
{
    public class Shares : IShares
    {
        static Activity activity;
        public static void Init(Activity a)
        {
            activity = a;
        }
        void IShares.Shares(string url, string title)
        {
            var sharesWidget = new SharesWidget(activity);
            sharesWidget.Open(url, title);
        }
        void IShares.SharesIcon(string url, string title, object icon)
        {
            var sharesWidget = new SharesWidget(activity);
            sharesWidget.Open(url, title, icon);
        }

    }
}