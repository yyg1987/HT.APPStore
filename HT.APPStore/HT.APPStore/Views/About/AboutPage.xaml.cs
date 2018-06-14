using HT.APPStore.Interfaces;
using HT.APPStore.ViewModels;
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
    public partial class AboutPage : ContentPage
    {
        AboutViewModel vm;
        public AboutPage()
        {
            InitializeComponent();
            BindingContext = vm = new AboutViewModel();
            var cancel = new ToolbarItem
            {
                Text = "分享",
                Command = new Command(() =>
                {
                    DependencyService.Get<IShares>().Shares("https://github.com/JoesWeek/XamCnblogs", "博客园第三方客户端，Xamarin.Forms App，支持IOS，Android");
                })
            };
            ToolbarItems.Add(cancel);

            if (Device.Android == Device.RuntimePlatform)
                cancel.Icon = "toolbar_share.png";

            this.VersionName.Text = DependencyService.Get<IVersionName>().GetVersionName();
        }
    }
}