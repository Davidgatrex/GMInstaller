namespace GMInstaller
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            label2 = new Label();
            pictureBox1 = new PictureBox();
            ProgFilesTB = new TextBox();
            label1 = new Label();
            ProgFilesButton = new Button();
            folderBrowserDialog1 = new FolderBrowserDialog();
            ProgramsListBox = new ListBox();
            InstallButton = new Button();
            UpdateButton = new Button();
            UninstallButton = new Button();
            button1 = new Button();
            ReloadListBtn = new Button();
            MamboSavePath = new Button();
            MamboFilesBTN = new Button();
            label4 = new Label();
            MamboFilesTB = new TextBox();
            panel2 = new Panel();
            PStat_Lab = new Label();
            PVer_Lab = new Label();
            PName_Lab = new Label();
            label7 = new Label();
            label6 = new Label();
            label5 = new Label();
            folderBrowserDialog2 = new FolderBrowserDialog();
            InstalledProgsLB = new ListBox();
            label3 = new Label();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            panel2.SuspendLayout();
            SuspendLayout();
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Arial", 12F, FontStyle.Bold);
            label2.Location = new Point(266, 9);
            label2.Name = "label2";
            label2.Size = new Size(205, 19);
            label2.TabIndex = 1;
            label2.Text = "Generally Mambo Installer";
            // 
            // pictureBox1
            // 
            pictureBox1.BackgroundImage = Properties.Resources.GMInstaller;
            pictureBox1.BackgroundImageLayout = ImageLayout.Zoom;
            pictureBox1.Location = new Point(12, 12);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(248, 253);
            pictureBox1.TabIndex = 2;
            pictureBox1.TabStop = false;
            // 
            // ProgFilesTB
            // 
            ProgFilesTB.Location = new Point(12, 287);
            ProgFilesTB.Name = "ProgFilesTB";
            ProgFilesTB.Size = new Size(168, 23);
            ProgFilesTB.TabIndex = 3;
            ProgFilesTB.TextChanged += ProgFilesTB_TextChanged;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(12, 269);
            label1.Name = "label1";
            label1.Size = new Size(106, 15);
            label1.TabIndex = 4;
            label1.Text = "Program Files Path";
            // 
            // ProgFilesButton
            // 
            ProgFilesButton.Location = new Point(186, 271);
            ProgFilesButton.Name = "ProgFilesButton";
            ProgFilesButton.Size = new Size(25, 38);
            ProgFilesButton.TabIndex = 5;
            ProgFilesButton.Text = "...";
            ProgFilesButton.UseVisualStyleBackColor = true;
            ProgFilesButton.Click += ProgFilesButton_Click;
            // 
            // ProgramsListBox
            // 
            ProgramsListBox.FormattingEnabled = true;
            ProgramsListBox.Location = new Point(12, 316);
            ProgramsListBox.Name = "ProgramsListBox";
            ProgramsListBox.Size = new Size(248, 94);
            ProgramsListBox.TabIndex = 6;
            ProgramsListBox.SelectedIndexChanged += ProgramsListBox_SelectedIndexChanged;
            // 
            // InstallButton
            // 
            InstallButton.BackColor = Color.Green;
            InstallButton.FlatStyle = FlatStyle.Flat;
            InstallButton.Location = new Point(12, 416);
            InstallButton.Name = "InstallButton";
            InstallButton.Size = new Size(74, 30);
            InstallButton.TabIndex = 7;
            InstallButton.Text = "Install";
            InstallButton.UseVisualStyleBackColor = false;
            InstallButton.Click += InstallButton_Click;
            // 
            // UpdateButton
            // 
            UpdateButton.BackColor = Color.FromArgb(255, 128, 0);
            UpdateButton.FlatStyle = FlatStyle.Flat;
            UpdateButton.Location = new Point(92, 416);
            UpdateButton.Name = "UpdateButton";
            UpdateButton.Size = new Size(88, 30);
            UpdateButton.TabIndex = 8;
            UpdateButton.Text = "Update";
            UpdateButton.UseVisualStyleBackColor = false;
            UpdateButton.Click += UpdateButton_Click;
            // 
            // UninstallButton
            // 
            UninstallButton.BackColor = Color.Red;
            UninstallButton.FlatStyle = FlatStyle.Flat;
            UninstallButton.Location = new Point(186, 416);
            UninstallButton.Name = "UninstallButton";
            UninstallButton.Size = new Size(74, 30);
            UninstallButton.TabIndex = 9;
            UninstallButton.Text = "Uninstall";
            UninstallButton.UseVisualStyleBackColor = false;
            UninstallButton.Click += UninstallButton_Click;
            // 
            // button1
            // 
            button1.Location = new Point(217, 271);
            button1.Name = "button1";
            button1.Size = new Size(43, 39);
            button1.TabIndex = 12;
            button1.Text = "Save Path";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // ReloadListBtn
            // 
            ReloadListBtn.Location = new Point(266, 316);
            ReloadListBtn.Name = "ReloadListBtn";
            ReloadListBtn.Size = new Size(53, 38);
            ReloadListBtn.TabIndex = 13;
            ReloadListBtn.Text = "Reload List";
            ReloadListBtn.UseVisualStyleBackColor = true;
            ReloadListBtn.Click += ReloadListBtn_Click;
            // 
            // MamboSavePath
            // 
            MamboSavePath.Location = new Point(483, 270);
            MamboSavePath.Name = "MamboSavePath";
            MamboSavePath.Size = new Size(43, 39);
            MamboSavePath.TabIndex = 17;
            MamboSavePath.Text = "Save Path";
            MamboSavePath.UseVisualStyleBackColor = true;
            MamboSavePath.Click += MamboSavePath_Click;
            // 
            // MamboFilesBTN
            // 
            MamboFilesBTN.Location = new Point(452, 270);
            MamboFilesBTN.Name = "MamboFilesBTN";
            MamboFilesBTN.Size = new Size(25, 38);
            MamboFilesBTN.TabIndex = 16;
            MamboFilesBTN.Text = "...";
            MamboFilesBTN.UseVisualStyleBackColor = true;
            MamboFilesBTN.Click += MamboFilesBTN_Click;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(278, 268);
            label4.Name = "label4";
            label4.Size = new Size(150, 15);
            label4.TabIndex = 15;
            label4.Text = "Mambo files directory path";
            // 
            // MamboFilesTB
            // 
            MamboFilesTB.Location = new Point(278, 286);
            MamboFilesTB.Name = "MamboFilesTB";
            MamboFilesTB.Size = new Size(168, 23);
            MamboFilesTB.TabIndex = 14;
            MamboFilesTB.TextChanged += MamboFilesTB_TextChanged;
            // 
            // panel2
            // 
            panel2.BackColor = Color.Silver;
            panel2.Controls.Add(PStat_Lab);
            panel2.Controls.Add(PVer_Lab);
            panel2.Controls.Add(PName_Lab);
            panel2.Controls.Add(label7);
            panel2.Controls.Add(label6);
            panel2.Controls.Add(label5);
            panel2.Location = new Point(266, 153);
            panel2.Name = "panel2";
            panel2.Size = new Size(304, 111);
            panel2.TabIndex = 18;
            // 
            // PStat_Lab
            // 
            PStat_Lab.AutoSize = true;
            PStat_Lab.Font = new Font("Segoe UI", 9F);
            PStat_Lab.Location = new Point(76, 80);
            PStat_Lab.Name = "PStat_Lab";
            PStat_Lab.Size = new Size(88, 15);
            PStat_Lab.TabIndex = 5;
            PStat_Lab.Text = "Program Status";
            // 
            // PVer_Lab
            // 
            PVer_Lab.AutoSize = true;
            PVer_Lab.Font = new Font("Segoe UI", 9F);
            PVer_Lab.Location = new Point(76, 46);
            PVer_Lab.Name = "PVer_Lab";
            PVer_Lab.Size = new Size(94, 15);
            PVer_Lab.TabIndex = 4;
            PVer_Lab.Text = "Program Version";
            // 
            // PName_Lab
            // 
            PName_Lab.AutoSize = true;
            PName_Lab.Font = new Font("Segoe UI", 9F);
            PName_Lab.Location = new Point(76, 9);
            PName_Lab.Name = "PName_Lab";
            PName_Lab.Size = new Size(88, 15);
            PName_Lab.TabIndex = 3;
            PName_Lab.Text = "Program Name";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            label7.Location = new Point(12, 80);
            label7.Name = "label7";
            label7.Size = new Size(45, 15);
            label7.TabIndex = 2;
            label7.Text = "Status:";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            label6.Location = new Point(12, 46);
            label6.Name = "label6";
            label6.Size = new Size(51, 15);
            label6.TabIndex = 1;
            label6.Text = "Version:";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            label5.Location = new Point(12, 9);
            label5.Name = "label5";
            label5.Size = new Size(58, 15);
            label5.TabIndex = 0;
            label5.Text = "Program:";
            // 
            // InstalledProgsLB
            // 
            InstalledProgsLB.FormattingEnabled = true;
            InstalledProgsLB.Location = new Point(266, 53);
            InstalledProgsLB.Name = "InstalledProgsLB";
            InstalledProgsLB.Size = new Size(302, 94);
            InstalledProgsLB.TabIndex = 19;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(266, 35);
            label3.Name = "label3";
            label3.Size = new Size(105, 15);
            label3.TabIndex = 20;
            label3.Text = "Installed Programs";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(255, 224, 192);
            ClientSize = new Size(580, 458);
            Controls.Add(label3);
            Controls.Add(InstalledProgsLB);
            Controls.Add(panel2);
            Controls.Add(MamboSavePath);
            Controls.Add(MamboFilesBTN);
            Controls.Add(label4);
            Controls.Add(MamboFilesTB);
            Controls.Add(ReloadListBtn);
            Controls.Add(button1);
            Controls.Add(UninstallButton);
            Controls.Add(UpdateButton);
            Controls.Add(InstallButton);
            Controls.Add(ProgramsListBox);
            Controls.Add(ProgFilesButton);
            Controls.Add(label1);
            Controls.Add(ProgFilesTB);
            Controls.Add(pictureBox1);
            Controls.Add(label2);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "Form1";
            Text = "GMInstaller";
            Load += Form1_Load;
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label2;
        private PictureBox pictureBox1;
        private TextBox ProgFilesTB;
        private Label label1;
        private Button ProgFilesButton;
        private FolderBrowserDialog folderBrowserDialog1;
        private ListBox ProgramsListBox;
        private Button InstallButton;
        private Button UpdateButton;
        private Button UninstallButton;
        private Button button1;
        private Button ReloadListBtn;
        private Button MamboSavePath;
        private Button MamboFilesBTN;
        private Label label4;
        private TextBox MamboFilesTB;
        private Panel panel2;
        private FolderBrowserDialog folderBrowserDialog2;
        private Label label5;
        private Label PStat_Lab;
        private Label PVer_Lab;
        private Label PName_Lab;
        private Label label7;
        private Label label6;
        private ListBox InstalledProgsLB;
        private Label label3;
    }
}
