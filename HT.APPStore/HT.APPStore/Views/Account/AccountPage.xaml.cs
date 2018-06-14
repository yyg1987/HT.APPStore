using HT.APPStore.Helpers.Settings;
using HT.APPStore.Interfaces;
using HT.APPStore.Views.About;
using Plugin.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HT.APPStore.Views.Account
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AccountPage : ContentPage
    {
        public AccountPage()
        {
            InitializeComponent();
            Title = "我";
            Icon = "menu_avatar.png";
            //var cancel = new ToolbarItem
            //{
            //    Text = "关闭",
            //    Command = new Command(async () =>
            //    {
            //        await Navigation.PopModalAsync();
            //    })
            //};
            //ToolbarItems.Add(cancel);

            //if (Device.Android == Device.RuntimePlatform)
            //    cancel.Icon = "toolbar_close.png";

            //UpdateUser();
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            UpdateUser();
        }
        /// <summary>
        /// 检验用户是否登录
        /// </summary>
        void UpdateUser()
        {
            //if (UserSettings.Current.Avatar == "")
            
            if(UserTokenSettings.Current.HasExpiresIn())
            {
                this.UserName.IsVisible = false;
                //园龄
                this.UserSeniority.IsVisible = false;
                this.LogoutLayout.IsVisible = false;
                this.Login.IsVisible = true;

                this.AvatarLayout.Source = "avatar_placeholder.png";
                this.UserName.Text = "";
                this.UserSeniority.Text = "";
            }
            else
            {
                this.UserName.IsVisible = true;
                this.UserSeniority.IsVisible = true;
                this.LogoutLayout.IsVisible = true;
                this.Login.IsVisible = false;
                //头像
                this.AvatarLayout.Source = UserSettings.Current.Avatar;
                this.UserName.Text = UserSettings.Current.DisplayName;
                this.UserSeniority.Text = "园龄：" + UserSettings.Current.Seniority;
            }
        }
        void OnLogin(object sender, EventArgs args)
        {
            if (UserTokenSettings.Current.HasExpiresIn())
            {
                Navigation.PushAsync(new AuthorizePage());
            }
        }
        void OnLogout(object sender, EventArgs args)
        {
            //注销登录
            if (!UserTokenSettings.Current.HasExpiresIn())
            {
                UserSettings.Current.UpdateUser(new HT.APPStore.Models.User());
                UserTokenSettings.Current.UpdateUserToken(new HT.APPStore.Models.Token() { ExpiresIn = 0 });

                UpdateUser();
            }
        }
        void OnBlog(object sender, EventArgs args)
        {
            if (UserTokenSettings.Current.HasExpiresIn())
            {
                Navigation.PushAsync(new AuthorizePage());
            }
            else
            {
                Navigation.PushAsync(new ArticlesPage(UserSettings.Current.BlogApp));
            }
        }
        void OnBookmarks(object sender, EventArgs args)
        {
            if (UserTokenSettings.Current.HasExpiresIn())
            {
                Navigation.PushAsync(new AuthorizePage());
            }
            else
            {
                Navigation.PushAsync(new BookmarksPage());
            }
        }
        void OnSetting(object sender, EventArgs args)
        {
            Navigation.PushAsync(new SettingPage());
        }
        void OnAbout(object sender, EventArgs args)
        {
            Navigation.PushAsync(new HT.APPStore.Views.About.AboutPage());
        }
        void OnEmail(object sender, EventArgs args)
        {
            var emailMessenger = CrossMessaging.Current.EmailMessenger;
            if (emailMessenger.CanSendEmail)
            {
                emailMessenger.SendEmail("476920650@qq.com", "来自 XamCnblogs - " + DependencyService.Get<IVersionName>().GetVersionName() + " 的客户端反馈", "");
            }
            else
            {
                DependencyService.Get<IToast>().SendToast("系统中没有安装邮件客户端");
            }
        }

    }
}