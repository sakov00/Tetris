﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Tetris.Containers;

namespace Tetris
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            StartupUri = new Uri("/Tetris; component/Views/MainWindow.xaml", UriKind.Relative);
            AutofacConfig.ConfigureContainer();
        }
    }
}