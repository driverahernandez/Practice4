using Microsoft.Data.SqlClient;
using Practice4;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Practice4
{
    public class PurchasesRepository : IRepository
    {
        public List<Purchase> AllPurchases = new List<Purchase>();
        public void GetDataFromDB(SqlConnection connection)
        {
            const string queryString =
                "SELECT * from dbo.Purchases ";

            SqlCommand command = new(queryString, connection);

            SqlDataReader reader = command.ExecuteReader();
            var purchases = new List<Purchase>();
            while (reader.Read())
            {
                purchases.Add(new Purchase((int)reader[0], (int)reader[1], (decimal)reader[2], (DateTime)reader[3]));
            }

            reader.Close();
            AllPurchases = purchases;
        }
    }
}
