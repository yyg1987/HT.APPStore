using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace HT.APPStore.Controls
{
    public class NavigationView : ContentView
    {
        public void OnNavigationItemSelected(NavigationItemSelectedEventArgs e)
        {
            NavigationItemSelected?.Invoke(this, e);
        }

        public event NavigationItemSelectedEventHandler NavigationItemSelected;

    }
    public class NavigationItemSelectedEventArgs : EventArgs
    {
        public int Index { get; set; }
    }

    public delegate void NavigationItemSelectedEventHandler(object sender, NavigationItemSelectedEventArgs e);
}
