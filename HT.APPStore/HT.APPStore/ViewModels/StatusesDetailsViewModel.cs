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
    public class StatusesDetailsViewModel : ViewModelBase
    {
        public ObservableRangeCollection<StatusesComments> StatusesComments { get; } = new ObservableRangeCollection<StatusesComments>();
        Statuses statuses;
        public DateTime NextRefreshTime { get; set; }

        public Statuses Statuses
        {
            get { return statuses; }
            set { SetProperty(ref statuses, value); }
        }
        LoadMoreStatus loadStatus;
        public LoadMoreStatus LoadStatus
        {
            get { return loadStatus; }
            set { SetProperty(ref loadStatus, value); }
        }
        private string comment;
        public string CommentDisplay
        {
            get { return comment; }
            set { SetProperty(ref comment, value); }
        }
        public StatusesDetailsViewModel(Statuses statuses)
        {
            this.statuses = statuses;
            NextRefreshTime = DateTime.Now.AddMinutes(15);
            CanLoadMore = false;
            CommentDisplay = statuses.CommentCount > 0 ? statuses.CommentCount.ToString() : "评论";
        }
        ICommand refreshCommand;
        public ICommand RefreshCommand =>
            refreshCommand ?? (refreshCommand = new Command(async () =>
            {
                try
                {
                    NextRefreshTime = DateTime.Now.AddMinutes(15);
                    IsBusy = true;
                    await Task.Run(async () =>
                    {
                        var result = await StoreManager.StatusesCommentsService.GetCommentsAsync(statuses.Id);
                        if (result.Success)
                        {
                            var comments = JsonConvert.DeserializeObject<List<StatusesComments>>(result.Message.ToString());
                            if (comments.Count > 0)
                            {
                                if (StatusesComments.Count > 0)
                                    StatusesComments.Clear();
                                StatusesComments.AddRange(comments);
                                LoadStatus = LoadMoreStatus.StausEnd;
                            }
                            else
                            {
                                LoadStatus = LoadMoreStatus.StausNodata;
                            }
                        }
                        else
                        {
                            Log.SendLog("StatusesDetailsViewModel.GetCommentsAsync:" + result.Message);
                            LoadStatus = LoadMoreStatus.StausError;
                            if (StatusesComments.Count > 0)
                                StatusesComments.Clear();
                        }
                        CanLoadMore = false;
                    });
                }
                catch (Exception ex)
                {
                    Log.SendLog("StatusesDetailsViewModel.RefreshCommand:" + ex.Message);
                }
                finally
                {
                    IsBusy = false;
                }
            }));
        public async Task<bool> ExecuteCommentEditCommandAsync(int id, string content)
        {
            var result = await StoreManager.StatusesCommentsService.PostCommentAsync(id, content.ToString());
            if (result.Success)
            {
                Toast.SendToast("评论成功");
            }
            else
            {
                Log.SendLog("StatusesDetailsViewModel.PostCommentAsync:" + result.Message);
                Toast.SendToast(result.Message.ToString());
            }
            return result.Success;
        }
        ICommand deleteCommand;
        public ICommand DeleteCommand =>
            deleteCommand ?? (deleteCommand = new Command<StatusesComments>(async (comment) =>
            {
                var index = StatusesComments.IndexOf(comment);
                if (!StatusesComments[index].IsDelete)
                {
                    StatusesComments[index].IsDelete = true;
                    var result = await StoreManager.StatusesCommentsService.DeleteCommentAsync(comment.StatusId, comment.Id);
                    if (result.Success)
                    {
                        await Task.Delay(1000);
                        index = StatusesComments.IndexOf(comment);
                        StatusesComments.RemoveAt(index);
                        if (StatusesComments.Count == 0)
                            LoadStatus = LoadMoreStatus.StausNodata;
                        CommentDisplay = (Statuses.CommentCount - 1).ToString();
                    }
                    else
                    {
                        Log.SendLog("StatusesDetailsViewModel.DeleteCommentAsync:" + result.Message);
                        index = StatusesComments.IndexOf(comment);
                        StatusesComments[index].IsDelete = false;
                        Toast.SendToast("删除失败");
                    }
                }
            }));
        public void AddComment(StatusesComments comment)
        {
            StatusesComments.Add(comment);
            if (LoadStatus == LoadMoreStatus.StausNodata)
                LoadStatus = LoadMoreStatus.StausEnd;
            CommentDisplay = (Statuses.CommentCount + 1).ToString();
        }
    }
}
