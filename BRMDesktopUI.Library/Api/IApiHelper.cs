using BRMDesktopUI.Models;
using System.Threading.Tasks;

namespace BRMDesktopUI.Library.Api
{
	public interface IApiHelper
	{
		Task<AuthenticatedUser> Authenticate(string username, string password);
		Task GetLoggedInUserInfo(string token);
	}
}