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
using Com.Umeng.Weixin.Callback;

namespace HT.APPStore.Droid.wxapi
{
    [Activity(Name = "com.HT.APPStore.wxapi.WXEntryActivity", Exported = true, Theme = "@android:style/Theme.Translucent.NoTitleBar", ConfigurationChanges = Android.Content.PM.ConfigChanges.KeyboardHidden | Android.Content.PM.ConfigChanges.Orientation | Android.Content.PM.ConfigChanges.ScreenSize, ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class WXEntryActivity : WXCallbackActivity
    {
    }
}