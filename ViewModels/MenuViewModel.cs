﻿using Mayuri.Commands;
using Mayuri.DBContexts;
using Mayuri.Services;
using Mayuri.Stores;
using Mayuri.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Mayuri.ViewModels
{
    public class MenuViewModel : ViewModelBase
    {
        public ICommand NavigateStopwatchCommand { get; }
        public ICommand OpenAddSourceView { get; }
        public ICommand OpenCreateLogView { get; }
        public string TotalTime {  get; set; }
        public string TotalTimeDay {  get; set; }
        public string TotalTimeGivenPeriod {  get; set; }
        public string TimeAverageGivenPeriod { get; set; }
        public MenuViewModel(NavigationStore navigationStore)
        {
            NavigateStopwatchCommand = new NavigateCommand<ImmersionTimeViewModel>(navigationStore, () => new ImmersionTimeViewModel(navigationStore));
            OpenAddSourceView = new OpenViewCommand<AddSourceViewModel>(navigationStore, () => new AddSourceViewModel());
            OpenCreateLogView = new OpenViewCommand<CreateLogViewModel>(navigationStore, () => new CreateLogViewModel());
        }
    }
}
