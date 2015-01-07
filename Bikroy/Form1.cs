using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using System.Collections.Specialized;
using HtmlAgilityPack;
using System.Reflection;
namespace Bikroy
{
    public partial class Form1 : Form
    {
        private static Browser browser = new Browser();
        public Form1()
        {
            InitializeComponent();
        }

        private void ScrapProcess()
        {

            List<Product> Products = new List<Product>();           
            int FromPage = Convert.ToInt32(txtfrom.Text);
            int ToPages = Convert.ToInt32(txtTo.Text);

            for (int i = FromPage; i <= ToPages; i++)
            {
                if (i <= 1)
                    browser.Url = "http://bikroy.com/en/ads-in-bangladesh?_=1420294568273";
                else
                    browser.Url = "http://bikroy.com/en/ads-in-bangladesh?page=" + i.ToString() + "&_=1420294568273";


                Console.Write("Please Wait.....");
                String response = browser.AjaxPost();

                String productsJson = browser.parseJson(response)["ads"].ToString().ToString();

                Newtonsoft.Json.Linq.JArray a = Newtonsoft.Json.Linq.JArray.Parse(productsJson);
                foreach (Newtonsoft.Json.Linq.JObject o in a.Children<Newtonsoft.Json.Linq.JObject>())
                {
                    Product oProduct = new Product();
                    foreach (Newtonsoft.Json.Linq.JProperty p in o.Properties())
                    {
                        string name = p.Name;
                        string value = p.Value.ToString();
                        if (name == "location")
                        {
                            oProduct.location = p.Value.ToString();
                        }
                        if (name == "category")
                        {
                            oProduct.category = p.Value.ToString();
                        }
                        if (name == "slug")
                        {
                            string url = "http://bikroy.com/en/" + p.Value.ToString();
                            oProduct.URL = url;
                            oProduct = parsepage(url, oProduct);
                        }
                        if (name == "poster_name")
                        {
                            oProduct.poster_name = p.Value.ToString();
                        }
                        if (name == "title")
                        {
                            oProduct.title = p.Value.ToString();
                        }
                        //if (name == "show_image")
                        //{
                        //    oProduct.show_image = p.Value.ToString();
                        //}
                        if (name == "show_attr")
                        {
                            oProduct.show_attr = p.Value.ToString().Replace("{", "").Replace("}", "").Replace('"', ' ').Replace(" value :", "").Trim();

                        }
                    }

                    Products.Add(oProduct);
                }
            }

            //Export to Excel
            var wb = new ClosedXML.Excel.XLWorkbook();
            DataTable dt = Products.ToDataTable();

            // Add a DataTable as a worksheet
            wb.Worksheets.Add(dt, "Report");
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Excel Documents (*.xlsx)|*.xlsx";
            sfd.FileName = "Report.xlsx";
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                //ToCsV(dataGridView1, @"c:\export.xls");
                wb.SaveAs(sfd.FileName); // Here dataGridview1 is your grid view name 
            }
            //
        }

        private void btnTest_Click(object sender, EventArgs e)
        {

            ScrapProcess();

            //List<Product> Products = new List<Product>();
            //String criteria = "";
            //browser.Url = "http://bikroy.com/en/ads-in-bangladesh?_=1420294568273";
            //Console.Write("Please Wait.....");
            //String response = browser.AjaxPost();
            ////String productsJson = browser.parseJson(browser.parseJson(browser.parseJson(response)["ads"].ToString())["Response"].ToString())["Products"].ToString();
            //String productsJson = browser.parseJson(response)["ads"].ToString().ToString();
            //// Product oProducts = JsonConvert.DeserializeObject<Product>(productsJson);

            ////int CurrentPage = Convert.ToInt32(browser.parseJson(productsJson)["CurrentPage"].ToString());
            ////int TotalPages = Convert.ToInt32(browser.parseJson(productsJson)["TotalPages"].ToString());

            ////productsJson = browser.parseJson(productsJson).ToString();
            //Newtonsoft.Json.Linq.JArray a = Newtonsoft.Json.Linq.JArray.Parse(productsJson);
            //foreach (Newtonsoft.Json.Linq.JObject o in a.Children<Newtonsoft.Json.Linq.JObject>())
            //{
            //    Product oProduct = new Product();
            //    foreach (Newtonsoft.Json.Linq.JProperty p in o.Properties())
            //    {
            //        string name = p.Name;
            //        string value = p.Value.ToString();
            //        if (name == "location")
            //        {
            //            oProduct.location = p.Value.ToString();
            //        }
            //        if (name == "category")
            //        {
            //            oProduct.category = p.Value.ToString();
            //        }
            //        if (name == "slug")
            //        {                        
            //            string url = "http://bikroy.com/en/" + p.Value.ToString();
            //            oProduct.URL = url;
            //            oProduct = parsepage(url, oProduct);
            //        }
            //        if (name == "poster_name")
            //        {
            //            oProduct.poster_name = p.Value.ToString();
            //        }
            //        if (name == "title")
            //        {
            //            oProduct.title = p.Value.ToString();
            //        }
            //        //if (name == "show_image")
            //        //{
            //        //    oProduct.show_image = p.Value.ToString();
            //        //}
            //        if (name == "show_attr")
            //        {
            //            oProduct.show_attr = p.Value.ToString().Replace("{", "").Replace("}", "").Replace('"', ' ').Replace(" value :", "").Trim();

            //        }
            //    }

            //    Products.Add(oProduct);
            //}

            //////Get other details
            ////if (Products.Count() > 0)
            ////{
            ////    foreach (Product p in Products)
            ////    {
            ////        string url = "http://bikroy.com/en/" + p.slug;
            ////        parsepage(url,p);
            ////    }
            ////}
            //var wb = new ClosedXML.Excel.XLWorkbook();
            //DataTable dt = Products.ToDataTable();   

            //// Add a DataTable as a worksheet
            //wb.Worksheets.Add(dt, "Report");
            //SaveFileDialog sfd = new SaveFileDialog();
            //sfd.Filter = "Excel Documents (*.xlsx)|*.xlsx";
            //sfd.FileName = "Report.xlsx";
            //if (sfd.ShowDialog() == DialogResult.OK)
            //{
            //    //ToCsV(dataGridView1, @"c:\export.xls");
            //    wb.SaveAs(sfd.FileName); // Here dataGridview1 is your grid view name 
            //}
            //Application.Exit();
        }

        private Product parsepage(string url, Product oProduct)
        {
            //Product oProduct = new Product();
            browser.Url = url;
            var doc = browser.GetWebRequest();
            HtmlNode node = doc.DocumentNode.SelectSingleNode("//div[@class='item-description copy']/p");
            oProduct.Desc =node.InnerText;

            HtmlNodeCollection linkNodes1 = doc.DocumentNode.SelectNodes("//div[@class='attr']");
            if (linkNodes1 != null)
            {
                foreach (HtmlNode linkNode in linkNodes1)
                {
                    HtmlNode labelNode = linkNode.SelectSingleNode(".//span[@class='label']");
                    if (labelNode.InnerText == "Location:")
                    {
                        HtmlNode valueNode = linkNode.SelectSingleNode(".//span[@class='value']");
                        oProduct.location = valueNode.InnerText;
                    }
                }
            }
            //if (oProduct.show_image == "true")
            //{
                HtmlNode node1 = doc.DocumentNode.SelectSingleNode("//div[@class='number']");
                oProduct.Phone = node1.InnerText;
                HtmlNodeCollection linkNodes = doc.DocumentNode.SelectNodes("//div[@class='item']");
                // HtmlNode node2 = doc.DocumentNode.SelectSingleNode("//div[@class='frame']//img/@src");
                //oProduct.ImagePath = new List<string>();
                if (linkNodes!=null)
                {
                    foreach (HtmlNode linkNode in linkNodes)
                    {
                        HtmlNode imageNode = linkNode.SelectSingleNode(".//img");
                        HtmlAttribute att = imageNode.Attributes["data-src"];
                        if (att != null)
                        {
                            //oProduct.ImagePath.Add("http://bikroy.com/" + att.Value);
                            oProduct.ImageSrc="http://bikroy.com/" + att.Value;
                            break;
                        }
                    }
                }
            //}
            return oProduct;
        }

        

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }
    }
}
