using BRMDataManager.Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using BRMDataManager.Library.DataAccess;

namespace BRMDataManager.Controllers
{
    [Authorize]
    public class UserController : ApiController
    {
        // GET: User/GetById
        public UserModel GetById()
        {
            UserData data = new UserData();

            string userId = RequestContext.Principal.Identity.GetUserId();
            return data.GetUserById(userId).FirstOrDefault();
        }
    }
}
