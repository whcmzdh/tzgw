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
    public partial class wsdevice2 : Form
    {
        public wsdevice2()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string sql1 = string.Format("select * from wsdplan");

            DataSet ds = new DataSet();
            ds = Class1.GetAllDataSet(sql1);
            DataTable dt = new DataTable();
            dt = ds.Tables[0];

            if (dt.Rows.Count > 0)
            {
                SetDGVSourceFunction1(dt);

            }

        }

        private delegate void SetDGVSource(DataTable dt);//添加设置DataGridView的DataSource的代理  
        public static void SetDGVSourceFunction1(DataTable dt)
        {
            if (dataGridView1.InvokeRequired)
            {
                SetDGVSource delegateSetSource1 = new SetDGVSource(SetDGVSourceFunction1);
                dataGridView1.Invoke(delegateSetSource1, new object[] { dt });
            }
            else
            {
                dataGridView1.DataSource = dt;
            }

        } //状态清单

    }
}
