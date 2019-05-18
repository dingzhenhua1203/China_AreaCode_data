using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace China_AreaCode_data
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnChoose_Click(object sender, EventArgs e)
        {
            //FolderBrowserDialog path = new FolderBrowserDialog();
            //path.ShowDialog();
            //txtPath.Text = path.SelectedPath;
        }   

        private void btnDownload_Click(object sender, EventArgs e)
        {
            //if (txtPath.Text == "")
            //{
            //    MessageBox.Show("请选择路径");
            //    return;
            //}
            var result=ChinaAreaCrawer.CreateJson();
            result = "const china_area_data =\r\n" + result;
            ExportFile(result, txtPath.Text);
        }

        private void ExportFile(string log, string filePath )
        {
            try
            {
                //SaveFileDialog 是用来打开资源管理器给用户选择地址的
                SaveFileDialog sflg = new SaveFileDialog();
                sflg.FileName = "area.js";//给弹出框默认值
                sflg.Filter = "JS类型(*.js)|*.js";
                if (sflg.ShowDialog() == DialogResult.OK)
                {
                    filePath = sflg.FileName;//这里返回一个完整的文件名含地址
                    using (FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.ReadWrite))
                    {
                        byte[] data = System.Text.Encoding.UTF8.GetBytes(log);
                        fs.Write(data, 0, data.Length);
                        fs.Flush();
                        fs.Close();
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
    }
}
