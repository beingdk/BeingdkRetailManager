using BRMDesktopUI.Library.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BRMDesktopUI.Library.Api
{
	public interface IProductEndPoint
	{
		Task<List<ProductModel>> GetAll();
	}
}