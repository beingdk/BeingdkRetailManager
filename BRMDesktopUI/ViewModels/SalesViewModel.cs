using BRMDesktopUI.Library.Api;
using BRMDesktopUI.Library.Models;
using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BRMDesktopUI.ViewModels
{
	public class SalesViewModel : Screen
	{
		private IProductEndPoint _productEndPoint;
		public SalesViewModel(IProductEndPoint productEndPoint)
		{
			_productEndPoint = productEndPoint;
		}

		protected override async void OnViewLoaded(object view)
		{
			await LoadProducts();
		}

		private async Task LoadProducts()
		{
			var productList = await _productEndPoint.GetAll();
			Products = new BindingList<ProductModel>(productList);
		}

		private BindingList<ProductModel> _products;
		public BindingList<ProductModel> Products
		{
			get { return _products; }
			set
			{
				_products = value;
				NotifyOfPropertyChange(() => Products);
			}
		}

		private BindingList<ProductModel> _cart;
		public BindingList<ProductModel> Cart
		{
			get { return _cart; }
			set
			{
				_cart = value;
				NotifyOfPropertyChange(() => Cart);
			}
		}

		private int _itemQuantity;
		public int ItemQuantity
		{
			get { return _itemQuantity; }
			set
			{
				_itemQuantity = value;
				NotifyOfPropertyChange(() => ItemQuantity);
			}
		}

		public string SubTotal
		{
			get
			{
				//TODO
				return "Rs 0.00";
			}
		}

		public string Tax
		{
			get
			{
				//TODO
				return "Rs 0.00";
			}
		}

		public string Total
		{
			get
			{
				//TODO
				return "Rs 0.00";
			}
		}

		public bool CanAddToCart
		{
			get
			{
				bool output = false;
				//Make sure there are item quantities
				//Make sure an item is selected
				return output;
			}
		}

		public void AddToCart()
		{
		}

		public bool CanRemoveFromCart
		{
			get
			{
				bool output = false;
				//Make sure something is selected
				return output;
			}
		}

		public void RemoveFromCart()
		{
		}

		public bool CanCheckOut
		{
			get
			{
				bool output = false;
				//Make sure there is something in the cart
				return output;
			}
		}

		public void CheckOut()
		{
		}
	}
}
