using BRMDesktopUI.Library.Models;
using System.Threading.Tasks;

namespace BRMDesktopUI.Library.Api
{
	public interface ISaleEndPoint
	{
		Task PostSale(SaleModel sale);
	}
}