﻿using FormsToolkit;
using HT.APPStore.Helpers;
using HT.APPStore.Helpers.Settings;
using HT.APPStore.Models;
using HT.APPStore.ViewModels;
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
    public partial class QuestionsPage : ContentPage
    {
        QuestionsViewModel ViewModel => vm ?? (vm = BindingContext as QuestionsViewModel);
        QuestionsViewModel vm;
        public QuestionsPage(int position = 0) : base()
        {
            InitializeComponent();

            BindingContext = new QuestionsViewModel(position);

            var cancel = new ToolbarItem
            {
                Text = "添加",
                Command = new Command(async () =>
                {
                    if (UserTokenSettings.Current.HasExpiresIn())
                    {
                        MessagingService.Current.SendMessage(MessageKeys.NavigateLogin);
                    }
                    else
                    {
                        await NavigationService.PushAsync(Navigation, new QuestionsEditPage(new Questions(), new Action<Questions>(OnResult)));
                    }
                })
            };
            ToolbarItems.Add(cancel);

            if (Device.Android == Device.RuntimePlatform)
                cancel.Icon = "toolbar_add.png";


            this.QuestionsListView.ItemSelected += async delegate
            {
                var questions = QuestionsListView.SelectedItem as Questions;
                if (questions == null)
                    return;

                var questionsDetails = new QuestionsDetailsPage(questions);

                await NavigationService.PushAsync(Navigation, questionsDetails);
                this.QuestionsListView.SelectedItem = null;
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
                if (ViewModel.Questions.Count == 0)
                    ViewModel.RefreshCommand.Execute(null);
            }
        }

        private void OnResult(Questions result)
        {
            if (result != null)
            {
                ViewModel.EditQuestions(result);
                QuestionsListView.ScrollTo(ViewModel.Questions.FirstOrDefault(), ScrollToPosition.Start, false);
            }
        }
    }
}