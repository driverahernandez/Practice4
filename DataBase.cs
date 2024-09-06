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
        List<IRepository> allTables = new List<IRepository>();
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

            allTables.Add(productsRepository);
            allTables.Add(salesRepository);
            allTables.Add(purchasesRepository);

        }

        public void ReadDataFromProductsTable()
        {
            Console.WriteLine("****PRODUCTS*****");
            var Products = (ProductsRepository)allTables[0];
            var ProductsAsStrings = Products.AllProducts.Select(p => $"ID:\t{p.Id}\nName:\t{p.Name}\nCost:\t{p.Cost}\n");
            Console.WriteLine(string.Join(Environment.NewLine, ProductsAsStrings));
        }

        public void CreateNewProduct()
        {
            var Products = (ProductsRepository)allTables[0];
            Console.Write("Product name: ");
            var name = Console.ReadLine();
            Console.Write("Product cost: ");
            var cost = Convert.ToDecimal(Console.ReadLine());

            var id = Products.AllProducts.Count();
            id++;

            Products.AllProducts.Add(new Product(id, name, cost));

            Console.WriteLine("New product created successfully.");
        }
        
        public void FilterProducts(string name)
        {
            var Products = (ProductsRepository)allTables[0];
            var ProductsAsStrings = Products.AllProducts.Where(p => p.Name == name)
                                                        .Select(p => $"ID:\t{p.Id}\nName:\t{p.Name}\nCost:\t{p.Cost}\n");
            Console.WriteLine(string.Join(Environment.NewLine, ProductsAsStrings)); ;
        }
        public void FilterProducts(int id)
        {
            var Products = (ProductsRepository)allTables[0];
            var ProductsAsStrings = Products.AllProducts.Where(p => p.Id == id)
                                                        .Select(p => $"ID:\t{p.Id}\nName:\t{p.Name}\nCost:\t{p.Cost}\n");
            Console.WriteLine(string.Join(Environment.NewLine, ProductsAsStrings)); 
        }
        public void GroupAndSortSales()
        {
            var Sales = (SalesRepository)allTables[1];
            var SalesGroups = Sales.AllSales.GroupBy(s => s.Date)
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
            var Products = (ProductsRepository)allTables[0];
            var Sales = (SalesRepository)allTables[1];
            var ProductAndSalesInfo = Products.AllProducts.Join(Sales.AllSales, p => p.Id, s => s.ProductId,
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
            var Products = (ProductsRepository)allTables[0];
            var Sales = (SalesRepository)allTables[1];
            var Purchases = (PurchasesRepository)allTables[2];
            var ProductsWithoutPurchasesOrSales = Products.AllProducts.GroupJoin(Purchases.AllPurchases, p => p.Id, pu => pu.ProductId,
                (p, purchasesGroup) => new
                {
                    p.Id,
                    p.Name,
                    ProductsPurchased = purchasesGroup.Count()

                })
                .GroupJoin(Sales.AllSales, p => p.Id, s=>s.ProductId,
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
            var Sales = (SalesRepository)allTables[1];
            var SumOfProductsSold = Sales.AllSales.Sum(s=>s.Total);
            Console.WriteLine($"Sum of all sales in the month: {SumOfProductsSold}");
        }
    }
}
