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
using System.IO;
using System.Text.RegularExpressions;
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
                            oProduct.ImageDir = p.Value.ToString();
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
            if (node != null)
            {
                oProduct.Desc = node.InnerText;
                oProduct.Email = emas(oProduct.Desc);
            }

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
                if (node1 != null)
                oProduct.Phone = node1.InnerText;

                HtmlNodeCollection linkNodes = doc.DocumentNode.SelectNodes("//div[@class='thumbs']//a");
                // HtmlNode node2 = doc.DocumentNode.SelectSingleNode("//div[@class='frame']//img/@src");
                //oProduct.ImagePath = new List<string>();
                if (linkNodes != null)
                {
                    foreach (HtmlNode linkNode in linkNodes)
                    {
                        HtmlNode imageNode = linkNode.SelectSingleNode(".//img");
                        HtmlAttribute att = imageNode.Attributes["src"];
                        if (att != null)
                        {
                            //oProduct.ImagePath.Add("http://bikroy.com/" + att.Value);
                            oProduct.ImageSrc = "http://bikroy.com/" + att.Value;
                            SaveProductBigImage(oProduct.ImageSrc, oProduct);
                            //break;
                        }
                    }
                }
                else
                {
                    oProduct.ImageDir = "";
                }
            //}
            return oProduct;
        }

        public static String GetValidDirName(String DirName)
        {
            try
            {
                //com1, com2, com3, com4, com5, com6, com7, com8, com9, lpt1, lpt2, lpt3, lpt4, lpt5, lpt6, lpt7, lpt8, lpt9, con, nul, and prn
                DirName = DirName.Replace(" ", "_");
                DirName = DirName.Replace("/", "_");
                DirName = DirName.Replace("?", "_");
                DirName = DirName.Replace("<", "_");
                DirName = DirName.Replace(">", "_");
                DirName = DirName.Replace("\\", "_");
                DirName = DirName.Replace(":", "_");
                DirName = DirName.Replace("*", "_");
                DirName = DirName.Replace("|", "_");
                DirName = DirName.Replace("\"", "_");

                return DirName;
            }
            catch
            {
                return "";
            }
        }
        private String CreateProductDirectory(Product oProduct)
        {
            String ValidDirName = GetValidDirName(oProduct.ImageDir);
            String DirName;

            DirName = String.Format("{0}\\{1}", GetImageFolder(), ValidDirName);
            if (!Directory.Exists(DirName))
                Directory.CreateDirectory(DirName);
            return ValidDirName;
        }

        private String GetImageFolder()
        {
            String FolderName = String.Format("{0}\\Image", Application.StartupPath);            
            return FolderName;
        }
        public static String GetFileExtension(string FileName)
        {
            try
            {
                return FileName.Substring(FileName.LastIndexOf('.') + 1);
            }
            catch
            {
                return "";
            }
        }

        public static String GetFileAndExtension(string FileName)
        {
            try
            {
                return FileName.Substring(FileName.LastIndexOf('/') + 1);
            }
            catch
            {
                return "";
            }
        }

        private String SaveProductBigImage(string SmallImage, Product oProduct)
        {


            if (SmallImage == null || SmallImage.Length == 0)
                return null;

            String ImageFileName = GetFileAndExtension(SmallImage);
            // Big Image

            String FileName = String.Format("{0}\\{1}\\{2}", GetImageFolder(), CreateProductDirectory(oProduct), ImageFileName);
            browser.Url = (SmallImage.StartsWith("http:") ? "" : "\\images\\gallerythumb") + SmallImage;
            browser.DownloadFile(FileName);

            return ImageFileName;
        }

        public string emas(string text)
        {
            string email = string.Empty;
            const string MatchEmailPattern =
           @"(([\w-]+\.)+[\w-]+|([a-zA-Z]{1}|[\w-]{2,}))@"
           + @"((([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\."
             + @"([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])){1}|"
           + @"([a-zA-Z]+[\w-]+\.)+[a-zA-Z]{2,4})";
            Regex rx = new Regex(MatchEmailPattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);
            // Find matches.
            MatchCollection matches = rx.Matches(text);
            // Report the number of matches found.
            int noOfMatches = matches.Count;
            // Report on each match.
            foreach (Match match in matches)
            {
                email=match.Value.ToString();
                break;
            }
            return email;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }
    }
}
