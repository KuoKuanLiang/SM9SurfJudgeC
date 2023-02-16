namespace SM9SurfJudge
{
    partial class Main
    {
        /// <summary>
        /// 設計工具所需的變數。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清除任何使用中的資源。
        /// </summary>
        /// <param name="disposing">如果應該處置受控資源則為 true，否則為 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 設計工具產生的程式碼

        /// <summary>
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器修改
        /// 這個方法的內容。
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.ParaSetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.产品量测ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.系统参数设定ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.grid_Show = new System.Windows.Forms.DataGridView();
            this.chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.ShowID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DetermineDatatime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FileName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Bmp = new System.Windows.Forms.DataGridViewImageColumn();
            this.MultipleRA = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MultipleRAresult = new System.Windows.Forms.DataGridViewImageColumn();
            this.MultipleRZ = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MultipleRZresult = new System.Windows.Forms.DataGridViewImageColumn();
            this.SingleRA = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SingleRAresult = new System.Windows.Forms.DataGridViewImageColumn();
            this.SingleRZ = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SingleRZresult = new System.Windows.Forms.DataGridViewImageColumn();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grid_Show)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).BeginInit();
            this.panel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ParaSetToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(5, 2, 0, 2);
            this.menuStrip1.Size = new System.Drawing.Size(1687, 33);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // ParaSetToolStripMenuItem
            // 
            this.ParaSetToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.产品量测ToolStripMenuItem,
            this.系统参数设定ToolStripMenuItem});
            this.ParaSetToolStripMenuItem.Name = "ParaSetToolStripMenuItem";
            this.ParaSetToolStripMenuItem.Size = new System.Drawing.Size(104, 29);
            this.ParaSetToolStripMenuItem.Text = "系统设定";
            // 
            // 产品量测ToolStripMenuItem
            // 
            this.产品量测ToolStripMenuItem.Image = global::SM9SurfJudge.Properties.Resources.PSET;
            this.产品量测ToolStripMenuItem.Name = "产品量测ToolStripMenuItem";
            this.产品量测ToolStripMenuItem.Size = new System.Drawing.Size(254, 30);
            this.产品量测ToolStripMenuItem.Text = "產品量測參數設定";
            this.产品量测ToolStripMenuItem.Click += new System.EventHandler(this.产品量测ToolStripMenuItem_Click);
            // 
            // 系统参数设定ToolStripMenuItem
            // 
            this.系统参数设定ToolStripMenuItem.Image = global::SM9SurfJudge.Properties.Resources.SET;
            this.系统参数设定ToolStripMenuItem.Name = "系统参数设定ToolStripMenuItem";
            this.系统参数设定ToolStripMenuItem.Size = new System.Drawing.Size(254, 30);
            this.系统参数设定ToolStripMenuItem.Text = "系統參數測定";
            this.系统参数设定ToolStripMenuItem.Click += new System.EventHandler(this.系统参数设定ToolStripMenuItem_Click);
            // 
            // grid_Show
            // 
            this.grid_Show.AllowUserToAddRows = false;
            this.grid_Show.AllowUserToDeleteRows = false;
            this.grid_Show.AllowUserToResizeColumns = false;
            this.grid_Show.AllowUserToResizeRows = false;
            this.grid_Show.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grid_Show.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.DisplayedCellsExceptHeaders;
            this.grid_Show.BackgroundColor = System.Drawing.SystemColors.Control;
            this.grid_Show.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleVertical;
            this.grid_Show.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("宋体", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            this.grid_Show.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.grid_Show.ColumnHeadersHeight = 50;
            this.grid_Show.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ShowID,
            this.DetermineDatatime,
            this.FileName,
            this.Bmp,
            this.MultipleRA,
            this.MultipleRAresult,
            this.MultipleRZ,
            this.MultipleRZresult,
            this.SingleRA,
            this.SingleRAresult,
            this.SingleRZ,
            this.SingleRZresult});
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle8.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle8.Font = new System.Drawing.Font("新細明體", 9F);
            dataGridViewCellStyle8.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle8.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle8.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle8.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.grid_Show.DefaultCellStyle = dataGridViewCellStyle8;
            this.grid_Show.Location = new System.Drawing.Point(12, 29);
            this.grid_Show.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.grid_Show.Name = "grid_Show";
            this.grid_Show.RowHeadersWidth = 20;
            dataGridViewCellStyle9.Font = new System.Drawing.Font("宋体", 12F);
            dataGridViewCellStyle9.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.grid_Show.RowsDefaultCellStyle = dataGridViewCellStyle9;
            this.grid_Show.RowTemplate.DefaultCellStyle.Font = new System.Drawing.Font("宋体", 12F);
            this.grid_Show.RowTemplate.Height = 192;
            this.grid_Show.RowTemplate.ReadOnly = true;
            this.grid_Show.Size = new System.Drawing.Size(1663, 503);
            this.grid_Show.TabIndex = 1;
            this.grid_Show.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.grid_Show_CellDoubleClick);
            // 
            // chart1
            // 
            chartArea1.AxisX.Enabled = System.Windows.Forms.DataVisualization.Charting.AxisEnabled.False;
            chartArea1.AxisX.MajorGrid.Enabled = false;
            chartArea1.AxisX.MajorTickMark.Enabled = false;
            chartArea1.AxisX.ScrollBar.Enabled = false;
            chartArea1.AxisX2.Enabled = System.Windows.Forms.DataVisualization.Charting.AxisEnabled.False;
            chartArea1.AxisX2.MajorGrid.Enabled = false;
            chartArea1.AxisX2.MajorTickMark.Enabled = false;
            chartArea1.AxisX2.ScrollBar.Enabled = false;
            chartArea1.AxisY.Enabled = System.Windows.Forms.DataVisualization.Charting.AxisEnabled.False;
            chartArea1.AxisY.MajorGrid.Enabled = false;
            chartArea1.AxisY.MajorTickMark.Enabled = false;
            chartArea1.AxisY.ScrollBar.Enabled = false;
            chartArea1.AxisY2.Enabled = System.Windows.Forms.DataVisualization.Charting.AxisEnabled.False;
            chartArea1.AxisY2.MajorGrid.Enabled = false;
            chartArea1.AxisY2.MajorTickMark.Enabled = false;
            chartArea1.AxisY2.ScrollBar.Enabled = false;
            chartArea1.Name = "ChartArea1";
            this.chart1.ChartAreas.Add(chartArea1);
            legend1.Enabled = false;
            legend1.Name = "Legend1";
            this.chart1.Legends.Add(legend1);
            this.chart1.Location = new System.Drawing.Point(382, 80);
            this.chart1.Name = "chart1";
            series1.ChartArea = "ChartArea1";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series1.Legend = "Legend1";
            series1.MarkerColor = System.Drawing.Color.DodgerBlue;
            series1.MarkerStyle = System.Windows.Forms.DataVisualization.Charting.MarkerStyle.Diamond;
            series1.Name = "Series1";
            series2.ChartArea = "ChartArea1";
            series2.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Point;
            series2.Enabled = false;
            series2.Legend = "Legend1";
            series2.MarkerColor = System.Drawing.Color.Red;
            series2.MarkerStyle = System.Windows.Forms.DataVisualization.Charting.MarkerStyle.Circle;
            series2.Name = "Series2";
            this.chart1.Series.Add(series1);
            this.chart1.Series.Add(series2);
            this.chart1.Size = new System.Drawing.Size(448, 76);
            this.chart1.TabIndex = 3;
            this.chart1.Text = "chart1";
            this.chart1.Visible = false;
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("新細明體", 12F);
            this.button1.Location = new System.Drawing.Point(12, 537);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(187, 76);
            this.button1.TabIndex = 4;
            this.button1.Text = "回針至起始位置";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Font = new System.Drawing.Font("新細明體", 12F);
            this.button2.Location = new System.Drawing.Point(205, 537);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(187, 76);
            this.button2.TabIndex = 5;
            this.button2.Text = "開始量測與判定";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.Font = new System.Drawing.Font("新細明體", 12F);
            this.button3.Location = new System.Drawing.Point(398, 537);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(187, 76);
            this.button3.TabIndex = 6;
            this.button3.Text = "中斷量測";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // textBox1
            // 
            this.textBox1.Font = new System.Drawing.Font("新細明體", 20F);
            this.textBox1.Location = new System.Drawing.Point(194, 12);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(658, 47);
            this.textBox1.TabIndex = 7;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("新細明體", 20F);
            this.label1.Location = new System.Drawing.Point(3, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(185, 34);
            this.label1.TabIndex = 9;
            this.label1.Text = "目前狀態：";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.textBox1);
            this.panel1.Location = new System.Drawing.Point(820, 537);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(855, 76);
            this.panel1.TabIndex = 10;
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.Checked = true;
            this.radioButton1.Font = new System.Drawing.Font("新細明體", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.radioButton1.Location = new System.Drawing.Point(6, 27);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(89, 32);
            this.radioButton1.TabIndex = 11;
            this.radioButton1.TabStop = true;
            this.radioButton1.Text = "啟用";
            this.radioButton1.UseVisualStyleBackColor = true;
            this.radioButton1.CheckedChanged += new System.EventHandler(this.radioButton1_CheckedChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.radioButton2);
            this.groupBox1.Controls.Add(this.radioButton1);
            this.groupBox1.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.groupBox1.Location = new System.Drawing.Point(591, 537);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(223, 76);
            this.groupBox1.TabIndex = 12;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "快捷鍵";
            // 
            // radioButton2
            // 
            this.radioButton2.AutoSize = true;
            this.radioButton2.Font = new System.Drawing.Font("新細明體", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.radioButton2.Location = new System.Drawing.Point(101, 27);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(117, 32);
            this.radioButton2.TabIndex = 12;
            this.radioButton2.Text = "未啟用";
            this.radioButton2.UseVisualStyleBackColor = true;
            this.radioButton2.CheckedChanged += new System.EventHandler(this.radioButton2_CheckedChanged);
            // 
            // ShowID
            // 
            this.ShowID.HeaderText = "id";
            this.ShowID.MinimumWidth = 8;
            this.ShowID.Name = "ShowID";
            this.ShowID.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.ShowID.Visible = false;
            this.ShowID.Width = 65;
            // 
            // DetermineDatatime
            // 
            dataGridViewCellStyle2.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.DetermineDatatime.DefaultCellStyle = dataGridViewCellStyle2;
            this.DetermineDatatime.DividerWidth = 1;
            this.DetermineDatatime.HeaderText = "判定時間";
            this.DetermineDatatime.MinimumWidth = 8;
            this.DetermineDatatime.Name = "DetermineDatatime";
            this.DetermineDatatime.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.DetermineDatatime.Width = 220;
            // 
            // FileName
            // 
            dataGridViewCellStyle3.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.FileName.DefaultCellStyle = dataGridViewCellStyle3;
            this.FileName.DividerWidth = 1;
            this.FileName.HeaderText = "流水碼";
            this.FileName.MinimumWidth = 8;
            this.FileName.Name = "FileName";
            this.FileName.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // Bmp
            // 
            this.Bmp.DividerWidth = 1;
            this.Bmp.HeaderText = "波形圖";
            this.Bmp.ImageLayout = System.Windows.Forms.DataGridViewImageCellLayout.Zoom;
            this.Bmp.MinimumWidth = 8;
            this.Bmp.Name = "Bmp";
            this.Bmp.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.Bmp.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.Bmp.Width = 450;
            // 
            // MultipleRA
            // 
            dataGridViewCellStyle4.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.MultipleRA.DefaultCellStyle = dataGridViewCellStyle4;
            this.MultipleRA.HeaderText = "多RA";
            this.MultipleRA.Name = "MultipleRA";
            this.MultipleRA.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // MultipleRAresult
            // 
            this.MultipleRAresult.HeaderText = "多RA結果";
            this.MultipleRAresult.ImageLayout = System.Windows.Forms.DataGridViewImageCellLayout.Zoom;
            this.MultipleRAresult.Name = "MultipleRAresult";
            this.MultipleRAresult.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.MultipleRAresult.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.MultipleRAresult.Width = 145;
            // 
            // MultipleRZ
            // 
            dataGridViewCellStyle5.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.MultipleRZ.DefaultCellStyle = dataGridViewCellStyle5;
            this.MultipleRZ.HeaderText = "多RZ";
            this.MultipleRZ.Name = "MultipleRZ";
            this.MultipleRZ.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // MultipleRZresult
            // 
            this.MultipleRZresult.HeaderText = "多RZ結果";
            this.MultipleRZresult.ImageLayout = System.Windows.Forms.DataGridViewImageCellLayout.Zoom;
            this.MultipleRZresult.Name = "MultipleRZresult";
            this.MultipleRZresult.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.MultipleRZresult.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.MultipleRZresult.Width = 145;
            // 
            // SingleRA
            // 
            dataGridViewCellStyle6.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.SingleRA.DefaultCellStyle = dataGridViewCellStyle6;
            this.SingleRA.HeaderText = "單RA";
            this.SingleRA.Name = "SingleRA";
            this.SingleRA.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // SingleRAresult
            // 
            this.SingleRAresult.HeaderText = "單RA結果";
            this.SingleRAresult.ImageLayout = System.Windows.Forms.DataGridViewImageCellLayout.Zoom;
            this.SingleRAresult.Name = "SingleRAresult";
            this.SingleRAresult.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.SingleRAresult.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.SingleRAresult.Width = 145;
            // 
            // SingleRZ
            // 
            dataGridViewCellStyle7.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.SingleRZ.DefaultCellStyle = dataGridViewCellStyle7;
            this.SingleRZ.HeaderText = "單RZ";
            this.SingleRZ.Name = "SingleRZ";
            this.SingleRZ.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // SingleRZresult
            // 
            this.SingleRZresult.HeaderText = "單RZ結果";
            this.SingleRZresult.ImageLayout = System.Windows.Forms.DataGridViewImageCellLayout.Zoom;
            this.SingleRZresult.Name = "SingleRZresult";
            this.SingleRZresult.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.SingleRZresult.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.SingleRZresult.Width = 145;
            // 
            // Main
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(1687, 618);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.chart1);
            this.Controls.Add(this.grid_Show);
            this.Controls.Add(this.menuStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.MaximizeBox = false;
            this.Name = "Main";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "SM9粗造度量测判定系统";
            this.Load += new System.EventHandler(this.Main_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Main_KeyDown);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grid_Show)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem ParaSetToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 产品量测ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 系统参数设定ToolStripMenuItem;
        private System.Windows.Forms.DataGridView grid_Show;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.RadioButton radioButton1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton radioButton2;
        private System.Windows.Forms.DataGridViewTextBoxColumn ShowID;
        private System.Windows.Forms.DataGridViewTextBoxColumn DetermineDatatime;
        private System.Windows.Forms.DataGridViewTextBoxColumn FileName;
        private System.Windows.Forms.DataGridViewImageColumn Bmp;
        private System.Windows.Forms.DataGridViewTextBoxColumn MultipleRA;
        private System.Windows.Forms.DataGridViewImageColumn MultipleRAresult;
        private System.Windows.Forms.DataGridViewTextBoxColumn MultipleRZ;
        private System.Windows.Forms.DataGridViewImageColumn MultipleRZresult;
        private System.Windows.Forms.DataGridViewTextBoxColumn SingleRA;
        private System.Windows.Forms.DataGridViewImageColumn SingleRAresult;
        private System.Windows.Forms.DataGridViewTextBoxColumn SingleRZ;
        private System.Windows.Forms.DataGridViewImageColumn SingleRZresult;
    }
}

