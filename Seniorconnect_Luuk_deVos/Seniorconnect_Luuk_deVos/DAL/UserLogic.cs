using Microsoft.AspNetCore.Identity;
using MySql.Data.MySqlClient;
using Seniorconnect_Luuk_deVos.Model;

namespace Seniorconnect_Luuk_deVos.DAL
{
    public class UserLogic : BaseLogic
    {
        private readonly PasswordHasher<string> _passwordHasher;

        public UserLogic(MySqlDbContext dbContext, IConfiguration configuration) : base(dbContext, configuration)
        {
            _passwordHasher = new PasswordHasher<string>();
        }


        public User AuthenticateUser(string email, string password)
        {
            User user = ValidateUser(email, password);
            if (user == null)
            {
                return null;
            }

            return user;
        }

        private User ValidateUser(string email, string password)
        {
            if (email == null & password == null)
            {
                return null;
            }

            using (var conn = _dbContext.GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT * FROM users WHERE Email = @email", conn);
                cmd.Parameters.AddWithValue("@email", email);

                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        var storedPasswordHash = reader.GetString("PasswordHash");

                        var result = _passwordHasher.VerifyHashedPassword(null, storedPasswordHash, password);

                        if (result == PasswordVerificationResult.Success)
                        {
                            return new User { userId = reader.GetInt32("UserId"), email = reader.GetString("Email"), name = reader.GetString("Name") };
                        }
                    }
                }
                return null;
            }
        }

        public bool CreateAccount(string email, string password, string name)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
                    return false;

                string userName = name ?? "Gebruiker";

                string hashedPassword = _passwordHasher.HashPassword(null, password);

                using (var conn = _dbContext.GetConnection())
                {
                    conn.Open();

                    // Check if email exists
                    var checkEmailCmd = new MySqlCommand("SELECT COUNT(*) FROM users WHERE Email = @Email", conn);
                    checkEmailCmd.Parameters.AddWithValue("@Email", email);
                    int emailExists = Convert.ToInt32(checkEmailCmd.ExecuteScalar());

                    if (emailExists > 0)
                        return false;

                    // Insert new user
                    var cmd = new MySqlCommand("INSERT INTO users (Email, PasswordHash, Name) VALUES (@Email, @PasswordHash, @Name)", conn);
                    cmd.Parameters.AddWithValue("@Email", email);
                    cmd.Parameters.AddWithValue("@PasswordHash", hashedPassword);
                    cmd.Parameters.AddWithValue("@Name", userName);

                    int rowsAffected = cmd.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating account: {ex.Message}");
                return false;
            }
        }

        public string HashPassword(string password)
        {
            return _passwordHasher.HashPassword(null, password);
        }
    }

}