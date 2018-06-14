using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Xam.Plugin.WebView.Droid;
using FormsToolkit.Droid;
using FFImageLoading.Forms.Droid;
using HT.APPStore.Droid.Helpers;
using Com.Umeng.Socialize;
using Android.Content;

namespace HT.APPStore.Droid
{
    [Activity(Label = "@string/AppName",
        Exported = true,
        Icon = "@drawable/ic_launcher",
        LaunchMode = LaunchMode.SingleTask,
        ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);

            FormsWebViewRenderer.Initialize();
            Toolkit.Init();
            CachedImageRenderer.Init(true);
            Shares.Init(this);

            global::Xamarin.Forms.Forms.Init(this, bundle);
            LoadApplication(new HT.APPStore.App());
        }
        protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            UMShareAPI.Get(this).OnActivityResult(requestCode, (int)resultCode, data);
        }

    }
}

