using HT.APPStore.Helpers.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HT.APPStore.Views.About
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingWeibaPage : ContentPage
    {
        public SettingWeibaPage()
        {
            InitializeComponent();
            var cancel = new ToolbarItem
            {
                Text = "默认小尾巴",
                Command = new Command(() =>
                {
                    editorContent.Text = AboutSettings.DefaultWeibaContent;
                })
            };
            ToolbarItems.Add(cancel);
            editorContent.Text = AboutSettings.Current.WeibaContent;

            editorContent.TextChanged += (object sender, TextChangedEventArgs e) =>
            {
                AboutSettings.Current.WeibaContent = e.NewTextValue;
            };
        }
    }
}