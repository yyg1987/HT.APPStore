using HT.APPStore.Helpers;
using Naxam.Controls.Forms;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace HT.APPStore.Views.Question
{
    public class QuestionsTopTabbedPage : TopTabbedPage
    {
        public QuestionsTopTabbedPage()
        {
            BarTextColor = (Color)Application.Current.Resources["NavigationText"];
            BarIndicatorColor = (Color)Application.Current.Resources["Divider"];
            BarBackgroundColor = (Color)Application.Current.Resources["Primary"];

            Title = "博问";
            Icon = "menu_question.png";

            this.Children.Add(new QuestionsPage() { Title = "待解决" });
            this.Children.Add(new QuestionsPage(1) { Title = "高分" });
            this.Children.Add(new QuestionsPage(2) { Title = "没有答案" });
            this.Children.Add(new QuestionsPage(3) { Title = "已解决" });
            this.Children.Add(new QuestionsPage(4) { Title = "我的问题" });

            var cancel = new ToolbarItem
            {
                Text = "搜索",
                Command = new Command(async () =>
                {
                    await NavigationService.PushAsync(Navigation, new QuestionsSearchPage());
                })
            };
            ToolbarItems.Add(cancel);

            if (Device.Android == Device.RuntimePlatform)
                cancel.Icon = "toolbar_search.png";
        }
    }
}
