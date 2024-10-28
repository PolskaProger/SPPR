using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web_253505_Tarhonski.Domain.Entities
{
    public class Airsoft
    {
        public Guid ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Category? Category { get; set; }
        public Guid? CategoryId { get; set; }
        public decimal Price { get; set; }
        public string ImagePath { get; set; }
    }
}
