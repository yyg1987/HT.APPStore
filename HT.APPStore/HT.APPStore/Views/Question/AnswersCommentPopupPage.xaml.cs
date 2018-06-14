﻿using HT.APPStore.Helpers.Settings;
using HT.APPStore.Interfaces;
using HT.APPStore.Models;
using HT.APPStore.ViewModels;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HT.APPStore.Views.Question
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AnswersCommentPopupPage : PopupPage
    {
        AnswersDetailsViewModel ViewModel => vm ?? (vm = BindingContext as AnswersDetailsViewModel);
        AnswersDetailsViewModel vm;
        Action<AnswersComment> result;
        QuestionsAnswers answers;
        AnswersComment answersComment;
        public AnswersCommentPopupPage(QuestionsAnswers answers, Action<AnswersComment> result, AnswersComment answersComment = null)
        {
            this.answers = answers;
            this.result = result;
            this.answersComment = answersComment;
            InitializeComponent();
            BindingContext = new AnswersDetailsViewModel(answers);
            if (answersComment != null)
            {
                this.Comment.Text = answersComment.Content;
            }
            this.Comment.Focus();
        }
        private void OnClose(object sender, EventArgs e)
        {
            ClosePopupPage(null);
        }
        private void ClosePopupPage(string result)
        {
            if (result != null)
            {
                if (answersComment == null)
                {
                    answersComment = new AnswersComment();
                    answersComment.PostUserInfo = new QuestionUserInfo()
                    {
                        UserID = UserSettings.Current.SpaceUserId,
                        IconName = UserSettings.Current.Avatar,
                        UCUserID = UserSettings.Current.UserId,
                        UserName = UserSettings.Current.DisplayName,
                        QScore = UserSettings.Current.Score
                    };
                }
                answersComment.Content = result;
                answersComment.DateAdded = DateTime.Now;
                this.result.Invoke(answersComment);
            }
            PopupNavigation.PopAsync();
        }
        async void OnSendComment(object sender, EventArgs args)
        {
            var toast = DependencyService.Get<IToast>();
            var content = this.Comment.Text;
            if (content == null)
            {
                toast.SendToast("说点什么吧.");
            }
            else if (content.Length < 5)
            {
                toast.SendToast("多说一点吧.");
            }
            else
            {
                SendButton.IsRunning = true;
                if (answersComment == null)
                {
                    if (await ViewModel.ExecuteCommentPostCommandAsync(answers.Qid, answers.AnswerID, content))
                    {
                        SendButton.IsRunning = false;
                        ClosePopupPage(content);
                    }
                    else
                    {
                        SendButton.IsRunning = false;
                    }
                }
                else
                {
                    if (await ViewModel.ExecuteCommentEditCommentAsync(answers.Qid, answers.AnswerID, answersComment.CommentID, answersComment.PostUserID, content))
                    {
                        SendButton.IsRunning = false;
                        ClosePopupPage(content);
                    }
                    else
                    {
                        SendButton.IsRunning = false;
                    }
                }
            }
        }
    }
}