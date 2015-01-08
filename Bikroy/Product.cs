using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikroy
{
    public class Product
    {
        public String location { get; set; }
        public String category { get; set; }
        public String poster_name { get; set; }
        public String title { get; set; }
        public String URL { get; set; }
        //public String has_many_images { get; set; }
        //public String show_image { get; set; }

        public string show_attr { get; set; }
        //public String is_business { get; set; }
        //public String negotiable { get; set; }
        //public String featured { get; set; }
        //public String paid { get; set; }
        //public String is_bumped { get; set; }
        //public String is_topad { get; set; }
        //public String created_at { get; set; }
        //public String updated_at { get; set; }
        //public String published_at { get; set; }

        public string Email { get; set; }
        public string Phone { get; set; }
        public string Desc { get; set; }
        //public List<string> ImagePath { get; set; }
        public string ImageSrc { get; set; }
        public String ImageDir { get; set; }
        public Product()
        {
            //ImagePath = new List<string>();
            //show_attr = new List<ProductPrice>();
        }
    }
   public class ProductPrice
   {
       public string PropertyName { get; set; }
       public string PropertyValue { get; set; }
   }
   public class RootObject
   {
       public List<Product> listProduct { get; set; }
}
   }

