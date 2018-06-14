using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Text;
using Android.Views;
using Android.Widget;

namespace HT.APPStore.Droid.Helpers
{
    public class HtmlUtils
    {
        /// <summary>
        /// 使用txtview显示html代码
        /// </summary>
        /// <param name="html">html代码</param>
        /// <param name="flags">换行符，根据api版本设置显示</param>
        /// <returns></returns>
        public static ISpanned GetHtml(string html, FromHtmlOptions flags = FromHtmlOptions.ModeLegacy)
        {
            if (Build.VERSION.SdkInt >= BuildVersionCodes.N)
            {
                return Html.FromHtml(html, flags);
            }
            else
            {
                return Html.FromHtml(html);
            }
        }
    }
}