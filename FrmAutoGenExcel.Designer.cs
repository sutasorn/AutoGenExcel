﻿namespace AutoGenExcel {
    partial class FrmAutoGenExcel {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if(disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.panel2 = new System.Windows.Forms.Panel();
            this.button2 = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.panel3 = new System.Windows.Forms.Panel();
            this.label4 = new System.Windows.Forms.Label();
            this.btnExit = new System.Windows.Forms.Button();
            this.btnGenExcel = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.cbTemplate = new System.Windows.Forms.ComboBox();
            this.cbGroupRecieve = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.cbEmployeename = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.cbFilter = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.cbhost = new System.Windows.Forms.ComboBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.cbsignature = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.panel3.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel2
            // 
            this.panel2.AutoScroll = true;
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Location = new System.Drawing.Point(12, 206);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(521, 177);
            this.panel2.TabIndex = 8;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(249, 7);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 11;
            this.button2.Text = "Browse";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click_1);
            // 
            // textBox1
            // 
            this.textBox1.BackColor = System.Drawing.SystemColors.HighlightText;
            this.textBox1.Location = new System.Drawing.Point(76, 9);
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(167, 20);
            this.textBox1.TabIndex = 12;
            // 
            // panel3
            // 
            this.panel3.AutoScroll = true;
            this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel3.Controls.Add(this.textBox1);
            this.panel3.Controls.Add(this.label4);
            this.panel3.Controls.Add(this.button2);
            this.panel3.Controls.Add(this.btnExit);
            this.panel3.Controls.Add(this.btnGenExcel);
            this.panel3.Location = new System.Drawing.Point(12, 398);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(521, 42);
            this.panel3.TabIndex = 13;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(5, 12);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(67, 13);
            this.label4.TabIndex = 12;
            this.label4.Text = "Output Path:";
            // 
            // btnExit
            // 
            this.btnExit.Location = new System.Drawing.Point(431, 7);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(75, 23);
            this.btnExit.TabIndex = 9;
            this.btnExit.Text = "E&xit";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // btnGenExcel
            // 
            this.btnGenExcel.Location = new System.Drawing.Point(350, 7);
            this.btnGenExcel.Name = "btnGenExcel";
            this.btnGenExcel.Size = new System.Drawing.Size(75, 23);
            this.btnGenExcel.TabIndex = 8;
            this.btnGenExcel.Text = "&Gen Excel";
            this.btnGenExcel.UseVisualStyleBackColor = true;
            this.btnGenExcel.Click += new System.EventHandler(this.btnGenExcel_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 11);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(83, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "รูปแบบเอกสาร :";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(23, 66);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(69, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "รายการผู้รับ :";
            // 
            // cbTemplate
            // 
            this.cbTemplate.FormattingEnabled = true;
            this.cbTemplate.Location = new System.Drawing.Point(94, 8);
            this.cbTemplate.Name = "cbTemplate";
            this.cbTemplate.Size = new System.Drawing.Size(412, 21);
            this.cbTemplate.TabIndex = 1;
            this.cbTemplate.SelectedIndexChanged += new System.EventHandler(this.cbTemplate_SelectedIndexChanged);
            // 
            // cbGroupRecieve
            // 
            this.cbGroupRecieve.FormattingEnabled = true;
            this.cbGroupRecieve.Location = new System.Drawing.Point(94, 63);
            this.cbGroupRecieve.Name = "cbGroupRecieve";
            this.cbGroupRecieve.Size = new System.Drawing.Size(412, 21);
            this.cbGroupRecieve.TabIndex = 1;
            this.cbGroupRecieve.SelectedIndexChanged += new System.EventHandler(this.cbGroupRecieve_SelectedIndexChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(7, 93);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(85, 13);
            this.label6.TabIndex = 7;
            this.label6.Text = "รายชื่อพนักงาน :";
            // 
            // cbEmployeename
            // 
            this.cbEmployeename.FormattingEnabled = true;
            this.cbEmployeename.Location = new System.Drawing.Point(94, 90);
            this.cbEmployeename.Name = "cbEmployeename";
            this.cbEmployeename.Size = new System.Drawing.Size(412, 21);
            this.cbEmployeename.TabIndex = 8;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(5, 39);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(87, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "คัดกรองรายการ :";
            // 
            // cbFilter
            // 
            this.cbFilter.FormattingEnabled = true;
            this.cbFilter.Location = new System.Drawing.Point(94, 36);
            this.cbFilter.Name = "cbFilter";
            this.cbFilter.Size = new System.Drawing.Size(412, 21);
            this.cbFilter.TabIndex = 3;
            this.cbFilter.SelectedIndexChanged += new System.EventHandler(this.cbFilter_SelectedIndexChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(41, 120);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(49, 13);
            this.label5.TabIndex = 5;
            this.label5.Text = "เจ้าภาพ :";
            // 
            // cbhost
            // 
            this.cbhost.FormattingEnabled = true;
            this.cbhost.Location = new System.Drawing.Point(94, 117);
            this.cbhost.Name = "cbhost";
            this.cbhost.Size = new System.Drawing.Size(412, 21);
            this.cbhost.TabIndex = 6;
            this.cbhost.SelectedIndexChanged += new System.EventHandler(this.cbhost_SelectedIndexChanged);
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.cbsignature);
            this.panel1.Controls.Add(this.label7);
            this.panel1.Controls.Add(this.cbhost);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.cbFilter);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.cbEmployeename);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.cbGroupRecieve);
            this.panel1.Controls.Add(this.cbTemplate);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Location = new System.Drawing.Point(12, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(521, 178);
            this.panel1.TabIndex = 5;
            // 
            // cbsignature
            // 
            this.cbsignature.FormattingEnabled = true;
            this.cbsignature.Location = new System.Drawing.Point(94, 144);
            this.cbsignature.Name = "cbsignature";
            this.cbsignature.Size = new System.Drawing.Size(412, 21);
            this.cbsignature.TabIndex = 10;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(39, 147);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(51, 13);
            this.label7.TabIndex = 9;
            this.label7.Text = "ลายเซ็น :";
            // 
            // FrmAutoGenExcel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(548, 480);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "FrmAutoGenExcel";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Auto Generate Excel";
            this.Load += new System.EventHandler(this.FrmAutoGenExcel_Load);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Button btnGenExcel;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cbTemplate;
        private System.Windows.Forms.ComboBox cbGroupRecieve;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox cbEmployeename;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cbFilter;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox cbhost;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ComboBox cbsignature;
        private System.Windows.Forms.Label label7;
    }
}