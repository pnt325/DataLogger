namespace DataLogger.Layout
{
    partial class FrmSelectComponent
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmSelectComponent));
            this.label1 = new System.Windows.Forms.Label();
            this.txbTitle = new System.Windows.Forms.TextBox();
            this.cbbComponent = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.nudPeriod = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.nudSample = new System.Windows.Forms.NumericUpDown();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.btnAddSingle = new System.Windows.Forms.Button();
            this.btnRemoveSingle = new System.Windows.Forms.Button();
            this.btnChange = new System.Windows.Forms.Button();
            this.txbName = new System.Windows.Forms.TextBox();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.colorHexagon1 = new DataLogger.Layout.ColorHexagon();
            this.lsvSelect = new DataLogger.Layout.ListViewNF();
            this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader6 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader7 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader8 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.lsvData = new DataLogger.Layout.ListViewNF();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            ((System.ComponentModel.ISupportInitialize)(this.nudPeriod)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudSample)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(17, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(27, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Title";
            // 
            // txbTitle
            // 
            this.txbTitle.Location = new System.Drawing.Point(50, 19);
            this.txbTitle.Name = "txbTitle";
            this.txbTitle.Size = new System.Drawing.Size(147, 20);
            this.txbTitle.TabIndex = 1;
            // 
            // cbbComponent
            // 
            this.cbbComponent.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbbComponent.FormattingEnabled = true;
            this.cbbComponent.Location = new System.Drawing.Point(49, 45);
            this.cbbComponent.Name = "cbbComponent";
            this.cbbComponent.Size = new System.Drawing.Size(148, 21);
            this.cbbComponent.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 48);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(31, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Type";
            // 
            // nudPeriod
            // 
            this.nudPeriod.Location = new System.Drawing.Point(49, 72);
            this.nudPeriod.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.nudPeriod.Name = "nudPeriod";
            this.nudPeriod.Size = new System.Drawing.Size(48, 20);
            this.nudPeriod.TabIndex = 4;
            this.nudPeriod.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 77);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(37, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Period";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(103, 74);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(42, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "Sample";
            // 
            // nudSample
            // 
            this.nudSample.Location = new System.Drawing.Point(151, 72);
            this.nudSample.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.nudSample.Name = "nudSample";
            this.nudSample.Size = new System.Drawing.Size(46, 20);
            this.nudSample.TabIndex = 7;
            this.nudSample.Value = new decimal(new int[] {
            500,
            0,
            0,
            0});
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lsvData);
            this.groupBox1.Location = new System.Drawing.Point(12, 124);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(207, 212);
            this.groupBox1.TabIndex = 8;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Data";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.cbbComponent);
            this.groupBox2.Controls.Add(this.nudSample);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.nudPeriod);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.txbTitle);
            this.groupBox2.Location = new System.Drawing.Point(12, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(207, 106);
            this.groupBox2.TabIndex = 8;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Info";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.lsvSelect);
            this.groupBox3.Location = new System.Drawing.Point(257, 12);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(289, 324);
            this.groupBox3.TabIndex = 9;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Selected";
            // 
            // btnAddSingle
            // 
            this.btnAddSingle.Location = new System.Drawing.Point(222, 209);
            this.btnAddSingle.Name = "btnAddSingle";
            this.btnAddSingle.Size = new System.Drawing.Size(29, 23);
            this.btnAddSingle.TabIndex = 10;
            this.btnAddSingle.Text = ">";
            this.btnAddSingle.UseVisualStyleBackColor = true;
            this.btnAddSingle.Click += new System.EventHandler(this.btnAddSingle_Click);
            // 
            // btnRemoveSingle
            // 
            this.btnRemoveSingle.Location = new System.Drawing.Point(222, 238);
            this.btnRemoveSingle.Name = "btnRemoveSingle";
            this.btnRemoveSingle.Size = new System.Drawing.Size(29, 23);
            this.btnRemoveSingle.TabIndex = 10;
            this.btnRemoveSingle.Text = "<";
            this.btnRemoveSingle.UseVisualStyleBackColor = true;
            this.btnRemoveSingle.Click += new System.EventHandler(this.btnRemoveSingle_Click);
            // 
            // btnChange
            // 
            this.btnChange.Location = new System.Drawing.Point(231, 17);
            this.btnChange.Name = "btnChange";
            this.btnChange.Size = new System.Drawing.Size(63, 23);
            this.btnChange.TabIndex = 7;
            this.btnChange.Text = "Change";
            this.btnChange.UseVisualStyleBackColor = true;
            // 
            // txbName
            // 
            this.txbName.Location = new System.Drawing.Point(6, 19);
            this.txbName.Name = "txbName";
            this.txbName.Size = new System.Drawing.Size(219, 20);
            this.txbName.TabIndex = 1;
            // 
            // btnOk
            // 
            this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOk.Location = new System.Drawing.Point(785, 347);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 11;
            this.btnOk.Text = "&Ok";
            this.btnOk.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.Location = new System.Drawing.Point(704, 347);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 11;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.colorHexagon1);
            this.groupBox5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox5.Location = new System.Drawing.Point(3, 58);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(300, 263);
            this.groupBox5.TabIndex = 1;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Color";
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.btnChange);
            this.groupBox6.Controls.Add(this.txbName);
            this.groupBox6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox6.Location = new System.Drawing.Point(3, 3);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(300, 49);
            this.groupBox6.TabIndex = 2;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Name";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.groupBox6, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.groupBox5, 0, 1);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(552, 12);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 269F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(306, 324);
            this.tableLayoutPanel1.TabIndex = 13;
            // 
            // colorHexagon1
            // 
            this.colorHexagon1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.colorHexagon1.Location = new System.Drawing.Point(3, 16);
            this.colorHexagon1.Name = "colorHexagon1";
            this.colorHexagon1.Size = new System.Drawing.Size(294, 244);
            this.colorHexagon1.TabIndex = 0;
            this.colorHexagon1.ColorChanged += new DataLogger.Layout.ColorHexagon.ColorChangedEventHandler(this.colorHexagon1_ColorChanged);
            // 
            // lsvSelect
            // 
            this.lsvSelect.CheckBoxes = true;
            this.lsvSelect.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader5,
            this.columnHeader6,
            this.columnHeader7,
            this.columnHeader8});
            this.lsvSelect.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lsvSelect.GridLines = true;
            this.lsvSelect.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lsvSelect.HideSelection = false;
            this.lsvSelect.Location = new System.Drawing.Point(3, 16);
            this.lsvSelect.Name = "lsvSelect";
            this.lsvSelect.Size = new System.Drawing.Size(283, 305);
            this.lsvSelect.TabIndex = 0;
            this.lsvSelect.UseCompatibleStateImageBehavior = false;
            this.lsvSelect.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader5
            // 
            this.columnHeader5.Text = "Name";
            this.columnHeader5.Width = 98;
            // 
            // columnHeader6
            // 
            this.columnHeader6.Text = "VarName";
            this.columnHeader6.Width = 75;
            // 
            // columnHeader7
            // 
            this.columnHeader7.Text = "Type";
            this.columnHeader7.Width = 46;
            // 
            // columnHeader8
            // 
            this.columnHeader8.Text = "Color";
            this.columnHeader8.Width = 47;
            // 
            // lsvData
            // 
            this.lsvData.CheckBoxes = true;
            this.lsvData.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2});
            this.lsvData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lsvData.GridLines = true;
            this.lsvData.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lsvData.HideSelection = false;
            this.lsvData.Location = new System.Drawing.Point(3, 16);
            this.lsvData.Name = "lsvData";
            this.lsvData.Size = new System.Drawing.Size(201, 193);
            this.lsvData.TabIndex = 0;
            this.lsvData.UseCompatibleStateImageBehavior = false;
            this.lsvData.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "VarName";
            this.columnHeader1.Width = 130;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Type";
            this.columnHeader2.Width = 54;
            // 
            // FrmSelectComponent
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(870, 382);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.btnRemoveSingle);
            this.Controls.Add(this.btnAddSingle);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmSelectComponent";
            this.Text = "Component Selection";
            ((System.ComponentModel.ISupportInitialize)(this.nudPeriod)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudSample)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox5.ResumeLayout(false);
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txbTitle;
        private System.Windows.Forms.ComboBox cbbComponent;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown nudPeriod;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown nudSample;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private ListViewNF lsvData;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.GroupBox groupBox3;
        private ListViewNF lsvSelect;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.ColumnHeader columnHeader6;
        private System.Windows.Forms.ColumnHeader columnHeader7;
        private System.Windows.Forms.ColumnHeader columnHeader8;
        private System.Windows.Forms.Button btnAddSingle;
        private System.Windows.Forms.Button btnRemoveSingle;
        private System.Windows.Forms.TextBox txbName;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnChange;
        private ColorHexagon colorHexagon1;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
    }
}