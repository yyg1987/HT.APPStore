﻿using HT.APPStore.Helpers.Settings;
using HT.APPStore.Interfaces;
using HT.APPStore.Models;
using HT.APPStore.ViewModels;
using Rg.Plugins.Popup.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HT.APPStore.Views.Status
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StatusesEditPage : ContentPage
    {
        StatusesViewModel ViewModel => vm ?? (vm = BindingContext as StatusesViewModel);
        StatusesViewModel vm;
        Statuses statuses;
        ActivityIndicatorPopupPage popupPage;
        Action<Statuses> result;
        public StatusesEditPage(Statuses statuses) : base()
        {
            Init(statuses);
        }
        public StatusesEditPage(Statuses statuses, Action<Statuses> result) : base()
        {
            this.result = result;
            Init(statuses);
        }
        void Init(Statuses statuses)
        {
            this.statuses = statuses;
            InitializeComponent();
            BindingContext = new StatusesViewModel();
            if (statuses.Id > 0)
            {
                Title = "编辑闪存";
            }
            else
            {
                Title = "发布闪存";
            }
            var cancel = new ToolbarItem
            {
                Text = "保存",
                Command = new Command(async () =>
                {
                    await ExecuteStatusesEditAsync();
                })
            };
            ToolbarItems.Add(cancel);

            if (Device.Android == Device.RuntimePlatform)
                cancel.Icon = "menu_send.png";

            this.editorContent.Text = statuses.Content;
        }
        public async Task ExecuteStatusesEditAsync()
        {
            var toast = DependencyService.Get<IToast>();
            var content = this.editorContent.Text;
            if (content == null)
            {
                toast.SendToast("请输入内容");
            }
            else if (content.Length < 3)
            {
                toast.SendToast("内容最少要3个字.");
            }
            else
            {
                statuses.Content = content;
                statuses.DateAdded = DateTime.Now;
                statuses.UserId = UserSettings.Current.SpaceUserId;
                statuses.UserDisplayName = UserSettings.Current.DisplayName;
                statuses.UserIconUrl = UserSettings.Current.Avatar;
            };
            if (popupPage == null)
            {
                popupPage = new ActivityIndicatorPopupPage();
            }
            await Navigation.PushPopupAsync(popupPage);

            if (await ViewModel.ExecuteStatusesEditCommandAsync(statuses))
            {
                await Navigation.RemovePopupPageAsync(popupPage);
                if (result != null)
                    result.Invoke(statuses);
                Navigation.RemovePage(this);
            }
            else
            {
                await Navigation.RemovePopupPageAsync(popupPage);
            }
        }
    }
}