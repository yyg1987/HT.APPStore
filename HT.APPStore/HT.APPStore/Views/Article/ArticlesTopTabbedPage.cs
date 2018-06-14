using HT.APPStore.Helpers;
using HT.APPStore.Views.KbArticle;
using Naxam.Controls.Forms;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace HT.APPStore.Views.Article
{
    public class ArticlesTopTabbedPage : TopTabbedPage
    {
        public ArticlesTopTabbedPage()
        {
            BarTextColor = (Color)Application.Current.Resources["NavigationText"];
            BarIndicatorColor = (Color)Application.Current.Resources["Divider"];
            BarBackgroundColor = (Color)Application.Current.Resources["Primary"];

            Title = "首页";
            Icon = "menu_blog.png";
            this.Children.Add(new ArticlesPage() { Title = "博客" });
            this.Children.Add(new ArticlesPage(1) { Title = "精华" });
            this.Children.Add(new KbArticlesPage() { Title = "知识库" });
            var cancel = new ToolbarItem
            {
                Text = "搜索",
                Command = new Command(async () =>
                {
                    await NavigationService.PushAsync(Navigation, new ArticlesSearchPage());
                })
            };
            ToolbarItems.Add(cancel);

            if (Device.Android == Device.RuntimePlatform)
                cancel.Icon = "toolbar_search.png";
        }
    }
}
