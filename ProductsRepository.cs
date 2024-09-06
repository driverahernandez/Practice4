using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace Practice4
{
    public interface IRepository 
    {
        public void GetDataFromDB(SqlConnection connection);


    }
    public class ProductsRepository : IRepository
    {
        public List<Product> AllProducts = new List<Product>(); 
        public void GetDataFromDB(SqlConnection connection)
        {
            const string queryString =
                "SELECT * from dbo.Products ";

            SqlCommand command = new(queryString, connection);

            SqlDataReader reader = command.ExecuteReader();
            var products = new List<Product>();
            while (reader.Read())
            {
                products.Add(new Product((int)reader[0], (string)reader[1], (decimal)reader[2]));
            }

            reader.Close();
            AllProducts = products;
        }
        
    }
}
