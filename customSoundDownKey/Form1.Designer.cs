namespace customSoundDownKey
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
            quit_Button = new Button();
            VolumeBar = new TrackBar();
            SoundList_Box = new ListBox();
            volume_label = new Label();
            reload_Button = new Button();
            button3 = new Button();
            ((System.ComponentModel.ISupportInitialize)VolumeBar).BeginInit();
            SuspendLayout();
            // 
            // quit_Button
            // 
            quit_Button.Font = new Font("맑은 고딕", 8F);
            quit_Button.Location = new Point(12, 172);
            quit_Button.Name = "quit_Button";
            quit_Button.Size = new Size(88, 26);
            quit_Button.TabIndex = 0;
            quit_Button.Text = "종료";
            quit_Button.UseVisualStyleBackColor = true;
            quit_Button.Click += button1_Click;
            // 
            // VolumeBar
            // 
            VolumeBar.LargeChange = 1;
            VolumeBar.Location = new Point(12, 30);
            VolumeBar.Maximum = 25;
            VolumeBar.Name = "VolumeBar";
            VolumeBar.Size = new Size(205, 45);
            VolumeBar.TabIndex = 1;
            VolumeBar.Value = 25;
            VolumeBar.Scroll += trackBar1_Scroll;
            // 
            // SoundList_Box
            // 
            SoundList_Box.FormattingEnabled = true;
            SoundList_Box.ItemHeight = 15;
            SoundList_Box.Items.AddRange(new object[] { "하나", "둘", "셋", "넷", "다섯", "여섯", "일곱", "여덟", "아홉", "열" });
            SoundList_Box.Location = new Point(223, 12);
            SoundList_Box.Name = "SoundList_Box";
            SoundList_Box.Size = new Size(257, 139);
            SoundList_Box.TabIndex = 2;
            //SoundList_Box.Visible = false;
            SoundList_Box.SelectedIndexChanged += SoundList_Box_SelectedIndexChanged;
            // 
            // volume_label
            // 
            volume_label.AutoSize = true;
            volume_label.Location = new Point(12, 12);
            volume_label.Name = "volume_label";
            volume_label.Size = new Size(52, 15);
            volume_label.TabIndex = 3;
            volume_label.Text = "음량: 25";
            // 
            // reload_Button
            // 
            reload_Button.Font = new Font("맑은 고딕", 8F);
            reload_Button.Location = new Point(129, 125);
            reload_Button.Name = "reload_Button";
            reload_Button.Size = new Size(88, 26);
            reload_Button.TabIndex = 4;
            reload_Button.Text = "재탐색";
            reload_Button.UseVisualStyleBackColor = true;
            // 
            // button3
            // 
            button3.Font = new Font("맑은 고딕", 8F);
            button3.Location = new Point(392, 172);
            button3.Name = "button3";
            button3.Size = new Size(88, 26);
            button3.TabIndex = 5;
            button3.Text = "적용";
            button3.UseVisualStyleBackColor = true;
            button3.Click += button3_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(492, 204);
            Controls.Add(button3);
            Controls.Add(reload_Button);
            Controls.Add(volume_label);
            Controls.Add(SoundList_Box);
            Controls.Add(VolumeBar);
            Controls.Add(quit_Button);
            ForeColor = SystemColors.ControlText;
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "Form1";
            Text = "Form1";
            Load += Form1_Load;
            ((System.ComponentModel.ISupportInitialize)VolumeBar).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button quit_Button;
        private TrackBar VolumeBar;
        private Label volume_label;
        private Button reload_Button;
        private Button button3;
        public ListBox SoundList_Box;
    }
}
