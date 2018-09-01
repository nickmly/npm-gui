namespace NPMGui
{
    partial class Form1
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
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.gitBashBtn = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.gitTextBox = new System.Windows.Forms.TextBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.openFileDialog2 = new System.Windows.Forms.OpenFileDialog();
            this.label2 = new System.Windows.Forms.Label();
            this.repoTextBox = new System.Windows.Forms.TextBox();
            this.versionTextBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // gitBashBtn
            // 
            this.gitBashBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.gitBashBtn.Location = new System.Drawing.Point(653, 30);
            this.gitBashBtn.Name = "gitBashBtn";
            this.gitBashBtn.Size = new System.Drawing.Size(135, 21);
            this.gitBashBtn.TabIndex = 4;
            this.gitBashBtn.Text = "Install Package";
            this.gitBashBtn.UseVisualStyleBackColor = true;
            this.gitBashBtn.Click += new System.EventHandler(this.gitBashBtn_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(89, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Package Installer";
            // 
            // gitTextBox
            // 
            this.gitTextBox.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.gitTextBox.Location = new System.Drawing.Point(13, 57);
            this.gitTextBox.Multiline = true;
            this.gitTextBox.Name = "gitTextBox";
            this.gitTextBox.ReadOnly = true;
            this.gitTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.gitTextBox.Size = new System.Drawing.Size(775, 200);
            this.gitTextBox.TabIndex = 5;
            // 
            // textBox1
            // 
            this.textBox1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.textBox1.Location = new System.Drawing.Point(13, 292);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox1.Size = new System.Drawing.Size(775, 117);
            this.textBox1.TabIndex = 7;
            // 
            // openFileDialog2
            // 
            this.openFileDialog2.FileName = "openFileDialog2";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 276);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Converter";
            // 
            // repoTextBox
            // 
            this.repoTextBox.Location = new System.Drawing.Point(13, 31);
            this.repoTextBox.Name = "repoTextBox";
            this.repoTextBox.Size = new System.Drawing.Size(588, 20);
            this.repoTextBox.TabIndex = 1;
            // 
            // versionTextBox
            // 
            this.versionTextBox.Location = new System.Drawing.Point(607, 31);
            this.versionTextBox.Name = "versionTextBox";
            this.versionTextBox.Size = new System.Drawing.Size(40, 20);
            this.versionTextBox.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(604, 11);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(42, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Version";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.versionTextBox);
            this.Controls.Add(this.repoTextBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.gitTextBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.gitBashBtn);
            this.Controls.Add(this.textBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "Form1";
            this.Text = "SSH NPM";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Button gitBashBtn;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox gitTextBox;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.OpenFileDialog openFileDialog2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox repoTextBox;
        private System.Windows.Forms.TextBox versionTextBox;
        private System.Windows.Forms.Label label3;
    }
}

