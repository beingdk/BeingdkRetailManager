using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BRMDesktopUI.EventModels;
using Caliburn.Micro;

namespace BRMDesktopUI.ViewModels
{
	public class ShellViewModel : Conductor<object>,IHandle<LogOnEvent>
	{
		private IEventAggregator _events;
		private SalesViewModel _salesVM;
		public ShellViewModel(IEventAggregator events,SalesViewModel salesVM)
		{
			_events = events;
			_events.Subscribe(this);

			_salesVM = salesVM;

			ActivateItem(IoC.Get<LoginViewModel>());
		}

		public void Handle(LogOnEvent message)
		{
			ActivateItem(_salesVM);
		}
	}
}
