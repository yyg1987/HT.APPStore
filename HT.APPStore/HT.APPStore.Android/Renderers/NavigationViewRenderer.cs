using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Views;
using Android.Widget;
using FFImageLoading;
using FFImageLoading.Transformations;
using FFImageLoading.Views;
using FormsToolkit;
using HT.APPStore.Droid.Renderers;
using HT.APPStore.Helpers;
using HT.APPStore.Helpers.Settings;
using HT.APPStore.Models;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(HT.APPStore.Controls.NavigationView),typeof(NavigationViewRenderer))]
namespace HT.APPStore.Droid.Renderers
{
    /// <summary>
    /// 重写 HT.APPStore.Controls.NavigationView
    /// </summary>
    public class NavigationViewRenderer : ViewRenderer<HT.APPStore.Controls.NavigationView, NavigationView>
    {
        NavigationView navView;
        private ImageViewAsync avatar;
        private TextView author;
        private TextView logout;
        public NavigationViewRenderer(Context context) : base(context)
        {

        }
        protected override void OnElementChanged(ElementChangedEventArgs<HT.APPStore.Controls.NavigationView> e)
        {
            base.OnElementChanged(e);
            if (e.OldElement != null || Element == null)
                return;
            var view = Inflate(this.Context, Resource.Layout.NavView, null);
            navView = view.JavaCast<NavigationView>();

            UserSettings.Current.PropertyChanged += SettingsPropertyChanged;

            navView.NavigationItemSelected += NavView_NavigationItemSelected;

            SetNativeControl(navView);

            var header = navView.GetHeaderView(0);
            avatar = header.FindViewById<ImageViewAsync>(Resource.Id.headerAvatar);
            author = header.FindViewById<TextView>(Resource.Id.headerAuthor);
            logout = header.FindViewById<TextView>(Resource.Id.headerLogout);

            avatar.Click += (sender, e2) => NavigateToLogin();
            logout.Click += (sender, e2) => NavigateToLogout();

            UpdateAvatar();

            navView.SetCheckedItem(Resource.Id.menu_blog);
        }
        public override void OnViewRemoved(Android.Views.View child)
        {
            base.OnViewRemoved(child);
            navView.NavigationItemSelected -= NavView_NavigationItemSelected;
            UserSettings.Current.PropertyChanged -= SettingsPropertyChanged;
        }
        IMenuItem previousItem;

        void NavView_NavigationItemSelected(object sender, NavigationView.NavigationItemSelectedEventArgs e)
        {
            if (previousItem != null)
                previousItem.SetChecked(false);

            navView.SetCheckedItem(e.MenuItem.ItemId);

            previousItem = e.MenuItem;

            int id = 0;
            switch (e.MenuItem.ItemId)
            {
                case Resource.Id.menu_blog:
                    id = (int)AppPage.Articles;
                    break;
                case Resource.Id.menu_news:
                    id = (int)AppPage.News;
                    break;
                case Resource.Id.menu_kbarticles:
                    id = (int)AppPage.KbArticles;
                    break;
                case Resource.Id.menu_statuses:
                    id = (int)AppPage.Statuses;
                    break;
                case Resource.Id.menu_question:
                    id = (int)AppPage.Questions;
                    break;
                case Resource.Id.menu_setting:
                    id = (int)AppPage.Setting;
                    break;
                case Resource.Id.menu_about:
                    id = (int)AppPage.About;
                    break;
            }
            this.Element.OnNavigationItemSelected(new HT.APPStore.Controls.NavigationItemSelectedEventArgs
            {
                Index = id
            });
        }

        void SettingsPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(UserSettings.Current.Avatar) || e.PropertyName == nameof(UserSettings.Current.DisplayName))
            {
                UpdateAvatar();
            }
        }

        void UpdateAvatar()
        {
            if (UserSettings.Current.Avatar == "")
            {
                avatar.SetImageDrawable(Context.GetDrawable(Resource.Drawable.avatar_placeholder));
            }
            else
            {
                ImageService.Instance.LoadUrl(UserSettings.Current.Avatar)
                   .Retry(3, 200)
                   .DownSample(200, 200)
                   .Transform(new CircleTransformation())
                   .LoadingPlaceholder("avatar_placeholder.png", FFImageLoading.Work.ImageSource.EmbeddedResource)
                   .ErrorPlaceholder("avatar_placeholder.png", FFImageLoading.Work.ImageSource.EmbeddedResource)
                   .Into(avatar);
            }
            if (UserSettings.Current.DisplayName == "")
            {
                logout.Visibility = ViewStates.Gone;
                author.Text = "点击头像登录";
            }
            else
            {
                author.Text = UserSettings.Current.DisplayName + (UserSettings.Current.Seniority != "" ? ("(园龄：" + UserSettings.Current.Seniority + ")") : "");
                logout.Visibility = ViewStates.Visible;
            }
        }

        void NavigateToLogin()
        {
            if (UserTokenSettings.Current.HasExpiresIn())
            {
                MessagingService.Current.SendMessage(MessageKeys.NavigateLogin);
            }
            else
            {
                MessagingService.Current.SendMessage(MessageKeys.NavigateAccount);
            }
        }
        void NavigateToLogout()
        {
            UserSettings.Current.UpdateUser(new User());
            UserTokenSettings.Current.UpdateUserToken(new Token() { RefreshTime = DateTime.MinValue });
        }
    }
}