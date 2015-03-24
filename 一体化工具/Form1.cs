using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace 一体化工具
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        string src = null;
        private ArrayList getFile()
        {
            StreamReader objReader = new StreamReader(src);
            string sLine = "";
            ArrayList fileContext = new ArrayList();
            while (sLine != null)
            {
                sLine = objReader.ReadLine();
                if (sLine != null && !sLine.Equals(""))
                {
                    string[] str = sLine.Split(',');
                    ArrayList line = new ArrayList();
                    line.Add(str[0]);
                    line.Add(str[1]);
                    fileContext.Add(line);
                }
            }
            objReader.Close();
            return fileContext;
        }
        private ArrayList getNo(ArrayList fileContext)
        {
            conn = new SqlConnection();
            cmd = conn.CreateCommand();
            conn.ConnectionString = connStr;
            conn.Open();
            ArrayList prices = new ArrayList();
            for (int i = 0; i < fileContext.Count; i++)
            {
                ArrayList temp = (ArrayList)fileContext[i];
                string no = temp[0].ToString();
                string sql = "SELECT b.id from AA_Inventory as a ,AA_InventoryPrice as b where a.id = b.idinventory and a.code like '%" + no + "%'";
                cmd = new SqlCommand(sql, conn);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    ArrayList price = new ArrayList();
                    price.Add(reader["id"].ToString());
                    price.Add(temp[1]);
                    price.Add(temp[0]);
                    prices.Add(price);
                }
                reader.Close();
            }
            conn.Close();
            return prices;
        }


        private ArrayList getNoName(ArrayList fileContext)
        {
            conn = new SqlConnection();
            cmd = conn.CreateCommand();
            conn.ConnectionString = connStr;
            conn.Open();
            ArrayList prices = new ArrayList();
            for (int i = 0; i < fileContext.Count; i++)
            {
                ArrayList temp = (ArrayList)fileContext[i];
                string no = temp[0].ToString();
                string sql = "SELECT code from AA_Inventory  where code like '%" + no + "%'";
                cmd = new SqlCommand(sql, conn);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    ArrayList price = new ArrayList();
                    price.Add(reader["code"].ToString());
                    price.Add(temp[1]);
                    price.Add(temp[0]);
                    prices.Add(price);
                }
                reader.Close();
            }
            conn.Close();
            return prices;
        }
        private SqlConnection conn;
        private SqlCommand cmd;
        private const string connStr = "server=192.168.8.4;uid=sa;pwd=!@#$wass6677;database=UFTData292713_000019";
        private void button1_Click(object sender, EventArgs e)
        {

            openFileDialog1.Filter = "导入文件(*.txt)|*.txt";
            openFileDialog1.ShowDialog();
            src = openFileDialog1.FileName;
            toolStripStatusLabel1.Text = "已打开文件："+src;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (src != null)
            {

                textBox1.Text = "";
                StreamReader objReader = new StreamReader(src);
                string sLine = "";
                ArrayList SKUcode = new ArrayList();
                string temp = "";
                string line = objReader.ReadLine();
                string nul = textBox3.Text;
                int nl = 0;
                bool isStart = true;
                try
                {
                    nl = int.Parse(nul);
                }
                catch (Exception ex)
                {

                }
                int a =0;
                string str1 = "";
                string str2 = "";
                while (sLine != null)
                {
                    if (isStart)
                    {
                        str1 = line.Substring(0, line.Length - nl);
                        str2 = line.Substring(line.Length - nl, nl);
                        line = str1+"," + str2;
                        isStart = false;
                        continue;
                    }
                    sLine = objReader.ReadLine();
                    if (sLine != null && !sLine.Equals(""))
                    {
                        str1 = sLine.Substring(0, sLine.Length - nl);
                        str2 = sLine.Substring(sLine.Length - nl, nl);

                        if (temp.Equals(str1))
                        {

                            line += "," + str2;

                        }
                        else
                        {                           
                            string[] s = line.Split(',');
                            a = s.Length - 1;
                            line += ",共计" + a.ToString() + "双";
                            textBox1.Text += line;
                            line = "\r\n" + str1 + "," + str2;
                        }

                        temp = str1;
                        toolStripStatusLabel2.Text = "正在分析";
                    }
                }

                string[] sa = line.Split(',');
                a = sa.Length - 1;
                line += ",共计" + a.ToString() + "双";
                textBox1.Text += line;
                line = "\r\n" + str1 + "," + str2;
                objReader.Close();
                toolStripStatusLabel2.Text = "操作完成";
            }
            
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (src != null)
            {
                textBox1.Text = "";
                StreamReader objReader = new StreamReader(src);
                string sLine = "";
                ArrayList SKUcode = new ArrayList();
                string sLine1 = objReader.ReadLine();
                while (sLine != null)
                {


                    sLine = objReader.ReadLine();
                    try
                    {
                        if (sLine.Equals(sLine1))
                        {
                            continue;
                        }


                    }
                    catch (Exception ex)
                    {

                    }
                    textBox1.Text += sLine + "\r\n";
                    sLine1 = sLine;

                }
                objReader.Close();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (src != null)
            {
                textBox1.Text = "";
                StreamReader objReader = new StreamReader(src);
                string sLine = "";
                ArrayList SKUcode = new ArrayList();
                int n = 0;
                while (sLine != null)
                {
                    sLine = objReader.ReadLine();
                    if (sLine != null && !sLine.Equals(""))
                    {
                        conn = new SqlConnection();
                        cmd = conn.CreateCommand();
                        conn.ConnectionString = connStr;
                        conn.Open();
                        string sql1 = "select id from AA_InventoryClass where code like '" + textBox2.Text + "'";
                        cmd = new SqlCommand(sql1, conn);
                        SqlDataReader reader = cmd.ExecuteReader();
                        string id = "";
                        while (reader.Read())
                        {
                            id = reader["id"].ToString();
                        }
                        reader.Close();

                        string sql = "update AA_Inventory set idinventoryclass = '" + id + "' WHERE code like '" + sLine + "'";

                        cmd = new SqlCommand(sql, conn);
                        int i = cmd.ExecuteNonQuery();
                        conn.Close();

                    }
                }
                objReader.Close();
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.A && e.Control)
            {
                textBox1.SelectAll();
            }   
        }

        private void button5_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
            if (src != null)
            {
                ArrayList temp = getFile();
                ArrayList temp2 = getNoName(temp);
                for (int i = 0; i < temp2.Count; i++)
                {
                    ArrayList temp3 = (ArrayList)temp2[i];
                    textBox1.Text += temp3[0] +","+temp3[1]+ "\r\n";
                }
            }
            toolStripStatusLabel2.Text = "操作完成";
        }
    }
}
