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
    public class ArticlesViewModel : ViewModelBase
    {
        public ObservableRangeCollection<Articles> Articles { get; } = new ObservableRangeCollection<Articles>();
        public DateTime NextRefreshTime { get; set; }
        private int pageIndex = 1;
        private int pageSize = 20;
        private int position = 0;

        public ArticlesViewModel(int position)
        {
            this.position = position;
            NextRefreshTime = DateTime.Now.AddMinutes(15);
        }
        ICommand refreshCommand;
        public ICommand RefreshCommand =>
            refreshCommand ?? (refreshCommand = new Command(async () =>
            {
                try
                {
                    NextRefreshTime = DateTime.Now.AddMinutes(15);
                    IsBusy = true;
                    CanLoadMore = false;
                    pageIndex = 1;
                    await Task.Run(async () =>
                    {
                        await ExecuteRefreshCommandAsync();
                    });
                }
                catch (Exception ex)
                {
                    Log.SendLog("ArticlesViewModel.RefreshCommand:" + ex.Message);
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
        public ICommand LoadMoreCommand => loadMoreCommand ?? (loadMoreCommand = new Command(async () =>
        {
            try
            {
                await ExecuteRefreshCommandAsync();
            }
            catch (Exception ex)
            {
                Log.SendLog("ArticlesViewModel.LoadMoreCommand:" + ex.Message);
                LoadStatus = LoadMoreStatus.StausError;
            }
        }));

        async Task ExecuteRefreshCommandAsync()
        {
            var result = await StoreManager.ArticlesService.GetArticlesAsync(position, pageIndex, pageSize);
            if (result.Success)
            {
                var articles = JsonConvert.DeserializeObject<List<Articles>>(result.Message.ToString());
                if (articles.Count > 0)
                {
                    try
                    {
                        if (pageIndex == 1 && Articles.Count > 0)
                            Articles.Clear();
                        Articles.AddRange(articles);
                        pageIndex++;
                    }
                    catch (Exception ex)
                    {
                        Log.SendLog("ArticlesViewModel.ExecuteRefreshCommandAsync:" + ex.Message);
                    }
                    if (Articles.Count >= pageSize)
                    {
                        LoadStatus = LoadMoreStatus.StausDefault;
                        CanLoadMore = true;
                    }
                    else
                    {
                        LoadStatus = LoadMoreStatus.StausEnd;
                        CanLoadMore = false;
                    }
                }
                else
                {
                    CanLoadMore = false;
                    LoadStatus = pageIndex > 1 ? LoadMoreStatus.StausEnd : LoadMoreStatus.StausNodata;
                }
            }
            else
            {
                Log.SendLog("ArticlesViewModel.GetArticlesAsync:" + result.Message);
                LoadStatus = pageIndex > 1 ? LoadMoreStatus.StausError : LoadMoreStatus.StausFail;
            }
        }
    }
}
