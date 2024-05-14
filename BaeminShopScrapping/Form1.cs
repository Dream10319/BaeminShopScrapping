using Nancy.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BaeminShopScrapping
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                Thread th = new Thread(new ThreadStart(() =>
                {
                    string filePath = @"locationinfo.txt"; // Adjust the path to where your file is stored.

                    // Regular expression to match one or more spaces or tabs
                    Regex delimiterRegex = new Regex(@"[\s\t]+");

                    // List to hold the coordinate tuples
                    List<(string Latitude, string Longitude)> coordinates = new List<(string, string)>();

                    // Read the file line by line
                    using (StreamReader reader = new StreamReader(filePath))
                    {
                        string line;
                        bool isFirstLine = true; // Variable to skip the header

                        while ((line = reader.ReadLine()) != null)
                        {
                            if (isFirstLine)
                            {
                                isFirstLine = false; // Skip the first line which is the header
                                continue;
                            }

                            // Split the line into latitude and longitude parts using the regex
                            string[] parts = delimiterRegex.Split(line.Trim());
                            if (parts.Length >= 2)
                            {
                                // Add the latitude and longitude as a tuple to the list
                                coordinates.Add((parts[0], parts[1]));
                            }
                        }
                    }

                    // Output the list of coordinates
                    int locationNum = 0;
                    long count = 296980;
                    foreach (var (Latitude, Longitude) in coordinates)
                    {
                        locationNum++;
                        this.Invoke(new Action(() =>
                        {
                            Lat.Text = Latitude.ToString();
                            Lon.Text = Longitude.ToString();
                            LocationNum.Text = "Location" + locationNum.ToString();
                        }));
                        File.WriteAllText("log.txt", Environment.NewLine + Lat.Text + ", " + Lon.Text + "   count: " + (count + 1).ToString());
                        int shopcount = 2000;
                        int reviewcount = 0;
                        for (int i = 0; i < (int)(shopcount / 25); i++)
                        {
                            string strUrl = string.Format("https://shopdp-api.baemin.com/v3/BAEMIN_ONE_HOME/shops?displayCategory=BAEMIN_ONE_HOME_ALL&latitude={0}&longitude={1}&sort=SORT__DEFAULT&filter=&distance=3&offset={2}&limit=25&extension=1FYh_P4msOMcOUJVrQHAutAcO8q-i_jL8vfzU6-yoY06oOalI5Uww5eaD5RCu1oFaUuevU1iQ_4cNaVteAvSVH9vWZTM5qD-NVSEAq1v3mww5z7yTDRUkhBqZbXB4cSd-h-CzX59vdNs8ncfchvDH3QzWF57VK7ZlCTjzbuc80tX6msJ_foLEHgRUTMHmaRzH0rY7AuaY4hIg95XXswTtO2KNj7YzZN81yrfO-I-l8-Qjxx68aMk6Xrd3gKrSxWj4VkanX8EQ295ht_BebJsWZ5WoAhG5qRhyB-JH14n2-7Vl2z2nsFc39bCRw5u7Yet_3xm187Vy0Grw9qm36vSglgUz23FQjgZT7blmdNb2G6Y_xke8li7u2w8Qdhq65qCyr3vSUidcNZduENiR1VFeSe-dTW4AOnqvEYenaODcnM2oE6VFFFJw-IM6RtDhr0nB7CLaEd8A8yi5J0mFE04JD8OM7xhD9jwWkxZvP9BTaRB2OGz0qfS9uMTQxmv7SbQ6GIytfUbk-cnbTkq8WgdGKxuOQEj0StijolrbVxhKcm5BK6FlrvXCSVcD6rGB3pRESbuePQXg87PX0XQjvMn2u1EOBTnyk_o4hJRH2erOQGK2Unnxumzkq_yt30Q9q_1sOU7oE1yCZ21aS_w9oPAljqoaytx8Jpe2JQYMQB3Bdin2rmke-PS5IJ1IoErC6vhl1ZbOFSIN3sUHjAqoqD--2kj3MfnEgjN7rltgV_cfUxUusJkrro_jNVl3_IRvwGA21s0C6xHnErKredLlPexA6BRPjjU-4dU59wmY77OdCFnTzudEE0nRfd3o5Sw_fbmLIUUY5K6FVsek5t629P52889PO1OT8igq_XEA93nA_MIpbO3cPNim0drmqvgUgvCIXvRE5LcbLXgM24VHGcHW2wbfzwGGtaKyjnSkeG_UaCz8JuO7_sYrwdg3_Fn1lRckyBZoe_acuSQFDPPYbZutlq-BQ33e8I3UaTk_ZxIjHohof4e2nuUrpCTYV2fddlktgWlkWg1ZPgywRZYVN2lxH0gv6-Jwwg_Vro1SgxzLmBl_msIAbk4MhKKuI8z4mwzBtRhcYATrFWBlNOmiMoNt1SQcmYwX-WIDWA0GW70lr7ueri2GcxhLtb1hVc2FUHn7tMVrAyXk32sAJk2hasLx_JCvgqtsaRv-OIdQkkl6Zly3jjuJP9IIjqh8wuKjiWx_ugxy7FkRPKtyFmmIOOugEFozaIvSPHQdjeDHInevpX0wWuAT5f99HoCtn3-EJqzHgd4XX4ukSy-I8vhoUJJ39euZ3FFbpJ89qgi-gCJtd6rOak9VBHadM9IILbxWY6zQGOVeHiV-IMOBmJiMSsSuGP_jlWyPkZiyXMW0TdAPKxBqwGeuOktph-P-xNSJu9ChUQ23LGc_f6lv8NATU00TTSlIoanYukD88EcyWsZvtMsc0jnOeYepS3_POo0JJDaIhzCIoeXCET5qiEzuF0RxZxiEVulSpWFTkOSEKM-fRV9-yXhn7OmjPeze4hihopTAowZb4oqNwjSwnrMof1HG-xfK33rNNmM4E2N2TX_d54QAXd4AQE7uA62SnwLXv1ucEHf6iwkTpPk2wIqrBVQ52WjTuwnvYidN4WLmgmfoVPBhNo3RoWWjkTTIRjD1esH72QREqJ_BI8lm3PtqDntnGf34WrFB8diom-ij6BjEB5JcM0BiYZmlzwCz2yRVpCkZsHiJUIiZ3aUyMvJnbTBy9IUptfNBlt1WUA-wKQLAIx5q3pR6RAYuIJICKbstAZ7sLGLnTkfAUproYg7nEstxPyOu2miNhNkHhJ6hYuvvWCl2yPhIePyFU2TXUh-vI9qhWJaa2k-BiUza8ybuhRWIvDR-_UTxNq__YlAownzpvceTrxqnqoWA-9AQcMIJck0qZogymnC8ws_FYmFnECkKmWZLdvC8ZMotyoYbfG4T33LFxXBG5Bt5Z1i93u2ye9yqIkKBTf-1PpvCPZ-P7Xn62vZp1_9TmSPeLymocGBMeybNZxXW6AerttmiIrWBUFvQD17l1QRkyWZKakhRFJtYRy7EjENp1z3qkQCVhNGuwVkcb6kiFE-f2edmomE1XKi1rBhO82Vn3fCJUrJ7RORRAr6urSUmCnyBlgj2XDA2oBs3d-SlM8f0kN-cvMsj6_Nvy1pMYiKXnjl0Gp2hC0wBCJJshwLquHKUTmxyQxTX4MGRDdDctERZP_y39wpYpSg&perseusSessionId=1713643971490.651434705214992219.SrofotoxLE&memberNumber=000000000000&sessionId=34d130d02c97878260355f77f8&carrier=&site=7jWXRELC2e&dvcid=OPUD3ae65d495619f1bc&adid=cc6caa45-4929-4445-ae3b-4ec047f2d35c&deviceModel=CPH1823&appver=12.15.0&oscd=2&osver=32&dongCode=11140112&zipCode=04529", Lat.Text, Lon.Text, 25 * i);
                            var client = new RestClient(strUrl);
                            var request = new RestRequest();
                            request.AddHeader("Accept-Encoding", "gzip, deflate");
                            request.AddHeader("Connection", "Keep-Alive");
                            request.AddHeader("Host", "shopdp-api.baemin.com");
                            request.AddHeader("User-Agent", "and1_12.15.0");
                            request.AddHeader("USER-BAEDAL", "5NcSD/b68BGSy+oL0soV4ZfI1zMjFb66m5fAaVhfwkqehqmm1VKuSu6QLV/ywRNdPKvFwHecyiVhezXwhHVQEsYJJY28heF75jt87YBZ5jJMxYp7Cs4bfTgFE1WW9H3FTWH/t5Z+YVxnG5JTj57jE0fJC21uf0TjStlUnMuA9fNzyL+lxU9CZTUvsg9M1u9U");
                            string strReturn = client.ExecuteGet(request).Content;
                            JavaScriptSerializer jss2 = new JavaScriptSerializer();
                            dynamic data2 = jss2.Deserialize<dynamic>(strReturn);
                            dynamic shops = data2["data"]["shops"];
                            shopcount = (int)data2["data"]["totalCount"];
                            foreach (dynamic shop in shops)
                            {
                                reviewcount++;
                                string shopnumber = shop["shopInfo"]["shopNumber"].ToString();
                                string scrapUrl = string.Format("https://review-api.baemin.com/v1/shops/{0}/reviews?sort=MOST_RECENT&filter=ALL&offset=0&limit=25&sessionId=&carrier=&site=7jWXRELC2e&dvcid=OPUD4bf43dcba41eff4a&adid=NONE&deviceModel=CPH1823&appver=12.15.0&oscd=2&osver=32&dongCode=11140112&zipCode=04529", shopnumber);
                                var client1 = new RestClient(scrapUrl);
                                var request1 = new RestRequest();
                                request1.AddHeader("Accept-Encoding", "gzip, deflate");
                                request1.AddHeader("Connection", "Keep-Alive");
                                request1.AddHeader("Host", "review-api.baemin.com");
                                request1.AddHeader("User-Agent", "and1_12.15.0");
                                request1.AddHeader("Authorization", "bearer guest");
                                request1.AddHeader("USER-BAEDAL", "5NcSD/b68BGSy+oL0soV4ZfI1zMjFb66m5fAaVhfwkqehqmm1VKuSu6QLV/ywRNdPKvFwHecyiVhezXwhHVQEsYJJY28heF75jt87YBZ5jJMxYp7Cs4bfTgFE1WW9H3FTWH/t5Z+YVxnG5JTj57jE0fJC21uf0TjStlUnMuA9fNzyL+lxU9CZTUvsg9M1u9U");
                                string strReturn1 = client1.ExecuteGet(request1).Content;
                                if (!strReturn1.Contains("reviews")) continue;
                                count++;
                                var dir = @"C:\Review";
                                Directory.CreateDirectory(dir);
                                File.WriteAllText(string.Format(@"{0}\review-2-{1}.json", dir, count), strReturn1);
                                this.Invoke(new Action(() =>
                                {
                                    progressBar1.Value = (int)(10000 * reviewcount / shopcount);
                                }));
                            }
                        }

                    }
                    MessageBox.Show("Successfully done!!!");
                }));
                th.Start();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
                MessageBox.Show(LocationNum.Text);
            }
        }
    }
}
