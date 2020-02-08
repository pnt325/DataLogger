namespace new_chart_delections.gui
{
    partial class FrmSelectItem
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
            this.label1 = new System.Windows.Forms.Label();
            this.cbbItem = new System.Windows.Forms.ComboBox();
            this.cbbChartItem = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txbPeriod = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.grbDatas = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txbTitle = new System.Windows.Forms.TextBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOk = new System.Windows.Forms.Button();
            this.lsvMain = new new_chart_delections.gui.ListViewNF();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.grbDatas.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(27, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Item";
            // 
            // cbbItem
            // 
            this.cbbItem.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbbItem.FormattingEnabled = true;
            this.cbbItem.Location = new System.Drawing.Point(55, 6);
            this.cbbItem.Name = "cbbItem";
            this.cbbItem.Size = new System.Drawing.Size(98, 21);
            this.cbbItem.TabIndex = 1;
            // 
            // cbbChartItem
            // 
            this.cbbChartItem.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbbChartItem.FormattingEnabled = true;
            this.cbbChartItem.Location = new System.Drawing.Point(159, 6);
            this.cbbChartItem.Name = "cbbChartItem";
            this.cbbChartItem.Size = new System.Drawing.Size(98, 21);
            this.cbbChartItem.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 36);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(37, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Period";
            // 
            // txbPeriod
            // 
            this.txbPeriod.Location = new System.Drawing.Point(55, 33);
            this.txbPeriod.Name = "txbPeriod";
            this.txbPeriod.Size = new System.Drawing.Size(100, 20);
            this.txbPeriod.TabIndex = 2;
            this.txbPeriod.Text = "10";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(161, 36);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(20, 13);
            this.label3.TabIndex = 0;
            this.label3.Text = "Hz";
            // 
            // grbDatas
            // 
            this.grbDatas.Controls.Add(this.lsvMain);
            this.grbDatas.Location = new System.Drawing.Point(15, 85);
            this.grbDatas.Name = "grbDatas";
            this.grbDatas.Size = new System.Drawing.Size(242, 221);
            this.grbDatas.TabIndex = 3;
            this.grbDatas.TabStop = false;
            this.grbDatas.Text = "Datas";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 62);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(27, 13);
            this.label4.TabIndex = 0;
            this.label4.Text = "Title";
            // 
            // txbTitle
            // 
            this.txbTitle.Location = new System.Drawing.Point(55, 59);
            this.txbTitle.Name = "txbTitle";
            this.txbTitle.Size = new System.Drawing.Size(202, 20);
            this.txbTitle.TabIndex = 2;
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(179, 309);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 4;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(98, 309);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 4;
            this.btnOk.Text = "Ok";
            this.btnOk.UseVisualStyleBackColor = true;
            // 
            // lsvMain
            // 
            this.lsvMain.CheckBoxes = true;
            this.lsvMain.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2});
            this.lsvMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lsvMain.GridLines = true;
            this.lsvMain.Location = new System.Drawing.Point(3, 16);
            this.lsvMain.Name = "lsvMain";
            this.lsvMain.Size = new System.Drawing.Size(236, 202);
            this.lsvMain.TabIndex = 0;
            this.lsvMain.UseCompatibleStateImageBehavior = false;
            this.lsvMain.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Name";
            this.columnHeader1.Width = 140;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Unit";
            // 
            // FrmSelectItem
            // 
            this.AcceptButton = this.btnOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(272, 344);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.grbDatas);
            this.Controls.Add(this.txbTitle);
            this.Controls.Add(this.txbPeriod);
            this.Controls.Add(this.cbbChartItem);
            this.Controls.Add(this.cbbItem);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "FrmSelectItem";
            this.Text = "Select Data";
            this.grbDatas.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cbbItem;
        private System.Windows.Forms.ComboBox cbbChartItem;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txbPeriod;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox grbDatas;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txbTitle;
        private ListViewNF lsvMain;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOk;
    }
}