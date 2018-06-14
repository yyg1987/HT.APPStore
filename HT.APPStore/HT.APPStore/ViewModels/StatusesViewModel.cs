﻿using HT.APPStore.Helpers;
using HT.APPStore.Helpers.Settings;
using HT.APPStore.Models;
using MvvmHelpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace HT.APPStore.ViewModels
{
    public class StatusesViewModel : ViewModelBase
    {
        public ObservableRangeCollection<Statuses> Statuses { get; } = new ObservableRangeCollection<Statuses>();
        public DateTime NextRefreshTime { get; set; }
        private int pageIndex = 1;
        private int pageSize = 20;
        private int position = 0;
        public StatusesViewModel(int position = 0)
        {
            this.position = position;
            NextRefreshTime = DateTime.Now.AddMinutes(15);
            CanLoadMore = false;
            //判断有没有登录
            if (position > 0 && UserTokenSettings.Current.HasExpiresIn())
            {
                LoadStatus = LoadMoreStatus.StausNologin;
            }
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
                        if (position > 0 && UserTokenSettings.Current.HasExpiresIn())
                        {
                            //判断有没有登录
                            LoadStatus = LoadMoreStatus.StausNologin;
                            if (Statuses != null && Statuses.Count > 0)
                                Statuses.Clear();
                        }
                        else
                        {
                            await ExecuteRefreshCommandAsync();
                        }
                    });
                }
                catch (Exception ex)
                {
                    Log.SendLog("StatusesViewModel.RefreshCommand:" + ex.Message);
                    if (Statuses != null && Statuses.Count > 0)
                        Statuses.Clear();
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
            var result = await StoreManager.StatusesService.GetStatusesAsync(position, pageIndex, pageSize);
            if (result.Success)
            {
                var statuses = JsonConvert.DeserializeObject<List<Statuses>>(result.Message.ToString());
                if (statuses.Count > 0)
                {
                    if (pageIndex == 1 && Statuses.Count > 0)
                        Statuses.Clear();
                    Statuses.AddRange(statuses);
                    pageIndex++;
                    if (Statuses.Count >= pageSize)
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
                Log.SendLog("StatusesViewModel.GetStatusesAsync:" + result.Message);
                LoadStatus = pageIndex > 1 ? LoadMoreStatus.StausError : LoadMoreStatus.StausFail;
            }
        }

        public async Task<bool> ExecuteStatusesEditCommandAsync(Statuses statuses)
        {
            var result = await StoreManager.StatusesService.EditStatusesAsync(statuses);
            if (result.Success)
            {
                Toast.SendToast(statuses.Id > 0 ? "修改闪存成功" : "发布成功");
            }
            else
            {
                Log.SendLog("StatusesViewModel.EditStatusesAsync:" + result.Message);
                Toast.SendToast(result.Message.ToString());
            }
            return result.Success;
        }
        public void EditStatuses(Statuses statuses)
        {
            var book = Statuses.Where(b => b.Id == statuses.Id).FirstOrDefault();
            if (book == null)
            {
                if (position == 0 || position == 2)
                {
                    Statuses.Insert(0, statuses);
                }
            }
            else
            {
                var index = Statuses.IndexOf(book);
                Statuses[index] = statuses;
            }
            if (LoadStatus == LoadMoreStatus.StausNodata)
                LoadStatus = LoadMoreStatus.StausEnd;
        }
        ICommand deleteCommand;
        public ICommand DeleteCommand =>
            deleteCommand ?? (deleteCommand = new Command<Statuses>(async (statuses) =>
            {
                var index = Statuses.IndexOf(statuses);
                if (!Statuses[index].IsDelete)
                {
                    Statuses[index].IsDelete = true;
                    var result = await StoreManager.StatusesService.DeleteStatusesAsync(statuses.Id);
                    if (result.Success)
                    {
                        await Task.Delay(1000);
                        index = Statuses.IndexOf(statuses);
                        Statuses.RemoveAt(index);
                        if (Statuses.Count == 0)
                            LoadStatus = LoadMoreStatus.StausNodata;
                    }
                    else
                    {
                        Log.SendLog("StatusesViewModel.DeleteStatusesAsync:" + result.Message);
                        index = Statuses.IndexOf(statuses);
                        Statuses[index].IsDelete = false;
                        Toast.SendToast("删除失败");
                    }
                }
            }));
    }
}
