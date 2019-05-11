using Microsoft.VisualBasic;
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
    public partial class aftergenyuansi : Form
    {
        public aftergenyuansi()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.Manual;
            this.Location = new Point(0, 0);
            Font font = new Font("宋体", 14);
            dataGridView1.Font = font;
           

        }

        public DataSet dsx = new DataSet();
        public DataTable dtx = new DataTable();

        private static aftergenyuansi childFromInstanc;
        public static aftergenyuansi ChildFromInstanc
        {
            get
            {
                if (childFromInstanc == null || childFromInstanc.IsDisposed)
                {
                    childFromInstanc = new aftergenyuansi();
                }

                return childFromInstanc;
            }
        }

        private void aftergen_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add(new DataColumn("卷号"));
            dt.Columns.Add(new DataColumn("物料"));
            dt.Columns.Add(new DataColumn("入库时间"));
            dt.Columns.Add(new DataColumn("重量"));
            dt.Columns.Add(new DataColumn("长度"));

                if (Class1.isNumeric(start1.Text) && Class1.isNumeric(qty1.Text))
                {
                    for (int i = 1; i <= Convert.ToInt32(qty1.Text); i++)
                    {
                        
                        DataRow dr1 = dt.NewRow();
                        dr1[0] = b0.Text+ (Convert.ToInt64(start1.Text) + i-1).ToString()+textBox2.Text;
                        dr1[1] = textBox1.Text;
                        dr1[2] = DateTime.Now.ToString("yyyy/MM/dd HH:mm");
                        dr1[3] = wt.Text;
                        dr1[4] = len1.Text;
                    dt.Rows.Add(dr1);

                    }
                    dataGridView1.DataSource = dt;
                    if (dt.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            string batch0 = dataGridView1.Rows[i].Cells[0].Value.ToString();
                            string sql0 = string.Format("select top 1 '已存在',batch from stock where batch='{0}'", batch0);

                            DataTable dts = new DataTable();
                            DataSet dss = new DataSet();
                            dss = Class1.GetAllDataSet(sql0);
                            dts = dss.Tables[0];
                            if (dts.Rows.Count == 1)
                            {
                                dataGridView1.Rows[i].DefaultCellStyle.BackColor = Color.Red;
                                dataGridView1.Rows[i].Cells[1].Value = dts.Rows[0][0].ToString().Trim();

                            }
                        }
                    }
                }
    
        }

        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
    
              
        }




        private void button3_Click(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count > 0)
            {
                for (int i = 0; i < dataGridView1.Rows.Count-1; i++)
                {
                    
                    if (dataGridView1.Rows[i].Cells[1].Value.ToString().Length>=4)
                    {
                        if (dataGridView1.Rows[i].DefaultCellStyle.BackColor != Color.Red)
                        {      
                            string batch = dataGridView1.Rows[i].Cells[0].Value.ToString();
                            string sloc = "tsw";
                            string material= dataGridView1.Rows[i].Cells[1].Value.ToString();
                            string stockin = dataGridView1.Rows[i].Cells[2].Value.ToString();
                            string t1 = DateTime.Now.ToString();
                            string t2 = DateTime.Now.ToString();
                            string tpno = "ysw-"+DateTime.Now.ToString("yyyyMMdd");
                            string qa = "M";
                            string pro1 = dataGridView1.Rows[i].Cells[3].Value.ToString();
                            string res = dataGridView1.Rows[i].Cells[3].Value.ToString();
                            string len = dataGridView1.Rows[i].Cells[4].Value.ToString();
                            string sqladd = string.Format("insert into stock(branch,sloc,material,batch,displaybatch,stockin,t1,t2,tpno,qa,pro1,res,len,c5) values('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','O')", "tz", sloc, material, batch, batch, stockin, t1, t2, tpno, qa, pro1, res, len);
                            int c = Class1.ExcuteScal(sqladd);
                            if (c == 1)
                            {
                                dataGridView1.Rows[i].DefaultCellStyle.BackColor = Color.LawnGreen;            
                            }
                            else
                            {
                                dataGridView1.Rows[i].DefaultCellStyle.BackColor = Color.Yellow;
                            }

                        }
                        

                        
                    }
                }
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            string sql = string.Format("select desc1,sp1 from masterdata where itemcode='{0}'",textBox1.Text);
            DataTable dt = new DataTable();
            DataSet ds = new DataSet();
            ds = Class1.GetAllDataSet(sql);
            dt = ds.Tables[0];
            if (dt.Rows.Count > 0)
            {
                desc1.Text = dt.Rows[0][0].ToString().Trim();
                xmd.Text = dt.Rows[0][1].ToString().Trim();
            }
            else
            {
                desc1.Text ="不存在";
                xmd.Text ="1";
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {

            tansi_qian tq = (tansi_qian)this.Owner;
            this.Owner.Show();
            this.Dispose();
        }





        private void label4_Click(object sender, EventArgs e)
        {
            filldt(wt.Text,3);
        }

        private void filldt(string fillx,int lx)
        {
            if (dataGridView1.Rows.Count > 0)
            {
                for (int i = 0; i < dataGridView1.Rows.Count - 1; i++)
                {
                    dataGridView1.Rows[i].Cells[lx].Value = fillx;
                }
            }
        }





        public void itemcodeload()
        {
            textBox1.Text = "";
            textBox1.Text = Class1.itemmu1;
        }

        private void button5_Click_1(object sender, EventArgs e)
        {
            Class1.fromform = "mu";
            this.Hide();
            itemselect ist = itemselect.ChildFromInstanc;
            if (ist != null)
            {
                ist.Owner = this;
                ist.Show();
            }
        }

        private void label5_Click(object sender, EventArgs e)
        {
            filldt(len1.Text, 4);
        }

        private void label10_Click(object sender, EventArgs e)
        {
            
        }

        private void label10_Click_1(object sender, EventArgs e)
        {
            filldt(len1.Text, 1);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            dataGridView1.DataSource = null;
        }

        private void len1_TextChanged(object sender, EventArgs e)
        {
            wt.Text = (Convert.ToDecimal(len1.Text) * Convert.ToDecimal(xmd.Text)).ToString("#0.00");
        }
    }
}

