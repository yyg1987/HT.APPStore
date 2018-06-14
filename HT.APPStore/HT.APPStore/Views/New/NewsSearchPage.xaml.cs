using HT.APPStore.Helpers;
using HT.APPStore.Models;
using HT.APPStore.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HT.APPStore.Views.New
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NewsSearchPage : ContentPage
    {
        SearchViewModel ViewModel => vm ?? (vm = BindingContext as SearchViewModel);
        SearchViewModel vm;
        public NewsSearchPage() : base()
        {
            InitializeComponent();
            BindingContext = new SearchViewModel(1);
            this.SearchsListView.ItemSelected += async delegate
            {
                var search = SearchsListView.SelectedItem as Search;
                if (search == null)
                    return;

                var news = new News()
                {
                    IsHot = false,
                    IsRecommend = false,
                    TopicIcon = "",
                    TopicId = 0,
                    Body = "",
                    CommentCount = search.CommentTimes,
                    Summary = search.Content,
                    DiggCount = search.VoteTimes,
                    Id = int.Parse(search.Id),
                    DateAdded = search.PublishTime,
                    Title = search.Title.Replace("<strong>", "").Replace("</strong>", ""),
                    ViewCount = search.ViewTimes
                };
                var articlesDetails = new NewsDetailsPage(news);

                await NavigationService.PushAsync(Navigation, articlesDetails);
                this.SearchsListView.SelectedItem = null;
            };
        }
    }
}