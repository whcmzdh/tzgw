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
    public partial class batchchange : Form
    {
        public batchchange()
        {
            InitializeComponent();
            label3.Text = Class1.workshop;
            textBox1.Text = Class1.frombatchtxt;
            
        }


        private static batchchange childFromInstanc;
        public static batchchange ChildFromInstanc
        {
            get
            {
                if (childFromInstanc == null || childFromInstanc.IsDisposed)
                {
                    childFromInstanc = new batchchange();
                }

                return childFromInstanc;
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (textBox1.Text != "")
            {
                fresh1();
            }
        }

        private void clear1()
        {
            c3.Text = "";
            c4.Text = "";
            c5.Text = "";
            c6.Text = "";
            c7.Text = "";
            c8.Text = "";
            c9.Text = "";
            c10.Text = "";
            c11.Text = "";
            c12.Text = "";
            c13.Text = "";
            c14.Text = "";
            c15.Text = "";
            c16.Text = "";
            c17.Text = "";
            c18.Text = "";
            c19.Text = "";
            c20.Text = "";
            c21.Text = "";
            c22.Text = "";
            c23.Text = "";
            c24.Text = "";
            c25.Text = "";
            
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            tansi_hou th = (tansi_hou)this.Owner;
            th.backitm();
            this.Owner.Show();
            this.Close();
        }

        private void c25_TextChanged(object sender, EventArgs e)
        {
            string sql = string.Format("select T2.desc1 from masterdata T2 where itemcode='{0}'", c25.Text);
            DataSet ds = new DataSet();
            ds = Class1.GetAllDataSet(sql);
            DataTable dt = new DataTable();
            dt = ds.Tables[0];

            if (dt.Rows.Count > 0)
            {
                desc1.Text = dt.Rows[0][0].ToString();
            }
            else
            {
                desc1.Text = "";
            }
         }

        private void fresh1()
        {

            string sql = string.Format("select T1.*,T2.desc1 from stock T1 left join masterdata T2 on T1.material=T2.itemcode where batch='{0}'", textBox1.Text);
            DataSet ds = new DataSet();
            ds = Class1.GetAllDataSet(sql);
            DataTable dt = new DataTable();
            dt = ds.Tables[0];

            if (dt.Rows.Count > 0)
            {

                c2.Text = dt.Rows[0][5].ToString();
                c3.Text = dt.Rows[0][6].ToString();
                c4.Text = dt.Rows[0][7].ToString();
                c5.Text = dt.Rows[0][8].ToString();
                c6.Text = dt.Rows[0][9].ToString();
                c7.Text = dt.Rows[0][10].ToString();
                c8.Text = dt.Rows[0][11].ToString();
                c9.Text = dt.Rows[0][12].ToString();
                c10.Text = dt.Rows[0][13].ToString();
                c11.Text = dt.Rows[0][14].ToString();
                c12.Text = dt.Rows[0][15].ToString();
                c13.Text = dt.Rows[0][16].ToString();
                c14.Text = dt.Rows[0][17].ToString();
                c15.Text = dt.Rows[0][18].ToString();
                c16.Text = dt.Rows[0][19].ToString();
                c17.Text = dt.Rows[0][20].ToString();
                c18.Text = dt.Rows[0][21].ToString();
                c19.Text = dt.Rows[0][22].ToString();
                c20.Text = dt.Rows[0][23].ToString();
                c21.Text = dt.Rows[0][24].ToString();
                c22.Text = dt.Rows[0][25].ToString();
                c23.Text = dt.Rows[0][26].ToString();
                c24.Text = dt.Rows[0][27].ToString();
                c25.Text = dt.Rows[0][2].ToString();
                desc1.Text = dt.Rows[0][28].ToString();

            }
            else
            {
                clear1();
            }
        }


        private void button1_Click(object sender, EventArgs e)
        {
            string sql = string.Format("update stock set material='{0}',stockin='{1}',t1='{2}',t2='{3}',c5='{4}',pro1='{5}',boxno='{6}',qatype='{7}',batchbig='{8}',len='{9}',qa2='{11}',qa2r='{12}',qa='{13}' where batch='{10}' and sloc='{14}'",c25.Text.Trim(),c2.Text.Trim(),c3.Text.Trim(),c4.Text.Trim(),c14.Text.Trim(), c15.Text.Trim(), c18.Text.Trim(), c19.Text.Trim(), c20.Text.Trim(), c21.Text.Trim(),textBox1.Text,c12.Text.Trim(), c13.Text.Trim(), c11.Text.Trim(),label3.Text);
            int c = Class1.ExcuteScal(sql);
            if (c == 1)
            {
                MessageBox.Show("更新成功");
                fresh1();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
            textBox1.Focus();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string sql = string.Format("delete from stock where batch='{0}' and sloc='{1}'", textBox1.Text, Class1.workshop);
            int c = Class1.ExcuteScal(sql);
            if (c == 1)
            {
                MessageBox.Show("删除成功");
                clear1();
            }
            else
            {
                MessageBox.Show("删除失败");
            }
        }

        private void batchchange_Load(object sender, EventArgs e)
        {

        }
    }
}
