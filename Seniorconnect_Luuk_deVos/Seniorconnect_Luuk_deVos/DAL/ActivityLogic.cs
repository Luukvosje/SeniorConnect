using MySql.Data.MySqlClient;
using Seniorconnect_Luuk_deVos.Model;

namespace Seniorconnect_Luuk_deVos.DAL
{
    public class ActivityLogic : BaseLogic
    {
        public ActivityLogic(MySqlDbContext dbContext, IConfiguration configuration) : base(dbContext, configuration)
        {
        }

        public List<ActivityModel> GetActivities()
        {
            List<ActivityModel> activities = new List<ActivityModel>();

            using (var conn = _dbContext.GetConnection())
            {
                conn.Open();
                string query = "SELECT * FROM activities ORDER BY startTime";

                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var activity = new ActivityModel
                            {
                                id = reader.GetInt32("ID"),
                                title = reader.GetString("Title"),
                                description = reader.GetString("Description"),
                                maxUsers = reader.GetInt32("MaxUsers"),
                                endTime = reader.GetDateTime("EndTime"),
                                price = reader.GetInt32("Price"),
                                startTime = reader.GetDateTime("StartTime"),
                                joined = false
                            };
                            activities.Add(activity);
                        }
                    }
                }
            }
            return activities;
        }

        public List<ActivityModel> GetJoinedActivities(int userId)
        {
            if (userId == null || userId == 0)
                return null;
            List<ActivityModel> activities = new List<ActivityModel>();

            using (var conn = _dbContext.GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT * FROM activities a JOIN chosenactivities ca ON a.ID = ca.ActivityId WHERE ca.UserId = @userId;", conn);
                cmd.Parameters.AddWithValue("@userId", userId);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var activity = new ActivityModel
                        {
                            id = reader.GetInt32("ID"),
                            title = reader.GetString("Title"),
                            description = reader.GetString("Description"),
                            maxUsers = reader.GetInt32("MaxUsers"),
                            endTime = reader.GetDateTime("EndTime"),
                            price = reader.GetInt32("Price"),
                            startTime = reader.GetDateTime("StartTime"),
                            joined = true
                        };
                        activities.Add(activity);
                    }
                }

                return activities;
            }
        }

        public bool JoinActivity(int userId, int activityId)
        {

            if (userId == null || userId == 0 || activityId == 0 || activityId == null)
                return false;

            using (var conn = _dbContext.GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("INSERT INTO chosenactivities (UserId,ActivityId) VALUES (@id, @activity)", conn);
                cmd.Parameters.AddWithValue("@id", userId);
                cmd.Parameters.AddWithValue("@activity", activityId);

                int rowsAffected = cmd.ExecuteNonQuery();
                return rowsAffected > 0;
            }
        }

        public bool LeaveActivity(int userId, int activityId)
        {
            if (userId == 0 || activityId == 0)
                return false;

            using (var conn = _dbContext.GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("DELETE FROM chosenactivities WHERE UserId = @id AND ActivityId = @activity", conn);
                cmd.Parameters.AddWithValue("@id", userId);
                cmd.Parameters.AddWithValue("@activity", activityId);

                int rowsAffected = cmd.ExecuteNonQuery();
                return rowsAffected > 0;
            }
        }

        public List<User> GetUsersFromActivity(int activityId)
        {
            if (activityId == 0)
                return null;
            List<User> users = new List<User>();

            using (var conn = _dbContext.GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(" SELECT * FROM users a JOIN chosenactivities ca ON a.UserId = ca.UserId where ca.ActivityId = @activity", conn);
                cmd.Parameters.AddWithValue("@activity", activityId);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var User = new User
                        {
                            email = reader.GetString("Email"),
                            name = reader.GetString("Name"),
                            userId = reader.GetInt32("UserId"),

                        };
                        users.Add(User);
                    }
                }
            }
            return users;
        }

        public ActivityModel GetActivity(int activityId)
        {
            if (activityId == 0)
                return null;
            ActivityModel activity = null;

            using (var conn = _dbContext.GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(" SELECT * FROM activities where ID = @activity", conn);
                cmd.Parameters.AddWithValue("@activity", activityId);
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        activity = new ActivityModel
                        {
                            id = reader.GetInt32("ID"),
                            title = reader.GetString("Title"),
                            description = reader.GetString("Description"),
                            maxUsers = reader.GetInt32("MaxUsers"),
                            endTime = reader.GetDateTime("EndTime"),
                            price = reader.GetInt32("Price"),
                            startTime = reader.GetDateTime("StartTime"),
                            joined = true
                        };
                    }
                }
            }
            return activity;
        }
    }
}
