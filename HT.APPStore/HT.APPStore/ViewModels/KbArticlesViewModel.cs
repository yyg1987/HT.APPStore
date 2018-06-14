﻿using HT.APPStore.Helpers;
using HT.APPStore.Models;
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
    public class KbArticlesViewModel : ViewModelBase
    {
        public ObservableRangeCollection<KbArticles> KbArticles { get; } = new ObservableRangeCollection<KbArticles>();
        public DateTime NextRefreshTime { get; set; }
        private int pageIndex = 1;
        public KbArticlesViewModel()
        {
            NextRefreshTime = DateTime.Now.AddMinutes(15);
            CanLoadMore = false;
        }
        ICommand refreshCommand;
        public ICommand RefreshCommand =>
            refreshCommand ?? (refreshCommand = new Command(async () =>
            {
                try
                {
                    IsBusy = true;
                    NextRefreshTime = DateTime.Now.AddMinutes(15);
                    CanLoadMore = false;
                    pageIndex = 1;
                    await Task.Run(async () =>
                    {
                        await ExecuteRefreshCommandAsync();
                    });
                }
                catch (Exception ex)
                {
                    Log.SendLog("KbArticlesViewModel.RefreshCommand:" + ex.Message);
                    if (KbArticles.Count > 0)
                        KbArticles.Clear();
                    LoadStatus = LoadMoreStatus.StausFail;
                }
                finally
                {
                    IsBusy = false;
                }
            }));


        LoadMoreStatus loadStatus;
        public LoadMoreStatus LoadStatus
        {
            get { return loadStatus; }
            set { SetProperty(ref loadStatus, value); }
        }
        ICommand loadMoreCommand;
        public ICommand LoadMoreCommand =>
            loadMoreCommand ?? (loadMoreCommand = new Command(async () =>
            {
                try
                {
                    LoadStatus = LoadMoreStatus.StausLoading;
                    await ExecuteRefreshCommandAsync();
                }
                catch (Exception)
                {
                    LoadStatus = LoadMoreStatus.StausError;
                }
            }));
        async Task ExecuteRefreshCommandAsync()
        {
            var result = await StoreManager.KbArticlesService.GetKbArticlesAsync(pageIndex);
            if (result.Success)
            {
                var kbArticles = JsonConvert.DeserializeObject<List<KbArticles>>(result.Message.ToString());
                if (kbArticles.Count > 0)
                {
                    if (pageIndex == 1 && KbArticles.Count > 0)
                        KbArticles.Clear();
                    KbArticles.AddRange(kbArticles);
                    pageIndex++;
                    LoadStatus = LoadMoreStatus.StausDefault;
                    CanLoadMore = true;
                }
                else
                {
                    CanLoadMore = false;
                    LoadStatus = pageIndex > 1 ? LoadMoreStatus.StausEnd : LoadMoreStatus.StausNodata;
                }
            }
            else
            {
                Log.SendLog("KbArticlesViewModel.GetKbArticlesAsync:" + result.Message);
                LoadStatus = pageIndex > 1 ? LoadMoreStatus.StausError : LoadMoreStatus.StausFail;
            }
        }

    }
}
