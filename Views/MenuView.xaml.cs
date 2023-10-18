﻿using Mayuri.Models;
using Microsoft.Extensions.DependencyInjection;
using ScottPlot;
using ScottPlot.Plottable;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Mayuri.Views
{
    /// <summary>
    /// Interaction logic for MenuView.xaml
    /// </summary>
    public partial class MenuView : UserControl
    {
        public MenuView()
        {
            InitializeComponent();
            ILogList lgList = App.Current.Services.GetService<ILogList>();
            Plot plt = WeeklyLogs.Plot;
            ScottPlot.Control.Configuration cf = WeeklyLogs.Configuration;

            PlotLogList(lgList, plt, cf, 7); 


            WeeklyLogs.Refresh();
        }

        // Input: ILogList l, int dayPeriod
        // Output: two arrays containing the x and y axis for the given day period to be 
        //         set on a bar graph
        // Assumption: The logs are in order by datetime from most to least recent
        //
        // Process: For every log, check to see if the date is the same as the last one in the date list
        //          If it is, add the time to the last value in the other list, if not, add it to the end of the list along with the new datetime.
        // 
        // OR
        //
        // Input: ILogList l, Plot plot, Configuration conf, int dayPeriod
        // Output: void
        //         Adds bars to plot
        // Assumption: In order datetime from most to least
        // 
        // Process: load first values (time, genre) into the first two arrays, set current day
        //          for every next value:
        //          if the date is the same as the one in the list, put the values in and go next
        //          if not, add bar using the current arrays, then create a new array with the new


        private void PlotLogList(ILogList l, ScottPlot.Plot plot, ScottPlot.Control.Configuration conf, int dayPeriod)
        {
            Dictionary<string, Color> SourceColor = new Dictionary<string, Color>
            {
                { "Book", Color.FromArgb(255, 105, 97) },
                { "Anime", Color.FromArgb(255, 180, 128)},
                { "Manga", Color.FromArgb(248, 243, 141)},
                { "Visual Novel", Color.FromArgb(66, 214, 164) },
                { "Video Game", Color.FromArgb(8, 202, 209)},
                { "Reading", Color.FromArgb(89, 173, 246) },
                { "Listening", Color.FromArgb(157, 148, 255)},
                { "Other", Color.FromArgb(199, 128, 232)},
            };
            IEnumerable<Log> logEnum = l.GetAllLogs().Result;
            IEnumerator<Log> logs = logEnum.GetEnumerator();
            DateTime nowDate = DateTime.Today;
            DateTime oldestDate = nowDate.AddDays(dayPeriod * -1);
            double tallestBar = 10;

            plot.SetAxisLimitsX(oldestDate.AddDays(-1).ToOADate(), nowDate.AddDays(.5).ToOADate());
            plot.XAxis.DateTimeFormat(true);
            plot.YAxis2.SetSizeLimit(min: 0);
            conf.Pan = false;
            conf.Zoom = false;
            conf.ScrollWheelZoom = false;
            conf.MiddleClickDragZoom = false;
            plot.XAxis.Layout(padding: 0, maximumSize: 22);
            //plot.YAxis.Layout(padding: 0, maximumSize: 30);
            plot.XAxis.Label("");
            plot.YAxis.Label("Minutes");
            //plot.Style(Color.Transparent);
            plot.Style(figureBackground: Color.FromArgb(127, 0, 0, 0),grid: Color.FromArgb(127, 0, 0, 0) ,axisLabel: Color.White, tick: Color.White) ;
            plot.Style();


            if (logEnum == null || !logEnum.Any())
            {
                return;
            }

            BarSeries barSeries = plot.AddBarSeries();
            double lastBarTop = 0;
            logs.MoveNext();
            DateTime currentBarDate = logs.Current.LoggedAt.Date;
            while (currentBarDate < oldestDate)
            {
                logs.MoveNext();
                currentBarDate = logs.Current.LoggedAt.Date;
            }
            do
            {
                Log curLog = logs.Current;
                if (currentBarDate.Year != curLog.LoggedAt.Year ||
                    currentBarDate.Month != curLog.LoggedAt.Month ||
                    currentBarDate.Day != curLog.LoggedAt.Day)
                {
                    lastBarTop = 0;
                    currentBarDate = curLog.LoggedAt.Date;
                } 
                double barTop = lastBarTop + curLog.Duration;
                double barBottom = lastBarTop;
                lastBarTop += curLog.Duration;
                if (barTop > tallestBar)
                {
                    tallestBar = barTop;
                }

                barSeries.Bars.Add(new Bar()
                {
                    Value = barTop,
                    ValueBase = barBottom,
                    FillColor = SourceColor[$"{curLog.LogSource.Type}"],
                    LineColor = Color.Black,
                    LineWidth = 1,
                    Position = currentBarDate.ToOADate(),


                });
            } while(logs.MoveNext());
            plot.SetAxisLimitsY(0, tallestBar + 10);
        }
    }
}
