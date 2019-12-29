using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace DBTest_split_interface
{
    class DB_interface
    {
        private string ConnectionString;
        //private SqlConnection conn;

        public DB_interface()//default DB
        {
            ConnectionString = @"Data Source = (LocalDB)\MSSQLLocalDB; AttachDbFilename = D:\Documents\Ayrum\3 курс\SPZ\DBTest_split_interface\DBTest_split_interface\Database.mdf; Integrated Security = True";
            //conn = new SqlConnection(ConnectionString);
        }
        public DB_interface(string connectionString)//custom DB
        {
            ConnectionString = connectionString;
            //conn = new SqlConnection(ConnectionString);
        }

        public string[] SelectAllFrom(string Table)//string and int values only(can be expanded)
        {
            List<string> Values = new List<string>();

            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
            SqlCommand cmdSelectAll = new SqlCommand("SELECT * FROM " + Table, conn);            

                conn.Open();
                using (SqlDataReader DR = cmdSelectAll.ExecuteReader())
                {
                    if (DR.HasRows)
                        while (DR.Read())
                        {
                            for (int i = 0; i < DR.FieldCount; i++)
                            {
                                if (DR.GetValue(i).GetType() == typeof(string))
                                {
                                    Values.Add(DR.GetString(i));
                                }
                                else if (DR.GetValue(i).GetType() == typeof(int))
                                {
                                    Values.Add(DR.GetInt32(i).ToString());
                                }
                            }
                        }
                }
                conn.Close();
            }
            return Values.ToArray();
        }

        public void InsertInto(string Table, string[] ParamNames, object[] ParamValues)//only 1 row per call!
        {
            SqlConnection conn = new SqlConnection(ConnectionString);
            int N = ParamNames.Length;
            SqlCommand cmdInsertIn = new SqlCommand();
            cmdInsertIn.Connection = conn;
            StringBuilder CommandSB = new StringBuilder();//for cmdInsertIn
            CommandSB.Append("INSERT INTO ");
            CommandSB.AppendFormat("{0} VALUES(", Table);
            for (int i = 0; i < N; i++)
            {
                CommandSB.AppendFormat("@{0},",ParamNames[i]);
                //cmdInsertIn.Parameters.AddWithValue(ParamNames[i], Params[i]);
                cmdInsertIn.Parameters.AddWithValue(ParamNames[i], ParamValues[i]);
            }

            cmdInsertIn.CommandText = CommandSB.ToString().TrimEnd(',')+')';
            using (conn)
            {
                conn.Open();
                //cmdInsertIn.ExecuteNonQuery();//testing row
                try {
                cmdInsertIn.ExecuteNonQuery();
                }
                catch(SqlException exc)
                {
                    if(exc.Number == 2627)//2627
                    MessageBox.Show("Cannot execute writing!\nPrimary key violation!", "Data error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                conn.Close();
            }
        }

        public int DeleteRow(string Table, string WhereParam, string ParamValue)
        {
            int RowsDeleted;
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                SqlCommand cmdDeleteRow = new SqlCommand();
                cmdDeleteRow.Connection = conn;
                StringBuilder CommandSB = new StringBuilder();
                CommandSB.AppendFormat("DELETE FROM {0} ", Table);
                CommandSB.AppendFormat("WHERE {0} = {1}", WhereParam, ParamValue);
                cmdDeleteRow.CommandText = CommandSB.ToString();
                conn.Open();
                RowsDeleted = cmdDeleteRow.ExecuteNonQuery();
                conn.Close();
            }
            return RowsDeleted;
        }

        public int ClearTable(string Table)//be careful there
        {
            int RowsDeleted;
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                SqlCommand cmdDeleteRow = new SqlCommand();
                cmdDeleteRow.Connection = conn;
                cmdDeleteRow.CommandText = "DELETE FROM " + Table;
                conn.Open();
                RowsDeleted = cmdDeleteRow.ExecuteNonQuery();
                conn.Close();
            }
            return RowsDeleted;
        }
    }
}
