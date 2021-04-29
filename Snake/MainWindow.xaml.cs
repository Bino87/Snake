﻿using System;
using System.Windows;
using DataAccessLibrary.Helpers.SQL;
using UserControls.Models;

namespace Snake
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainViewModel();
            SQLHelper.CreateStoredProcedures();
            Environment.Exit(1);
        }
    }
}
