using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fruits
{
    public class ProductType
    {
        public int prt_id { get; set; }
        public string prt_name { get; set; }
    }
    
    public class WeightCost
    {
        public float weight { get; set; }
        public float cost { get; set; }
    }
}
