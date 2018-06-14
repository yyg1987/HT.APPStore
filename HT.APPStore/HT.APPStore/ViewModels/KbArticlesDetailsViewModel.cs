﻿using HT.APPStore.Models;
using MvvmHelpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace HT.APPStore.ViewModels
{
    public class KbArticlesDetailsViewModel : ViewModelBase
    {
        public ObservableRangeCollection<string> KbArticlesComments { get; } = new ObservableRangeCollection<string>();
        private KbArticles articles;
        public DateTime NextRefreshTime { get; set; }

        KbArticlesDetailsModel articlesDetailsModel;
        public KbArticlesDetailsModel KbArticlesDetails
        {
            get { return articlesDetailsModel; }
            set { SetProperty(ref articlesDetailsModel, value); }
        }
        public KbArticlesDetailsViewModel(KbArticles articles)
        {
            this.articles = articles;
            Title = articles.Title;
            NextRefreshTime = DateTime.Now.AddMinutes(15);
            KbArticlesDetails = new KbArticlesDetailsModel()
            {
                HasContent = false,
                DiggDisplay = articles.DiggCount > 0 ? articles.DiggCount.ToString() : "推荐",
                ViewDisplay = articles.ViewCount > 0 ? articles.ViewCount.ToString() : "阅读",
                DateDisplay = "发布于 " + articles.DateDisplay
            };
        }
        ICommand refreshCommand;
        public ICommand RefreshCommand =>
            refreshCommand ?? (refreshCommand = new Command(async () =>
            {
                try
                {
                    IsBusy = true;
                    NextRefreshTime = DateTime.Now.AddMinutes(15);
                    await Task.Run(async () =>
                    {
                        var result = await StoreManager.KbArticlesDetailsService.GetKbArticlesAsync(articles.Id);
                        if (result.Success)
                        {
                            articles.Body = JsonConvert.DeserializeObject<string>(result.Message.ToString());

                            KbArticlesDetails.Title = articles.Title;
                            KbArticlesDetails.Content = articles.BodyDisplay;
                            KbArticlesDetails.DiggDisplay = articles.DiggCount > 0 ? articles.DiggCount.ToString() : "推荐";
                            KbArticlesDetails.ViewDisplay = articles.ViewCount > 0 ? articles.ViewCount.ToString() : "阅读";
                            KbArticlesDetails.DateDisplay = "发布与 " + articles.DateDisplay;

                            KbArticlesDetails.HasError = false;
                            KbArticlesDetails.HasContent = true;
                        }
                        else
                        {
                            Log.SendLog("KbArticlesDetailsViewModel.GetKbArticlesAsync:" + result.Message);
                            KbArticlesDetails.HasError = true;
                            KbArticlesDetails.HasContent = false;
                        }
                    });
                }
                catch (Exception ex)
                {
                    Log.SendLog("KbArticlesDetailsViewModel.RefreshCommand:" + ex.Message);
                }
                finally
                {
                    IsBusy = false;
                }
            }));
        public class KbArticlesDetailsModel : BaseViewModel
        {
            string diggDisplay;
            public string DiggDisplay
            {
                get { return diggDisplay; }
                set { SetProperty(ref diggDisplay, value); }
            }
            string viewDisplay;
            public string ViewDisplay
            {
                get { return viewDisplay; }
                set { SetProperty(ref viewDisplay, value); }
            }
            string content;
            public string Content
            {
                get { return content; }
                set { SetProperty(ref content, value); }
            }
            string dateDisplay;
            public string DateDisplay
            {
                get { return dateDisplay; }
                set { SetProperty(ref dateDisplay, value); }
            }
            bool hasError;
            public bool HasError
            {
                get { return hasError; }
                set { SetProperty(ref hasError, value); }
            }
            bool hasContent;
            public bool HasContent
            {
                get { return hasContent; }
                set { SetProperty(ref hasContent, value); }
            }
        }
    }
}
