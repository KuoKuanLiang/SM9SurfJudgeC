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
using System.Windows.Forms.DataVisualization.Charting;

namespace SM9SurfJudge
{
    public partial class Shower : Form
    {
        public int pt_result_id ;

        public Shower()
        {
            InitializeComponent();
        }

        private void Shower_Load(object sender, EventArgs e)
        {
            PointEntry();
            chart1.MouseClick += new System.Windows.Forms.MouseEventHandler(chart1_MouseClick);
        }

        /// <summary>
        /// 查找資料庫中的對應資料的所有點資料 X(in)、Z(uin)
        /// </summary>
        public void PointEntry()
        {
            SqlTool sqltool = new SqlTool();
            string select_pt_result = "select curve_file from pt_result where pt_result_id = " + pt_result_id;
            DataTable pt_result = sqltool.ExecuteQuery(select_pt_result);
            DataRow pt = pt_result.Rows[0];
            //透過']'切分出所有的點
            string[] points = pt["curve_file"].ToString().Split(']');

            Series series = chart1.Series[0];
            series.Points.Clear();
            foreach (string p in points)
            {
                //移除掉 [
                string XZ = p.Replace("[", "");
                //透過中間的','區分出x和z值
                string[] str = XZ.Split(',');

                if (str.Length == 2)
                {
                    // 取得的是x(in)z(uin)，要嘛換成x(uin)z(uin)，或是換成x(in)z(in)，目前先使用x(in)z(in)
                    series.Points.AddXY(Convert.ToDecimal(str[0])*1000000, Convert.ToDecimal(str[1]));
                }
            }

            //_______________________________________________________________________________________________________________________

            //最好是標出裁切什麼位置，所以還是要有起終點
            sqltool = new SqlTool();
            string select_single_curve_data = "select * from single_curve_data where pt_result_id = " + pt_result_id;
            DataTable single_curve_data = sqltool.ExecuteQuery(select_single_curve_data);

            Series series2 = chart1.Series[1];
            series2.Points.Clear();
            foreach (DataRow sc in single_curve_data.Rows)
            {
                //x(in)z(in)
                series2.Points.AddXY(Convert.ToDecimal(sc["l_crop_x"]) * 1000000, Convert.ToDecimal(sc["l_crop_z"]));
                series2.Points.AddXY(Convert.ToDecimal(sc["r_crop_x"]) * 1000000, Convert.ToDecimal(sc["r_crop_z"]));
            }

            //_______________________________________________________________________________________________________________________

            //可能要再加上最左及最右點

        }

        /// <summary>
        /// 按下滑鼠右鍵就回復原本大小
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chart1_MouseClick(object sender, MouseEventArgs e)
        {
            Chart chart1 = sender as Chart;

            if (e.Button == MouseButtons.Right)
            {
                chart1.ChartAreas[0].AxisX.ScaleView.ZoomReset(1);
                chart1.ChartAreas[0].AxisY.ScaleView.ZoomReset(1);
            }
        }
    }
}
