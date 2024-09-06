using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Data.OleDb;
using System.Runtime.Versioning;
using Microsoft.Data.SqlClient;

namespace Practice4
{
    class Program
    {
        public static void Main(string[] args)
        {
            const string connectionString = "Data Source=localhost;Initial Catalog=DB_Practice4_1;Trusted_Connection=True;Encrypt=True;TrustServerCertificate=True;";

            DataBase db = new DataBase();
            db.ReadFromDataBase(connectionString);
            //db.ReadDataFromProductsTable();
            //db.CreateNewProduct();
            //db.FilterProducts("Chair");
            //db.GroupAndSortSales();

            db.ProductAndSalesInformation();
        }

    }
}