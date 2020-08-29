using BRMDataManager.Library.Internal.DataAccess;
using BRMDataManager.Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BRMDataManager.Library.DataAccess
{
	public class SaleData
	{
		public void SaveSale(SaleModel saleInfo, string cashierId)
		{
			//TODO: make this SOLID/DRY/better

			//Start filling the models with data which we will save to the database
			List<SaleDetailDBModel> details = new List<SaleDetailDBModel>();
			ProductData products = new ProductData();

			var taxRate = ConfigHelper.GetTaxRate() / 100;

			foreach (var item in saleInfo.SaleDeatils)
			{
				var detail = new SaleDetailDBModel
				{
					ProductId = item.ProductId,
					Quantity = item.Quantity
				};

				//Get the information for the product
				var productInfo = products.GetProductById(detail.ProductId);
				if (productInfo == null)
				{
					throw new Exception($"The product Id of { detail.ProductId } could not be found in the database");
				}
				detail.PurchasePrice = productInfo.RetailPrice * detail.Quantity;
				if (productInfo.IsTaxable)
				{
					detail.Tax = detail.PurchasePrice * taxRate;
				}
				details.Add(detail);
			}
			//Crate the sale model
			SaleDBModel sale = new SaleDBModel
			{
				SubTotal = details.Sum(x => x.PurchasePrice),
				Tax = details.Sum(x => x.Tax),
				CashierId = cashierId
			};
			sale.Total = sale.SubTotal + sale.Tax;

			//save the sale model
			SqlDataAccess data = new SqlDataAccess();
			data.SaveData("dbo.spSale_Insert", sale, "BRMData");

			//Get the SaleId 
			sale.Id = data.LoadData<int, dynamic>("dbo.spSale_Lookup", new { sale.CashierId, sale.SaleDate }, "BRMData").FirstOrDefault();

			foreach (var item in details)
			{
				item.SaleId = sale.Id;
				//save the sale detail model
				data.SaveData("dbo.spSaleDetail_Insert", item, "BRMData");
			}
			
		}
	}
}
