﻿using FormsToolkit;
using HT.APPStore.Helpers;
using HT.APPStore.Helpers.Settings;
using HT.APPStore.Interfaces;
using HT.APPStore.Models;
using HT.APPStore.ViewModels;
using HT.APPStore.Views.Account;
using Rg.Plugins.Popup.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HT.APPStore.Views.Question
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class QuestionsDetailsPage : ContentPage
    {
        QuestionsDetailsViewModel ViewModel => vm ?? (vm = BindingContext as QuestionsDetailsViewModel);
        QuestionsDetailsViewModel vm;
        Questions questions;
        public QuestionsDetailsPage(Questions questions)
        {
            this.questions = questions;
            InitializeComponent();
            BindingContext = new QuestionsDetailsViewModel(questions);

            var cancel = new ToolbarItem
            {
                Text = "分享",
                Command = new Command(() =>
                {
                    DependencyService.Get<IShares>().Shares("https://q.cnblogs.com/q/" + questions.Qid + "/", questions.Title);
                })
            };
            ToolbarItems.Add(cancel);

            if (Device.Android == Device.RuntimePlatform)
                cancel.Icon = "toolbar_share.png";

            this.QuestionsDetailsView.ItemSelected += async delegate
            {
                var answers = QuestionsDetailsView.SelectedItem as QuestionsAnswers;
                if (answers == null)
                    return;

                var answersDetails = new AnswersDetailsPage(answers);
                if (answers.AnswerID > 0)
                    await NavigationService.PushAsync(Navigation, answersDetails);
                this.QuestionsDetailsView.SelectedItem = null;
            };
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            UpdatePage();
        }

        private void UpdatePage()
        {
            bool forceRefresh = (DateTime.Now > (ViewModel?.NextRefreshTime ?? DateTime.Now));

            if (forceRefresh)
            {
                //刷新
                ViewModel.RefreshCommand.Execute(null);
            }
            else
            {
                //加载本地数据
                ViewModel.RefreshCommand.Execute(null);
            }
        }
        void OnTapped(object sender, EventArgs args)
        {
            ViewModel.RefreshCommand.Execute(null);
        }
        void OnScrollComment(object sender, EventArgs args)
        {
            if (ViewModel.QuestionAnswers.Count > 0)
                QuestionsDetailsView.ScrollTo(ViewModel.QuestionAnswers.Last(), ScrollToPosition.Start, false);
        }
        async void OnShowComment(object sender, EventArgs args)
        {
            if (UserTokenSettings.Current.HasExpiresIn())
            {
                MessagingService.Current.SendMessage(MessageKeys.NavigateLogin);
            }
            else
            {
                var page = new QuestionsAnswersPopupPage(questions, new Action<QuestionsAnswers>(OnResult));
                if (page != null && Navigation != null)
                    await Navigation.PushPopupAsync(page);
            }
        }
        private void OnResult(QuestionsAnswers result)
        {
            if (result != null)
            {
                ViewModel.EditComment(result);
                QuestionsDetailsView.ScrollTo(ViewModel.QuestionAnswers.Last(), ScrollToPosition.Start, false);
            }
        }
        async void OnBookmarks(object sender, EventArgs args)
        {
            if (UserTokenSettings.Current.HasExpiresIn())
            {
                MessagingService.Current.SendMessage(MessageKeys.NavigateLogin);
            }
            else
            {
                var url = "https://q.cnblogs.com/q/" + questions.Qid + "/";
                await NavigationService.PushAsync(Navigation, new BookmarksEditPage(new Bookmarks() { Title = questions.Title, LinkUrl = url, FromCNBlogs = true }));
            }
        }
        public ICommand EditCommand
        {
            get
            {
                return new Command(async (e) =>
                {
                    var page = new QuestionsAnswersPopupPage(questions, new Action<QuestionsAnswers>(OnResult), e as QuestionsAnswers);
                    await Navigation.PushPopupAsync(page);
                });
            }
        }
    }
}