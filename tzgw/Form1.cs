using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.IO.Ports;

namespace tzgw
{
    public partial class Start : Form
    {
        public Start()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.Manual;
            this.Location = new Point(0, 0);
            this.Text = Class1.companyinfo;
            
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (comboBox1.Text == "生产环境")
            {
                //1 拓展 50 复材
                //Class1.sqlstr = "server=192.168.50.200\\SQLEXPRESS;Database=gwtz;User ID=sa;Password=sa2sa2!!";
                Class1.sqlstr = "server=192.168.1.200\\SQLEXPRESS;Database=gwtz;User ID=sa;Password=sa2sa2!!";
              
            }

            if (comboBox1.Text == ".")
            {
                Class1.sqlstr = "server=.\\SQLEXPRESS;Database=gwtz;User ID=sa;Password=sa2sa2!!";
            }
            if (comboBox1.Text == "test")
            {
                Class1.sqlstr = "server=192.168.10.200\\SQLEXPRESS;Database=gwtz;User ID=sa;Password=sa2sa2!!";
            }

            try
            {
                if (txtUsn.Text.Trim() == "")
                {
                    labMessage.Text = "用户名不能为空!";
                    txtUsn.Focus();//获取焦点
                    return;
                }
                else if (txtPwd.Text.Trim() == "")
                {
                    labMessage.Text = "密码不能为空!";
                    txtPwd.Focus();
                    return;
                }

                
                string sqlStr2 = string.Format("select username,password,rg1,rg2,rg3,rg4,rg5,rg6,rg7,rg8,rg9,rg10,rg11,rg12,rg13,rg14,shift1,rg15,rg16,rg17,rg18,rg19,rg20,rg21,rg22 from userlist where username='{0}'", txtUsn.Text);
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();

                ds = Class1.GetAllDataSet(sqlStr2);
                dt = ds.Tables[0];
                if (dt.Rows.Count <= 0)//因为是通过userName查询数据的，所以如果没有读到这条数据，肯定是用户名不存在
                {
                    labMessage.Text = "用户名不存在！请重新输入";
                    txtUsn.Text = "";//文本框置空
                    txtPwd.Text = "";
                    txtUsn.Focus();
                }
                else if (dt.Rows[0][1].ToString().Trim() == txtPwd.Text.Trim())
                {
                    Class1.rg1 = dt.Rows[0][2].ToString().Trim();
                    Class1.rg2 = dt.Rows[0][3].ToString().Trim();
                    Class1.rg3 = dt.Rows[0][4].ToString().Trim();
                    Class1.rg4 = dt.Rows[0][5].ToString().Trim();
                    Class1.rg5 = dt.Rows[0][6].ToString().Trim();
                    Class1.rg6 = dt.Rows[0][7].ToString().Trim();
                    Class1.rg7 = dt.Rows[0][8].ToString().Trim();
                    Class1.rg8 = dt.Rows[0][9].ToString().Trim();
                    Class1.rg9 = dt.Rows[0][10].ToString().Trim();
                    Class1.rg10 = dt.Rows[0][11].ToString().Trim();
                    Class1.rg11 = dt.Rows[0][12].ToString().Trim();
                    Class1.rg12 = dt.Rows[0][13].ToString().Trim();
                    Class1.rg13 = dt.Rows[0][14].ToString().Trim();
                    Class1.rg14 = dt.Rows[0][15].ToString().Trim();
                    Class1.rg15 = dt.Rows[0][17].ToString().Trim();
                    Class1.curuser = txtUsn.Text;
                    Class1.shift1= dt.Rows[0][16].ToString().Trim();
                    Class1.rg17 = dt.Rows[0][19].ToString().Trim();
                    Class1.rg18 = dt.Rows[0][20].ToString().Trim();
                    Class1.rg19 = dt.Rows[0][21].ToString().Trim();
                    Class1.rg20 = dt.Rows[0][22].ToString().Trim();
                    Class1.rg21 = dt.Rows[0][23].ToString().Trim();
                    Class1.rg22 = dt.Rows[0][24].ToString().Trim();

                    this.Hide();
                    mainmenu mm = mainmenu.ChildFromInstanc;
                    if (mm != null)
                    {
                        mm.Owner = this;
                        mm.Show();
                    }
                }
                else
                {
                    labMessage.Text = "密码错误！请重新输入！";
                    txtPwd.Text = "";
                    txtPwd.Focus();
                }
            }
            catch (Exception ex)
            {
                labMessage.Text = "登录异常：" + ex.Message;
                txtUsn.Text = "";
                txtPwd.Text = "";
                txtUsn.Focus();
            }
            finally
            {

            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Application.Exit();//退出
        }

        private void Start_Load(object sender, EventArgs e)
        {

        }

       


        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {

        }
    }
}
