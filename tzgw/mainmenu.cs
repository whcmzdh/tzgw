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
    public partial class mainmenu : Form
    {
        public mainmenu()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.Manual;
            this.Location = new Point(0, 0);
            init1();
        }


        private void init1()
        {   
            // 1 原丝2 
            // 2 碳丝7 前
            // 3 碳丝7 后
            // 4 实验室
            // 5 仓库
            // 6 原丝1
            // 8 碳丝8 前
            // 9 碳丝8 后
            // 10 碳丝0 前
            // 11 碳丝0 后
            // 12 碳丝1 前
            // 13 碳丝1 后
            // 14 聚合
            // 15 yuansi 3
            // 16 yuansi 4
            // 17 碳丝西2 前
            // 18 碳丝西2 后
            // 19 仓库2
            // 20 实验室2
            // 21 机织物
            // 22 原丝 5



            if (Class1.rg1 == "1")
            {
                button1.Visible = true;
            }
            else
            {
                button1.Visible = false;
            }

            if (Class1.rg2 == "1")
            {
                button2.Visible = true;
            }
            else
            {
                button2.Visible = false;
            }

            if (Class1.rg3 == "1")
            {
                button3.Visible = true;
            }
            else
            {
                button3.Visible = false;
            }

            if (Class1.rg4 == "1")
            {
                button4.Visible = true;
            }
            else
            {
                button4.Visible = false;
            }

            if (Class1.rg5 == "1")
            {
                button5.Visible = true;
            }
            else
            {
                button5.Visible = false;
            }

            if (Class1.rg6 == "1")
            {
                button6.Visible = true;
            }
            else
            {
                button6.Visible = false;
            }

            if (Class1.rg7 == "1")
            {
                button8.Visible = true;
            }
            else
            {
                button8.Visible = false;
            }

            if (Class1.rg8 == "1")
            {
                button9.Visible = true;
            }
            else
            {
                button9.Visible = false;
            }

            if (Class1.rg9 == "1")
            {
                button10.Visible = true;
            }
            else
            {
                button10.Visible = false;
            }

            if (Class1.rg10 == "1")
            {
                button11.Visible = true;
            }
            else
            {
                button11.Visible = false;
            }
            if (Class1.rg11 == "1")
            {
                button12.Visible = true;
            }
            else
            {
                button12.Visible = false;
            }

            if (Class1.rg12 == "1")
            {
                button13.Visible = true;
            }
            else
            {
                button13.Visible = false;
            }


            if (Class1.rg13 == "1")
            {
                button14.Visible = true;
            }
            else
            {
                button14.Visible = false;
            }

            if (Class1.rg14== "1")
            {
                button15.Visible = true;
            }
            else
            {
                button15.Visible = false;
            }

            if (Class1.rg15 == "1")
            {
                button16.Visible = true;
            }
            else
            {
                button16.Visible = false;
            }

            if (Class1.rg17 == "1")
            {
                button17.Visible = true;
            }
            else
            {
                button17.Visible = false;
            }

            if (Class1.rg18 == "1")
            {
                button18.Visible = true;
            }
            else
            {
                button18.Visible = false;
            }

            if (Class1.rg19 == "1")
            {
                button19.Visible = true;
            }
            else
            {
                button19.Visible = false;
            }

            if (Class1.rg20 == "1")
            {
                button20.Visible = true;
            }
            else
            {
                button20.Visible = false;
            }

            if (Class1.rg21 == "1")
            {
                button21.Visible = true;
            }
            else
            {
                button21.Visible = false;
            }

            if (Class1.rg22 == "1")
            {
                button22.Visible = true;
            }
            else
            {
                button22.Visible = false;
            }

        }
        private void mainmenu_Load(object sender, EventArgs e)
        {
        }

        private static mainmenu childFromInstanc;
        public static mainmenu ChildFromInstanc
        {
            get
            {
                if (childFromInstanc == null || childFromInstanc.IsDisposed)
                {
                    childFromInstanc = new mainmenu();
                }

                return childFromInstanc;
            }
         }

        private void button1_Click(object sender, EventArgs e)
        {
            Class1.workshop = "ys2";
            this.Hide();
            yuansi ys2 = yuansi.ChildFromInstanc;
            if (ys2 != null)
            {
                ys2.Owner = this;
                ys2.Show();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Class1.workshop = "ts7";
            
            this.Hide();
            tansi_qian tsq7 = tansi_qian.ChildFromInstanc;
            if (tsq7 != null)
            {
                tsq7.Owner = this;
                tsq7.Show();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Class1.workshop = "ts7";
            this.Hide();
            tansi_hou tsh7 = tansi_hou.ChildFromInstanc;
            if (tsh7 != null)
            {
                tsh7.Owner = this;
                tsh7.Show();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Hide();
            lab lb = lab.ChildFromInstanc;
            if (lb != null)
            {
                lb.Owner = this;
                lb.Show();
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.Hide();
            wh wh1 = wh.ChildFromInstanc;
            if (wh1 != null)
            {
                wh1.Owner = this;
                wh1.Show();
            }
        }

        private void mainmenu_FormClosing(object sender, FormClosingEventArgs e)
        {
            Class1.killbartender();
            Class1.killapp();
            Application.Exit();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Class1.workshop = "ys1";
            this.Hide();
            yuansi ys1 = yuansi.ChildFromInstanc;
            if (ys1 != null)
            {
                ys1.Owner = this;
                ys1.Show();
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            Class1.killbartender();
            Class1.killapp();
            Application.Exit();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            Class1.workshop = "ts8";
            this.Hide();
            tansi_hou tsh8 = tansi_hou.ChildFromInstanc;
            if (tsh8 != null)
            {
                tsh8.Owner = this;
                tsh8.Show();
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            Class1.workshop = "ts01";
            this.Hide();
            tansi_qian tsq01 = tansi_qian.ChildFromInstanc;
            if (tsq01 != null)
            {
                tsq01.Owner = this;
                tsq01.Show();
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {

            Class1.workshop = "ts8";
            this.Hide();
            tansi_qian tsq8 = tansi_qian.ChildFromInstanc;
            if (tsq8 != null)
            {
                tsq8.Owner = this;
                tsq8.Show();
            }
            
        }

        private void button11_Click(object sender, EventArgs e)
        {
            Class1.workshop = "ts01";
            this.Hide();
            tansi_hou tsh01 = tansi_hou.ChildFromInstanc;
            if (tsh01 != null)
            {
                tsh01.Owner = this;
                tsh01.Show();
            }
        }

        private void button13_Click(object sender, EventArgs e)
        {
            Class1.workshop = "ts02";
            this.Hide();
            tansi_hou tsh02 = tansi_hou.ChildFromInstanc;
            if (tsh02 != null)
            {
                tsh02.Owner = this;
                tsh02.Show();
            }
        }

        private void button12_Click(object sender, EventArgs e)
        {
            Class1.workshop = "ts02";
            this.Hide();
            tansi_qian tsq02 = tansi_qian.ChildFromInstanc;
            if (tsq02 != null)
            {
                tsq02.Owner = this;
                tsq02.Show();
            }
        }

        private void button14_Click(object sender, EventArgs e)
        {
            juhe jh = juhe.ChildFromInstanc;
            if (jh != null)
            {
                jh.Owner = this;
                jh.Show();
            }
        }

        private void button15_Click(object sender, EventArgs e)
        {
            Class1.workshop = "ys3";
            this.Hide();
            yuansi ys3 = yuansi.ChildFromInstanc;
            if (ys3 != null)
            {
                ys3.Owner = this;
                ys3.Show();
            }
        }


        private void button16_Click(object sender, EventArgs e)
        {
            Class1.workshop = "ys4";
            this.Hide();
            yuansi ys4 = yuansi.ChildFromInstanc;
            if (ys4 != null)
            {
                ys4.Owner = this;
                ys4.Show();
            }
        }

        private void button17_Click_1(object sender, EventArgs e)
        {
            Class1.workshop = "ts2";
            this.Hide();
            tansi_qian tsx2q = tansi_qian.ChildFromInstanc;
            if (tsx2q != null)
            {
                tsx2q.Owner = this;
                tsx2q.Show();
            }
        }

        private void button18_Click_1(object sender, EventArgs e)
        {
            Class1.workshop = "ts2";
            this.Hide();
            tansi_hou tsx2h = tansi_hou.ChildFromInstanc;
            if (tsx2h != null)
            {
                tsx2h.Owner = this;
                tsx2h.Show();
            }
        }

        private void button19_Click(object sender, EventArgs e)
        {
            this.Hide();
            lab lb = lab.ChildFromInstanc;
            if (lb != null)
            {
                lb.Owner = this;
                lb.Show();
            }
        }

        private void button20_Click(object sender, EventArgs e)
        {
            Class1.workshop = "wh2";
            this.Hide();
            wh2 wh2 = wh2.ChildFromInstanc;
            if (wh2 != null)
            {
                wh2.Owner = this;
                wh2.Show();
            }
        }

        private void button21_Click(object sender, EventArgs e)
        {
            Class1.workshop = Class1.curuser;
            this.Hide();
            jizhiwu jz = jizhiwu.ChildFromInstanc;
            if (jz != null)
            {
                jz.Owner = this;
                jz.Show();
            }
        }

        private void button22_Click(object sender, EventArgs e)
        {
            Class1.workshop = "ys6";
            this.Hide();
            yuansi ys6 = yuansi.ChildFromInstanc;
            if (ys6 != null)
            {
                ys6.Owner = this;
                ys6.Show();
            }
        }
    }

}
