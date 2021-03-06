﻿using AutoMapper;
using BRMDesktopUI.Library.Api;
using BRMDesktopUI.Library.Helpers;
using BRMDesktopUI.Library.Models;
using BRMDesktopUI.Models;
using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BRMDesktopUI.ViewModels
{
	public class SalesViewModel : Screen
	{
		private IProductEndPoint _productEndPoint;
		private ISaleEndPoint _saleEndPoint;
		private IConfigHelper _configHelper;
		private IMapper _mapper;

		public SalesViewModel(IProductEndPoint productEndPoint, IConfigHelper configHelper, ISaleEndPoint saleEndPoint,
			IMapper mapper)
		{
			_productEndPoint = productEndPoint;
			_saleEndPoint = saleEndPoint;
			_configHelper = configHelper;
			_mapper = mapper;
		}

		protected override async void OnViewLoaded(object view)
		{
			await LoadProducts();
		}

		private async Task LoadProducts()
		{
			var productList = await _productEndPoint.GetAll();
			var products = _mapper.Map<List<ProductDisplayModel>>(productList);
			Products = new BindingList<ProductDisplayModel>(products);
		}

		private BindingList<ProductDisplayModel> _products;
		public BindingList<ProductDisplayModel> Products
		{
			get { return _products; }
			set
			{
				_products = value;
				NotifyOfPropertyChange(() => Products);
			}
		}

		private ProductDisplayModel _selectedProduct;
		public ProductDisplayModel SelectedProduct
		{
			get { return _selectedProduct; }
			set
			{
				_selectedProduct = value;
				NotifyOfPropertyChange(() => SelectedProduct);
				NotifyOfPropertyChange(() => CanAddToCart);
			}
		}

		private CartItemDisplayModel _selectedCartItem;
		public CartItemDisplayModel SelectedCartItem
		{
			get { return _selectedCartItem; }
			set
			{
				_selectedCartItem = value;
				NotifyOfPropertyChange(() => SelectedCartItem);
				NotifyOfPropertyChange(() => CanRemoveFromCart);
			}
		}

		private BindingList<CartItemDisplayModel> _cart = new BindingList<CartItemDisplayModel>();
		public BindingList<CartItemDisplayModel> Cart
		{
			get { return _cart; }
			set
			{
				_cart = value;
				NotifyOfPropertyChange(() => Cart);
			}
		}

		private int _itemQuantity = 1;
		public int ItemQuantity
		{
			get { return _itemQuantity; }
			set
			{
				_itemQuantity = value;
				NotifyOfPropertyChange(() => ItemQuantity);
				NotifyOfPropertyChange(() => CanAddToCart);
			}
		}

		public string SubTotal
		{
			get
			{
				return CalculateSubTotal().ToString("C", new CultureInfo("hi-IN"));
			}
		}

		private decimal CalculateSubTotal()
		{
			decimal subTotal = 0;

			foreach (var item in Cart)
			{
				subTotal += item.Product.RetailPrice * item.QuantityInCart;
			}

			return subTotal;
		}

		private decimal CalculateTax()
		{
			decimal taxAmount = 0;
			decimal taxRate = _configHelper.GetTaxRate() / 100;

			taxAmount = Cart
				.Where(x => x.Product.IsTaxable)
				.Sum(y => y.Product.RetailPrice * y.QuantityInCart * taxRate);

			//foreach (var item in Cart)
			//{
			//	if (item.Product.IsTaxable)
			//	{
			//		taxAmount += (item.Product.RetailPrice * item.QuantityInCart * (taxRate));
			//	}
			//}

			return taxAmount;
		}

		public string Tax
		{
			get
			{
				return CalculateTax().ToString("C", new CultureInfo("hi-IN"));
			}
		}

		public string Total
		{
			get
			{
				decimal total = CalculateSubTotal() + CalculateTax();

				return total.ToString("C", new CultureInfo("hi-IN"));
			}
		}

		public bool CanAddToCart
		{
			get
			{
				bool output = false;
				//Make sure there are item quantities
				//Make sure an item is selected
				if (ItemQuantity >= 1 && SelectedProduct?.QuantityInStock >= ItemQuantity)
				{
					output = true;
				}
				return output;
			}
		}

		public void AddToCart()
		{
			CartItemDisplayModel existingItem = Cart.FirstOrDefault(x => x.Product == SelectedProduct);
			if (existingItem != null)
			{
				existingItem.QuantityInCart += ItemQuantity;
				//Cart.Remove(existingItem);
				//Cart.Add(existingItem);
			}
			else
			{
				CartItemDisplayModel item = new CartItemDisplayModel
				{
					Product = SelectedProduct,
					QuantityInCart = ItemQuantity
				};
				Cart.Add(item);
			}
			
			SelectedProduct.QuantityInStock -= ItemQuantity;
			ItemQuantity = 1;
			NotifyOfPropertyChange(() => SubTotal);
			NotifyOfPropertyChange(() => Tax);
			NotifyOfPropertyChange(() => Total);
			NotifyOfPropertyChange(() => CanCheckOut);
		}

		public bool CanRemoveFromCart
		{
			get
			{
				bool output = false;
				//Make sure something is selected
				if (SelectedCartItem != null && SelectedCartItem?.QuantityInCart > 0)
				{
					output = true;
				}
				return output;
			}
		}

		public void RemoveFromCart()
		{
			SelectedCartItem.Product.QuantityInStock += 1;

			if (SelectedCartItem.QuantityInCart > 1)
			{
				SelectedCartItem.QuantityInCart -= 1;
			}
			else
			{
				Cart.Remove(SelectedCartItem);
			}

			NotifyOfPropertyChange(() => SubTotal);
			NotifyOfPropertyChange(() => Tax);
			NotifyOfPropertyChange(() => Total);
			NotifyOfPropertyChange(() => CanCheckOut);
		}

		public bool CanCheckOut
		{
			get
			{
				bool output = false;
				//Make sure there is something in the cart
				if (Cart.Count > 0)
				{
					output = true;
				}

				return output;
			}
		}

		public async Task CheckOut()
		{
			//Create a SaleModel and post it to the API
			SaleModel sale = new SaleModel();

			foreach (var item in Cart)
			{
				sale.SaleDeatils.Add(new SaleDetailModel
				{
					ProductId = item.Product.Id,
					Quantity = item.QuantityInCart
				});
			}

			await _saleEndPoint.PostSale(sale);
		}
	}
}
