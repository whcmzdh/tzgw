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
    public partial class lab2 : Form
    {
        public lab2()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.Manual;
            this.Location = new Point(0, 0);
        }


        private static lab2 childFromInstanc;
        public static lab2 ChildFromInstanc
        {
            get
            {
                if (childFromInstanc == null || childFromInstanc.IsDisposed)
                {
                    childFromInstanc = new lab2();
                }

                return childFromInstanc;
            }
        }


        private void button1_Click(object sender, EventArgs e)
        {
            string sql1 = string.Format("select devicenum as 纺位,project as 项目,value as 检测值,lower as 下限,upper as 上限,labrecord2.comm2 as 结论,workshop as 车间,shift1 as 班组,date2 as 提交时间,status as 状态,rm as 原材料批次,batchbig as 批次,labrecord2.comm1 as 备注 from labrecord2 left join laboption on labrecord2.itemcode=laboption.itemcode and labrecord2.project=laboption.labpj where date1='{0}'", dt1.Text);
            if (pj.Text != "")
            {
                sql1 = sql1 + string.Format(" and project like '%{0}%'",pj.Text);
            }
            if (shift1.Text != "")
            {
                sql1 = sql1 + string.Format(" and shift1='{0}'", shift1.Text);
            }
            if (wk.Text != "")
            {
                sql1 = sql1 + string.Format(" and workshop='{0}'", wk.Text);
            }
            if (checkBox1.Checked == true)
            {

            }
            else
            {
                sql1 = sql1 + string.Format(" and value is null");
            }
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            ds = Class1.GetAllDataSet(sql1);
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

        private void button3_Click(object sender, EventArgs e)
        {
            lab lb = (lab)this.Owner;
            this.Owner.Show();
            this.Dispose();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                // devicenum as 纺位,project as 项目,value as 检测值,comm2 as 备注,workshop as 车间,shift1 as 班组,date2 as 提交时间,status as 状态,rm as 原材料批次,batchbig as 批次,comm1 as 备注
                string v1 = Convert.ToString(dataGridView1.Rows[i].Cells[2].Value).Trim();
                string v2 = Convert.ToString(dataGridView1.Rows[i].Cells[5].Value).Trim();
                string d1 = Convert.ToString(dataGridView1.Rows[i].Cells[6].Value).Trim();
                string d2 = Convert.ToString(dataGridView1.Rows[i].Cells[7].Value).Trim();
                string d3 = Convert.ToString(dataGridView1.Rows[i].Cells[8].Value).Trim();
                string d4 = Convert.ToString(dataGridView1.Rows[i].Cells[9].Value).Trim();
                string d5 = Convert.ToString(dataGridView1.Rows[i].Cells[1].Value).Trim();
                string d6 = Convert.ToString(dataGridView1.Rows[i].Cells[0].Value).Trim();
                string SQL1 = string.Format("update labrecord2 set value='{0}',comm2='{1}' where workshop='{2}' and shift1='{3}' and date2='{4}' and status='{5}' and project='{6}' and devicenum='{7}'",v1,v2,d1,d2,d3,d4,d5,d6);
                //MessageBox.Show(SQL1);
                int x = Class1.ExcuteScal(SQL1);
                if (x == 1)
                {
                    dataGridView1.Rows[i].DefaultCellStyle.BackColor = Color.LawnGreen;
                }

            }
        }

        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 2)
            {
                if (dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString() != "" & dataGridView1.Rows[e.RowIndex].Cells[4].Value.ToString() != "")
                {
                    if (Convert.ToDecimal(dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString().Trim()) >= Convert.ToDecimal(dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString().Trim()) & Convert.ToDecimal(dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString().Trim()) <= Convert.ToDecimal(dataGridView1.Rows[e.RowIndex].Cells[4].Value.ToString().Trim()))
                    {
                        dataGridView1.Rows[e.RowIndex].Cells[5].Value = "合格";
                    }
                }

            }
        }
    }
}
