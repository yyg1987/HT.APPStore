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
    public partial class NewsPage : ContentPage
    {
        NewsViewModel ViewModel => vm ?? (vm = BindingContext as NewsViewModel);
        NewsViewModel vm;
        public NewsPage(int position = 0) : base()
        {
            InitializeComponent();
            BindingContext = new NewsViewModel(position);
            this.NewsListView.ItemSelected += async delegate
            {
                var news = NewsListView.SelectedItem as News;
                if (news == null)
                    return;

                var newsDetails = new NewsDetailsPage(news);

                await NavigationService.PushAsync(Navigation, newsDetails);
                this.NewsListView.SelectedItem = null;
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
                if (ViewModel.News.Count == 0)
                    ViewModel.RefreshCommand.Execute(null);
            }
        }
    }
}