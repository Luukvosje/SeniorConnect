using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Seniorconnect_Luuk_deVos.DAL;
using Seniorconnect_Luuk_deVos.Model;
using System.Security.Claims;

namespace Seniorconnect_Luuk_deVos.Pages.Activity
{
    [Authorize]
    public class IndexModel : PageModel
    {
        public List<ActivityModel> currentActivities = new List<ActivityModel>();
        public List<ActivityModel> joinedActivities = new List<ActivityModel>();
        public static User user;
        public bool ShowJoinedOnly { get; private set; } = false;

        private readonly ActivityLogic activityLogic;

        public IndexModel(ActivityLogic activityLogic)
        {
            this.activityLogic = activityLogic;
        }

        public IActionResult OnPostToggle()
        {
            ShowJoinedOnly = !(HttpContext.Session.GetInt32("ShowJoinedOnly") == 1) || false;
            HttpContext.Session.SetInt32("ShowJoinedOnly", ShowJoinedOnly ? 1 : 0);
            LoadActivities();
            return Page();
        }

        public void OnGet()
        {
            ShowJoinedOnly = (HttpContext.Session.GetInt32("ShowJoinedOnly") == 1) || false;
            LoadUserData();
            currentActivities = CheckIfJoined();
        }


        public IActionResult OnPostRunActivity(int id)
        {
            LoadUserData();
            activityLogic.JoinActivity(user.userId, id);
            return RedirectToPage();
        }

        public IActionResult OnPostLeaveActivity(int id)
        {
            LoadUserData();
            activityLogic.LeaveActivity(user.userId, id);
            return RedirectToPage();
        }

        private void LoadActivities()
        {
            if (ShowJoinedOnly)
            {
                Console.WriteLine(user);
                joinedActivities = activityLogic.GetJoinedActivities(user.userId);
                currentActivities = joinedActivities;
            }
            else
            {
                currentActivities = CheckIfJoined();
            }
        }

        private void LoadUserData()
        {
            if (User.Identity.IsAuthenticated)
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var email = User.FindFirst(ClaimTypes.Email)?.Value;
                var name = User.FindFirst(ClaimTypes.Name)?.Value;

                if (!string.IsNullOrEmpty(userId) && int.TryParse(userId, out var id))
                {
                    user = new User
                    {
                        name = name ?? "Unknown",
                        email = email ?? "",
                        userId = id
                    };
                }
            }

        }


        private List<ActivityModel> CheckIfJoined()
        {
            List<ActivityModel> allActivities = activityLogic.GetActivities();
            joinedActivities = activityLogic.GetJoinedActivities(user.userId);

            foreach (var activity in allActivities)
            {
                activity.joined = joinedActivities.Any(joined => joined.id == activity.id);
            }

            return allActivities;
        }
    }
}
