﻿using FormsToolkit;
using HT.APPStore.Controls;
using HT.APPStore.Models;
using HT.APPStore.Views.About;
using HT.APPStore.Views.Article;
using HT.APPStore.Views.KbArticle;
using HT.APPStore.Views.New;
using HT.APPStore.Views.Question;
using HT.APPStore.Views.Status;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace HT.APPStore.Views.Android
{
    public class RootPage : MasterDetailPage
    {
        Dictionary<int, XamNavigationPage> pages;
        DeepLinkPage page;
        bool isRunning = false;
        public RootPage()
        {
            pages = new Dictionary<int, XamNavigationPage>();
            Master = new MenuPage(this);

            pages.Add(0, new XamNavigationPage(new ArticlesTopTabbedPage()));

            Detail = pages[0];

            MessagingService.Current.Subscribe<DeepLinkPage>("DeepLinkPage", async (m, p) =>
            {
                page = p;

                if (isRunning)
                    await GoToDeepLink();
            });
        }

        public async Task NavigateAsync(int menuId)
        {
            XamNavigationPage newPage = null;
            if (!pages.ContainsKey(menuId))
            {
                switch (menuId)
                {
                    case (int)AppPage.Articles:
                        pages.Add(menuId, new XamNavigationPage(new ArticlesTopTabbedPage()));
                        break;
                    case (int)AppPage.News:
                        pages.Add(menuId, new XamNavigationPage(new NewsTopTabbedPage()));
                        break;
                    case (int)AppPage.KbArticles:
                        pages.Add(menuId, new XamNavigationPage(new KbArticlesPage()));
                        break;
                    case (int)AppPage.Statuses:
                        pages.Add(menuId, new XamNavigationPage(new StatusesTopTabbedPage()));
                        break;
                    case (int)AppPage.Questions:
                        pages.Add(menuId, new XamNavigationPage(new QuestionsTopTabbedPage()));
                        break;
                    case (int)AppPage.Setting:
                        pages.Add(menuId, new XamNavigationPage(new SettingPage()));
                        break;
                    case (int)AppPage.About:
                        pages.Add(menuId, new XamNavigationPage(new AboutPage()));
                        break;
                }
            }

            if (newPage == null)
                newPage = pages[menuId];

            if (newPage == null)
                return;

            if (Detail == newPage)
            {
                await newPage.Navigation.PopToRootAsync();
            }

            Detail = newPage;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            isRunning = true;

            await GoToDeepLink();

        }
        async Task GoToDeepLink()
        {
            if (page == null)
                return;
            var p = page.Page;
            var id = page.Id;
            page = null;
            switch (p)
            {
                //case AppPage.Sessions:
                //    await NavigateAsync((int)AppPage.Sessions);
                //    break;
            }

        }
    }
}
