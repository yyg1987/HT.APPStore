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
    public partial class KbArticlesPage : ContentPage
    {
        KbArticlesViewModel ViewModel => vm ?? (vm = BindingContext as KbArticlesViewModel);
        KbArticlesViewModel vm;
        public KbArticlesPage()
        {
            InitializeComponent();
            BindingContext = new KbArticlesViewModel();

            var cancel = new ToolbarItem
            {
                Text = "搜索",
                Command = new Command(async () =>
                {
                    await NavigationService.PushAsync(Navigation, new KbArticlesSearchPage());
                })
            };
            ToolbarItems.Add(cancel);

            if (Device.Android == Device.RuntimePlatform)
                cancel.Icon = "toolbar_search.png";

            this.KbArticlesListView.ItemSelected += async delegate
            {
                var kbarticles = KbArticlesListView.SelectedItem as KbArticles;
                if (kbarticles == null)
                    return;

                var kbarticlesDetails = new KbArticlesDetailsPage(kbarticles);

                await NavigationService.PushAsync(Navigation, kbarticlesDetails);

                this.KbArticlesListView.SelectedItem = null;
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
                if (ViewModel.KbArticles.Count == 0)
                    ViewModel.RefreshCommand.Execute(null);
            }
        }
    }
}