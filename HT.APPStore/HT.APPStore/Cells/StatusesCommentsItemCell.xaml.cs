using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HT.APPStore.Cells
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class StatusesCommentsItemCell : ContentView
	{
		public StatusesCommentsItemCell ()
		{
			InitializeComponent ();
		}
        public static readonly BindableProperty DeleteCommandProperty =
            BindableProperty.Create(nameof(DeleteCommand), typeof(ICommand), typeof(StatusesCommentsItemCell), default(ICommand));

        public ICommand DeleteCommand
        {
            get { return GetValue(DeleteCommandProperty) as Command; }
            set { SetValue(DeleteCommandProperty, value); }
        }

    }
}