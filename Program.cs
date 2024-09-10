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
            
            while(true)
            { 
            var input = GetChoice();
                if (string.IsNullOrEmpty(input))
                    break;

            var choice = Convert.ToInt16(input);
            switch (choice)
            {
                case 1:
                    db.ReadDataFromProductsTable();
                    break;
                case 2:
                    db.CreateNewProduct();
                    break;
                case 3:
                    Console.WriteLine("Type the name or id of the product to filter.");
                    var product = Console.ReadLine();
                    if(int.TryParse(product, out var value))
                        db.FilterProducts(value);
                    else
                        db.FilterProducts(product);
                    break;
                case 4:
                    db.GroupAndSortSales();
                    break;
                case 5:
                    db.GetProductAndSalesInformation();
                    break;
                case 6:
                    db.GetProductsWithoutPurchasesOrSales();
                    break;
                case 7:
                    db.GetSumOfProductsSold();
                    break;
            }
            }
        }

        public static string GetChoice()
        {
            
                Console.WriteLine(@"
* * * * * * * * MENU * * * * * * * *

Please type in the number of the activity to be performed.

Press the 1 key to get the data from the products table. 

Press the 2 key to request the creation of a new product.

Press the 3 key to filter de information by text o id from  product table.

Press the 4 key to group the sales by day and sort them by sales number from highest to lowest.

Press the 5 key to get the product and sales information for the month using line joins. 

Press de 6 key to get products that have no sales or purchases.

Press the 7 key to perform the sum of the products sold in the month.

");
                var choice = Console.ReadLine();
                return choice;
            
        }

    }
}