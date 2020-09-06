using AutoMapper;
using BRMDesktopUI.Helpers;
using BRMDesktopUI.Library.Api;
using BRMDesktopUI.Library.Helpers;
using BRMDesktopUI.Library.Models;
using BRMDesktopUI.ViewModels;
using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using AutoMapper;
using BRMDesktopUI.Models;

namespace BRMDesktopUI
{
	public class BootStrapper : BootstrapperBase
	{
		private SimpleContainer _container = new SimpleContainer();
		public BootStrapper()
		{
			Initialize();
		}

		private IMapper ConfigureAutoMapper()
		{
			var config = new MapperConfiguration(cfg =>
			{
				cfg.CreateMap<ProductModel, ProductDisplayModel>();
				cfg.CreateMap<CartItemModel, CartItemDisplayModel>();
			});

			var output = config.CreateMapper();

			return output;
		}

		protected override void Configure()
		{
			_container.Instance(ConfigureAutoMapper());

			_container.Instance(_container)
				.PerRequest<IProductEndPoint, ProductEndPoint>()
				.PerRequest<ISaleEndPoint, SaleEndPoint>();

			_container
				.Singleton<IWindowManager, WindowManager>()
				.Singleton<IEventAggregator, EventAggregator>()
				.Singleton<ILoggedInUserModel, LoggedInUserModel>()
				.Singleton<IConfigHelper, ConfigHelper>()
				.Singleton<IApiHelper, ApiHelper>();

			GetType().Assembly.GetTypes()
				.Where(Type => Type.IsClass)
				.Where(Type => Type.Name.EndsWith("ViewModel"))
				.ToList()
				.ForEach(viewModelType => _container.RegisterPerRequest(
					viewModelType, viewModelType.ToString(), viewModelType));

			ConventionManager.AddElementConvention<PasswordBox>(
			PasswordBoxHelper.BoundPasswordProperty,
			"Password",
			"PasswordChanged");
		}
		protected override void OnStartup(object sender, StartupEventArgs e)
		{
			DisplayRootViewFor<ShellViewModel>();
		}
		protected override object GetInstance(Type service, string key)
		{
			return _container.GetInstance(service, key);
		}
		protected override IEnumerable<object> GetAllInstances(Type service)
		{
			return _container.GetAllInstances(service);
		}
		protected override void BuildUp(object instance)
		{
			_container.BuildUp(instance);
		}
	}
}
