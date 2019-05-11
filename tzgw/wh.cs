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
    public partial class wh : Form
    {
        public wh()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.Manual;
            this.Location = new Point(0, 0);
            if (Class1.curuser != "wh")
            {
                button2.Enabled = false;
                button3.Enabled = false;
                

            }
            this.Text = Class1.workshop + " " + Class1.companyinfo;
        }

        private void wh_Load(object sender, EventArgs e)
        {

        }

        private static wh childFromInstanc;
        public static wh ChildFromInstanc
        {
            get
            {
                if (childFromInstanc == null || childFromInstanc.IsDisposed)
                {
                    childFromInstanc = new wh();
                }

                return childFromInstanc;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            mainmenu mn = (mainmenu)this.Owner;
            this.Owner.Show();
            this.Dispose();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            
         
        }



        private void finditem()
        {
            if (button6.Text == "扫码入库")
            {
                string sql0 = "";
                if (checkBox1.Checked == false)
                {
                    if (textBox1.Text != "")
                    {
                        sql0 = string.Format("select top {0} convert(int,replace(replace(T1.boxno,left(T1.boxno,charindex('-',T1.boxno)),''),left(replace(T1.boxno,left(T1.boxno,charindex('-',T1.boxno)),''),charindex('-',replace(T1.boxno,left(T1.boxno,charindex('-',T1.boxno)),''))),'')) as R1,T1.material as 物料,T2.desc1 as 型号,T1.sloc as 库位,T1.qatype as 类别,T1.batchbig as 批次,T1.boxno as 箱号,T1.batch as 卷号,T1.pro1 as 重量,convert(int,T1.pro1/T2.sp1) as 长度,T1.tpno as 托盘,T1.dlnote as 发货单号,T1.datewhin as 入库日期,T1.t1 as 开始生产时间,T1.t2 as 结束生产时间,T1.c5 as 占用工位,isnull(T1.res,0) as 剩余重量,isnull(T1.len,0) as 剩余长度,T1.dldate as 发货时间,T1.comm1 as 备注,T1.rmbatch1 as 原材料1批次,T1.rmbatch2 as 原材料2批次,T1.rmbatch3 as 原材料3批次,T1.rmbatch4 as 原材料4批次,T1.rmbatch5 as 原材料5批次,T1.rmbatch as 原丝批次 from stock T1 left join masterdata T2 on T1.material=T2.itemcode where 1=1", textBox1.Text);
                    }
                    else
                    {
                        sql0 = string.Format("select convert(int,replace(replace(T1.boxno,left(T1.boxno,charindex('-',T1.boxno)),''),left(replace(T1.boxno,left(T1.boxno,charindex('-',T1.boxno)),''),charindex('-',replace(T1.boxno,left(T1.boxno,charindex('-',T1.boxno)),''))),'')) as R1,T1.material as 物料,T2.desc1 as 型号,T1.sloc as 库位,T1.qatype as 类别,T1.batchbig as 批次,T1.boxno as 箱号,T1.batch as 卷号,T1.pro1 as 重量,convert(int,T1.pro1/T2.sp1) as 长度,T1.tpno as 托盘,T1.dlnote as 发货单号,T1.datewhin as 入库日期,T1.t1 as 开始生产时间,T1.t2 as 结束生产时间,T1.c5 as 占用工位,isnull(T1.res,0) as 剩余重量,isnull(T1.len,0) as 剩余长度,T1.dldate as 发货时间,T1.comm1 as 备注,T1.rmbatch1 as 原材料1批次,T1.rmbatch2 as 原材料2批次,T1.rmbatch3 as 原材料3批次,T1.rmbatch4 as 原材料4批次,T1.rmbatch5 as 原材料5批次,T1.rmbatch as 原丝批次 from stock T1 left join masterdata T2 on T1.material=T2.itemcode where 1=1", textBox1.Text);

                    }
                }
                else
                {
                    if (textBox1.Text != "")
                    {
                        sql0 = string.Format("select top {0} convert(int,replace(replace(T1.boxno,left(T1.boxno,charindex('-',T1.boxno)),''),left(replace(T1.boxno,left(T1.boxno,charindex('-',T1.boxno)),''),charindex('-',replace(T1.boxno,left(T1.boxno,charindex('-',T1.boxno)),''))),'')) as R1,T1.material as 物料,T2.desc1 as 型号,T1.sloc as 库位,T1.qatype as 类别,T1.batchbig as 批次,T1.boxno as 箱号,count(T1.batch) as 卷数,sum(T1.pro1) as 重量,sum(T1.len) as 长度,T1.tpno as 托盘,T1.dlnote as 发货单号,T1.datewhin as 入库日期,T1.comm1 as 备注 from stock T1 left join masterdata T2 on T1.material=T2.itemcode where 1=1 and T1.boxno is not null and T1.boxno<>''", textBox1.Text);
                    }
                    else
                    {
                        sql0 = string.Format("select convert(int,replace(replace(T1.boxno,left(T1.boxno,charindex('-',T1.boxno)),''),left(replace(T1.boxno,left(T1.boxno,charindex('-',T1.boxno)),''),charindex('-',replace(T1.boxno,left(T1.boxno,charindex('-',T1.boxno)),''))),'')) as R1,T1.material as 物料,T2.desc1 as 型号,T1.sloc as 库位,T1.qatype as 类别,T1.batchbig as 批次,T1.boxno as 箱号,count(T1.batch) as 卷数,sum(T1.pro1) as 重量,sum(T1.len) as 长度,T1.tpno as 托盘,T1.dlnote as 发货单号,T1.datewhin as 入库日期,T1.comm1 as 备注 from stock T1 left join masterdata T2 on T1.material=T2.itemcode where 1=1 and T1.boxno is not null and T1.boxno<>''", textBox1.Text);

                    }
                }

                if (checkBox0.Checked == true)
                {
                    if (c1.Text != "")
                    {
                        sql0 = sql0 + string.Format(" and T1.material='{0}'", c1.Text.Trim());
                    }
                    if (c2.Text != "")
                    {
                        sql0 = sql0 + string.Format(" and T1.batch='{0}'", c2.Text.Trim());
                    }
                    if (c3.Text != "")
                    {
                        sql0 = sql0 + string.Format(" and T1.boxno='{0}'", c3.Text.Trim());
                    }
                    if (c4.Text != "")
                    {
                        sql0 = sql0 + string.Format(" and T1.tpno='{0}'", c4.Text.Trim());
                    }
                    if (c5.Text != "")
                    {
                        sql0 = sql0 + string.Format(" and T1.sloc='{0}'", c5.Text.Trim());
                    }
                    if (c6.Text != "")
                    {
                        sql0 = sql0 + string.Format(" and T1.c5='{0}'", c6.Text.Trim());
                    }
                    if (c7.Text != "")
                    {
                        sql0 = sql0 + string.Format(" and T1.batchbig='{0}'", c7.Text.Trim());
                    }
                    if (c8.Text != "")
                    {
                        sql0 = sql0 + string.Format(" and T1.qatype='{0}'", c8.Text.Trim());
                    }
                    if (c9.Text != "")
                    {
                        sql0 = sql0 + string.Format(" and T1.qa='{0}'", c9.Text.Trim());
                    }
                    if (ct3.Checked == true)
                    {
                        sql0 = sql0 + string.Format(" and T1.datewhin >='{0}'", t3.Text);
                    }
                    if (ct4.Checked == true)
                    {
                        sql0 = sql0 + string.Format(" and T1.datewhin <='{0}'", t4.Text);
                    }
                    if (xh.Text !="")
                    {
                        sql0 = sql0 + string.Format(" and T2.desc1 like '%{0}%'", xh.Text);
                    }
                }
                else
                {
                    if (c1.Text != "")
                    {
                        sql0 = sql0 + string.Format(" and T1.material like '%{0}%'", c1.Text.Trim());
                    }
                    if (c2.Text != "")
                    {
                        sql0 = sql0 + string.Format(" and T1.batch like '%{0}%'", c2.Text.Trim());
                    }
                    if (c3.Text != "")
                    {
                        sql0 = sql0 + string.Format(" and T1.boxno like '%{0}%'", c3.Text.Trim());
                    }
                    if (c4.Text != "")
                    {
                        sql0 = sql0 + string.Format(" and T1.tpno like '%{0}%'", c4.Text.Trim());
                    }
                    if (c5.Text != "")
                    {
                        sql0 = sql0 + string.Format(" and T1.sloc like '%{0}%'", c5.Text.Trim());
                    }
                    if (c6.Text != "")
                    {
                        sql0 = sql0 + string.Format(" and T1.c5 like '%{0}%'", c6.Text.Trim());
                    }
                    if (c7.Text != "")
                    {
                        sql0 = sql0 + string.Format(" and T1.batchbig like '%{0}%'", c7.Text.Trim());
                    }

                    if (c8.Text != "")
                    {
                        sql0 = sql0 + string.Format(" and T1.qatype= '{0}'", c8.Text.Trim());
                    }
                    if (c9.Text != "")
                    {
                        sql0 = sql0 + string.Format(" and T1.qa like '%{0}%'", c9.Text.Trim());
                    }
                    if (ct3.Checked == true)
                    {
                        sql0 = sql0 + string.Format(" and T1.datewhin >='{0}'", t3.Text);
                    }
                    if (ct4.Checked == true)
                    {
                        sql0 = sql0 + string.Format(" and T1.datewhin <='{0}'", t4.Text);
                    }
                    if (xh.Text != "")
                    {
                        sql0 = sql0 + string.Format(" and T2.desc1 like '%{0}%'", xh.Text);
                    }

                }


                if (dlcb.Checked == true)
                {
                    sql0 = sql0 + " and left(T1.sloc,3)<>'DLV'";
                }


                if (checkBox1.Checked == true)
                {
                    sql0 = sql0 + " group by T1.boxno,T1.batchbig,T2.desc1,T1.sloc,T1.material,T1.dlnote,T1.qatype,T1.tpno,T1.datewhin,T1.comm1 order by T1.qatype,R1";
                }
                //MessageBox.Show(sql0);   
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                ds = Class1.GetAllDataSet(sql0);
                dt = ds.Tables[0];
                if (dt.Rows.Count > 0)
                {
                    dataGridView1.DataSource = dt;
                    decimal wet = 0;
                    decimal wet2 = 0;
                    decimal len = 0;
                    decimal len2 = 0;
                    decimal juan = 0;
                    for (int i = 0; i < dataGridView1.Rows.Count; i++)
                    {
                        dataGridView1.Rows[i].Selected = true;
                        if (dataGridView1.Rows[i].Cells[3].Value.ToString().Length >= 2)
                        {
                            if (dataGridView1.Rows[i].Cells[3].Value.ToString().Substring(0, 2) == "ys")
                            {
                                dataGridView1.Rows[i].DefaultCellStyle.BackColor = Color.Yellow;
                            }
                            if (dataGridView1.Rows[i].Cells[3].Value.ToString().Substring(0, 2) == "ts")
                            {
                                dataGridView1.Rows[i].DefaultCellStyle.BackColor = Color.LightGreen;
                            }

                        }
                        wet = wet + Convert.ToDecimal(dataGridView1.Rows[i].Cells[8].Value.ToString());
                        len = len + Convert.ToDecimal(dataGridView1.Rows[i].Cells[9].Value.ToString());
                        if (checkBox1.Checked == false)
                        {
                            wet2 = wet2 + Convert.ToDecimal(dataGridView1.Rows[i].Cells[16].Value.ToString());
                            len2 = len2 + Convert.ToDecimal(dataGridView1.Rows[i].Cells[17].Value.ToString());
                        }
                            
                        if (checkBox1.Checked == true)
                        {
                            juan = juan + Convert.ToDecimal(dataGridView1.Rows[i].Cells[7].Value.ToString());
                        }
                        else
                        {
                            juan = juan + 1;
                        }

                    }
                    tt.Text = dt.Rows.Count.ToString();
                    zz.Text = wet.ToString();
                    zc.Text = len.ToString();
                    zz2.Text = wet2.ToString();
                    zc2.Text = len2.ToString();
                    zj.Text = juan.ToString();
                }
                else
                {
                    dataGridView1.DataSource = null;

                    zz.Text = "0";
                    zc.Text = "0";
                    zz2.Text = "0";
                    zc2.Text = "0";
                    zj.Text = "0";
                    tt.Text = "0";

                }
            }
            

            
        }


        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
           
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
           
        }

        private void checkBox5_CheckedChanged(object sender, EventArgs e)
        {
         
        }
        private void button2_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确认将所列物料转移至库位"+sloc.Text+"?", "开始", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                if (sloc.Text != "")
                {
                    if (button6.Text == "扫码入库")
                    {
                        updatestock("transfer");
                    }
                    else
                    {
                        updatestock2("transfer");
                    }
                }
                else
                {
                    MessageBox.Show("目标库位为空");
                }

            }
        }


        private void updatestock2(string sql)
        {

            if (dataGridView2.Rows.Count >= 1)
            {
                for (int i = 0; i < dataGridView2.Rows.Count; i++)
                {
                    if (dataGridView2.Rows[i].DefaultCellStyle.BackColor == SystemColors.Control)
                    {
                        string sql0 = string.Format("update stock set sloc='{0}',comm1='{2}',datewhin='{3}' where boxno='{1}'", sloc.Text, dataGridView2.Rows[i].Cells[0].Value.ToString().Trim(), comm2.Text, dateTimePicker2.Text);
                        int x = Class1.ExcuteScal(sql0);
                        if (x > 0)
                        {
                            dataGridView2.Rows[i].DefaultCellStyle.BackColor = Color.LawnGreen;
                            dataGridView2.Rows[i].Cells[5].Value = sloc.Text;
                        }
                    }
                    
                }
            }

         }

        private void updatestock(string sql)
        {
            string sql0 = "";
            int flag = 0;
            if (MessageBox.Show("是否更新入库时间"+ dateTimePicker2.Text+"?", "入库", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                flag = 1;
            }
            

                if (dataGridView1.Rows.Count >= 1)
                {
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    if (dataGridView1.Rows[i].Selected == true)
                    {
                        if (sql == "transfer")
                        {
                            if (checkBox1.Checked == true) //成箱入库
                            {
                                if (flag==1)
                                {
                                    sql0 = string.Format("update stock set sloc='{0}',comm1='{2}',datewhin='{3}' where boxno='{1}'", sloc.Text, dataGridView1.Rows[i].Cells[6].Value.ToString().Trim(), comm2.Text, dateTimePicker2.Text);
                                }
                                else
                                {
                                    sql0 = string.Format("update stock set sloc='{0}',comm1='{2}' where boxno='{1}'", sloc.Text, dataGridView1.Rows[i].Cells[6].Value.ToString().Trim(), comm2.Text);
                                }
                            }
                            else
                            {
                                if (flag == 1)
                                {
                                    sql0 = string.Format("update stock set sloc='{0}',comm1='{2}',datewhin='{3}' where batch='{1}'", sloc.Text, dataGridView1.Rows[i].Cells[7].Value.ToString().Trim(), comm2.Text, dateTimePicker2.Text);
                                }
                                else
                                {
                                    sql0 = string.Format("update stock set sloc='{0}',comm1='{2}' where batch='{1}'", sloc.Text, dataGridView1.Rows[i].Cells[7].Value.ToString().Trim(), comm2.Text);
                                }
                            }
                        }
                        if (sql == "deliver")
                        {
                            if (checkBox1.Checked == true)
                            {
                                sql0 = string.Format("update stock set sloc='{0}',dlnote='{2}',dldate='{3}',comm1='{4}' where boxno='{1}'", "DLV-" + dataGridView1.Rows[i].Cells[3].Value.ToString().Trim(), dataGridView1.Rows[i].Cells[6].Value.ToString().Trim(), dlnote.Text, dateTimePicker1.Text, comm1.Text);
                            }
                            else
                            {
                                sql0 = string.Format("update stock set sloc='{0}',dlnote='{2}',dldate='{3}',comm1='{4}' where batch='{1}'", "DLV-" + dataGridView1.Rows[i].Cells[3].Value.ToString().Trim(), dataGridView1.Rows[i].Cells[7].Value.ToString().Trim(), dlnote.Text, dateTimePicker1.Text, comm1.Text);

                            }
                        }
                        //MessageBox.Show(sql0);
                        Class1.ExcuteScal(sql0);
                    }

                }

                finditem();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("以下产品链接到发货单" + dlnote.Text + "?", "发货", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                if (dlnote.Text != "")
                {
                    updatestock("deliver");
                }
                else
                {
                    MessageBox.Show("发货单号为空");
                }
                

            }
        }

        private void label4_Click(object sender, EventArgs e)
        {
            dateTimePicker1.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            finditem();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (button6.Text == "扫码入库")
            {
                c1.Text = "";
                c2.Text = "";
                c3.Text = "";
                c4.Text = "";
                c5.Text = "";
                c6.Text = "";
                c7.Text = "";
                c8.Text = "";
                c9.Text = "";
            }
            else
            {
                dataGridView2.Rows.Clear();
                zj.Text = "0";
                zz.Text = "0";
                zc.Text = "0";
                tt.Text = "0";
            }


        }

        private void checkBox0_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void checkBox1_CheckedChanged_1(object sender, EventArgs e)
        {
            finditem();
            if (checkBox1.Checked == true)
            {
                button6.Visible = true;
            }
            else
            {
                button6.Visible = false;
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (dataGridView2.Visible == false)
            {
                dataGridView2.Visible = true;
                dataGridView1.Visible = false;
                button6.Text = "选择入库";
                dataGridView2.CurrentCell = dataGridView2.Rows[0].Cells[0];
                zj.Text = "0";
                zz.Text = "0";
                zc.Text = "0";
                tt.Text = "0";
            }
            else
            {
                dataGridView2.Visible = false;
                dataGridView1.Visible = true;
                button6.Text = "扫码入库";
            }
        }

        private void dataGridView2_RowLeave(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView2_UserAddedRow(object sender, DataGridViewRowEventArgs e)
        {
            

       
        }


        private void rowstatuscheck()
        {
            decimal wet = 0;
            decimal len = 0;
            decimal juan = 0;
            decimal ct = 0;

            if (dataGridView2.Rows.Count > 0)
            {
                for (int c = 0; c < dataGridView2.Rows.Count - 1; c++)
                {
                    dataGridView2.Rows[c].Cells[1].Value = (c + 1).ToString();

                }


                if (dataGridView2.Rows.Count >2)
                {
                    //MessageBox.Show(dataGridView2.Rows.Count.ToString());
                    for (int c = dataGridView2.Rows.Count - 3; c >= 0; c--)
                    {
                        //MessageBox.Show(c.ToString()+"rc" + dataGridView2.Rows.Count.ToString());

                        string s = dataGridView2.Rows[c].Cells[0].Value.ToString();
                        //MessageBox.Show(s);
                        //MessageBox.Show("e-1:" + dataGridView2.Rows[e.RowIndex - 1].Cells[0].Value.ToString() + " cur:" + dataGridView2.Rows[s].Cells[0].Value.ToString());
                        if (s == dataGridView2.Rows[dataGridView2.Rows.Count - 2].Cells[0].Value.ToString())
                        {
                            dataGridView2.Rows[dataGridView2.Rows.Count - 2].Cells[2].Value = "重复";
                            dataGridView2.Rows[dataGridView2.Rows.Count - 2].Cells[9].Value = "0";
                            dataGridView2.Rows[dataGridView2.Rows.Count - 2].Cells[10].Value = "0";
                            dataGridView2.Rows[dataGridView2.Rows.Count - 2].Cells[11].Value = "0";
                            dataGridView2.Rows[dataGridView2.Rows.Count - 2].Cells[12].Value = "9999";
                            break;
                        }

                    }
                }





                for (int c = 0; c < dataGridView2.Rows.Count - 1; c++)
                {
                    if (dataGridView2.Rows[c].Cells[2].Value.ToString() == "重复")
                    {
                        dataGridView2.Rows[c].DefaultCellStyle.BackColor = Color.Yellow;
                    }
                    else if (dataGridView2.Rows[c].Cells[2].Value.ToString() == "不存在")
                    {
                        dataGridView2.Rows[c].DefaultCellStyle.BackColor = Color.Red;
                    }
                    else
                    {
                        dataGridView2.Rows[c].DefaultCellStyle.BackColor = SystemColors.Control;
                        ct = ct + 1;
                        wet = wet + Convert.ToDecimal(dataGridView2.Rows[c].Cells[10].Value.ToString());
                        len = len + Convert.ToDecimal(dataGridView2.Rows[c].Cells[11].Value.ToString());
                        juan = juan + Convert.ToDecimal(dataGridView2.Rows[c].Cells[9].Value.ToString());
                    }

                   
                }


                    zc.Text = len.ToString();
                    zj.Text = juan.ToString();
                    zz.Text = wet.ToString();
                    tt.Text = ct.ToString();


                
            }
            
        }


        private void dataGridView2_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > 0)
            {
                string sql0 = string.Format("select '',T1.material as 物料,T2.desc1 as 型号,T1.sloc as 库位,T1.qatype as 类别,T1.batchbig as 批次,T1.tpno as 托盘号,count(T1.batch) as 卷数,sum(T1.pro1) as 重量,sum(T1.len) as 长度 from stock T1 left join masterdata T2 on T1.material=T2.itemcode where 1=1 and T1.boxno='{0}' group by T1.boxno,T1.batchbig,T2.desc1,T1.sloc,T1.material,T1.qatype,T1.tpno", dataGridView2.Rows[e.RowIndex - 1].Cells[0].Value.ToString());
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                ds = Class1.GetAllDataSet(sql0);
                dt = ds.Tables[0];
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        dataGridView2.Rows[e.RowIndex - 1].Cells[i + 2].Value = dt.Rows[0][i].ToString().Trim();
                    }
                    //变色 默认插入标记色
                    dataGridView2.Rows[e.RowIndex - 1].DefaultCellStyle.BackColor = SystemColors.Control;
                    //分箱号
                    string x = dataGridView2.Rows[e.RowIndex - 1].Cells[0].Value.ToString().Trim();
                    string[] sArray = x.Split('-');
                    dataGridView2.Rows[e.RowIndex - 1].Cells[12].Value = sArray[sArray.Length - 1].ToString();
                }
                else
                {
                    dataGridView2.Rows[e.RowIndex - 1].Cells[2].Value = "不存在";
                    dataGridView2.Rows[e.RowIndex - 1].Cells[9].Value = "0";
                    dataGridView2.Rows[e.RowIndex - 1].Cells[10].Value = "0";
                    dataGridView2.Rows[e.RowIndex - 1].Cells[11].Value = "0";
                    dataGridView2.Rows[e.RowIndex - 1].Cells[12].Value = "9999";
                }




                rowstatuscheck();


            }
        }

        private void label20_Click(object sender, EventArgs e)
        {
            dateTimePicker2.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
        }

        private void label21_Click(object sender, EventArgs e)
        {

        }

        private void sloc_TextChanged(object sender, EventArgs e)
        {
            dateTimePicker2.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
        }

        private void button7_Click(object sender, EventArgs e)
        {
            this.Hide();
            wh_zutuo wz = wh_zutuo.ChildFromInstanc;
            if (wz != null)
            {
                wz.Owner = this;
                wz.Show();

            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            this.Hide();
            translogys trsl = translogys.ChildFromInstanc;
            if (trsl != null)
            {
                trsl.Owner = this;
                trsl.Show();

            }
        }
    }
}
