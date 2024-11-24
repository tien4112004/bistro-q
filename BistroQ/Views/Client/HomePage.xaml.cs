﻿using BistroQ.ViewModels.Client;
using Microsoft.UI.Xaml.Controls;


namespace BistroQ.Views.Client
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class HomePage : Page
    {
        public HomePageViewModel ViewModel { get; }

        public HomePage()
        {
            ViewModel = App.GetService<HomePageViewModel>();
            InitializeComponent();
        }
    }
}
