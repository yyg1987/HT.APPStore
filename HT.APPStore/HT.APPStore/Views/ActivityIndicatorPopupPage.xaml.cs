﻿using Rg.Plugins.Popup.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HT.APPStore.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ActivityIndicatorPopupPage : PopupPage
    {
		public ActivityIndicatorPopupPage ()
		{
			InitializeComponent ();
            this.CloseWhenBackgroundIsClicked = false;

        }
        protected override bool OnBackButtonPressed()
        {
            return true;
        }

    }
}