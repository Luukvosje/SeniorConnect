using MySql.Data.MySqlClient;

namespace Seniorconnect_Luuk_deVos.Model
{
    public class MySqlDbContext
    {
        private readonly string _connectionString;

        public MySqlDbContext(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public MySqlConnection GetConnection()
        {
            return new MySqlConnection(_connectionString);
        }
    }
}
