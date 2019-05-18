using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace China_AreaCode_data
{
    public class ChinaAreaCrawer
    {
        //国家民政部官网http://www.mca.gov.cn/article/sj/xzqh/2019/
        public static string url = "http://www.mca.gov.cn/article/sj/xzqh/2019/201901-06/201904301706.html";

        public static string CreateJson()
        {
            List<AreaConfig> result = new List<AreaConfig>();
            Dictionary<string, Dictionary<string, string>> resultJSON = new Dictionary<string, Dictionary<string, string>>();
            var response = CrawlerHttpUtil.HttpGetRequest(url, null, null, null);
            var response_Str = CrawlerHttpUtil.GetResponseStreamToStr(response);
            try
            {
                HtmlDocument doc = new HtmlDocument();
                doc.LoadHtml(response_Str);
                HtmlNode rootNode = doc.DocumentNode;
                var nodes = rootNode.SelectNodes("//tr[@height='19']");
                if (nodes != null && nodes.Any())
                {
                    HtmlAttribute att = null;
                    foreach (var elem in nodes)
                    {
                        var s = elem.SelectNodes(".//td[@class='xl7032454']");
                        result.Add(new AreaConfig()
                        {
                            AreaCode = s.First().InnerText,
                            AreaName = s.Last().InnerText
                        });
                    }
                }
                //省份包括直辖市
                var provList = result.Where(p => p.AreaCode.Substring(2) == "0000").ToList();
                var provDic = provList.ToDictionary(p => p.AreaCode, p => p.AreaName);//110000
                //直辖市(北京110000，天津120000，上海市310000,重庆500000)
                var specialProvs = new List<string>() { "110000", "120000", "310000", "500000" };
                string specialAreaCode = "";
                //市
                var city = result.Where(p => p.AreaCode.Substring(2) != "0000" && p.AreaCode.Substring(4) == "00").ToList();
                var cityDic = city.ToDictionary(p => p.AreaCode, p => p.AreaName);
                //区
                var area = result.Where(p => p.AreaCode.Substring(2) != "0000" && p.AreaCode.Substring(4) != "00").ToList();
                //json 格式
                //  {
                //     86:{},
                //     110000:{},
                //     120000:{},
                //   }
                //结束
                resultJSON.Add("86", provDic);
                foreach (var item in provList)
                {
                    //特殊处理直辖市(北京110000，天津120000，上海市310000,重庆500000)
                    if (specialProvs.Contains(item.AreaCode))
                    {
                        //直辖市强制为三级，二级设定为xxx100格式
                        specialAreaCode = (item.AreaCode.PackInt() + 100).PackString();
                        resultJSON.Add(item.AreaCode, new List<AreaConfig>() { item }.ToDictionary(p => specialAreaCode, p => p.AreaName));
                    }
                    else
                    {
                        resultJSON.Add(item.AreaCode, city.Where(p => p.AreaCode.Substring(0, 2) == item.AreaCode.Substring(0, 2)).ToList().ToDictionary(p => p.AreaCode, p => p.AreaName));
                    }
                }
                foreach (var item in city)
                {
                    resultJSON.Add(item.AreaCode, area.Where(p => p.AreaCode.Substring(0, 4) == item.AreaCode.Substring(0, 4)).ToList().ToDictionary(p => p.AreaCode, p => p.AreaName));
                }
                //直辖市加上第三级的区
                foreach (var item in specialProvs)
                {
                    specialAreaCode = (item.PackInt() + 100).PackString();
                    resultJSON.Add(specialAreaCode, area.Where(p => p.AreaCode.Substring(0, 4) == specialAreaCode.Substring(0, 4)).ToList().ToDictionary(p => p.AreaCode, p => p.AreaName));
                }
                var ss = resultJSON.PackJson();
                return ss;
            }
            catch (Exception ex)
            {
            }
            finally
            {
            }
            return "";

        }


    }

    public class AreaConfig
    {
        public string AreaCode { get; set; }

        public string AreaName { get; set; }
    }

    public class AreaJson
    {
        public string Key { get; set; }

        public List<AreaConfig> Value { get; set; }
    }
}
