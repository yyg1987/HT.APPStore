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
    public class QuestionsViewModel : ViewModelBase
    {
        public ObservableRangeCollection<Questions> Questions { get; } = new ObservableRangeCollection<Questions>();
        public DateTime NextRefreshTime { get; set; }
        private int pageIndex = 1;
        private int pageSize = 20;
        private int position = 1;
        public QuestionsViewModel(int position = 0)
        {
            this.position = position;
            NextRefreshTime = DateTime.Now.AddMinutes(15);
            CanLoadMore = false;
            //判断有没有登录
            if (position == 4 && UserTokenSettings.Current.HasExpiresIn())
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
                        if (position == 4 && UserTokenSettings.Current.HasExpiresIn())
                        {
                            //判断有没有登录
                            LoadStatus = LoadMoreStatus.StausNologin;
                            if (Questions.Count > 0)
                                Questions.Clear();
                        }
                        else
                        {
                            await ExecuteRefreshCommandAsync();
                        }
                    });
                }
                catch (Exception ex)
                {
                    Log.SendLog("QuestionsViewModel.RefreshCommand:" + ex.Message);
                    if (Questions.Count > 0)
                        Questions.Clear();
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
            var result = await StoreManager.QuestionsService.GetQuestionsAsync(position, pageIndex, pageSize);
            if (result.Success)
            {
                var questions = JsonConvert.DeserializeObject<List<Questions>>(result.Message.ToString());
                if (questions.Count > 0)
                {
                    if (pageIndex == 1 && Questions.Count > 0)
                        Questions.Clear();
                    Questions.AddRange(questions);
                    pageIndex++;
                    if (Questions.Count >= pageSize)
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
                Log.SendLog("QuestionsViewModel.GetQuestionsAsync:" + result.Message);
                LoadStatus = pageIndex > 1 ? LoadMoreStatus.StausError : LoadMoreStatus.StausFail;
            }
        }
        public async Task<bool> ExecuteQuestionsEditCommandAsync(Questions questions)
        {
            var result = await StoreManager.QuestionsService.EditQuestionsAsync(questions);
            if (result.Success)
            {
                Toast.SendToast(questions.Qid > 0 ? "修改问题成功" : "提问成功");
            }
            else
            {
                Log.SendLog("QuestionsViewModel.EditQuestionsAsync:" + result.Message);
                Toast.SendToast(result.Message.ToString());
            }
            return result.Success;
        }

        public void EditQuestions(Questions questions)
        {
            var book = Questions.Where(b => b.Qid == questions.Qid).FirstOrDefault();
            if (book == null)
            {
                if (position == 0 || position == 4)
                {
                    Questions.Insert(0, questions);
                }
            }
            else
            {
                var index = Questions.IndexOf(book);
                Questions[index] = questions;
                Questions[index].TagsDisplay = questions.TagsDisplay;
            }
            if (LoadStatus == LoadMoreStatus.StausNodata)
                LoadStatus = LoadMoreStatus.StausEnd;
        }

    }
}
