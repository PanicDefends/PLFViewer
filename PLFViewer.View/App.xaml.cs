﻿using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using PFLViewer.Model.Model.Interfaces;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace PLFViewer.View
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            DoLiveChartMappings();
        }

        private void DoLiveChartMappings()
        {
            LiveCharts.Configure(config =>
            {
                config.AddSkiaSharp()
                      .AddDefaultMappers()
                      .AddLightTheme()
                      .HasMap<IPointModel>((pointModel, chartPoint) =>
                      {
                          chartPoint.PrimaryValue = pointModel.Y;
                          chartPoint.SecondaryValue = pointModel.X;
                      });
            });
        }
    }
}
