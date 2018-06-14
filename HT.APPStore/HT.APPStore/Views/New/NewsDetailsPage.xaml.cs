﻿using FormsToolkit;
using HT.APPStore.Helpers;
using HT.APPStore.Helpers.Settings;
using HT.APPStore.Interfaces;
using HT.APPStore.Models;
using HT.APPStore.ViewModels;
using HT.APPStore.Views.Account;
using Rg.Plugins.Popup.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HT.APPStore.Views.New
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NewsDetailsPage : ContentPage
    {
        NewsDetailsViewModel ViewModel => vm ?? (vm = BindingContext as NewsDetailsViewModel);
        NewsDetailsViewModel vm;
        News news;
        public NewsDetailsPage(News news)
        {
            this.news = news;
            InitializeComponent();
            BindingContext = new NewsDetailsViewModel(news);

            var cancel = new ToolbarItem
            {
                Text = "分享",
                Command = new Command(() =>
                {
                    DependencyService.Get<IShares>().SharesIcon("https://news.cnblogs.com/n/" + news.Id + "/", news.Title, news.TopicIcon);
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
        void OnScrollComment(object sender, EventArgs args)
        {
            if (ViewModel.NewsComments.Count > 0)
                NewsDetailsView.ScrollTo(ViewModel.NewsComments.First(), ScrollToPosition.Start, false);
        }
        async void OnShowComment(object sender, EventArgs args)
        {
            if (UserTokenSettings.Current.HasExpiresIn())
            {
                MessagingService.Current.SendMessage(MessageKeys.NavigateLogin);
            }
            else
            {
                var page = new NewsCommentPopupPage(news, new Action<NewsComments>(OnResult));
                await Navigation.PushPopupAsync(page);
            }
        }
        private void OnResult(NewsComments result)
        {
            if (result != null)
            {
                ViewModel.AddComment(result);
                NewsDetailsView.ScrollTo(ViewModel.NewsComments.Last(), ScrollToPosition.Start, false);
            }
        }
        async void OnBookmarks(object sender, EventArgs args)
        {
            if (UserTokenSettings.Current.HasExpiresIn())
            {
                MessagingService.Current.SendMessage(MessageKeys.NavigateLogin);
            }
            else
            {
                var url = "https://news.cnblogs.com/n/" + news.Id + "/";
                await NavigationService.PushAsync(Navigation, new BookmarksEditPage(new Bookmarks() { Title = news.Title, LinkUrl = url, FromCNBlogs = true }));
            }
        }
        public ICommand EditCommand
        {
            get
            {
                return new Command(async (e) =>
                {
                    var page = new NewsCommentPopupPage(news, new Action<NewsComments>(OnResult), e as NewsComments);
                    await Navigation.PushPopupAsync(page);
                });
            }
        }
    }
}