using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Practice4
{
    public interface ISingleObject { }

    public class Product : ISingleObject
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Cost { get; set; }

        public Product(int id, string name, decimal cost)
        {
            Id = id;
            Name = name;
            Cost = cost;
        }

    }
}
