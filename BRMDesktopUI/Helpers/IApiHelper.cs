using BRMDesktopUI.Models;
using System.Threading.Tasks;

namespace BRMDesktopUI.Helpers
{
	public interface IApiHelper
	{
		Task<AuthenticatedUser> Authenticate(string username, string password);
	}
}