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

namespace HT.APPStore.Views.Article
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ArticlesPage : ContentPage
    {
        ArticlesViewModel ViewModel => vm ?? (vm = BindingContext as ArticlesViewModel);
        ArticlesViewModel vm;
        public ArticlesPage(int position = 0) : base()
        {
            InitializeComponent();
            BindingContext = new ArticlesViewModel(position);
            this.ArticlesListView.ItemSelected += async delegate
            {
                var articles = ArticlesListView.SelectedItem as Articles;
                if (articles == null)
                    return;

                var articlesDetails = new ArticlesDetailsPage(articles);

                await NavigationService.PushAsync(Navigation, articlesDetails);
                this.ArticlesListView.SelectedItem = null;
            };
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
                if (ViewModel.Articles.Count == 0)
                    ViewModel.RefreshCommand.Execute(null);
            }
        }
    }
}