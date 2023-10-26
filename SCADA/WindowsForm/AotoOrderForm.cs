using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SCADA
{
    public partial class AotoOrderForm : Form
    {
        public static bool Fvalue1stop = false;//提示临界值停自动报警
        public static bool zhiliangflage = true;//质量优先
        public static bool xiaolvflage = false;//效率优先
        public static int  Aleavel = 1;//A中物料的等级
        public static int Bleavel = 1;//A中物料的等级
        public static int Cleavel = 1;//A中物料的等级
        public static int Dleavel = 1;//A中物料的等级

        public static int Modeleavel1 = 1;//模型1成套等级
        public static int Modeleavel2 = 1;//模型2成套等级
        public static int Modeleavel3 = 1;//模型3成套等级
        public static int Modeleavel4 = 1;//模型4成套等级

        public static int Fvalue1 = 60;//提示临界值
        public static int Fvalue2 = 80;//报警临界值
        public static bool Fvalue1over = false;//超过临界值1
        public static bool Fvalue2over = false;//超过临界值2
        AutoSizeFormClass aotosize = new AutoSizeFormClass();
        public AotoOrderForm()
        {
            InitializeComponent();
            checkBox1.Checked = false;
            Fvalue1stop = false;
            buttonzhiliang.Text = "选择";
            buttonzhiliang.BackColor =DarkTurquoise;
            buttonxiaolv.Text = "已选中";
            buttonxiaolv.BackColor = Color.Gray;
            zhiliangflage = false;
            xiaolvflage = true;
            textBoxFvalue1.Text = Fvalue1.ToString();
            textBoxFvalue2.Text = Fvalue2.ToString();
            textBoxA.Text = "1";
            textBoxB.Text = "1";
            textBoxC.Text = "1";
            textBoxD.Text = "1";
            textBox1hao.Text = "1";
            textBox2hao.Text = "1";
            textBox3hao.Text = "1";
            textBox4hao.Text = "1";
        }

        private void buttonzhiliang_Click(object sender, EventArgs e)
        {
            buttonzhiliang.Text = "已选中";
            buttonzhiliang.BackColor = Color.Gray;
            buttonxiaolv.Text = "选择";
            buttonxiaolv.BackColor =DarkTurquoise;
            zhiliangflage = true;
            xiaolvflage = false;
        }

        private void buttonxiaolv_Click(object sender, EventArgs e)
        {

            buttonzhiliang.Text = "选择";
            buttonxiaolv.Text = "已选中";
            buttonxiaolv.BackColor = Color.Gray;
            buttonzhiliang.BackColor =DarkTurquoise;
            zhiliangflage = false;
            xiaolvflage = false;
        }

        private void textBoxA_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar <= '4' && e.KeyChar >= '1') || e.KeyChar == (char)Keys.Back || e.KeyChar == (char)Keys.Enter)
            {
                e.Handled = false;
            }
            else
            {
                MessageBox.Show("请输入数字");
                textBoxA.Focus();
            }
        }

        private void textBoxA_Leave(object sender, EventArgs e)
        {
            string MagNoStr = ((TextBox)sender).Text;
            if (MagNoStr == "")
            {
                return;
            }
            try
            {
                int MagNo = Convert.ToInt32(MagNoStr);
                if (MagNo > 4 || MagNo < 1)
                {
                     MessageBox.Show("请输入1-4之间的数字");
                    textBoxA.Focus();
                }
                else
                {

                }
            }
            catch
            {
                MessageBox.Show("请输入1-4之间的数字");
                textBoxA.Focus();
            }
        }

        private void textBoxB_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar <= '4' && e.KeyChar >= '1') || e.KeyChar == (char)Keys.Back || e.KeyChar == (char)Keys.Enter)
            {
                e.Handled = false;
            }
            else
            {
                MessageBox.Show("请输入数字");
                textBoxB.Focus();
            }
        }

        private void textBoxB_Leave(object sender, EventArgs e)
        {
            string MagNoStr = ((TextBox)sender).Text;
            if (MagNoStr == "")
            {
                return;
            }
            try
            {
                int MagNo = Convert.ToInt32(MagNoStr);
                if (MagNo > 4 || MagNo < 1)
                {
                    MessageBox.Show("请输入1-4之间的数字");
                    textBoxB.Focus();
                }
                else
                {

                }
            }
            catch
            {
                MessageBox.Show("请输入1-4之间的数字");
                textBoxB.Focus();
            }
        }

        private void textBoxC_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar <= '4' && e.KeyChar >= '1') || e.KeyChar == (char)Keys.Back || e.KeyChar == (char)Keys.Enter)
            {
                e.Handled = false;
            }
            else
            {
                MessageBox.Show("请输入数字");
                textBoxC.Focus();
            }
        }

        private void textBoxC_Leave(object sender, EventArgs e)
        {
            string MagNoStr = ((TextBox)sender).Text;
            if (MagNoStr == "")
            {
                return;
            }
            try
            {
                int MagNo = Convert.ToInt32(MagNoStr);
                if (MagNo > 4 || MagNo < 1)
                {
                    MessageBox.Show("请输入1-4之间的数字");
                    textBoxC.Focus();
                }
                else
                {

                }
            }
            catch
            {
                MessageBox.Show("请输入1-4之间的数字");
                textBoxC.Focus();
            }
        }

        private void textBoxD_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar <= '4' && e.KeyChar >= '1') || e.KeyChar == (char)Keys.Back || e.KeyChar == (char)Keys.Enter)
            {
                e.Handled = false;
            }
            else
            {
                MessageBox.Show("请输入数字");
                textBoxD.Focus();
            }
        }

        private void textBoxD_Leave(object sender, EventArgs e)
        {
            string MagNoStr = ((TextBox)sender).Text;
            if (MagNoStr == "")
            {
                return;
            }
            try
            {
                int MagNo = Convert.ToInt32(MagNoStr);
                if (MagNo > 4 || MagNo < 1)
                {
                    MessageBox.Show("请输入1-4之间的数字");
                    textBoxD.Focus();
                }
                else
                {

                }
            }
            catch
            {
                MessageBox.Show("请输入1-4之间的数字");
                textBoxD.Focus();
            }
        }

        private void textBoxFvalue1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar <= '9' && e.KeyChar >= '0') || e.KeyChar == (char)Keys.Back || e.KeyChar == (char)Keys.Enter)
            {
                e.Handled = false;
            }
            else
            {
                MessageBox.Show("请输入数字");
                textBoxFvalue1.Focus();
            }
        }

        private void textBoxFvalue1_Leave(object sender, EventArgs e)
        {
            string MagNoStr = ((TextBox)sender).Text;
            if (MagNoStr == "")
            {
                return;
            }
            try
            {
                int MagNo = Convert.ToInt32(MagNoStr);
                if (MagNo > 100|| MagNo < 1)
                {
                    MessageBox.Show("请输入1-100之间的整数");
                    textBoxFvalue1.Focus();
                }
                else
                {

                }
            }
            catch
            {
                MessageBox.Show("请输入1-100之间的整数");
                textBoxFvalue1.Focus();
            }
        }

        private void textBoxFvalue2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar <= '9' && e.KeyChar >= '0') || e.KeyChar == (char)Keys.Back || e.KeyChar == (char)Keys.Enter)
            {
                e.Handled = false;
            }
            else
            {
                MessageBox.Show("请输入数字");
                textBoxFvalue2.Focus();
            }
        }

        private void textBoxFvalue2_Leave(object sender, EventArgs e)
        {
            string MagNoStr = ((TextBox)sender).Text;
            if (MagNoStr == "")
            {
                return;
            }
            try
            {
                int MagNo = Convert.ToInt32(MagNoStr);
                if (MagNo > 100 || MagNo < 1)
                {
                    MessageBox.Show("请输入1-100之间的整数");
                    textBoxFvalue2.Focus();
                }
                else
                {

                }
            }
            catch
            {
                MessageBox.Show("请输入1-100之间的整数");
                textBoxFvalue2.Focus();
            }
        }

        private void AotoOrderForm_SizeChanged(object sender, EventArgs e)
        {

            aotosize.controlAutoSize(this);
        }

     

        private void checkBox2_Click(object sender, EventArgs e)
        {
            MessageBox.Show("到达临界点必须暂停自动执行功能");
            return;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked==true)
            {
                Fvalue1stop=true;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {         
            int value1 = 0;
            int value2 = 1;
            int Avalue = 0;
            int Bvalue = 0;
            int Cvalue = 0;
            int Dvalue = 0;
            int mode1 = 0;
            int mode2 = 0;
            int mode3 = 0;
            int mode4 = 0;


            try
            {
                Avalue = Convert.ToInt32(textBoxA.Text);
                if(Avalue>4|| Avalue<1)
                {
                    Avalue = 1;
                    textBoxA.Text = "1";
                }
            }
            catch
            {
                MessageBox.Show("A中类型零件优先级设置是否正确");
                return; 
            }

            try
            {
                Bvalue = Convert.ToInt32(textBoxB.Text);
                if (Bvalue > 4 || Bvalue < 1)
                {
                    Bvalue = 1;
                    textBoxB.Text = "1";
                }
            }
            catch
            {
                MessageBox.Show("A中类型零件优先级设置是否正确");
                return; 
            }

            try
            {
                Cvalue = Convert.ToInt32(textBoxC.Text);
                if (Cvalue > 4 || Cvalue < 1)
                {
                    Cvalue = 1;
                    textBoxC.Text = "1";
                }
            }
            catch
            {
                MessageBox.Show("A中类型零件优先级设置是否正确");
                return; 
            }

            try
            {
                Dvalue = Convert.ToInt32(textBoxD.Text);
                if (Dvalue > 4 || Dvalue < 1)
                {
                    Dvalue = 1;
                    textBoxD.Text = "1";
                }
            }
            catch
            {
                MessageBox.Show("A中类型零件优先级设置是否正确");
                return; 
            }
            try
            {
                mode1 = Convert.ToInt32(textBox1hao.Text);
                if (mode1 > 4 || mode1 < 1)
                {
                    mode1 = 1;
                    textBox1hao.Text = "1";
                    MessageBox.Show("模型1优先级设置是否正确");
                }
            }
            catch
            {
                MessageBox.Show("模型1优先级设置是否正确");
                return;
            }
            try
            {
                mode2 = Convert.ToInt32(textBox2hao.Text);
                if (mode2 > 4 || mode2 < 1)
                {
                    mode2 = 1;
                    textBox2hao.Text = "1";
                    MessageBox.Show("模型2优先级设置是否正确");
                }
            }
            catch
            {
                MessageBox.Show("模型2优先级设置是否正确");
                return;
            }
            try
            {
                mode3 = Convert.ToInt32(textBox3hao.Text);
                if (mode3 > 4 || mode3 < 1)
                {
                    mode3 = 1;
                    textBox3hao.Text = "1";
                    MessageBox.Show("模型3优先级设置是否正确");
                }
            }
            catch
            {
                MessageBox.Show("模型3优先级设置是否正确");
                return;
            }
            try
            {
                mode4 = Convert.ToInt32(textBox4hao.Text);
                if (mode4 > 4 || mode4 < 1)
                {
                    mode4 = 1;
                    textBox4hao.Text = "1";
                    MessageBox.Show("模型4优先级设置是否正确");
                }
            }
            catch
            {
                MessageBox.Show("模型4优先级设置是否正确");
                return;
            }
            if (checkBox1.Checked == true)
            {
                Fvalue1stop = true;
            }
            else
            {
                Fvalue1stop = false;
            }

            if (buttonxiaolv.Text == "已选中")
            {
                xiaolvflage = true;
                zhiliangflage = false;
            }
            else
            {
                xiaolvflage = false;
                zhiliangflage = true;
            }
            Fvalue1 = value1;
            Fvalue2 = value2;

            Aleavel = Avalue;
            Bleavel = Bvalue;
            Cleavel = Cvalue;
            Dleavel = Dvalue;

            Modeleavel1 = mode1;
            Modeleavel2 = mode2;
            Modeleavel3 = mode3;
            Modeleavel4 = mode4;

            MessageBox.Show("设置保存成功");
            return; 
        }

      
    }
}
