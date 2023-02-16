using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SM9SurfJudge
{
    public partial class ParaSet : Form
    {
        public string[] parastr;

        public ParaSet()
        {
            InitializeComponent();
        }

        private void ParaSet_Load(object sender, EventArgs e)
        {
            // 粗糙度儀編碼
            this.txtContourMachineNo.Text = parastr[0];
            // ParaFile-P.XML
            this.txtXmlPath.Text = parastr[1];
            // 資料庫url
            this.txtDBUrl.Text = parastr[2];
            // 測針未觸碰物體高度值
            this.textBox1.Text = parastr[3];
            // 資料庫port
            this.txtDBport.Text = parastr[4];
            // 紋路裁取寬度
            this.txtCutOff.Text = parastr[5];
        }

        /// <summary>
        /// 確定按鈕
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnsave_Click(object sender, EventArgs e)
        {
            string[] str = new string[6];

            str[0] = this.txtContourMachineNo.Text.Trim();
            str[1] = this.txtXmlPath.Text.Trim();
            str[2] = this.txtDBUrl.Text.Trim();
            str[3] = this.textBox1.Text.Trim();
            str[4] = this.txtDBport.Text.Trim();
            str[5] = this.txtCutOff.Text.Trim();

            for (int i = 0; i < str.Length; i++)
            {
                if (str[i].Trim() == "")
                {
                    MessageBox.Show("请确认参数不要为空");
                    return;
                }
            }
            //check mac
            if (Comm.CheckMachinenoAndMac(str[0]) < 1)
            {
                MessageBox.Show("『该粗糙度儀編號对应MAC地址不存在資料庫中, 需由資訊部門協助确认, 才能正常使用』", "保存失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            #region#本地参数修改类
            Comm.ModifyXml(Application.StartupPath + "\\ParaFile-S.XML", "Para", str);

            Main.para_s = str;
            MessageBox.Show("参数修改成功！", "操作提示", MessageBoxButtons.OK,MessageBoxIcon.Information);

            //Quit
            this.Close();

            #endregion
        }

        /// <summary>
        /// 取消按鈕
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btncancel_Click(object sender, EventArgs e)
        {
            this.Close();
            this.Dispose();
        }

        private void btnXmlPath_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.InitialDirectory = "c:\\";//默认路径
            openFile.Filter = "XML文件|*.xml|所有文件|*.*";
            openFile.RestoreDirectory = true;
            DialogResult dr = openFile.ShowDialog();
            if (dr == DialogResult.OK)
            {
                this.txtXmlPath.Text = openFile.FileName;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //OK-17268.9
            string returnRDPSA = Comm.InputStr("RDPSA");
            if (returnRDPSA.Contains("OK") == true)
            {
                // 取OK後的數字
                string num = returnRDPSA.Substring(2, returnRDPSA.Length - 2);//returnRDPSA.Length - 2 = 8
                // 移除小數點(包含)後的字元
                int p = num.IndexOf(".");//6
                if (p != -1)
                {
                    num = num.Remove(p, num.Length - p);
                }
                //-17268
                textBox1.Text = num;
            }
        }


        //有默认值暂时不需要了
        //Boolean textboxHasText = false;//判断输入框是否有文本
        //private void txtDBUrl_Enter(object sender, EventArgs e)
        //{
        //    if (textboxHasText == false)
        //        txtDBUrl.Text = "";

        //    txtDBUrl.ForeColor = Color.Black;

        //}

        //private void txtDBUrl_Leave(object sender, EventArgs e)
        //{
        //    if (txtDBUrl.Text == "")
        //    {
        //        txtDBUrl.Text = "例：192.168.2.99";
        //        txtDBUrl.ForeColor = Color.LightGray;
        //        textboxHasText = false;
        //    }
        //    else
        //        textboxHasText = true;
        //}
    }
}
