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
using MySql.Data.MySqlClient;

namespace Bikroy
{
    public partial class Form1 : Form
    {
        private static Browser browser = new Browser();
        public Form1()
        {
            InitializeComponent();
        }

        #region [Main Scraper]	
        
        private void ScrapProcess()
        {
            try
            {
                List<Product> Products = new List<Product>();
                int totalpage = 0;
                int FromPage = Convert.ToInt32(txtfrom.Text);
                int ToPages = Convert.ToInt32(txtTo.Text);

                browser.Url = "http://bikroy.com/en/ads-in-bangladesh?_=1420294568273";
                String response = browser.AjaxPost();
                String productsJson = browser.parseJson(response)["tabs"].ToString().ToString();
                totalpage = Convert.ToInt32(browser.parseJson(productsJson)["all"].ToString());
               
                //if ((totalpage % 27)>0)
                //    ToPages = (totalpage / 27)+1;
                //else
                //    ToPages = totalpage / 27;

                for (int i = FromPage; i <= ToPages; i++)
                {
                    if (i <= 1)
                        browser.Url = "http://bikroy.com/en/ads-in-bangladesh?_=1420294568273";
                    else
                        browser.Url = "http://bikroy.com/en/ads-in-bangladesh?page=" + i.ToString() + "&_=1420294568273";


                    Console.Write("Please Wait.....");
                    response = browser.AjaxPost();

                    productsJson = browser.parseJson(response)["ads"].ToString().ToString();

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
                                oProduct.slug = p.Value.ToString();
                                oProduct.ImageDir = p.Value.ToString();
                                oProduct.URL = url;
                               
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
                        if (CatagoryID(oProduct.category) > 0)
                        {
                            oProduct = parsepage(oProduct.URL, oProduct);
                            InsertToDatabase(oProduct);
                            Products.Add(oProduct);
                        }
                    }

                }

                ////Export to Excel
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
                //
            }
            catch(Exception ex) {
                Utility.ErrorLog(ex, null);
            }
        }
        #endregion

        private void btnTest_Click(object sender, EventArgs e)
        {

            ScrapProcess();

        }

        #region [Details Page Scrap]
       
        private Product parsepage(string url, Product oProduct)
        {
            try
            {
                //Product oProduct = new Product();
                browser.Url = url;
                var doc = browser.GetWebRequest();
                HtmlNode node = doc.DocumentNode.SelectSingleNode("//div[@class='item-description copy']/p");
                if (node != null)
                {
                    oProduct.Desc = node.InnerText;
                    oProduct.Email = emas(oProduct.Desc);
                    oProduct.Website = MakeLink(oProduct.Desc);
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
                            oProduct.Address = valueNode.InnerText;
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
                            oProduct.ImagePath.Add(oProduct.ImageSrc);
                            // SaveProductBigImage(oProduct.ImageSrc, oProduct);
                            //break;
                        }
                    }
                }
                else
                {
                    oProduct.ImageDir = "";
                }
            }
            catch (Exception ex)
            {
                Utility.ErrorLog(ex, null);
            }
            return oProduct;
        }
        #endregion

        #region [Image process]

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
        private String CreateProductDirectory(string id, Product oProduct)
        {
            //String ValidDirName = GetValidDirName(oProduct.ImageDir);
            String DirName;

            DirName = String.Format("{0}", GetImageFolder(id));
            if (!Directory.Exists(DirName))
                Directory.CreateDirectory(DirName);
            return DirName;
        }

        private String GetImageFolder(string id)
        {
            String FolderName = String.Format("{0}\\image\\" + System.DateTime.Now.Year.ToString() + "\\" + System.DateTime.Now.Month.ToString() + "\\" + System.DateTime.Now.Day.ToString() + "\\" + id, Application.StartupPath);            
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

        private void SaveProductBigImage(string id, Product oProduct)
        {


            if (oProduct.ImagePath.Count > 0)
            {

                foreach (string s in oProduct.ImagePath)
                {
                    if (s.Length != 0)
                    {
                        String ImageFileName = GetFileAndExtension(s);
                        String FileName = String.Format("{0}\\{1}",CreateProductDirectory(id, oProduct), ImageFileName);
                        browser.Url = (s.StartsWith("http:") ? s : "http:\\");
                        browser.DownloadFile(FileName);
                    }
                }
            }
        }
        #endregion

        #region [Email]
      
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
        #endregion

        #region [WEB]

        protected string MakeLink(string txt)
        {
            string webaddress = string.Empty;
            Regex urlRx = new
            Regex(@"(?<url>(http:|https:[/][/]|www.)([a-z]|[A-Z]|[0-9]|[/.]|[~])*)",
            RegexOptions.IgnoreCase);

            MatchCollection matches = urlRx.Matches(txt);
            foreach (Match match in matches)
            {
                webaddress=match.Value;
                break;
            }
           
            return webaddress;
        }
        #endregion

        #region [Catagory ID]
        private int CatagoryID(string category)
        {
            int catID = 0;           
            MyTable oMyTable = new MyTable();
            List<KeyValuePair<string, string>> catagoryList = new List<KeyValuePair<string, string>>();
            catagoryList = oMyTable.CatagoryList();
            foreach (KeyValuePair<string, string> kvp in catagoryList)
            {
                if (kvp.Key == category)
                    catID = int.Parse(kvp.Value);
            }
            return catID;
        }
        #endregion
        #region [Data Insert]
        private void InsertToDatabase(Product oProduct)
        {
            try
            {
                int catID = 0;
                int lID = 0;
                setting s = new setting();
                MyTable oMyTable = new MyTable();
                catID = CatagoryID(oProduct.category);
                //string catagory = "select id_category from oc_categories where name='" + oProduct.category + "'";
                //DataTable dt = s.selectAllfromDatabaseAndReturnDataTable(catagory);
                //if (dt.Rows.Count > 0)
                //{
                //    catID = int.Parse(dt.Rows[0][0].ToString());
                //}
                //else
                //{
                //    catagory = "INSERT INTO barua910_oc1.oc_categories(`name`,`order`,created,id_category_parent,parent_deep,seoname,description,price,last_modified,has_image)VALUES('" + oProduct.category + "',0,CURTIME(),0,0,'" + oProduct.category + "','" + oProduct.category + "',0,NOW(),0)";
                //    int k = s.InsertOrUpdateOrDeleteValueToDatabase(catagory);
                //    catagory = "select id_category from oc_categories where name='" + oProduct.category + "'";
                //    dt = s.selectAllfromDatabaseAndReturnDataTable(catagory);
                //    if (dt.Rows.Count > 0)
                //    {
                //        catID = int.Parse(dt.Rows[0][0].ToString());
                //    }
                //}

                List<KeyValuePair<string, string>> LocationList = new List<KeyValuePair<string, string>>();
                LocationList = oMyTable.LocationList();
                foreach (KeyValuePair<string, string> kvp in LocationList)
                {
                    if(kvp.Key==oProduct.location)
                        lID=int.Parse(kvp.Value);                    
                }               
                //string location = "select id_location from oc_locations where name='" + oProduct.location + "'";
                //DataTable dtL = s.selectAllfromDatabaseAndReturnDataTable(location);
                //if (dtL.Rows.Count > 0)
                //{
                //    lID = int.Parse(dtL.Rows[0][0].ToString());
                //}
                //else
                //{
                //    location = "INSERT INTO barua910_oc1.oc_locations(`name`,`order`,id_location_parent,parent_deep,seoname,description,last_modified,has_image)VALUES('" + oProduct.location + "',0,0,0,'" + oProduct.location + "','" + oProduct.location + "',NOW(),0);";
                //    int k = s.InsertOrUpdateOrDeleteValueToDatabase(location);
                //    location = "select id_location from oc_locations where name='" + oProduct.location + "'";
                //    dtL = s.selectAllfromDatabaseAndReturnDataTable(location);
                //    if (dtL.Rows.Count > 0)
                //    {
                //        lID = int.Parse(dtL.Rows[0][0].ToString());
                //    }
                //}

                if (catID > 0 && lID>0)
                {
                    decimal price = 0;
                    string p = oProduct.show_attr.Replace("Tk.", "");
                    if (p != "Negotiable price")
                    {
                        if (Decimal.TryParse(p, out price))
                            price = decimal.Parse(p);
                    }
                    string ads = "Select * from oc_ads where seotitle='" + oProduct.slug + "'";
                    DataTable dta = s.selectAllfromDatabaseAndReturnDataTable(ads);
                    if (dta.Rows.Count > 0)
                    { }
                    else
                    {
                        ads = "INSERT INTO barua910_oc1.oc_ads(id_user,id_category,id_location,title,seotitle,description,address,price,phone,website,ip_address,created,published,featured,last_modified,status,has_images,stock,rate)VALUES(1," + catID + "," + lID + ",'" + oProduct.title + "','" + oProduct.slug + "','" + oProduct.Desc + "','" + oProduct.Address + "'," + price + ",'" + oProduct.Phone + "','" + oProduct.Website + "',0,NOW(),NOW(),NOW(),NOW(),1,"+oProduct.ImagePath.Count+",0,0);";
                        int k2 = s.InsertOrUpdateOrDeleteValueToDatabase(ads);
                        if (k2 > 0)
                        {
                            ads = "Select id_ad from oc_ads where website='" + oProduct.URL + "'";
                            dta = s.selectAllfromDatabaseAndReturnDataTable(ads);
                            if (dta.Rows.Count > 0)
                            {
                                SaveProductBigImage(dta.Rows[0][0].ToString(), oProduct);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Utility.ErrorLog(ex, null);
            }
        }
        #endregion
        private void Form1_Load(object sender, EventArgs e)
        {
           
        }
    }
}
