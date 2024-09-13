using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web_253505_Tarhonski.Domain.Entities
{
    public class Category
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string NormalizedName { get; set; }
        public List<Airsoft> AirsoftObjects { get; set; }
        public Category()
        {
            AirsoftObjects = new List<Airsoft>();
        }
    }
}
