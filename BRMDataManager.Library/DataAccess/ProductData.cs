using BRMDataManager.Library.Internal.DataAccess;
using BRMDataManager.Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BRMDataManager.Library.DataAccess
{
	public class ProductData
	{
		public List<ProductModel> GetProducts()
		{
			SqlDataAccess data = new SqlDataAccess();

			var output = data.LoadData<ProductModel, dynamic>("dbo.spProduct_GetAll", new { }, "BRMData");
			return output;
		}
	}
}
