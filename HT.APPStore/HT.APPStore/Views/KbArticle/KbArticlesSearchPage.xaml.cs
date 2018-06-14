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

namespace HT.APPStore.Views.KbArticle
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class KbArticlesSearchPage : ContentPage
    {
        SearchViewModel ViewModel => vm ?? (vm = BindingContext as SearchViewModel);
        SearchViewModel vm;
        public KbArticlesSearchPage() : base()
        {
            InitializeComponent();
            BindingContext = new SearchViewModel(2);
            this.SearchsListView.ItemSelected += async delegate
            {
                var search = SearchsListView.SelectedItem as Search;
                if (search == null)
                    return;
                var articles = new KbArticles()
                {
                    Author = search.UserName,
                    Body = "",
                    Summary = search.Content,
                    DiggCount = search.VoteTimes,
                    Id = int.Parse(search.Id),
                    DateAdded = search.PublishTime,
                    Title = search.Title.Replace("<strong>", "").Replace("</strong>", ""),
                    ViewCount = search.ViewTimes
                };

                var articlesDetails = new KbArticlesDetailsPage(articles);

                await NavigationService.PushAsync(Navigation, articlesDetails);
                this.SearchsListView.SelectedItem = null;
            };
        }
    }
}