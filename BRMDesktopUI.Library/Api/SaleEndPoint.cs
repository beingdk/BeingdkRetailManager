using BRMDesktopUI.Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BRMDesktopUI.Library.Api
{
	public class SaleEndPoint : ISaleEndPoint
	{

		private IApiHelper _apiHelper;

		public SaleEndPoint(IApiHelper apiHelper)
		{
			_apiHelper = apiHelper;
		}

		public async Task PostSale(SaleModel sale)
		{
			using (HttpResponseMessage response = await _apiHelper.ApiClient.PostAsJsonAsync("/api/Sale", sale))
			{
				if (response.IsSuccessStatusCode)
				{
					//Log successfull?
				}
				else
				{
					throw new Exception(response.ReasonPhrase);
				}
			}
		}
	}
}
