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
    public partial class QuestionsAnswersPopupPage : PopupPage
    {
        QuestionsDetailsViewModel ViewModel => vm ?? (vm = BindingContext as QuestionsDetailsViewModel);
        QuestionsDetailsViewModel vm;
        Action<QuestionsAnswers> result;
        Questions questions;
        QuestionsAnswers questionsAnswers;
        public QuestionsAnswersPopupPage(Questions questions, Action<QuestionsAnswers> result, QuestionsAnswers questionsAnswers = null)
        {
            this.questions = questions;
            this.questionsAnswers = questionsAnswers;
            this.result = result;
            InitializeComponent();
            BindingContext = new QuestionsDetailsViewModel(questions);
            if (questionsAnswers != null)
            {
                this.Comment.Text = questionsAnswers.Answer;
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
                if (questionsAnswers == null)
                {
                    questionsAnswers = new QuestionsAnswers();
                    questionsAnswers.AnswerUserInfo = new QuestionUserInfo()
                    {
                        UserID = UserSettings.Current.SpaceUserId,
                        IconName = UserSettings.Current.Avatar,
                        UCUserID = UserSettings.Current.UserId,
                        UserName = UserSettings.Current.DisplayName,
                        QScore = UserSettings.Current.Score
                    };
                    questionsAnswers.Qid = questions.Qid;
                }
                questionsAnswers.Answer = result;
                questionsAnswers.DateAdded = DateTime.Now;
                this.result.Invoke(questionsAnswers);
            }
            PopupNavigation.PopAsync();
        }
        async void OnSendComment(object sender, EventArgs args)
        {
            var toast = DependencyService.Get<IToast>();
            var comment = this.Comment.Text;
            if (comment == null)
            {
                toast.SendToast("说点什么吧.");
            }
            else if (comment.Length < 5)
            {
                toast.SendToast("多说一点吧.");
            }
            else
            {
                SendButton.IsRunning = true;
                if (questionsAnswers == null)
                {
                    if (await ViewModel.ExecuteCommentPostCommandAsync(questions.Qid, comment))
                    {
                        SendButton.IsRunning = false;
                        ClosePopupPage(comment);
                    }
                    else
                    {
                        SendButton.IsRunning = false;
                    }
                }
                else
                {
                    if (await ViewModel.ExecuteCommentEditCommandAsync(questions.Qid, questionsAnswers.AnswerID, questionsAnswers.UserID, comment))
                    {
                        SendButton.IsRunning = false;
                        ClosePopupPage(comment);
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