using BRMDataManager.Library.Internal.DataAccess;
using BRMDataManager.Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BRMDataManager.Library.DataAccess
{
	public class UserData
	{
		public List<UserModel> GetUserById(string id)
		{
			SqlDataAccess data = new SqlDataAccess();

			var p = new { Id = id };

			var output = data.LoadData<UserModel, dynamic>("dbo.spUserLookUp", p, "BRMData");
			return output;
		}
	}
}
