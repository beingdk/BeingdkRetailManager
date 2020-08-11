﻿using BRMDesktopUI.ViewModels;
using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BRMDesktopUI
{
	public class BootStrapper : BootstrapperBase
	{
		public BootStrapper()
		{
			Initialize();
		}
		protected override void OnStartup(object sender, StartupEventArgs e)
		{
			DisplayRootViewFor<ShellViewModel>();
		}
	}
}
