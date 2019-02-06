using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EFCoreNewDatabase.Controllers
{
    public class RoleAdminController : Controller
    {
        private RoleManager<IdentityRole> roleManager;
        public RoleAdminController(RoleManager<IdentityRole> roleMgr)
        {
            roleManager = roleMgr;
        }

        public IActionResult Index() => View(roleManager.Roles);

    }
}
