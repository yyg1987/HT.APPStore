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
    public class SearchViewModel : ViewModelBase
    {
        public ObservableRangeCollection<Search> Searchs { get; } = new ObservableRangeCollection<Search>();
        private int pageIndex = 1;
        private int pageSize = 20;
        private int position = 0;

        public SearchViewModel(int position)
        {
            this.position = position;
        }
        LoadMoreStatus loadStatus;
        public LoadMoreStatus LoadStatus
        {
            get { return loadStatus; }
            set { SetProperty(ref loadStatus, value); }
        }
        string keyWords;
        public string KeyWords
        {
            get { return keyWords; }
            set
            {
                if (SetProperty(ref keyWords, value))
                    ExecuteSearchCommandAsync();
            }
        }
        ICommand refreshCommand;
        public ICommand RefreshCommand =>
            refreshCommand ?? (refreshCommand = new Command(async () =>
            {
                try
                {
                    IsBusy = true;
                    await Task.Run(async () =>
                    {
                        await ExecuteSearchCommandAsync();
                    });
                }
                catch (Exception ex)
                {
                    Log.SendLog("SearchViewModel.RefreshCommand:" + ex.Message);
                    LoadStatus = LoadMoreStatus.StausFail;
                }
                finally
                {
                    IsBusy = false;
                }
            }));

        ICommand loadMoreCommand;
        public ICommand LoadMoreCommand => loadMoreCommand ?? (loadMoreCommand = new Command(async () =>
        {
            try
            {
                await ExecuteRefreshCommandAsync();
            }
            catch (Exception ex)
            {
                Log.SendLog("SearchViewModel.LoadMoreCommand:" + ex.Message);
                LoadStatus = LoadMoreStatus.StausError;
            }
        }));

        async Task ExecuteRefreshCommandAsync()
        {
            var result = await StoreManager.SearchService.GetSearchAsync(position, KeyWords, pageIndex, pageSize);
            if (result.Success)
            {
                var articles = JsonConvert.DeserializeObject<List<Search>>(result.Message.ToString());
                if (articles.Count > 0)
                {
                    if (pageIndex == 1 && Searchs.Count > 0)
                        Searchs.Clear();
                    Searchs.AddRange(articles);
                    pageIndex++;
                    if (Searchs.Count >= pageSize)
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
                Log.SendLog("SearchViewModel.GetSearchAsync:" + result.Message);
                LoadStatus = pageIndex > 1 ? LoadMoreStatus.StausError : LoadMoreStatus.StausFail;
            }
        }

        async Task ExecuteSearchCommandAsync()
        {
            pageIndex = 1;
            CanLoadMore = false;
            if (KeyWords != "")
            {
                await ExecuteRefreshCommandAsync();
            }
            else
            {
                if (Searchs.Count > 0)
                    Searchs.Clear();
                LoadStatus = LoadMoreStatus.StausNodata;
            }
        }
    }
}
