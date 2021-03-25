﻿using HECICO_CSKH.ViewModels.Search;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HECICO_CSKH.Views.Search
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TraCuuHoaDonDienTuPage : ContentPage
    {
        TraCuuHoaDonDienTuViewModel viewModel;
        public TraCuuHoaDonDienTuPage()
        {
            InitializeComponent();
            BindingContext = viewModel = new TraCuuHoaDonDienTuViewModel();
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (viewModel.ListCustomer.Count == 0)
            {
                viewModel.LoadCommand.Execute(null);
            }    
        }
    }
}