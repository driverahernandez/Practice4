using Microsoft.Data.SqlClient;
using Practice4;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Practice4
{
    public class SalesRepository : IRepository
    {
        public List<Sale> AllSales = new List<Sale>();
        public void GetDataFromDB(SqlConnection connection)
        {
            const string queryString =
                "SELECT * from dbo.Sales ";

            SqlCommand command = new(queryString, connection);

            SqlDataReader reader = command.ExecuteReader();
            var sales = new List<Sale>();
            while (reader.Read())
            {
                sales.Add(new Sale((int)reader[0], (int)reader[1], (decimal)reader[2], (DateTime)reader[3]));
            }

            reader.Close();
            AllSales = sales;
        }
    }
}
