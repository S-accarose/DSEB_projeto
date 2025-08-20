using Microsoft.AspNetCore.Authorization;

namespace DSEB_projeto.Models
{
    public class IsAdminAttribute : AuthorizeAttribute
    {
        public IsAdminAttribute() : base("Admin")
        { }
    }
}
