using BRMDesktopUI.Library.Models;
using BRMDesktopUI.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace BRMDesktopUI.Library.Api
{
	public class ApiHelper : IApiHelper
	{
		private HttpClient apiClient;
		public ILoggedInUserModel _loggedInUser;

		public ApiHelper(ILoggedInUserModel loggedInUser)
		{
			InitializeClient();
			_loggedInUser = loggedInUser;
		}

		private void InitializeClient()
		{
			string api = ConfigurationManager.AppSettings["api"];

			apiClient = new HttpClient();
			apiClient.BaseAddress = new Uri(api);
			apiClient.DefaultRequestHeaders.Accept.Clear();
			apiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
		}

		public async Task<AuthenticatedUser> Authenticate(string username, string password)
		{
			var data = new FormUrlEncodedContent(new[]
			{
				new KeyValuePair<string,string>("grant_type","password"),
				new KeyValuePair<string,string>("username",username),
				new KeyValuePair<string,string>("password",password),
			});
			using (HttpResponseMessage response = await apiClient.PostAsync("/Token", data))
			{
				if (response.IsSuccessStatusCode)
				{
					var results = await response.Content.ReadAsAsync<AuthenticatedUser>();
					return results;
				}
				else
				{
					throw new Exception(response.ReasonPhrase);
				}
			}
		}

		public async Task GetLoggedInUserInfo(string token)
		{
			apiClient.DefaultRequestHeaders.Accept.Clear();
			apiClient.DefaultRequestHeaders.Clear();
			apiClient.DefaultRequestHeaders.Add("Authorization",$"Bearer {token}");

			using (HttpResponseMessage response = await apiClient.GetAsync("/api/User"))
			{
				if (response.IsSuccessStatusCode)
				{
					var results = await response.Content.ReadAsAsync<LoggedInUserModel>();
					_loggedInUser.CreatedDate = results.CreatedDate;
					_loggedInUser.EmailAddress = results.EmailAddress;
					_loggedInUser.FirstName = results.FirstName;
					_loggedInUser.LastName = results.LastName;
					_loggedInUser.Id = results.Id;
					_loggedInUser.Token = token;
				}
				else
				{
					throw new Exception(response.ReasonPhrase);
				}
			}
		}
	}
}
