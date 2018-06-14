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
using Plugin.CurrentActivity;
using Xamarin.Forms;

[assembly: Xamarin.Forms.Dependency(typeof(VersionNameer))]
namespace HT.APPStore.Droid.Helpers
{
    public class VersionNameer : IVersionName
    {
        public string GetVersionName()
        {
            try
            {

                var context = CrossCurrentActivity.Current.Activity ?? Android.App.Application.Context;
                var packageInfo = context.PackageManager.GetPackageInfo(context.PackageName, 0);
                return packageInfo.VersionName;
            }
            catch (System.Exception ex)
            {
                DependencyService.Get<ILog>().SendLog("VersionNameer：" + ex.Message);
            }
            return "";
        }
    }
}