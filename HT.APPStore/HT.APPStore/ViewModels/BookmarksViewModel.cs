﻿using HT.APPStore.Helpers;
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
    public class BookmarksViewModel : ViewModelBase
    {
        public ObservableRangeCollection<Bookmarks> Bookmarks { get; } = new ObservableRangeCollection<Bookmarks>();
        public DateTime NextRefreshTime { get; set; }
        private int pageIndex = 1;
        private int pageSize = 20;
        public BookmarksViewModel()
        {
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
                    Log.SendLog("BookmarksViewModel.RefreshCommand:" + ex.Message);
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
            catch (Exception)
            {
                LoadStatus = LoadMoreStatus.StausError;
            }
        }));

        async Task ExecuteRefreshCommandAsync()
        {
            var result = await StoreManager.BookmarksService.GetBookmarksAsync(pageIndex, pageSize);
            if (result.Success)
            {
                var articles = JsonConvert.DeserializeObject<List<Bookmarks>>(result.Message.ToString());
                if (articles.Count > 0)
                {
                    if (pageIndex == 1 && Bookmarks.Count > 0)
                        Bookmarks.Clear();
                    Bookmarks.AddRange(articles);
                    pageIndex++;
                    if (Bookmarks.Count >= pageSize)
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
                Log.SendLog("BookmarksViewModel.GetBookmarksAsync:" + result.Message);
                LoadStatus = pageIndex > 1 ? LoadMoreStatus.StausEnd : LoadMoreStatus.StausNodata;
            }
        }

        public async Task<bool> ExecuteBookmarkEditCommandAsync(Bookmarks bookmarks)
        {
            var result = await StoreManager.BookmarksService.EditBookmarkAsync(bookmarks);
            if (result.Success)
            {
                Toast.SendToast("收藏成功");
            }
            else
            {
                Log.SendLog("BookmarksViewModel.EditBookmarkAsync:" + result.Message);
                if (result.Message.ToString() == "Conflict")
                {
                    Toast.SendToast("收藏的网址已经存在了");
                }
                else
                {
                    Toast.SendToast(result.Message.ToString());
                }
            }
            return result.Success;
        }

        public void EditBookmark(Bookmarks bookmark)
        {
            var book = Bookmarks.Where(b => b.WzLinkId == bookmark.WzLinkId).FirstOrDefault();
            if (book == null)
            {
                Bookmarks.Insert(0, bookmark);
            }
            else
            {
                var index = Bookmarks.IndexOf(book);
                Bookmarks[index] = bookmark;
                Bookmarks[index].TagsDisplay = bookmark.TagsDisplay;
            }
            if (LoadStatus == LoadMoreStatus.StausNodata)
                LoadStatus = LoadMoreStatus.StausEnd;
        }

        ICommand deleteCommand;
        public ICommand DeleteCommand =>
            deleteCommand ?? (deleteCommand = new Command<Bookmarks>(async (bookmark) =>
            {
                var index = Bookmarks.IndexOf(bookmark);
                if (!Bookmarks[index].IsDelete)
                {
                    Bookmarks[index].IsDelete = true;
                    var result = await StoreManager.BookmarksService.DeleteBookmarkAsync(bookmark.WzLinkId);
                    if (result.Success)
                    {
                        await Task.Delay(1000);
                        index = Bookmarks.IndexOf(bookmark);
                        Bookmarks.RemoveAt(index);
                        if (Bookmarks.Count == 0)
                            LoadStatus = LoadMoreStatus.StausNodata;
                    }
                    else
                    {
                        Log.SendLog("BookmarksViewModel.DeleteBookmarkAsync:" + result.Message);
                        index = Bookmarks.IndexOf(bookmark);
                        Bookmarks[index].IsDelete = false;
                        Toast.SendToast("删除失败");
                    }
                }
            }));
    }
}
