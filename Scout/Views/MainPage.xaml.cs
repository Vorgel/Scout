﻿using System;

using Scout.ViewModels;

using Windows.UI.Xaml.Controls;

namespace Scout.Views
{
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private MainViewModel ViewModel
        {
            get { return DataContext as MainViewModel; }
        }
    }
}
