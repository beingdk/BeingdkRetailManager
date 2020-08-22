using BRMDesktopUI.Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BRMDesktopUI.Library.Api
{
	public class ProductEndPoint : IProductEndPoint
	{
		private IApiHelper _apiHelper;

		public ProductEndPoint(IApiHelper apiHelper)
		{
			_apiHelper = apiHelper;
		}

		public async Task<List<ProductModel>> GetAll()
		{
			using (HttpResponseMessage response = await _apiHelper.ApiClient.GetAsync("/api/Product"))
			{
				if (response.IsSuccessStatusCode)
				{
					var results = await response.Content.ReadAsAsync<List<ProductModel>>();
					return results;
				}
				else
				{
					throw new Exception(response.ReasonPhrase);
				}
			}
		}
	}
}
