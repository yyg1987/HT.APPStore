using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HT.APPStore.Views.Android
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MenuPage : ContentPage
    {
        RootPage root;
        public MenuPage(RootPage root)
        {
            this.root = root;
            InitializeComponent();

            NavView.NavigationItemSelected += async (sender, e) =>
            {
                this.root.IsPresented = false;

                await Task.Delay(225);
                await this.root.NavigateAsync(e.Index);
            };
        }
    }
}