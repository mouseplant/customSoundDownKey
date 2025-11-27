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
            Quit_Button = new Button();
            VolumeBar = new TrackBar();
            SoundList_Box = new ListBox();
            Volume_label = new Label();
            Reload_Button = new Button();
            Apply_Button = new Button();
            ((System.ComponentModel.ISupportInitialize)VolumeBar).BeginInit();
            SuspendLayout();
            // 
            // Quit_Button
            // 
            Quit_Button.Font = new Font("맑은 고딕", 8F);
            Quit_Button.Location = new Point(21, 344);
            Quit_Button.Margin = new Padding(5, 6, 5, 6);
            Quit_Button.Name = "Quit_Button";
            Quit_Button.Size = new Size(151, 52);
            Quit_Button.TabIndex = 0;
            Quit_Button.Text = "종료";
            Quit_Button.UseVisualStyleBackColor = true;
            Quit_Button.Click += button1_Click;
            // 
            // VolumeBar
            // 
            VolumeBar.LargeChange = 1;
            VolumeBar.Location = new Point(21, 60);
            VolumeBar.Margin = new Padding(5, 6, 5, 6);
            VolumeBar.Maximum = 25;
            VolumeBar.Name = "VolumeBar";
            VolumeBar.Size = new Size(351, 80);
            VolumeBar.TabIndex = 1;
            VolumeBar.Value = 25;
            VolumeBar.Scroll += trackBar1_Scroll;
            // 
            // SoundList_Box
            // 
            SoundList_Box.FormattingEnabled = true;
            SoundList_Box.ItemHeight = 30;
            SoundList_Box.Items.AddRange(new object[] { "하나", "둘", "셋", "넷", "다섯", "여섯", "일곱", "여덟", "아홉", "열" });
            SoundList_Box.Location = new Point(382, 24);
            SoundList_Box.Margin = new Padding(5, 6, 5, 6);
            SoundList_Box.Name = "SoundList_Box";
            SoundList_Box.Size = new Size(438, 274);
            SoundList_Box.TabIndex = 2;
            SoundList_Box.SelectedIndexChanged += SoundList_Box_SelectedIndexChanged;
            // 
            // Volume_label
            // 
            Volume_label.AutoSize = true;
            Volume_label.Location = new Point(21, 24);
            Volume_label.Margin = new Padding(5, 0, 5, 0);
            Volume_label.Name = "Volume_label";
            Volume_label.Size = new Size(91, 30);
            Volume_label.TabIndex = 3;
            Volume_label.Text = "음량: 25";
            // 
            // Reload_Button
            // 
            Reload_Button.Font = new Font("맑은 고딕", 8F);
            Reload_Button.Location = new Point(221, 250);
            Reload_Button.Margin = new Padding(5, 6, 5, 6);
            Reload_Button.Name = "Reload_Button";
            Reload_Button.Size = new Size(151, 52);
            Reload_Button.TabIndex = 4;
            Reload_Button.Text = "재탐색";
            Reload_Button.UseVisualStyleBackColor = true;
            Reload_Button.Click += Reload_Button_Click;
            // 
            // Apply_Button
            // 
            Apply_Button.Enabled = false;
            Apply_Button.Font = new Font("맑은 고딕", 8F);
            Apply_Button.Location = new Point(672, 344);
            Apply_Button.Margin = new Padding(5, 6, 5, 6);
            Apply_Button.Name = "Apply_Button";
            Apply_Button.Size = new Size(151, 52);
            Apply_Button.TabIndex = 5;
            Apply_Button.Text = "적용";
            Apply_Button.UseVisualStyleBackColor = true;
            Apply_Button.Click += button3_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(12F, 30F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(843, 408);
            Controls.Add(Apply_Button);
            Controls.Add(Reload_Button);
            Controls.Add(Volume_label);
            Controls.Add(SoundList_Box);
            Controls.Add(VolumeBar);
            Controls.Add(Quit_Button);
            ForeColor = SystemColors.ControlText;
            Icon = (Icon)resources.GetObject("$this.Icon");
            Margin = new Padding(5, 6, 5, 6);
            Name = "Form1";
            Text = "Form1";
            Load += Form1_Load;
            ((System.ComponentModel.ISupportInitialize)VolumeBar).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button Quit_Button;
        private TrackBar VolumeBar;
        private Label Volume_label;
        private Button Reload_Button;
        private Button Apply_Button;
        public ListBox SoundList_Box;
    }
}
