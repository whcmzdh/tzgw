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
    public partial class juhe : Form
    {
        public juhe()
        {
            InitializeComponent();
        }


        private static juhe childFromInstanc;
        public static juhe ChildFromInstanc
        {
            get
            {
                if (childFromInstanc == null || childFromInstanc.IsDisposed)
                {
                    childFromInstanc = new juhe();
                }

                return childFromInstanc;
            }
        }

        private void freshdt()
        {
            string SQL = string.Format("select workshop as 车间,lastbatch.itemcode as 物料, desc1 as 描述,batch1 as 丙烯腈,batch2 as 二甲基亚砜,batch3 as 衣康酸 from lastbatch left join masterdata on lastbatch.itemcode=masterdata.itemcode where workshop in ('ys1','ys2','ys3') and lastbatch.itemcode<>'' order by workshop");
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            ds = Class1.GetAllDataSet(SQL);
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

        private void button2_Click(object sender, EventArgs e)
        {
            freshdt();        
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int c = 0;
            for (int i = 0; i < dataGridView1.Rows.Count - 1; i++)
            {
                string b1 = dataGridView1.Rows[i].Cells[3].Value.ToString().Trim();
                string b2 = dataGridView1.Rows[i].Cells[4].Value.ToString().Trim();
                string b3 = dataGridView1.Rows[i].Cells[5].Value.ToString().Trim();
                string ws = dataGridView1.Rows[i].Cells[0].Value.ToString().Trim();
                string itm = dataGridView1.Rows[i].Cells[1].Value.ToString().Trim();
                string sql = string.Format("update lastbatch set batch1='{0}',batch2='{1}',batch3='{2}' where itemcode='{3}' and workshop='{4}'", b1,b2,b3,itm,ws);
                c = Class1.ExcuteScal(sql);
                //MessageBox.Show(sql);

            }
            if (c == 0)
            {
                MessageBox.Show("更新失败");
            }
            else
            {
                MessageBox.Show("更新成功");
            }
        }
    }
}
