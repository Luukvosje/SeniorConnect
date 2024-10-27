using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySql.Data.MySqlClient;

namespace Seniorconnect_Luuk_deVos.Pages
{
    [Authorize]
    public class ActiviteitenBordModel : PageModel
	{
		private readonly IConfiguration _configuration;
		public List<Activity> Activities { get; set; }

		public ActiviteitenBordModel(IConfiguration configuration)
		{
			_configuration = configuration;
		}

		public void OnGet()
		{
			string connectionString = _configuration.GetConnectionString("DefaultConnection");
			List<Activity> activities = new List<Activity>();
			using (MySqlConnection conn = new MySqlConnection(connectionString))
			{
				conn.Open();
				string query = "SELECT * FROM activities";

				using (MySqlCommand cmd = new MySqlCommand(query, conn))
				{
					using (MySqlDataReader reader = cmd.ExecuteReader())
					{
						while (reader.Read())
						{
							var activity = new Activity
							{
								ID = reader.GetInt32("ID"),
								date = reader.GetDateTime("Date"),
								title = reader.GetString("Title"),
								description = reader.GetString("Description"),
								maxUsers = reader.GetInt32("MaxUsers"),
								endTime = reader.GetDateTime("EndTime"),
								price = reader.GetInt32("Price"),
								startTime = reader.GetDateTime("StartTime")
							};
                            activities.Add(activity);
						}
					}
				}
				Activities = activities;
			}
		}
	}
	public class Activity
	{
		public int ID { get; set; }
		public string title { get; set; }
		public string description { get; set; }
		public DateTime date { get; set; }
		public int maxUsers { get; set; }
		public int price { get; set; }
		public DateTime startTime { get; set; }
		public DateTime endTime { get; set; }
	}
}
