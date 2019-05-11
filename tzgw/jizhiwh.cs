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
    public partial class jizhiwh : Form
    {
        public jizhiwh()
        {
            InitializeComponent();
            if (Class1.curuser != "wh3")
            {
                button4.Enabled = false;
                button5.Enabled = false;
            }
        }


        private static jizhiwh childFromInstanc;
        public static jizhiwh ChildFromInstanc
        {
            get
            {
                if (childFromInstanc == null || childFromInstanc.IsDisposed)
                {
                    childFromInstanc = new jizhiwh();
                }

                return childFromInstanc;
            }
        }

        private void jizhiwh_Load(object sender, EventArgs e)
        {

        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            checkBox1.Checked = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            finditem();
            
         }

        private void finditem()
        {
            string strsql0 = string.Format("select top {0} T2.pinming as 品名,T2.desc1 as 牌号,T1.sloc as 库位,T1.material as 物料编码,T1.batch as 卷号,T1.batchbig as 批号,T1.style as 幅宽,T1.qty as 数量,T1.productdate as 生产日期,T1.rmdate as 原丝日期,T1.dlnote as 发货信息,T1.dldate as 发货时间,T1.comm1 as 备注 from stockjz T1 left join masterdata2 T2 on T1.material = T2.itemcode where 1=1",textBox5.Text);

            if (pm.Text != "")
            {
                strsql0 = strsql0 + string.Format(" and T2.pinming like '%{0}%'", pm.Text.Trim());
            }
            if (batchbig.Text != "")
            {
                strsql0 = strsql0 + string.Format(" and T1.batchbig ='{0}'", batchbig.Text.Trim());
            }
            if (batch.Text != "")
            {
                strsql0 = strsql0 + string.Format(" and T1.batch ='{0}'", batch.Text.Trim());
            }
            if (fk.Text != "")
            {
                strsql0 = strsql0 + string.Format(" and T1.style='{0}'", fk.Text.Trim());
            }
            if (itemcode.Text != "")
            {
                strsql0 = strsql0 + string.Format(" and T1.material='{0}'", itemcode.Text.Trim());
            }
            if (ph.Text != "")
            {
                strsql0 = strsql0 + string.Format(" and T2.desc1 like '%{0}%'", ph.Text.Trim());
            }
            if (sloc.Text != "")
            {
                strsql0 = strsql0 + string.Format(" and T1.sloc='{0}'", sloc.Text.Trim());
            }
            if (checkBox1.Checked == true)
            {
                strsql0 = strsql0 + string.Format(" and T1.productdate >='{0}'", dateTimePicker1.Text.Trim());
            }
            if (checkBox2.Checked == true)
            {
                strsql0 = strsql0 + string.Format(" and T1.productdate <='{0}'", dateTimePicker2.Text.Trim());
            }

            strsql0 = strsql0 + " order by batchbig,batch,sloc,productdate,qty";


            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            ds = Class1.GetAllDataSet(strsql0);
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

        private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        {
            checkBox2.Checked = true;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (tarloc.Text != "")
            {
                updatestock("transfer");
            }
            else
            {
                MessageBox.Show("目标库位为空");
                tarloc.Focus();
            }
            
        }

        private void updatestock(string sql)
        {
            string sql0 = "";


            if (dataGridView1.Rows.Count >= 1)
            {
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    if (dataGridView1.Rows[i].Selected == true)
                    {
                        if (sql == "transfer")
                        {
                                    sql0 = string.Format("update stockjz set sloc='{0}' where batch='{1}'", tarloc.Text, dataGridView1.Rows[i].Cells[4].Value.ToString().Trim());
                        }
                        if (sql == "delivery")
                        {
                            sql0 = string.Format("update stockjz set sloc='{0}',dlnote='{2}',dldate='{3}' where batch='{1}'", "DLV-" + dataGridView1.Rows[i].Cells[2].Value.ToString().Trim(), dataGridView1.Rows[i].Cells[4].Value.ToString().Trim(),dlv.Text, DateTime.Now.ToString("yyyy-MM-dd HH:mm"));
                        }
                        //MessageBox.Show(sql0);
                        Class1.ExcuteScal(sql0);
                    }

                }
            finditem();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            sloc.Text = "";
            ph.Text = "";
            itemcode.Text = "";
            checkBox1.Checked = false;
            checkBox2.Checked = false;
            pm.Text = "";
            fk.Text = "";
            batchbig.Text = "";
            batch.Text = "";

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (dlv.Text != "")
            {
                updatestock("delivery");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            jizhiwu jzw = (jizhiwu)this.Owner;
            this.Owner.Show();
            this.Dispose();
        }
    }
}
