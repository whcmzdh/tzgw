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
    public partial class popgw : Form
    {
        public popgw()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.Manual;
            this.Location = new Point(0, 0);
        }


        private static popgw childFromInstanc;
        public static popgw ChildFromInstanc
        {
            get
            {
                if (childFromInstanc == null || childFromInstanc.IsDisposed)
                {
                    childFromInstanc = new popgw();
                }

                return childFromInstanc;
            }
        }


        public void init1(string itm)
        {
            textBox1.Text = itm;
        }

        private void button1_Click(object sender, EventArgs e)
        {

            tansi_hou th = (tansi_hou)this.Owner;
            this.Owner.Show();
            this.Dispose();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            freshdataview();
        }


        private void freshdataview()
        {
            string sql1 = "";
            if (textBox1.Text != "")
            {
                if (textBox2.Text != "")
                {
                    sql1 = string.Format("select top {1} batch as 卷号,material as 物料,t1 as 上卷时间,t2 as 下卷时间,pro1 as 重量,len as 长度,qa as 质检,qatype as 类型,boxno as 箱号,rmbatch as 原丝批次 from stock where c5='{0}'", textBox1.Text, textBox2.Text);
                }
                else
                {
                    sql1 = string.Format("select batch as 卷号,material as 物料,t1 as 上卷时间,t2 as 下卷时间,pro1 as 重量,len as 长度,qa as 质检,qatype as 类型,boxno as 箱号,rmbatch as 原丝批次 from stock where c5='{0}'", textBox1.Text, textBox2.Text);

                }

                string sql2 = "";
                if (textBox3.Text != "")
                {
                    sql2 = string.Format(" and rmbatch='{0}'", textBox3.Text);
                }

                string sql3 = " order by batch desc,rmbatch,t2 desc";

                string sql = sql1 + sql2 + sql3;
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                ds = Class1.GetAllDataSet(sql);
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
        }
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                textBox3.Text= dataGridView1.Rows[e.RowIndex].Cells[9].Value.ToString().Trim();
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            freshdataview();
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            freshdataview();
        }
    }
}
