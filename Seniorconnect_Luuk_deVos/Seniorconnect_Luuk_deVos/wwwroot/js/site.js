var conn_string = app.Configuration.GetConnectionString("DefaultConnection");

var conn2 = new MySqlConnection(conn_string);
conn2.Open();

if (!(conn2.Ping())) {
    Console.WriteLine("connection failed");
}