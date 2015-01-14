using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikroy
{
   public class MyTable
    {
       public List<KeyValuePair<string, string>> LocationList()
       {
           List<KeyValuePair<string, string>> kvpList = new List<KeyValuePair<string, string>>()
{
  new KeyValuePair<string, string>("Dhaka","3"),
new KeyValuePair<string, string>("Dhaka Division","3"),
new KeyValuePair<string, string>("Chittagong","4"),
new KeyValuePair<string, string>("Chittagong Division","4"),
new KeyValuePair<string, string>("Khulna","5"),
new KeyValuePair<string, string>("Khulna Division","5"),
new KeyValuePair<string, string>("Rajshahi Division","6"),
new KeyValuePair<string, string>("Rajshahi","6"),
new KeyValuePair<string, string>("Barisal","7"),
new KeyValuePair<string, string>("Barisal Division","7"),
new KeyValuePair<string, string>("Rangpur Division","8"),
new KeyValuePair<string, string>("Rangpur","8"),
new KeyValuePair<string, string>("Sylhet","9"),
new KeyValuePair<string, string>("Sylhet Division","9"),


};
           return kvpList;
       }

       public List<KeyValuePair<string, string>> CatagoryList()
       {
           List<KeyValuePair<string, string>> kvpList = new List<KeyValuePair<string, string>>()
{
  new KeyValuePair<string, string>("Accounting & Finance","2"),
new KeyValuePair<string, string>("Secretary & Office Admin","2"),
new KeyValuePair<string, string>("Agriculture","2"),
new KeyValuePair<string, string>("Design, Art & Photography","2"),
new KeyValuePair<string, string>("Civil Engineering, Construction & Technician","2"),
new KeyValuePair<string, string>("Customer Service","2"),
new KeyValuePair<string, string>("Teaching","2"),
new KeyValuePair<string, string>("Engineering & Architecture","2"),
new KeyValuePair<string, string>("Textile & Garments","2"),
new KeyValuePair<string, string>("Food & Catering","2"),
new KeyValuePair<string, string>("Hotels & Tourism","2"),
new KeyValuePair<string, string>("Household Help","2"),
new KeyValuePair<string, string>("IT & Telecom","2"),
new KeyValuePair<string, string>("Legal","2"),
new KeyValuePair<string, string>("Management Consulting","2"),
new KeyValuePair<string, string>("Manufacturing","2"),
new KeyValuePair<string, string>("Marketing & Advertising","2"),
new KeyValuePair<string, string>("Medical & Biotech","2"),
new KeyValuePair<string, string>("Sales & Retail","2"),
new KeyValuePair<string, string>("Security","2"),
new KeyValuePair<string, string>("Driver & Transportation","2"),
new KeyValuePair<string, string>("Work Overseas","2"),
new KeyValuePair<string, string>("General Labour","2"),
new KeyValuePair<string, string>("Sylhet Division","9"),
new KeyValuePair<string, string>("Other","4"),
new KeyValuePair<string, string>("Apartments & Flats","18"),
new KeyValuePair<string, string>("Plots & Land","19"),
new KeyValuePair<string, string>("Rooms","20"),
new KeyValuePair<string, string>("Houses","20"),
new KeyValuePair<string, string>("TVs","21"),
new KeyValuePair<string, string>("Speakers & Sound Systems","22"),
new KeyValuePair<string, string>("iPod & MP3 Players","22"),
new KeyValuePair<string, string>("Other Audio & MP3","22"),
new KeyValuePair<string, string>("Bedroom Furniture","23"),
new KeyValuePair<string, string>("Chairs & Tables","23"),
new KeyValuePair<string, string>("Shelves & Storage","23"),
new KeyValuePair<string, string>("Living Room Furniture","23"),
new KeyValuePair<string, string>("Textiles, Carpets & Decorations","23"),
new KeyValuePair<string, string>("Lighting","23"),
new KeyValuePair<string, string>("Antiques & Art","23"),
new KeyValuePair<string, string>("TV & Stereo Furniture","23"),
new KeyValuePair<string, string>("Other Furniture","23"),
new KeyValuePair<string, string>("Computer Accessories","36"),
new KeyValuePair<string, string>("Laptops & Netbooks","36"),
new KeyValuePair<string, string>("Desktop Computers","36"),
new KeyValuePair<string, string>("Tablets & Accessories","36"),
new KeyValuePair<string, string>("Cars","29"),
new KeyValuePair<string, string>("Motorbikes & Scooters","30"),
new KeyValuePair<string, string>("Bicycles","31"),
new KeyValuePair<string, string>("Trucks, Vans & Buses","32"),
new KeyValuePair<string, string>("Trucks, Vans & Buses","33"),
new KeyValuePair<string, string>("Heavy-Duty Vehicles","35"),
new KeyValuePair<string, string>("Electronics","43"),
new KeyValuePair<string, string>("Mobile Phones","46"),
new KeyValuePair<string, string>("Music & Instruments","50"),
new KeyValuePair<string, string>("Pets","53"),
new KeyValuePair<string, string>("Other Pets & Animals","53"),
new KeyValuePair<string, string>("Clothes, Footwear & Accessories","59"),

};
           return kvpList;
       }
    }
}
