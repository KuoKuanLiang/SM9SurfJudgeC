using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SM9SurfJudge
{
    public partial class ProductParaSet : Form
    {
        public string[] parastr_pp;

        public ProductParaSet()
        {
            InitializeComponent();
        }

        private void ProductParaSet_Load(object sender, EventArgs e)
        {
            //判斷是否為檔案路徑
            if (System.IO.File.Exists(parastr_pp[1]))
            {
                // 从参数文件ParaFiles-P.XML里获取参数值
                string[] parastr = Comm.ReadXml(parastr_pp[1], "Para");
                this.cbStand.SelectedItem = parastr[0];
                this.txtNumber.SelectedItem = parastr[1];
                this.cbLambda.SelectedItem = parastr[2];
                this.cbShort.SelectedItem = parastr[3];
            }
            else
            {
                MessageBox.Show("取得ParaFile-P.XML之路徑錯誤，無法取得產品量測設定參數，請至系統參數設定修改好路徑！");
                this.Close();
                this.Dispose();
            }
            
        }
        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Comm.IsNumberF(e.KeyChar.ToString()) && !(e.KeyChar == '\b')&& !(e.KeyChar == '.'))
            {
                e.Handled = true;
            }
        }

        private void btnsave_Click(object sender, EventArgs e)
        {
            string[] str = new string[4];

            str[0] = this.cbStand.SelectedItem.ToString();
            str[1] = this.txtNumber.SelectedItem.ToString();
            str[2] = this.cbLambda.SelectedItem.ToString();
            str[3] = this.cbShort.SelectedItem.ToString();

            for (int i = 0; i < str.Length; i++)
            {
                if (str[i].Trim() == "")
                {
                    MessageBox.Show("请确认参数不要为空");
                    return;
                }
            }

            #region#参数修改类

            Comm.ModifyXml(parastr_pp[1], "Para", str);
            MessageBox.Show("参数修改成功！", "", MessageBoxButtons.OK, MessageBoxIcon.Information);

            //Quit
            this.Close();

            #endregion
        }

        private void btncancel_Click(object sender, EventArgs e)
        {
            this.Close();
            this.Dispose();
        }


    }
}
