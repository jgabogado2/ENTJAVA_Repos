using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ENTJAVA_Week8.Models.DB;
using ENTJAVA_Week8.Models.EntityManager;
using Microsoft.AspNetCore.Mvc.Filters;


namespace ENTJAVA_Week8.Security
{
    public class AuthorizeRolesAttribute : AuthorizeAttribute, IAuthorizationFilter
    {
        private readonly string[] userAssignedRoles;

        public AuthorizeRolesAttribute(params string[] roles)
        {
            this.userAssignedRoles = roles;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            bool authorize = false;

            using (MyDBContext db = new MyDBContext())
            {
                UserManager um = new UserManager();
                foreach (var role in userAssignedRoles)
                {
                    authorize = um.IsUserInRole(context.HttpContext.User.Identity.Name, role);
                    if (authorize)
                        return;
                }
            }

            context.Result = new RedirectResult("~/Home/UnAuthorized"); // Need to create a separate page
        }
    }
}
