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
    public partial class showboxlist : Form
    {
        public showboxlist()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.Manual;
            this.Location = new Point(0, 0);
            dateTimePicker1.Text = DateTime.Now.ToString("yy/MM/dd");
        }


        private static showboxlist childFromInstanc;
        public static showboxlist ChildFromInstanc
        {
            get
            {
                if (childFromInstanc == null || childFromInstanc.IsDisposed)
                {
                    childFromInstanc = new showboxlist();
                }

                return childFromInstanc;
            }
        }


        private void button1_Click(object sender, EventArgs e)
        {
            string sql0;
            sql0 = string.Format("select convert(int,replace(replace(T1.boxno,left(T1.boxno,charindex('-',T1.boxno)),''),left(replace(T1.boxno,left(T1.boxno,charindex('-',T1.boxno)),''),charindex('-',replace(T1.boxno,left(T1.boxno,charindex('-',T1.boxno)),''))),'')) as X,T1.boxno as 箱号,count(T1.batch) as 卷数,sum(T1.pro1) as 重量,sum(T1.len) as 长度 from stock T1 left join masterdata T2 on T1.material=T2.itemcode where convert(varchar(100),T1.dateboxin,111)='{0}' and c5 like '%{1}%'", dateTimePicker1.Text, Class1.workshop);
            if (checkBox1.Checked == true)
            {
                sql0 = sql0 + string.Format(" and T1.shift1='{0}'", Class1.shift1);
            }

            sql0 = sql0 + " group by boxno,T1.qatype order by left(boxno,charindex('-',boxno,0)-1),T1.qatype,X";

            
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            ds = Class1.GetAllDataSet(sql0);
            dt = ds.Tables[0];
            if (dt.Rows.Count > 0)
            {
                label3.Text = dt.Rows.Count.ToString();
                dataGridView1.DataSource = dt;
            }
            else
            {
                label3.Text = "0";
                dataGridView1.DataSource = null;
            }


        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {

            boxpick bp1 = (boxpick)this.Owner;
            bp1.Owner.Show();
            this.Dispose();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            string sql0 = string.Format("select T1.material as 物料,T2.desc1 as 型号,T1.sloc as 库位,T1.qatype as 类别,T1.batchbig as 批次,T1.batch as 卷号,T1.pro1 as 重量,T1.len as 长度,T1.tpno as 托盘,T1.t1 as 开始生产时间,T1.t2 as 结束生产时间,T1.c5 as 工位 from stock T1 left join masterdata T2 on T1.material=T2.itemcode where boxno='{0}'", dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString().Trim());
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            ds = Class1.GetAllDataSet(sql0);
            dt = ds.Tables[0];
            if (dt.Rows.Count > 0)
            {
                dataGridView2.DataSource = dt;
            }
            else
            {
                dataGridView2.DataSource = null;
            }
        }
    }
}
