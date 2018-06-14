using FormsToolkit;
using HT.APPStore.Helpers;
using HT.APPStore.Helpers.Settings;
using HT.APPStore.Interfaces;
using HT.APPStore.Models;
using HT.APPStore.ViewModels;
using HT.APPStore.Views.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HT.APPStore.Views.KbArticle
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class KbArticlesDetailsPage : ContentPage
    {
        KbArticlesDetailsViewModel ViewModel => vm ?? (vm = BindingContext as KbArticlesDetailsViewModel);
        KbArticlesDetailsViewModel vm;
        KbArticles kbArticles;
        public KbArticlesDetailsPage(KbArticles kbArticles)
        {
            this.kbArticles = kbArticles;
            InitializeComponent();
            BindingContext = new KbArticlesDetailsViewModel(kbArticles);

            var cancel = new ToolbarItem
            {
                Text = "分享",
                Command = new Command(() =>
                {
                    DependencyService.Get<IShares>().Shares("http://kb.cnblogs.com/page/" + kbArticles.Id + "/", kbArticles.Title);
                })
            };
            ToolbarItems.Add(cancel);

            if (Device.Android == Device.RuntimePlatform)
                cancel.Icon = "toolbar_share.png";
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            UpdatePage();
        }
        private void UpdatePage()
        {
            bool forceRefresh = (DateTime.Now > (ViewModel?.NextRefreshTime ?? DateTime.Now));

            if (forceRefresh)
            {
                //刷新
                ViewModel.RefreshCommand.Execute(null);
            }
            else
            {
                //加载本地数据
                ViewModel.RefreshCommand.Execute(null);
            }
        }
        void OnTapped(object sender, EventArgs args)
        {
            ViewModel.RefreshCommand.Execute(null);
        }
        async void OnBookmarks(object sender, EventArgs args)
        {
            if (UserTokenSettings.Current.HasExpiresIn())
            {
                MessagingService.Current.SendMessage(MessageKeys.NavigateLogin);
            }
            else
            {
                var url = "http://kb.cnblogs.com/page/" + kbArticles.Id + "/";
                await NavigationService.PushAsync(Navigation, new BookmarksEditPage(new Bookmarks() { Title = kbArticles.Title, LinkUrl = url, FromCNBlogs = true }));
            }
        }
    }
}