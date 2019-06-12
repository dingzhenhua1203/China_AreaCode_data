using HtmlAgilityPack;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ClassBuilder
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            GenerateJD();
        }


        private void GenerateJD()
        {
            var response = CrawlerHttpUtil.HttpGetRequest(txtUrl.Text, null, null, null, contentType: CrawlerHttpUtil.JsonContentType);
            var response_Str = CrawlerHttpUtil.GetResponseStreamToStr(response);
            try
            {
                dynamic temp = JsonConvert.DeserializeObject(response_Str);
                var slists = temp.data.slists;
                string ss = JsonConvert.SerializeObject(slists);

                var blogPosts = JArray.Parse(ss);
                List<dynamic> result = blogPosts.Select(x => (dynamic)x).ToList();
                WriteJDCode(result);
            }
            catch (Exception ex)
            {
            }
            finally
            {
            }
        }

        private void WriteJDCode(List<dynamic> list)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("using System;");
            sb.AppendLine("using System.Collections.Generic;");
            sb.AppendLine("using System.Text;");
            sb.AppendLine("\r\n");
            sb.AppendLine("namespace SurpriseGamePoll.UnionService\r\n{");
            sb.AppendLine(Recurrence(list, ""));
            sb.AppendLine("}");
            richTextBox1.Text = sb.ToString();
        }

        public string Recurrence(List<dynamic> list, string parentId)
        {
            StringBuilder sb = new StringBuilder();
            if (list == null)
            {
                return "";
            }
            var root = list.FirstOrDefault();
            if (string.IsNullOrWhiteSpace(parentId))
            {
                root = list?.FirstOrDefault(t => t.parentId.ToString() == parentId);
            }
            else
            {
                root = list?.FirstOrDefault(t => t.nodeId.ToString() == parentId);
            }
            if (root == null)
            {
                return "";
            }
            sb.AppendLine($@"/// <summary>");
            sb.AppendLine($@"/// {root.description}");
            sb.AppendLine($@"/// </summary>");
            sb.AppendLine($@" public class  {root.dataName} { (!string.IsNullOrWhiteSpace(parentId) ? "" : ": PddDDKBaseRequest") }");
            sb.AppendLine("{");
            var propList = list.Where(t => t.parentId == root.nodeId).ToList();
            var comboList = new List<dynamic>();
            propList.ForEach(
                t =>
                {
                    if (list.Exists(p => p.parentId == t.nodeId))
                    {
                        comboList.Add(t);
                    }
                    else
                    {
                        list.Remove(t);
                    }
                    sb.AppendLine($"/// <summary>");
                    sb.AppendLine($"///  {t.description }");
                    sb.AppendLine($"/// </summary>");
                    sb.AppendLine($@"public { t.dataType } {t.dataName} {{ get; set; }}");

                });
            sb.AppendLine("}");
            list.Remove(root);
            if (comboList == null)
            {
                return sb.ToString();
            }
            else
            {
                comboList?.ForEach(t =>
                {
                    sb.AppendLine(Recurrence(list, t.nodeId.ToString()));
                });
                return sb.ToString();
            }
        }
    }
}
