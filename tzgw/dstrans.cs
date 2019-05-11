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
    public partial class dstrans : Form
    {
        public dstrans()
        {
            InitializeComponent();
        }

        private static dstrans childFromInstanc;
        public static dstrans ChildFromInstanc
        {
            get
            {
                if (childFromInstanc == null || childFromInstanc.IsDisposed)
                {
                    childFromInstanc = new dstrans();
                }

                return childFromInstanc;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox2.Text.Length > 0)
            {
                string sql = string.Format("SELECT batch as 卷号,REPLACE(batch,left(batch,{1}),{2}) as 替换后卷号,dstime as 断丝日期,dscs as 断丝次数,dssj as 断丝时间 from duansi where batch like '%{0}%'", textBox1.Text, textBox2.Text.Length.ToString(), textBox2.Text);
                DataSet ds = new DataSet();
                ds = Class1.GetAllDataSet(sql);
                DataTable dt = new DataTable();
                dt = ds.Tables[0];
                if (dt.Rows.Count > 0)
                {
                    dataGridView1.DataSource = dt;
                }
                else
                {
                    dataGridView1.DataSource = null;
                }
            }
            else
            {

            }
            
        }

        private void button3_Click(object sender, EventArgs e)
        {

            yuansi ys = (yuansi)this.Owner;
            this.Owner.Show();
            this.Dispose();
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                if (dataGridView1.Rows[i].Selected == true)
                {
                    string yuan = dataGridView1.Rows[i].Cells[0].Value.ToString().Trim();
                    string gai = dataGridView1.Rows[i].Cells[1].Value.ToString().Trim();
                    string a2 = dataGridView1.Rows[i].Cells[2].Value.ToString().Trim();
                    string a3 = dataGridView1.Rows[i].Cells[3].Value.ToString().Trim();
                    string a4 = dataGridView1.Rows[i].Cells[4].Value.ToString().Trim();
                    string sql = string.Format("update duansi set batch='{0}' where batch='{1}' and dstime='{2}' and dscs='{3}' and dssj='{4}'",gai,yuan,a2,a3,a4);
                    //MessageBox.Show(sql);
                    int c = Class1.ExcuteScal(sql);
                    if (c == 1)
                    {
                        dataGridView1.Rows[i].DefaultCellStyle.BackColor = Color.LawnGreen;
                    }
                }
            }
        }
    }
}
