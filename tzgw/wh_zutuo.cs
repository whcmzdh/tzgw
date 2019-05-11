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
    public partial class wh_zutuo : Form
    {
        public wh_zutuo()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.Manual;
            this.Location = new Point(0, 0);

        }

        private static wh_zutuo childFromInstanc;
        public static wh_zutuo ChildFromInstanc
        {
            get
            {
                if (childFromInstanc == null || childFromInstanc.IsDisposed)
                {
                    childFromInstanc = new wh_zutuo();
                }

                return childFromInstanc;
            }
        }

        private void wh_zutuo_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    if (dataGridView1.Rows[i].Selected == true)
                    {
                        string sql0 = string.Format("update stock set tpno='' where boxno='{0}'", dataGridView1.Rows[i].Cells[0].Value.ToString());
                        Class1.ExcuteScal(sql0);
                    }


                }
            freshleftbox1();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            freshleftbox1();
        }


        private void freshleftbox1()
        {
            string sql = string.Format("select top 1000 boxno as 箱号,material as 物料,count(batch) as 卷数,sum(pro1) as 重量 from stock where tpno='{0}' group by boxno,material order by boxno", textBox1.Text);
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            ds = Class1.GetAllDataSet(sql);
            dt = ds.Tables[0];
            if (dt.Rows.Count > 0)
            {
                dataGridView1.DataSource = dt;
                int x = 0; //juanshu
                decimal y = 0; //zhongliang
                
                //MessageBox.Show(x);
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    dataGridView1.Rows[i].Selected = true;
                    x = x + Convert.ToInt32(dataGridView1.Rows[i].Cells[2].Value);
                    y = y + Convert.ToDecimal(dataGridView1.Rows[i].Cells[3].Value);
                }

                js1.Text = x.ToString();
                zl1.Text = y.ToString();
                rl1.Text = (dataGridView1.Rows.Count).ToString();
            }
            else
            {
                dataGridView1.DataSource = null;
                js1.Text = "0";
                zl1.Text = "0";
                rl1.Text = "0";
            }
        }

        private void freshleftbox2()
        {
            string sql = string.Format("select top 1000 boxno as 箱号,material as 物料,count(batch) as 卷数,sum(pro1) as 重量 from stock where tpno='{0}' group by boxno,material order by boxno", textBox2.Text);
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            ds = Class1.GetAllDataSet(sql);
            dt = ds.Tables[0];
            if (dt.Rows.Count > 0)
            {
                dataGridView2.DataSource = dt;
                int x = 0; //juanshu
                decimal y = 0; //zhongliang

                //MessageBox.Show(x);
                for (int i = 0; i < dataGridView2.Rows.Count; i++)
                {
                    dataGridView2.Rows[i].Selected = true;
                    x = x + Convert.ToInt32(dataGridView2.Rows[i].Cells[2].Value);
                    y = y + Convert.ToDecimal(dataGridView2.Rows[i].Cells[3].Value);
                }

                js2.Text = x.ToString();
                zl2.Text = y.ToString();
                rl2.Text = (dataGridView2.Rows.Count).ToString();
            }
            else
            {
                dataGridView2.DataSource = null;
                js2.Text = "0";
                zl2.Text = "0";
                rl2.Text = "0";
            }
        }

        private void freshleftbox3()
        {
            string sql = string.Format("select top 1000 boxno as 箱号,material as 物料,count(batch) as 卷数,sum(pro1) as 重量 from stock where tpno='{0}' group by boxno,material order by boxno", textBox3.Text);
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            ds = Class1.GetAllDataSet(sql);
            dt = ds.Tables[0];
            if (dt.Rows.Count > 0)
            {
                dataGridView3.DataSource = dt;
                int x = 0; //juanshu
                decimal y = 0; //zhongliang

                //MessageBox.Show(x);
                for (int i = 0; i < dataGridView3.Rows.Count; i++)
                {
                    x = x + Convert.ToInt32(dataGridView3.Rows[i].Cells[2].Value);
                    y = y + Convert.ToDecimal(dataGridView3.Rows[i].Cells[3].Value);
                }

                js3.Text = x.ToString();
                zl3.Text = y.ToString();
                rl3.Text = (dataGridView3.Rows.Count).ToString();
            }
            else
            {
                dataGridView3.DataSource = null;
                js3.Text = "0";
                zl3.Text = "0";
                rl3.Text = "0";
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            freshleftbox2();
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            freshleftbox3();
        }

        private void button4_Click(object sender, EventArgs e)
        {

            for (int i = 0; i < dataGridView2.Rows.Count; i++)
            {
                if (dataGridView2.Rows[i].Selected == true)
                {
                    string sql0 = string.Format("update stock set tpno='' where boxno='{0}'", dataGridView2.Rows[i].Cells[0].Value.ToString());
                    Class1.ExcuteScal(sql0);
                }


            }
            freshleftbox2();
        }

        private void button2_Click(object sender, EventArgs e)
        {

            for (int i = 0; i < dataGridView3.Rows.Count; i++)
            {
                if (dataGridView3.Rows[i].Selected == true)
                {
                    string sql0 = string.Format("update stock set tpno='' where boxno='{0}'", dataGridView3.Rows[i].Cells[0].Value.ToString());
                    Class1.ExcuteScal(sql0);
                }
            }
            freshleftbox3();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string sql = "";
            
            if (MessageBox.Show("合并选中的箱到托盘"+textBox3.Text+"?", "合并", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    if (dataGridView1.Rows[i].Selected == true)
                    {
                        sql = string.Format("update stock set tpno='{0}' where boxno='{1}'", textBox3.Text,dataGridView1.Rows[i].Cells[0].Value.ToString().Trim());
                        int c = Class1.ExcuteScal(sql);

                    }
                }
                for (int i = 0; i < dataGridView2.Rows.Count; i++)
                {
                    if (dataGridView2.Rows[i].Selected == true)
                    {
                        sql = string.Format("update stock set tpno='{0}' where boxno='{1}'", textBox3.Text, dataGridView2.Rows[i].Cells[0].Value.ToString().Trim());
                        int c = Class1.ExcuteScal(sql);

                    }

                }

            }
            freshleftbox3();
            freshleftbox1();
            freshleftbox2();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            BarTender.Application btApp;
            BarTender.Format btFormat;
            btApp = new BarTender.Application();

            btFormat = btApp.Formats.Open(@"C:\data\tzgw-labeltemplete\box", false, "");
            btFormat.PrintSetup.IdenticalCopiesOfLabel = 1;  //设置同序列打印的份数
            btFormat.PrintSetup.NumberSerializedLabels = 1;  //设置需要打印的序列数
            btFormat.SetNamedSubStringValue("desc1", textBox3.Text); //向bartender模板传递变量
            btFormat.PrintOut(false, false); //第二个false设置打印时是否跳出打印属性
            btFormat.Close(BarTender.BtSaveOptions.btSaveChanges); //退出时是否保存标签

            Class1.killbartender();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            wh thwh = (wh)this.Owner;
            this.Owner.Show();
            this.Dispose();
        }
    }
}
