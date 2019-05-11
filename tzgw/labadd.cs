using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace tzgw
{
    public partial class labadd : Form
    {
        public labadd()
        {
            InitializeComponent();
        }



        private static labadd childFromInstanc;
        public static labadd ChildFromInstanc
        {
            get
            {
                if (childFromInstanc == null || childFromInstanc.IsDisposed)
                {
                    childFromInstanc = new labadd();
                }

                return childFromInstanc;
            }
        }

        public void addlab(string batch, string item1)
        {
            dataGridView1.Rows.Add();
            dataGridView1.Rows[this.dataGridView1.Rows.Count - 1].Cells[0].Value =batch;
            dataGridView1.Rows[this.dataGridView1.Rows.Count - 1].Cells[1].Value = item1;
        }


        private void button3_Click(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count > 0)
            {
                string sql1 = string.Format("select labpj,upper,lower from laboption where itemcode='{0}'",dataGridView1.Rows[0].Cells[1].Value.ToString().Trim());
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                ds = Class1.GetAllDataSet(sql1);
                dt = ds.Tables[0];
                if (dt.Rows.Count > 0)
                {
                    comboBox1.Items.Clear();
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        comboBox1.Items.Add(dt.Rows[i][0].ToString());
                    }
                    comboBox1.Text = dt.Rows[0][0].ToString();
                    up1.Text=  dt.Rows[0][1].ToString();
                    lo1.Text = dt.Rows[0][2].ToString();
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                string a0 = dataGridView1.Rows[i].Cells[0].Value.ToString();
                string a1 = dataGridView1.Rows[i].Cells[1].Value.ToString();
                string a2 = comboBox1.Text;
                string a3 = dataGridView1.Rows[i].Cells[2].Value.ToString();
                string a4 = DateTime.Now.ToString();
                string a5 = Class1.curuser;
                string a6 = "";
                if (dataGridView1.Rows[i].Cells[3].Value is null)
                {
                    a6 = "";
                }
                else
                {
                    a6 = dataGridView1.Rows[i].Cells[3].Value.ToString();
                }
                

                string sql = string.Format("insert into labrecord values('{0}','{1}','{2}','{3}','{4}','{5}','{6}')",a0,a1,a2,a3,a4,a5,a6);
                int c = Class1.ExcuteScal(sql);
                if (c == 1)
                {
                    //string sql2 = string.Format("update labwait set status='已完成 {0}' where batch='{1}'", DateTime.Now.ToString(),a0);
                    //int c2=Class1.ExcuteScal(sql2);
                   dataGridView1.Rows[i].DefaultCellStyle.BackColor = Color.LawnGreen;
                }

            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            lab kab1 = (lab)this.Owner;
            this.Owner.Show();
            this.Hide();
        }

        private void labadd_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count > 0)
            {
                string sql1 = string.Format("select upper,lower from laboption where itemcode='{0}' and labpj='{1}'", dataGridView1.Rows[0].Cells[1].Value.ToString().Trim(), comboBox1.Text);
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                ds = Class1.GetAllDataSet(sql1);
                dt = ds.Tables[0];
                if (dt.Rows.Count > 0)
                {
                    up1.Text = dt.Rows[0][0].ToString();
                    lo1.Text = dt.Rows[0][1].ToString();
                }
                else
                {
                    up1.Text ="9999";
                    lo1.Text = "0";
                }
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count > 0)
            {
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    double up11 = Convert.ToDouble(up1.Text);
                    double dw11 = Convert.ToDouble(lo1.Text);
                    double x = Convert.ToDouble(dataGridView1.Rows[i].Cells[2].Value.ToString().Trim());
                   
                        if (x >= dw11 & x <= up11)
                        {
                            dataGridView1.Rows[i].Cells[3].Value = "合格";
                        }
                        else
                        {
                            dataGridView1.Rows[i].Cells[3].Value = "不合格";
                        }
                    
                }
            }
        }
    }
}
