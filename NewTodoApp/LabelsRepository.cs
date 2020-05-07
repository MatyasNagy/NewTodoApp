using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace NewTodoApp
{
    class LabelsRepository : Ilabels
    {
        string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\Database1.mdf;Integrated Security=True";

        public List<string> Lista()
        {
            string queryString = "SELECT * FROM Labels";
            List<string> lista = new List<string>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlDataAdapter adapter = new SqlDataAdapter(queryString, connection))
            {
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                foreach (DataRow row in dt.Rows)
                {
                    lista.Add(row[1].ToString());
                }
                return lista;
            }
                
        }
        public void Listahozad(string srt)
        {
            string queryString = "INSERT INTO Labels VALUES(@Label)";

            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(queryString, connection))
            {
                connection.Open();
                command.Parameters.AddWithValue("@Label", srt);
                command.ExecuteNonQuery();
            }
            
        }

        public void Listabolelvesz(string srt)
        {
            string queryString = "DELETE FROM Labels WHERE Labels.Label=@Label";

            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(queryString, connection))
            {
                connection.Open();
                command.Parameters.AddWithValue("@Label", srt);
                command.ExecuteNonQuery();
            }
        }


        public void LabelFaRendez(List<string> labelekString, List<string> feladatokString)
        {

            List<int> outPutLabelsIds = new List<int>();
            string sqlcommand1 = "SELECT Id from Labels WHERE Labels.Label=@label ";

            foreach (string item in labelekString)
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                using (SqlCommand sqlCommand = new SqlCommand(sqlcommand1, connection))
                {
                    sqlCommand.Parameters.AddWithValue("@label", item);
                    connection.Open();
                    using (SqlDataReader dataReader = sqlCommand.ExecuteReader())
                    {
                        while (dataReader.Read())
                        {
                            for (int i = 0; i < dataReader.FieldCount; i++)
                            {
                                outPutLabelsIds.Add((int)dataReader.GetValue(i));
                            }

                        }

                    }
                }
            }

            List<int> outPutFeladatokIds = new List<int>();
            string sqlcommand2 = "SELECT Id from FeladatTable WHERE FeladatTable.FeladatCim=@FeladatCim ";

            foreach (string item in feladatokString)
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                using (SqlCommand sqlCommand = new SqlCommand(sqlcommand2, connection))
                {
                    sqlCommand.Parameters.AddWithValue("@FeladatCim", item);
                    connection.Open();
                    using (SqlDataReader dataReader = sqlCommand.ExecuteReader())
                    {
                        while (dataReader.Read())
                        {
                            for (int i = 0; i < dataReader.FieldCount; i++)
                            {
                                outPutFeladatokIds.Add((int)dataReader.GetValue(i));
                            }

                        }

                    }
                }
            }

            LabelFaIrasSqlbe(outPutLabelsIds, outPutFeladatokIds);



        }

        private void LabelFaIrasSqlbe(List<int> labelsIds, List<int> feladatIds  )
        {
            string queryString = "INSERT INTO FaLabelTable VALUES(@feladatIds, @labelsIds)";

            foreach (var feladatId in feladatIds)
            {
                foreach (var labelId in labelsIds)
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    using (SqlCommand command = new SqlCommand(queryString, connection))
                    {
                        connection.Open();
                        command.Parameters.AddWithValue("@feladatIds", feladatId);
                        command.Parameters.AddWithValue("@labelsIds", labelId);
                        command.ExecuteNonQuery();
                    }
                }

            }

            //TEST

            string queryStringtest = "SELECT * FROM FaLabelTable";
            List<string> lista = new List<string>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand sqlCommand = new SqlCommand(queryStringtest, connection))
            {
                connection.Open();
                using (SqlDataReader dataReader = sqlCommand.ExecuteReader())
                {
                    string test="";
                    while (dataReader.Read())
                    {

                        test += dataReader.GetValue(0).ToString() + " "+ dataReader.GetValue(1).ToString()+" "+ dataReader.GetValue(2).ToString()+" "+"\n";
                    }
                }
            }


        }
    }
}
