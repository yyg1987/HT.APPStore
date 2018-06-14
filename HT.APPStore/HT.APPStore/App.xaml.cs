using System;
using System.Threading.Tasks;
using FormsToolkit;
using HT.APPStore.Controls;
using HT.APPStore.Helpers;
using HT.APPStore.Helpers.Https;
using HT.APPStore.Helpers.Settings;
using HT.APPStore.Models;
using HT.APPStore.ViewModels;
using HT.APPStore.Views;
using HT.APPStore.Views.Account;
using HT.APPStore.Views.Article;
using HT.APPStore.Views.New;
using HT.APPStore.Views.Question;
using HT.APPStore.Views.Status;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Newtonsoft.Json;
using Xamarin.Forms;

namespace HT.APPStore
{
	public partial class App : Application
	{

		public App ()
		{
			InitializeComponent();

            AppCenter.Start("", typeof(Analytics), typeof(Crashes));
            ViewModelBase.Init();

            XamBottomBarPage bottomBarPage = new XamBottomBarPage() { Title = "和腾园" };
            bottomBarPage.BarTextColor = (Color)Application.Current.Resources["Primary"];
            bottomBarPage.FixedMode = true;
            bottomBarPage.BarTheme = XamBottomBarPage.BarThemeTypes.Light;

            bottomBarPage.Children.Add(new ArticlesTopTabbedPage());
            bottomBarPage.Children.Add(new NewsTopTabbedPage());
            //闪存
            bottomBarPage.Children.Add(new StatusesTopTabbedPage());
            bottomBarPage.Children.Add(new QuestionsTopTabbedPage());
            bottomBarPage.Children.Add(new AccountPage());

            MainPage = new NavigationPage(bottomBarPage);
            //switch (Device.RuntimePlatform)
            //{
            //    case Device.Android:
            //        MainPage = new Views.Android.RootPage();
            //        //MainPage = new Page1();
            //        break;
            //    case Device.iOS:
            //        break;
            //}
            //MainPage = new MainPage();

        }

        protected override void OnStart ()
		{
            // Handle when your app starts
            OnResume();
        }
        bool registered;

        protected override void OnSleep ()
		{
            // Handle when your app sleeps
            if (!registered)
                return;

            registered = false;
            MessagingService.Current.Unsubscribe(MessageKeys.NavigateLogin);
            MessagingService.Current.Unsubscribe(MessageKeys.NavigateToken);
            MessagingService.Current.Unsubscribe(MessageKeys.NavigateAccount);
        }

        protected async override void OnResume ()
		{
            // Handle when your app resumes
            await RefreshUserTokenAsync();

            if (registered)
                return;
            registered = true;

            MessagingService.Current.Subscribe(MessageKeys.NavigateLogin, async m =>
            {
                Page page = new NavigationPage(new AuthorizePage());

                var nav = Application.Current?.MainPage?.Navigation;
                if (nav == null)
                    return;

                await NavigationService.PushModalAsync(nav, page);
            });
            MessagingService.Current.Subscribe<string>(MessageKeys.NavigateToken, async (m, q) =>
            {
                var result = await TokenHttpClient.Current.PostTokenAsync(q);
                if (result.Success)
                {
                    var token = JsonConvert.DeserializeObject<Token>(result.Message.ToString());
                    token.RefreshTime = DateTime.Now;
                    UserTokenSettings.Current.UpdateUserToken(token);

                    var userResult = await UserHttpClient.Current.GetAsyn(Apis.Users);
                    if (userResult.Success)
                    {
                        var user = JsonConvert.DeserializeObject<User>(userResult.Message.ToString());

                        UserSettings.Current.UpdateUser(user);

                        var nav = Application.Current?.MainPage?.Navigation;
                        if (nav == null)
                            return;
                        await nav.PopModalAsync();
                    }
                }
            });
            MessagingService.Current.Subscribe(MessageKeys.NavigateAccount, async m =>
            {
                Page page = new NavigationPage(new AccountPage());

                var nav = Application.Current?.MainPage?.Navigation;
                if (nav == null)
                    return;

                await NavigationService.PushModalAsync(nav, page);
            });
        }
        private async Task RefreshUserTokenAsync()
        {
            if (UserTokenSettings.Current.UserRefreshToken != null)
            {
                var result = await UserHttpClient.Current.RefreshTokenAsync();
                if (result.Success)
                {
                    var token = JsonConvert.DeserializeObject<Token>(result.Message.ToString());
                    token.RefreshTime = DateTime.Now;
                    UserTokenSettings.Current.UpdateUserToken(token);

                    var userResult = await UserHttpClient.Current.GetAsyn(Apis.Users);
                    if (userResult.Success)
                    {
                        var user = JsonConvert.DeserializeObject<User>(userResult.Message.ToString());

                        UserSettings.Current.UpdateUser(user);

                    }
                }
            }
        }

    }
}
