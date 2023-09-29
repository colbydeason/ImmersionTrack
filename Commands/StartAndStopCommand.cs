﻿using Mayuri.Models;
using Mayuri.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Diagnostics;

namespace Mayuri.Commands
{
    public class StartAndStopCommand : CommandBase
    {
        private readonly ImmersionTime _immersionTime;
        private readonly ImmersionTimeViewModel _viewModel;
        public StartAndStopCommand(ImmersionTimeViewModel vm, ImmersionTime immersionTime)
        {
            _immersionTime = immersionTime;
            _viewModel = vm;
        }
        public override void Execute(object? parameter)
        {
            if (_immersionTime.IsRunning()) {
                _immersionTime.StopWatch();
            }
            else
            {
                _immersionTime.StartWatch();
            }
        }
    }
}
