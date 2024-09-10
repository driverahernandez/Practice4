using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Practice4
{
    public class DataBase
    {
        // List<IRepository> allTables = new List<IRepository>();
        List<Product> products = new List<Product>();
        List<Sale> sales = new List<Sale>();
        List<Purchase> purchases = new List<Purchase>();
        public void ReadFromDataBase(string connectionString)
        {
           
            ProductsRepository productsRepository = new ProductsRepository();
            SalesRepository salesRepository = new SalesRepository();
            PurchasesRepository purchasesRepository = new PurchasesRepository();

            using (SqlConnection connection =
                new(connectionString))
            {

                try
                {
                    connection.Open();
                    productsRepository.GetDataFromDB(connection);
                    salesRepository.GetDataFromDB(connection);
                    purchasesRepository.GetDataFromDB(connection);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            products = productsRepository.AllProducts;
            sales = salesRepository.AllSales;
            purchases = purchasesRepository.AllPurchases;

        }

        public void ReadDataFromProductsTable()
        {
            Console.WriteLine("****PRODUCTS*****");
            var ProductsAsStrings = products.Select(p => $"ID:\t{p.Id}\nName:\t{p.Name}\nCost:\t{p.Cost}\n");
            Console.WriteLine(string.Join(Environment.NewLine, ProductsAsStrings));
        }

        public void CreateNewProduct()
        {
            Console.Write("Product name: ");
            var name = Console.ReadLine();
            Console.Write("Product cost: ");
            var cost = Convert.ToDecimal(Console.ReadLine());

            var id = products.Count();
            id++;

            products.Add(new Product(id, name, cost));

            Console.WriteLine("New product created successfully.");
        }
        
        public void FilterProducts(string name)
        {
            var ProductsAsStrings = products.Where(p => p.Name == name)
                                                        .Select(p => $"ID:\t{p.Id}\nName:\t{p.Name}\nCost:\t{p.Cost}\n");
            Console.WriteLine(string.Join(Environment.NewLine, ProductsAsStrings)); ;
        }
        public void FilterProducts(int id)
        {
            var ProductsAsStrings = products.Where(p => p.Id == id)
                                                        .Select(p => $"ID:\t{p.Id}\nName:\t{p.Name}\nCost:\t{p.Cost}\n");
            Console.WriteLine(string.Join(Environment.NewLine, ProductsAsStrings)); 
        }
        public void GroupAndSortSales()
        {
            var SalesGroups = sales.GroupBy(s => s.Date)
                                            .OrderBy(s => s.Count());
            foreach (var group in SalesGroups) 
            {
                Console.WriteLine(group.Key);
                foreach(var s in group)
                {
                Console.WriteLine($"ID:\t{s.OrderId}\nProductId:\t{s.ProductId}\nTotal:\t{s.Total}\n");
                }
            }
        }

        public void GetProductAndSalesInformation()
        {
            //var Products = (ProductsRepository)allTables[0];
            //var Sales = (SalesRepository)allTables[1];
            
            //LINQ TO SQL 
            //var test = from p in Products.AllProducts
            //           join s in Sales.AllSales on p.Id equals s.ProductId
            //           select new
            //           {
            //               s.OrderId,
            //               s.ProductId,
            //               s.Date,
            //               IndividualCost = p.Cost,
            //               TotalCost = s.Total
            //           };

            //LINQ TO OBJECT
            var ProductAndSalesInfo = products.Join(sales, p => p.Id, s => s.ProductId,
                (p, s) => new
                {
                    s.OrderId,
                    s.ProductId,
                    s.Date,
                    IndividualCost = p.Cost,
                    TotalCost = s.Total

                })
                .OrderBy(ps => ps.OrderId)
                .Select(ps=> $"OrderID: {ps.OrderId}, ProductID: {ps.ProductId}, Date: {ps.Date}, IndividualCost: {ps.IndividualCost}, TotalCost: {ps.TotalCost}");
            
            Console.WriteLine(string.Join(Environment.NewLine, ProductAndSalesInfo));

        }
        public void GetProductsWithoutPurchasesOrSales()
        {
            var ProductsWithoutPurchasesOrSales = products.GroupJoin(purchases, p => p.Id, pu => pu.ProductId,
                (p, purchasesGroup) => new
                {
                    p.Id,
                    p.Name,
                    ProductsPurchased = purchasesGroup.Count()

                })
                .GroupJoin(sales, p => p.Id, s=>s.ProductId,
                (p, salesGroup) => new
                {
                    p.Id,
                    p.Name,
                    p.ProductsPurchased,
                    ProductsSold = salesGroup.Count()
                })
                .Where(p => p.ProductsPurchased==0 && p.ProductsSold==0)
                .Select(p => $"ID: {p.Id}, Name: {p.Name}");

            Console.WriteLine("Products with no sales or purchases:");
            Console.WriteLine(string.Join(Environment.NewLine, ProductsWithoutPurchasesOrSales));
        }
        public void GetSumOfProductsSold()
        {
            var SumOfProductsSold = sales.Sum(s=>s.Total);
            Console.WriteLine($"Sum of all sales in the month: {SumOfProductsSold}");
        }
    }
}
