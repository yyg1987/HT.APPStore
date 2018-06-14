using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace HT.APPStore.Helpers
{
    public static class NavigationService
    {
        static bool navigating;
        public static async Task PushAsync(INavigation navigation, Page page, bool animate = true)
        {
            if (navigating)
                return;

            navigating = true;
            await navigation.PushAsync(page, animate);
            navigating = false;
        }

        public static async Task PushModalAsync(INavigation navigation, Page page, bool animate = true)
        {
            if (navigating)
                return;

            navigating = true;
            await navigation.PushModalAsync(page, animate);
            navigating = false;
        }
    }
}
