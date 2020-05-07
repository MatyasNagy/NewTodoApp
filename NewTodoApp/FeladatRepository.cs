using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace NewTodoApp
{
    class FeladatRepository : IFeladatRepository
    {
        string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\Database1.mdf;Integrated Security=True";

        public List<Feladat> FeladatBeolvasas()
        {
            string queryString = "SELECT * FROM FeladatTable";

            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlDataAdapter adapter = new SqlDataAdapter(queryString, connection))
            {
                DataTable feladatTable = new DataTable();
                adapter.Fill(feladatTable);

                List<Feladat> feladatokakt = new List<Feladat>();

                foreach (DataRow row in feladatTable.Rows)
                {
                    feladatokakt.Add(new Feladat(row.Field<string>(1),row.Field<string>(2),row.Field<DateTime>(3)));
                }
                return feladatokakt;
            }
        }

        public void Listahozad(Feladat feladat, User user) //Újraírva
        {
            string queryString = "INSERT INTO FeladatTable VALUES(@FeladatCim, @FeladatLeiras, @FeladatDate) ";

            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(queryString, connection))
            {
                connection.Open();
                command.Parameters.AddWithValue("@FeladatCim", feladat.Cim);
                command.Parameters.AddWithValue("@FeladatLeiras", feladat.Leiras);
                command.Parameters.AddWithValue("@FeladatDate", feladat.Datum);
                command.ExecuteNonQuery();
            }

            string queryStringosszerendel = "DECLARE @FelId as int; " +
            "DECLARE @UserId as int; " +
            "SET @FelId = (SELECT FeladatTable.Id FROM FeladatTable WHERE FeladatTable.FeladatCim = @FeladatCim); " +
            "SET @UserId = (SELECT UserTable.Id FROM UserTable WHERE UserTable.UserName = @Username); " +
            "INSERT INTO FeladatKiosztasTable VALUES(@FelId, @UserId); ";

            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(queryStringosszerendel, connection))
            {
                connection.Open();
                command.Parameters.AddWithValue("@FeladatCim", feladat.Cim);
                command.Parameters.AddWithValue("@Username", user.UserName);
                command.ExecuteNonQuery();
            }
        }
        public void Listahozad_old(Feladat feladat, User user)  //RÉGI METODUS
        {
            string queryString = "INSERT INTO FeladatTable VALUES(@FeladatCim, @FeladatLeiras, @FeladatDate) ";
            //Feladat aktfeladat = new Feladat(TextBoxCim.,);

            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(queryString, connection))
            {

                connection.Open();
                command.Parameters.AddWithValue("@FeladatCim", feladat.Cim);
                command.Parameters.AddWithValue("@FeladatLeiras", feladat.Leiras);
                command.Parameters.AddWithValue("@FeladatDate", feladat.Datum);
                command.ExecuteNonQuery();
            }

            string queryString2 = "SELECT a.Id FROM UserTable a WHERE UserName=@user";
            DataTable dt1 = new DataTable();
            DataTable dt2 = new DataTable();

            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(queryString2, connection))
            using (SqlDataAdapter adapter = new SqlDataAdapter(command))
            {
                command.Parameters.AddWithValue("@user", user.UserName);
                adapter.Fill(dt1);
            }
            string queryString3 = "SELECT b.Id FROM FeladatTable b WHERE FeladatCim=@FeladatCim AND FeladatLeiras=@FeladatLeiras AND FeladatDate=@FeladatDate";

            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(queryString3, connection))
            using (SqlDataAdapter adapter = new SqlDataAdapter(command))
            {
                connection.Open();
                command.Parameters.AddWithValue("@FeladatCim", feladat.Cim);
                command.Parameters.AddWithValue("@FeladatLeiras", feladat.Leiras);
                command.Parameters.AddWithValue("@FeladatDate", feladat.Datum);
                command.ExecuteNonQuery();
                adapter.Fill(dt2);
               
            }
            int userTableId = dt1.Rows[0].Field<int>(0);
            int feladatTableId = dt2.Rows[0].Field<int>(0);

            string queryStringFeladatKiosztas = "INSERT INTO FeladatKiosztasTable VALUES(@FeladatId, @UserId) ";
            //Feladat aktfeladat = new Feladat(TextBoxCim.,);

            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(queryStringFeladatKiosztas, connection))
            {

                connection.Open();
                command.Parameters.AddWithValue("@FeladatId", feladatTableId);
                command.Parameters.AddWithValue("@UserId", userTableId);

                command.ExecuteNonQuery();
            }
        }

        public List<string> Megtekint(string fa) //ÚJRAIRVA
        {
            List<string> megjelenitoString = new List<string>();
            string sqlQuerryString = "" +
                "SELECT FeladatTable.FeladatCim,FeladatTable.FeladatLeiras,FeladatTable.FeladatDate, UserTable.UserName FROM FeladatTable "+
                "INNER JOIN FeladatKiosztasTable ON FeladatTable.Id = FeladatKiosztasTable.FeladatId "+
                "INNER JOIN UserTable ON FeladatKiosztasTable.UserId = UserTable.Id "+
                "WHERE FeladatTable.FeladatCim = @feladatcim ";

            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(sqlQuerryString, connection))
            {
                connection.Open();
                command.Parameters.AddWithValue("@feladatcim", fa);
                using (SqlDataReader sqlReader = command.ExecuteReader())
                {
                    while (sqlReader.Read())
                    {
                        for (int i = 0; i < sqlReader.FieldCount; i++)
                        {
                            megjelenitoString.Add(sqlReader.GetValue(i).ToString()+"\n");
                        }
                    }
                }
            }
            return megjelenitoString;
        }

        public List<string> Megtekint_old(string fa) //RÉGI METODUS
        {
            string queryString = "SELECT a.Id, a.FeladatId, a.UserId FROM FeladatKiosztasTable a " +
                                 "INNER JOIN FeladatTable b ON a.FeladatId = b.Id " +
                                 "WHERE b.FeladatCim = @fa";


                                 //"SELECT * FROM FeladatKiosztasTable a " +
                                 //"INNER JOIN FeladatTable b ON a.FeladatId = b.Id " +
                                 //"WHERE b.FeladatCim = @fa";

            DataTable keresetttable = new DataTable();

            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(queryString, connection))
            using (SqlDataAdapter adapter = new SqlDataAdapter(command))
            {
                connection.Open();
                command.Parameters.AddWithValue("@fa", fa);

                adapter.Fill(keresetttable);
            }

            DataTable resztable1 = new DataTable();
            DataTable resztable2 = new DataTable();

            List<string> megjelenitostring = new List<string>();

            int feladatid= (int)keresetttable.Rows[0]["FeladatId"];
            int userid =(int)keresetttable.Rows[0]["UserId"];

            List<string> megjelenitStr = new List<string>();

            string queryString1 = "SELECT * FROM FeladatTable WHERE Id=@FeladatId";
            string queryString2 = "SELECT * FROM UserTable WHERE Id=@UserId";

            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command1 = new SqlCommand(queryString1, connection))
            using (SqlDataAdapter adapter = new SqlDataAdapter(command1))
            {
                connection.Open();
                command1.Parameters.AddWithValue("@FeladatId", feladatid);

                adapter.Fill(resztable1);

                for (int i = 0; i < resztable1.Columns.Count; i++)
                {
                    megjelenitStr.Add(resztable1.Columns[i].ToString() + ": ");
                    megjelenitStr.Add(resztable1.Rows[0].ItemArray[i].ToString() + "\n");
                }
            }

            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command2 = new SqlCommand(queryString2, connection))
            using (SqlDataAdapter adapter = new SqlDataAdapter(command2))
            {
                connection.Open();
                command2.Parameters.AddWithValue("@UserId", userid);

                adapter.Fill(resztable2);

                for (int i = 0; i < resztable2.Columns.Count; i++)
                {
                    megjelenitStr.Add(resztable2.Columns[i].ToString()+": ");
                    megjelenitStr.Add(resztable2.Rows[0].ItemArray[i].ToString()+ "\n");
                }
            }

            return megjelenitStr;
        }


   }
}
