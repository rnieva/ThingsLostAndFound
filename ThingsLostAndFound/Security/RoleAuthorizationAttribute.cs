using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ThingsLostAndFound.Models;

namespace ThingsLostAndFound.Security
{
    public class RoleAuthorizationAttribute : AuthorizeAttribute
    {
        private TLAFEntities db = new TLAFEntities();
        // TODO: change this method, only check by user name

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {

            string rolAllow = Roles.ToString(); // this parameter came from controller, example [RoleAuthorization(Roles = "1")]
            bool authorize = false;
            string UserName = HttpContext.Current.User.Identity.Name;
            //Here read the user´s role from cookie or session value, and we will avoid to access DB
            string rol;
            InfoUser user = new InfoUser();
            if ((user = db.InfoUsers.Where(a => a.UserName.Equals(UserName)).FirstOrDefault()) != null)
            {
                rol = user.Rol.ToString();
                if (rol == rolAllow)     // 1 = Admin
                    return true;
            }
            return authorize;

        } 
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new HttpUnauthorizedResult();
        }
    }
}