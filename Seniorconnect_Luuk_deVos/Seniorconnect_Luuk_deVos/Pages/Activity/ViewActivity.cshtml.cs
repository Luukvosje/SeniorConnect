using Microsoft.AspNetCore.Mvc.RazorPages;
using Seniorconnect_Luuk_deVos.DAL;
using Seniorconnect_Luuk_deVos.Model;

namespace Seniorconnect_Luuk_deVos.Pages.Activity
{
    public class ViewActivityModel : PageModel
    {

        private readonly ActivityLogic activityLogic;
        public List<User> allusers = new List<User>();
        public ActivityModel activity = null;


        public ViewActivityModel(ActivityLogic activityLogic)
        {
            this.activityLogic = activityLogic;
        }

        public void OnGet(int id)
        {
            // Use the id to load specific data
            if (id != 0)
            {
                allusers = activityLogic.GetUsersFromActivity(id);
                activity = activityLogic.GetActivity(id);
            }
        }
    }
}
