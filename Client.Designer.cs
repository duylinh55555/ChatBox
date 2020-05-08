namespace ChatBox_v3
{
    partial class Client
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Client));
            this.khungHienThi = new System.Windows.Forms.RichTextBox();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.khungUserName = new System.Windows.Forms.RichTextBox();
            this.khungNoiDung = new System.Windows.Forms.RichTextBox();
            this.sendButton = new System.Windows.Forms.Button();
            this.connectButton = new System.Windows.Forms.Button();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.insertButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripSplitButton1 = new System.Windows.Forms.ToolStripSplitButton();
            this.yToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.yToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.loadingBar = new System.Windows.Forms.ToolStripProgressBar();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // khungHienThi
            // 
            this.khungHienThi.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.khungHienThi.Location = new System.Drawing.Point(12, 93);
            this.khungHienThi.Name = "khungHienThi";
            this.khungHienThi.Size = new System.Drawing.Size(684, 529);
            this.khungHienThi.TabIndex = 2;
            this.khungHienThi.Text = "";
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // khungUserName
            // 
            this.khungUserName.Location = new System.Drawing.Point(12, 28);
            this.khungUserName.Name = "khungUserName";
            this.khungUserName.Size = new System.Drawing.Size(542, 45);
            this.khungUserName.TabIndex = 1;
            this.khungUserName.Text = "";
            // 
            // khungNoiDung
            // 
            this.khungNoiDung.Font = new System.Drawing.Font("Times New Roman", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.khungNoiDung.Location = new System.Drawing.Point(12, 671);
            this.khungNoiDung.Name = "khungNoiDung";
            this.khungNoiDung.Size = new System.Drawing.Size(744, 110);
            this.khungNoiDung.TabIndex = 2;
            this.khungNoiDung.Text = "";
            // 
            // sendButton
            // 
            this.sendButton.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("sendButton.BackgroundImage")));
            this.sendButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.sendButton.Location = new System.Drawing.Point(762, 703);
            this.sendButton.Name = "sendButton";
            this.sendButton.Size = new System.Drawing.Size(58, 60);
            this.sendButton.TabIndex = 3;
            this.sendButton.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            this.sendButton.UseVisualStyleBackColor = true;
            this.sendButton.Click += new System.EventHandler(this.sendButton_Click);
            // 
            // connectButton
            // 
            this.connectButton.Location = new System.Drawing.Point(571, 28);
            this.connectButton.Name = "connectButton";
            this.connectButton.Size = new System.Drawing.Size(93, 45);
            this.connectButton.TabIndex = 4;
            this.connectButton.UseVisualStyleBackColor = true;
            this.connectButton.Click += new System.EventHandler(this.connectButton_Click);
            // 
            // toolStrip1
            // 
            this.toolStrip1.AutoSize = false;
            this.toolStrip1.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.toolStrip1.Dock = System.Windows.Forms.DockStyle.None;
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.insertButton,
            this.toolStripSeparator1,
            this.toolStripSplitButton1,
            this.loadingBar});
            this.toolStrip1.Location = new System.Drawing.Point(12, 625);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(851, 43);
            this.toolStrip1.TabIndex = 5;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // insertButton
            // 
            this.insertButton.AutoSize = false;
            this.insertButton.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.insertButton.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("insertButton.BackgroundImage")));
            this.insertButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.insertButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.insertButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.insertButton.Name = "insertButton";
            this.insertButton.Size = new System.Drawing.Size(40, 40);
            this.insertButton.Text = "toolStripButton1";
            this.insertButton.Click += new System.EventHandler(this.insertButton_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 43);
            // 
            // toolStripSplitButton1
            // 
            this.toolStripSplitButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripSplitButton1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.yToolStripMenuItem,
            this.yToolStripMenuItem1});
            this.toolStripSplitButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripSplitButton1.Image")));
            this.toolStripSplitButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripSplitButton1.Name = "toolStripSplitButton1";
            this.toolStripSplitButton1.Size = new System.Drawing.Size(39, 40);
            this.toolStripSplitButton1.Text = "toolStripSplitButton1";
            // 
            // yToolStripMenuItem
            // 
            this.yToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("yToolStripMenuItem.Image")));
            this.yToolStripMenuItem.Name = "yToolStripMenuItem";
            this.yToolStripMenuItem.Size = new System.Drawing.Size(224, 26);
            this.yToolStripMenuItem.Text = "y";
            // 
            // yToolStripMenuItem1
            // 
            this.yToolStripMenuItem1.Image = ((System.Drawing.Image)(resources.GetObject("yToolStripMenuItem1.Image")));
            this.yToolStripMenuItem1.Name = "yToolStripMenuItem1";
            this.yToolStripMenuItem1.Size = new System.Drawing.Size(224, 26);
            this.yToolStripMenuItem1.Text = "y";
            // 
            // loadingBar
            // 
            this.loadingBar.AutoSize = false;
            this.loadingBar.Name = "loadingBar";
            this.loadingBar.Size = new System.Drawing.Size(150, 25);
            this.loadingBar.Step = 5;
            this.loadingBar.ToolTipText = "ỷgt";
            this.loadingBar.Visible = false;
            // 
            // Client
            // 
            this.AcceptButton = this.sendButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(875, 793);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.connectButton);
            this.Controls.Add(this.sendButton);
            this.Controls.Add(this.khungNoiDung);
            this.Controls.Add(this.khungUserName);
            this.Controls.Add(this.khungHienThi);
            this.Name = "Client";
            this.Text = "Form1";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox khungHienThi;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.RichTextBox khungUserName;
        private System.Windows.Forms.RichTextBox khungNoiDung;
        private System.Windows.Forms.Button sendButton;
        private System.Windows.Forms.Button connectButton;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton insertButton;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripSplitButton toolStripSplitButton1;
        private System.Windows.Forms.ToolStripMenuItem yToolStripMenuItem;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.ToolStripMenuItem yToolStripMenuItem1;
        private System.Windows.Forms.ToolStripProgressBar loadingBar;
    }
}

