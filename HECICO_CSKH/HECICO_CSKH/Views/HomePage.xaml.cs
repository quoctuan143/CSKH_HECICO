using HECICO_CSKH.Interface;
using HECICO_CSKH.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HECICO_CSKH.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomePage : ContentPage
    {
        HomeViewModel viewModel;
        Timer timer;
        int position = 0;
        public HomePage()
        {
            InitializeComponent();
            //DependencyService.Get<IStatusBar>().translucentStatusBar();
            NavigationPage.SetHasNavigationBar(this, false);
            BindingContext = viewModel = new HomeViewModel();
            timer = new Timer();
            timer.Interval = 2000;
            timer.Enabled = true;
            timer.Elapsed += Timer_Elapsed;
            timer.Start();
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                position += 1;
                viewImage.Position = position;
                if (viewImage.Position > 2)
                {
                    position = 0;
                    viewImage.Position = position;
                }
            });
             
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (viewModel.ListCustomer.Count  == 0 )
            {
                viewModel.LoadCommand.Execute(null);
            }    
        }
    }
}