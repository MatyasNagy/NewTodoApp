using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;

namespace NewTodoApp
{
    class UserRepository : IUserRepository
    {

        string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\Database1.mdf;Integrated Security=True";
        
        public List<User> UserekBeolvasas()
        {
            string queryString = @"SELECT * FROM UserTable";

            using (SqlConnection connection= new SqlConnection(connectionString))
            using (SqlDataAdapter adapter = new SqlDataAdapter(queryString, connection))
            {
                DataTable usersTable = new DataTable();
                adapter.Fill(usersTable);

                List<User> userekakt = new List<User>();

                foreach (DataRow row in usersTable.Rows)
                {
                    userekakt.Add(new User(row.Field<string>(1)));
                }
                return userekakt;
            }
        }

        public void UserHozzaad(User user)
        {
            string queryString = "INSERT INTO UserTable VALUES(@UserName)";

            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(queryString, connection))
            {
                connection.Open();
                command.Parameters.AddWithValue("@UserName", user.UserName.ToString());
                command.ExecuteNonQuery();
            }
        }


    }
}
