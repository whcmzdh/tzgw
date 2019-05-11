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
    public partial class translogys : Form
    {
        public translogys()
        {
            InitializeComponent();
        }


        private static translogys childFromInstanc;
        public static translogys ChildFromInstanc
        {
            get
            {
                if (childFromInstanc == null || childFromInstanc.IsDisposed)
                {
                    childFromInstanc = new translogys();
                }

                return childFromInstanc;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string strSQL1 = string.Format("SELECT [batch] as 卷号,[loc1] as 源库位,[loc2] as 目标库位,[date1] as 日期,[action1] as 类型,[devicenum] as 工位,[qty1] as 剩余重量,[res] as 剩余长度 FROM transferlog T1 where 1=1");
            if (comboBox1.Text != "")
            {
                strSQL1 = strSQL1 + string.Format(" and T1.action1='{0}'", comboBox1.Text);
            }
            if (batch.Text != "")
            {
                strSQL1 = strSQL1 + string.Format(" and T1.batch like '%{0}%'", batch.Text);
            }
            if (loc1.Text != "")
            {
                strSQL1 = strSQL1 + string.Format(" and T1.loc1='{0}'", loc1.Text);
            }
            if (loc2.Text != "")
            {
                strSQL1 = strSQL1 + string.Format(" and T1.loc2='{0}'", loc2.Text);
            }
            if (device.Text != "")
            {
                strSQL1 = strSQL1 + string.Format(" and T1.devicenum='{0}'", device.Text);
            }
            if (dateTimePicker1.Text != "")
            {
                strSQL1 = strSQL1 + string.Format(" and year(T1.date1)=year('{0}') and month(T1.date1)=month('{0}') and day(T1.date1)=day('{0}')", dateTimePicker1.Text);
            }
            DataSet ds = new DataSet();
            ds = Class1.GetAllDataSet(strSQL1);
            DataTable dt = new DataTable();
            dt = ds.Tables[0];
            if (dt.Rows.Count > 0)
            {
                dataGridView1.DataSource = dt;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            dateTimePicker1.Text = DateTime.Now.ToString() ;
            loc1.Text = "";
            loc2.Text = "";
            device.Text = "";
            batch.Text = "";
            comboBox1.Text = "";
            dataGridView1.DataSource = null;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            wh th = (wh)this.Owner;
            this.Owner.Show();
            this.Dispose();
        }
    }
}
