using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace tzgw
{
    public partial class tansi_qian : Form
    {
        public tansi_qian()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.Manual;
            this.Location = new Point(0, 0);
            flashdataview();
            t1.Value = DateTime.Now;
            this.Text = Class1.workshop+" "+Class1.companyinfo;
        }



        private delegate void SetDGVSource(DataTable dt);//添加设置DataGridView的DataSource的代理  
        public static void SetDGVSourceFunction(DataTable dt)
        {
            if (dataGridView1.InvokeRequired)
            {
                SetDGVSource delegateSetSource = new SetDGVSource(SetDGVSourceFunction);
                dataGridView1.Invoke(delegateSetSource, new object[] { dt });

            }
            else
            {
                dataGridView1.DataSource = dt;
            }

        }





        private static tansi_qian childFromInstanc;
        public static tansi_qian ChildFromInstanc
        {
            get
            {
                if (childFromInstanc == null || childFromInstanc.IsDisposed)
                {
                    childFromInstanc = new tansi_qian();
                }

                return childFromInstanc;
            }
        }


        private void gw_TextChanged(object sender, EventArgs e)
        {
            yspc.Text = "";
            if (speedlock.Checked==false)
            {
                speed.Text = "";
            }



            flashdataview();

            if (gw.Text != "")
            {
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    if (dataGridView1.Rows[i].Cells[0].Value.ToString().Trim() == gw.Text)
                    {
                        dataGridView1.FirstDisplayedScrollingRowIndex = i;
                        //dataGridView1.Rows[i].Selected = true;
                        if (speedlock.Checked == false)
                        {
                            speed.Text = dataGridView1.Rows[i].Cells[7].Value.ToString().Trim();
                        }

                        yspc.Text = dataGridView1.Rows[i].Cells[3].Value.ToString().Trim();
                        dateTimePicker1.Text= DateTime.Now.ToString();

                    }
                }
            }

            button1.Enabled = true;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (gw.Text != "" & yspc.Text!="")
            {
                //查询当前状态
                string sqlpre = string.Format("select status1,batchcus from wsdevice where workshop='{0}' and devicenum='{1}'", Class1.workshop, gw.Text);
                //更新状态
                string sqlrun = string.Format("update wsdevice set t1='{0}',batchcus='{1}',itemuse='{2}',status1='运行',speed1='{5}' where workshop='{3}' and devicenum='{4}'", dateTimePicker1.Text, yspc.Text, rmet.Text.Trim(), Class1.workshop, gw.Text,speed.Text.Trim());
                string sqluse = "";
                string translog1 = ""; //记录上卷源和目标库位
                int sqlrunS;
                int sqluseS;
                int a = 0;
                string insyuansi = "";
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                ds = Class1.GetAllDataSet(sqlpre);
                dt = ds.Tables[0];
                string status1 = dt.Rows[0][0].ToString().Trim();
                if (usedL.Text.Trim() == "O")
                {
                    if (status1 == "运行")
                    {

                        if (MessageBox.Show("当前纺位原丝批次" + dt.Rows[0][1].ToString().Trim() + "将被替换为" + yspc.Text + ",确认操作?", "覆盖", MessageBoxButtons.OKCancel) == DialogResult.OK)
                        {
                            sqluse = string.Format("update stock set sloc='{1}',c5='O' where batch='{0}'", dt.Rows[0][1].ToString().Trim(), Class1.workshop);
                            sqluseS = Class1.ExcuteScal(sqluse);
                            insyuansi = string.Format("insert into yuansi(workshop,devicenum,date1,qty1,res,action1,batch) values('{0}','{1}','{2}','{3}','{4}','{5}','{6}')", Class1.workshop, gw.Text, dateTimePicker1.Text, reskg.Text, resm.Text, "下卷", dt.Rows[0][1].ToString().Trim());
                            a = Class1.ExcuteScal(insyuansi);
                            //                                                                                                                     batch,loc1,loc2,date1,action1,devicenum,qty1,res
                            translog1 = string.Format("insert into transferlog select '{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}'", yspc.Text, label25.Text, Class1.workshop, dateTimePicker1.Text, "退丝棚上线", gw.Text, reskg.Text, resm.Text);
                            a = Class1.ExcuteScal(translog1);
                            sqluse = string.Format("update stock set sloc='{2}',c5='{0}' where batch='{1}'", Class1.workshop + "-" + gw.Text, yspc.Text, Class1.workshop);
                            insyuansi = string.Format("insert into yuansi(workshop,devicenum,date1,qty1,res,action1,batch) values('{0}','{1}','{2}','{3}','{4}','{5}','{6}')", Class1.workshop, gw.Text, dateTimePicker1.Text, reskg.Text, resm.Text, "上卷", yspc.Text);
                            a = Class1.ExcuteScal(insyuansi);


                            sqluseS = Class1.ExcuteScal(sqluse);
                            sqlrunS = Class1.ExcuteScal(sqlrun);

                            if (sqluseS == 1)
                            {

                                flashdataview();
                                button1.Enabled = false;
                            }
                            else
                            {
                                MessageBox.Show("更新失败");
                            }
                        }


                    }
                    else
                    {
                        translog1 = string.Format("insert into transferlog select '{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}'", yspc.Text, label25.Text, Class1.workshop, dateTimePicker1.Text, "退丝棚上线", gw.Text, reskg.Text, resm.Text);
                        a = Class1.ExcuteScal(translog1);

                        sqlrunS = Class1.ExcuteScal(sqlrun);
                        sqluse = string.Format("update stock set sloc='{2}',c5='{0}' where batch='{1}'", Class1.workshop + "-" + gw.Text, yspc.Text, Class1.workshop);
                        sqluseS = Class1.ExcuteScal(sqluse);
                        insyuansi = string.Format("insert into yuansi(workshop,devicenum,date1,qty1,res,action1,batch) values('{0}','{1}','{2}','{3}','{4}','{5}','{6}')", Class1.workshop, gw.Text, dateTimePicker1.Text, reskg.Text, resm.Text, "上卷", yspc.Text);
                        a = Class1.ExcuteScal(insyuansi);
                        if (sqluseS == 1)
                        {

                            MessageBox.Show("成功");
                            button1.Enabled = false;
                            flashdataview();
                        }
                        else
                        {
                            MessageBox.Show("更新失败");
                        }
                    }
                }
                else
                {
                    MessageBox.Show("只能对非使用状态的原丝操作,当前原丝使用位置"+usedL.Text);

                }




                queryyuansi();
            }
        }

        private void yspc_TextChanged(object sender, EventArgs e)
        {
            if (yspc.Text != "")
            {
               
                string sql1 = "";

                sql1 = string.Format("select T2.desc1,T1.res,T1.qa,T1.len,T2.itemcode,c5,qa2,qa2r,T2.sp1,T1.sloc from stock T1 left join masterdata T2 on T1.material=T2.itemcode where T1.batch='{0}'", yspc.Text);

                DataSet ds = new DataSet();
                ds = Class1.GetAllDataSet(sql1);
                DataTable dt = new DataTable();
                dt = ds.Tables[0];
                if (dt.Rows.Count > 0)
                {
                    xmd.Text = dt.Rows[0][8].ToString().Trim();
                    desc1.Text = dt.Rows[0][0].ToString();
                    resm.Text = dt.Rows[0][3].ToString();
                    reskg.Text = dt.Rows[0][1].ToString();
                    qa.Text = dt.Rows[0][2].ToString();
                    rmet.Text = dt.Rows[0][4].ToString();
                    usedL.Text= dt.Rows[0][5].ToString().Trim();
                    qa2.Text= dt.Rows[0][6].ToString().Trim();
                    qa2r.Text = dt.Rows[0][7].ToString().Trim();
                    label25.Text= dt.Rows[0][9].ToString().Trim();
                }
                else
                {
                    desc1.Text = "";
                    reskg.Text = "";
                    resm.Text = "";
                    qa.Text = "";
                    rmet.Text = "";
                    usedL.Text = "";
                    qa2.Text = "";
                    qa2r.Text = "";
                    xmd.Text = "1";
                    label25.Text = "-";
                }



                string sql2 = string.Format("select date1 as 日期,devicenum as 工位,action1 as 动作,workshop as 车间,res as 长度,qty1 as 重量 from yuansi where batch='{0}' order by date1 desc", yspc.Text);
                DataSet ds2 = new DataSet();
                ds2 = Class1.GetAllDataSet(sql2);
                DataTable dt2 = new DataTable();
                dt2 = ds2.Tables[0];
                if (dt2.Rows.Count > 0)
                {
                    dataGridView3.DataSource = dt2;
                }
                else
                {
                    dataGridView3.DataSource = null;
                }
             }
            else
            {
                desc1.Text = "";
                reskg.Text = "";
                resm.Text = "";
                qa.Text = "";
                rmet.Text = "";
                usedL.Text = "";
                qa2.Text = "";
                qa2r.Text = "";
            }

        }

       


        private void flashdataview()
        {
            DataTable dt2 = new DataTable();
            SetDGVSourceFunction(dt2);

            string sql1 = "";

            sql1 = string.Format("select convert(int,rtrim(T1.devicenum)) as 纺位,rtrim(status1) as 状态, batchrun as 上个成品批次, batchcus as 原丝编号, T1.itemuse as 原丝描述, T1.t1 as 开始时间, DATEDIFF(Minute, T1.t1, GETDATE()) as 已运行分, speed1 as 速度, cast((T2.len - convert(float,speed1) * DATEDIFF(Minute, T1.t1, GETDATE()))as decimal(18,2)) as 剩余长度,cast(((T2.res - convert(float,speed1) * convert(float,T3.sp1) * DATEDIFF(Minute, T1.t1, GETDATE()))) as decimal(18,2)) as 剩余重量,T4.dslocfinish as 断丝点,T5.date1 as 最近上卷 from wsdevice T1 left join stock T2 on T1.batchcus = T2.batch left join masterdata T3 on T2.material = T3.itemcode left join duansi T4 on batchcus=T4.batch and T4.dslocfinish<=cast((T2.len - convert(float,speed1) * DATEDIFF(Minute, T1.t1, GETDATE()))as decimal(18,2)) left join (select * from (select batch,workshop,devicenum,date1,action1,row_number() over(partition by devicenum,workshop order by date1 desc) as rows1 from yuansi where action1='上卷') tt where rows1<=1) T5 on T1.devicenum=T5.devicenum and T1.workshop=T5.workshop and T1.batchcus=T5.batch where T1.workshop = '{0}' and 1=1", Class1.workshop);

            if (workonly.Checked == true)
            {
                sql1 = sql1 + string.Format(" and status1='运行'");
            }

            sql1 = sql1 + " order by 最近上卷 desc,剩余长度";

            DataSet ds = new DataSet();
            ds = Class1.GetAllDataSet(sql1);
            DataTable dt = new DataTable();
            dt = ds.Tables[0];

            if (dt.Rows.Count > 0)
            {
                SetDGVSourceFunction(dt);
                
            }


            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                dataGridView1.Rows[i].DefaultCellStyle.BackColor = SystemColors.Control;
                if (dataGridView1.Rows[i].Cells[1].Value.ToString().Trim() != "运行")
                {
                    dataGridView1.Rows[i].Cells[6].Value = "0";
                    dataGridView1.Rows[i].Cells[8].Value = "0";
                    dataGridView1.Rows[i].Cells[9].Value = "0";
                }
                else
                {
                    if (dataGridView1.Rows[i].Cells[10].Value.ToString()=="")
                    {
                        dataGridView1.Rows[i].Cells[10].Value = "0";
                    }


                    if (dataGridView1.Rows[i].Cells[8].Value.ToString() == "")
                    {
                        dataGridView1.Rows[i].Cells[8].Value = "0";
                    }
                    else
                    {
                        if (Convert.ToDecimal(dataGridView1.Rows[i].Cells[8].Value.ToString()) - Convert.ToDecimal(dataGridView1.Rows[i].Cells[10].Value.ToString()) <= 15 * Convert.ToDecimal(dataGridView1.Rows[i].Cells[7].Value.ToString()))
                        {
                            dataGridView1.Rows[i].DefaultCellStyle.BackColor = Color.LightBlue;
                        }

                        if (Convert.ToDecimal(dataGridView1.Rows[i].Cells[8].Value.ToString()) <= 5000)
                        {
                            dataGridView1.Rows[i].DefaultCellStyle.BackColor = Color.Yellow;
                        }

                        if (Convert.ToDecimal(dataGridView1.Rows[i].Cells[8].Value.ToString()) <= 3000)
                        {
                            dataGridView1.Rows[i].DefaultCellStyle.BackColor = Color.DarkRed;
                        }

                        if (Convert.ToDecimal(dataGridView1.Rows[i].Cells[8].Value.ToString()) <= 0)
                        {
                            dataGridView1.Rows[i].Cells[8].Value = "0";
                            dataGridView1.Rows[i].Cells[9].Value = "0";
                            dataGridView1.Rows[i].DefaultCellStyle.BackColor = Color.Red;
                        }
                    }

                }
            }


            //纠正过期纺位
            string sqlxx = string.Format("update wsdevice set t1='{0}' where DATEDIFF(day,GETDATE(),t1)>10 and workshop='{1}'", DateTime.Now.ToString(), Class1.workshop);
            int x = Class1.ExcuteScal(sqlxx);
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView1_Click(object sender, EventArgs e)
        {


                
        }

        private void button3_Click(object sender, EventArgs e)
        {
            flashdataview();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            gw.Text = "";
            yspc.Text = "";

            gw.Focus();


        }

        private void workonly_CheckStateChanged(object sender, EventArgs e)
        {
            flashdataview();
        }

        private void button6_Click(object sender, EventArgs e)
        {

            mainmenu mn = (mainmenu)this.Owner;
            this.Owner.Show();
            this.Dispose();
        }


        private void wait5()
        {
            while (checkBox1.Checked==true)
            {
                Thread.Sleep(5000);
                flashdataview();
            }
            
        }



    private void checkBox1_CheckStateChanged(object sender, EventArgs e)
        {

            Thread th =new Thread(wait5);
            th.IsBackground = true;
            th.Start();

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void dataGridView1_DataSourceChanged(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {
            int a = 0;
            int tkflag = 0;
            string insyuansi = "";
            string instransfer = "";
            if (yspc.Text != "")
            {
                string sqluse = "";
                if (MessageBox.Show("是-移至退库待定,否-留在车间库位?", "库位", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    sqluse = string.Format("update stock set sloc='{1}-tk',c5='O' where batch='{0}'", yspc.Text, Class1.workshop);
                    tkflag = 1;
                }
                else
                {
                    sqluse = string.Format("update stock set sloc='{1}',c5='O' where batch='{0}'", yspc.Text, Class1.workshop);
                }

                int sqlrunuse = Class1.ExcuteScal(sqluse);
                




                //MessageBox.Show(insyuansi +"-"+ a.ToString());

                if (sqlrunuse == 1)
                {
                    //插入原丝上下卷记录
                    insyuansi = string.Format("insert into yuansi(workshop,devicenum,date1,qty1,res,action1,batch) values('{0}','{1}','{2}','{3}','{4}','{5}','{6}')", Class1.workshop, gw.Text, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), reskg.Text, resm.Text, "下卷", yspc.Text);
                    a = Class1.ExcuteScal(insyuansi);

                    usedL.Text = "O";

                    if (tkflag == 1)
                    {
                        //插入数据转移记录，用于查看原丝
                        instransfer = string.Format("insert into transferlog(batch,loc1,loc2,date1,action1,devicenum,qty1,res) values('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}')", yspc.Text, Class1.workshop, Class1.workshop + "-tk", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), "原丝退库", gw.Text, reskg.Text, resm.Text);
                        //MessageBox.Show(instransfer);
                        int x = Class1.ExcuteScal(instransfer);
                    }
                }
                else
                {
                    MessageBox.Show("更新失败");
                }
            }
            string sqlrun = string.Format("update wsdevice set t2='{0}',batchcus='',itemuse='',status1='停止',speed1='0' where workshop='{1}' and devicenum='{2}'", DateTime.Now.ToString(), Class1.workshop, gw.Text);
            
            int sqlrunS = Class1.ExcuteScal(sqlrun);
            
            if (sqlrunS == 1)
            {
                flashdataview();
            }
            else
            {
                MessageBox.Show("更新失败");
            }

            queryyuansi();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            checkBox1.Checked = false;
            flashdataview();
            if (MessageBox.Show("所有在用原丝将更新为显示的剩余长度,已耗用完的原丝纺位将变为停止,确认执行操作?", "更新", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    if (dataGridView1.Rows[i].Cells[1].Value.ToString().Trim() == "运行")
                    {
                        if (dataGridView1.Rows[i].Cells[8].Value.ToString().Trim() == "0" || dataGridView1.Rows[i].Cells[9].Value.ToString().Trim() == "0")
                        {
                            string sqluse = string.Format("update stock set sloc='{2}-xh',c5='O',res='{1}',len='0' where batch='{0}'", dataGridView1.Rows[i].Cells[3].Value.ToString(), dataGridView1.Rows[i].Cells[9].Value.ToString(), Class1.workshop);
                            int sqluse1 = Class1.ExcuteScal(sqluse);
                            if (sqluse1 == 1)
                            {
                                string sqlrun = string.Format("update wsdevice set t2='{0}',batchcus='',itemuse='',status1='停止' where workshop='{1}' and devicenum='{2}'", DateTime.Now.ToString(), Class1.workshop, dataGridView1.Rows[i].Cells[0].Value.ToString());
                                int sqlrun1 = Class1.ExcuteScal(sqlrun);
                                if (sqlrun1 == 1)
                                {
                                    dataGridView1.Rows[i].DefaultCellStyle.BackColor = SystemColors.Control;
                                }
                                string insyuansi = string.Format("insert into yuansi(workshop,devicenum,date1,qty1,res,action1,batch) values('{0}','{1}','{2}','{3}','{4}','{5}','{6}')", Class1.workshop, dataGridView1.Rows[i].Cells[0].Value.ToString(), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), "0", "0", "下卷", dataGridView1.Rows[i].Cells[3].Value.ToString());
                                int insyuansi1 = Class1.ExcuteScal(insyuansi);
                                string instransferx = string.Format("insert into transferlog(batch,loc1,loc2,date1,action1,devicenum,qty1,res) values('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}')", dataGridView1.Rows[i].Cells[3].Value.ToString(), Class1.workshop, Class1.workshop + "-xh", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), "原丝消耗", dataGridView1.Rows[i].Cells[0].Value.ToString(), "0", "0");
                                //MessageBox.Show(instransfer);
                                int x = Class1.ExcuteScal(instransferx);
                            }

                        }
                        else
                        {
                            //只更新长度 不改变状态
                            string sqluse = string.Format("update stock set sloc='{2}',res='{1}',len='{3}' where batch='{0}'", dataGridView1.Rows[i].Cells[3].Value.ToString(), dataGridView1.Rows[i].Cells[9].Value.ToString(), Class1.workshop, dataGridView1.Rows[i].Cells[8].Value.ToString());
                            int sqluse1 = Class1.ExcuteScal(sqluse);
                            if (sqluse1 == 1)
                            {
                                string sqlrun = string.Format("update wsdevice set t1='{0}' where workshop='{1}' and devicenum='{2}'", DateTime.Now.ToString(), Class1.workshop, dataGridView1.Rows[i].Cells[0].Value.ToString());
                                int sqlrun1 = Class1.ExcuteScal(sqlrun);
                                if (sqlrun1 == 1)
                                {
                                    dataGridView1.Rows[i].DefaultCellStyle.BackColor = SystemColors.Control;
                                }
                            }
                        }
                        
                        
                    }
                   
                }

                flashdataview();
            }

        }

        private void button8_Click(object sender, EventArgs e)
        {
            if (yspc.Text != "")
            {
                string sqluse = string.Format("update stock set sloc='{2}',res='{1}',len='{3}' where batch='{0}'", yspc.Text, reskg.Text, Class1.workshop, resm.Text);
                int sqluse1 = Class1.ExcuteScal(sqluse);
                if (sqluse1 == 1)
                {
                    flashdataview();
                }
            }
            queryyuansi();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (yspc.Text != "")
            {
                string sqluse = string.Format("update stock set qa2='{0}',qa2r='{1}' where batch='{2}'", qa2.Text, qa2r.Text,yspc.Text);
                int sqluse1 = Class1.ExcuteScal(sqluse);
                if (sqluse1 == 1)
                {
                    flashdataview();
                }
            }
            queryyuansi();

        }

        private void tansi_qian_FormClosing(object sender, FormClosingEventArgs e)
        {
            Class1.killapp();
            Application.Exit();
        }

        private void label12_Click(object sender, EventArgs e)
        {
            dateTimePicker1.Value = DateTime.Now;
        }

        private void resm_TextChanged(object sender, EventArgs e)
        {
            if (resm.Text != "")
            {
                reskg.Text = (Convert.ToDecimal(resm.Text) * Convert.ToDecimal(xmd.Text)).ToString("#0.00");
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            queryyuansi();
        }

        private void queryyuansi()
        {
            string sql = "";
            string sql2 = "";
            string sql3 = "";
            string sql4 = "";
            string sql5 = "";
            string sql6 = "";
            if (comboBox1.Text != "")
            {
                sql = string.Format("select top 1000 T1.batch as 批次,T2.desc1 as 描述,T1.sloc as 库位,T1.qa2 as 质检,T1.len as 长度,T1.res as 剩余重量,T1.pro1 as 原重量 from stock T1 left join masterdata T2 on T1.material=T2.itemcode where left(tpno,3)='{0}'", comboBox1.Text);
                if (t1.Text != "")
                {
                    sql2 = string.Format(" and T1.stockin >='{0}'", t1.Text);
                }
                if (t2.Text != "")
                {
                    sql3 = string.Format(" and T1.stockin <='{0}'", t2.Text);
                }

                if (textBox2.Text != "")
                {
                    sql4 = string.Format(" and T2.desc1 like '%{0}%'", textBox2.Text);
                }
                if (batchx.Text != "")
                {
                    sql5 = string.Format(" and T1.batch like '%{0}%'", batchx.Text);
                }
                if (checkBox2.Checked == true)
                {
                    sql6 = string.Format(" and T1.len>0");
                }

                if (textBox1.Text != "")
                {
                    sql3 = string.Format(" and T1.sloc='{0}'", textBox1.Text);
                }

                if (textBox3.Text != "")
                {
                    sql3 = string.Format(" and T1.qa2 ='{0}'", textBox3.Text);
                }

                sql = sql + sql2 + sql3 + sql4 + sql5 + sql6;
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                ds = Class1.GetAllDataSet(sql);
                dt = ds.Tables[0];
                if (dt.Rows.Count > 0)
                {
                    dataGridView2.DataSource = dt;
                    label21.Text = dt.Rows.Count.ToString();
                    int s = 0;
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        s = s + Convert.ToInt32(dt.Rows[0][4]);
                    }
                    label23.Text = s.ToString();
                }
                else
                {
                    dataGridView2.DataSource = null;
                    label21.Text = "0";
                    label23.Text = "0";
                }
            }
        }
        
        private void dataGridView2_Click(object sender, EventArgs e)
        {
           
        }

        private void dataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                yspc.Text = dataGridView2.Rows[e.RowIndex].Cells[0].Value.ToString();
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            batchx.Text = "";
            t1.Text = DateTime.Now.ToString("yyyy/MM/dd");
            t2.Text = "";
            checkBox2.Checked = true;
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";

        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                gw.Text = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString().Trim();
            }
        }

        private void button11_Click(object sender, EventArgs e)
        {
            this.Hide();
            aftergenyuansi ag1 = aftergenyuansi.ChildFromInstanc;
            if (ag1 != null)
            {
                ag1.Owner = this;
                ag1.Show();
            }
        }

        private void label14_Click(object sender, EventArgs e)
        {
            t1.Value = DateTime.Now;
        }

        private void label16_Click(object sender, EventArgs e)
        {
            t2.Value = DateTime.Now;
        }

        private void button12_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确认更新全部纺位速度为" + speed.Text + "?(请先更新剩余长度后执行操作)", "更新", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                string sql = string.Format("update wsdevice set speed1='{0}' where workshop='{1}'", speed.Text, Class1.workshop);
                int c = Class1.ExcuteScal(sql);
                if (c >= 1)
                {
                    flashdataview();
                }
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button14_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确认将所选物料移至库位" + textBox4.Text + "?(只对库位为本车间的物料生效)", "转移", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                for (int i = 0; i < dataGridView2.Rows.Count -1; i++)
                {

                    if (dataGridView2.Rows[i].Selected == true && dataGridView2.Rows[i].Cells[2].Value.ToString().IndexOf(Class1.workshop)!=-1)
                    {
                        string SQL1 = string.Format("update stock set sloc='{0}' where batch='{1}'", textBox4.Text,dataGridView2.Rows[i].Cells[0].Value.ToString());
                        int c = Class1.ExcuteScal(SQL1);
                        if (textBox4.Text.IndexOf("tk") > 0)
                        {
                            string instransfer = "";
                            //插入数据转移记录，用于查看原丝
                            instransfer = string.Format("insert into transferlog(batch,loc1,loc2,date1,action1,devicenum,qty1,res) values('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}')", dataGridView2.Rows[i].Cells[0].Value.ToString(), dataGridView2.Rows[i].Cells[2].Value.ToString(), textBox4.Text, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), "手动转移", "" , dataGridView2.Rows[i].Cells[5].Value.ToString(), dataGridView2.Rows[i].Cells[4].Value.ToString());
                            int x = Class1.ExcuteScal(instransfer);
                        }
                    }
                }

            }
            queryyuansi();


        }

        private void button13_Click(object sender, EventArgs e)
        {
            string date1 = Interaction.InputBox("请输入下卷时间", "下卷", DateTime.Now.ToString(), -1, -1);

            if (yspc.Text != "" & speed.Text != "")
            {
                string sql = string.Format("select top 1 res,date1,DATEDIFF(MINUTE,date1,'{1}') from yuansi where batch='{0}' and action1='上卷' order by date1 desc",yspc.Text,date1);
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                ds = Class1.GetAllDataSet(sql);
                dt = ds.Tables[0];
                if (dt.Rows.Count > 0)
                {
                    string sj = dt.Rows[0][1].ToString();
                    decimal long1 = Convert.ToDecimal(dt.Rows[0][0].ToString());
                    decimal last1 = Convert.ToDecimal(dt.Rows[0][2].ToString());
                    decimal cost = last1 * Convert.ToDecimal(speed.Text);
                    decimal res1 = long1 - cost;
                    decimal reskg = res1 * Convert.ToDecimal(xmd.Text);
                    if (res1 < 0)
                    {
                        res1 = 0;
                    }
                    if (reskg < 0)
                    {
                        reskg = 0;
                    }
                    int res11 = Convert.ToInt32(res1);
                    if (MessageBox.Show("上卷时间"+sj+" 上卷时剩余"+ long1.ToString()+ "\r\n 历时分钟:" + last1.ToString()+" 历时消耗:"+cost.ToString()+ "\r\n 剩余长度:" + res11.ToString() + "\r\n 剩余重量:" + reskg.ToString("#0.00") + "\r\n"+" 是否按此剩余长度更新当前卷"+yspc.Text+"?", "更新剩余长度", MessageBoxButtons.OKCancel) == DialogResult.OK)
                    {
                        string sql2 = string.Format("update stock set len='{0}',res='{1}' where batch='{2}'", res11.ToString(),reskg.ToString("#0.00"), yspc.Text);
                        int c = Class1.ExcuteScal(sql2);
                        if (c == 1)
                        {
                            yspc_TextChanged(sender, e);
                        }
                        else
                        {
                            MessageBox.Show("更新失败");
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("原丝批次或速度为空");
            }
        }

        private void reskg_TextChanged(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
