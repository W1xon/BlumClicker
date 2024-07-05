namespace BlumClickWinForm
{
    partial class BlumClick
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BlumClick));
            this.pictureBoxScreen = new System.Windows.Forms.PictureBox();
            this.buttonRun = new System.Windows.Forms.Button();
            this.trackBarWidth = new System.Windows.Forms.TrackBar();
            this.trackBarHeight = new System.Windows.Forms.TrackBar();
            this.buttonHelp = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxScreen)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarWidth)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarHeight)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBoxScreen
            // 
            this.pictureBoxScreen.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBoxScreen.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBoxScreen.Location = new System.Drawing.Point(0, 0);
            this.pictureBoxScreen.Name = "pictureBoxScreen";
            this.pictureBoxScreen.Size = new System.Drawing.Size(434, 195);
            this.pictureBoxScreen.TabIndex = 0;
            this.pictureBoxScreen.TabStop = false;
            // 
            // buttonRun
            // 
            this.buttonRun.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonRun.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.buttonRun.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonRun.Location = new System.Drawing.Point(0, 163);
            this.buttonRun.Name = "buttonRun";
            this.buttonRun.Size = new System.Drawing.Size(93, 33);
            this.buttonRun.TabIndex = 1;
            this.buttonRun.Text = "Start";
            this.buttonRun.UseVisualStyleBackColor = true;
            this.buttonRun.Click += new System.EventHandler(this.buttonRun_Click);
            // 
            // trackBarWidth
            // 
            this.trackBarWidth.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.trackBarWidth.Location = new System.Drawing.Point(99, 148);
            this.trackBarWidth.Maximum = 150;
            this.trackBarWidth.Minimum = -150;
            this.trackBarWidth.Name = "trackBarWidth";
            this.trackBarWidth.Size = new System.Drawing.Size(104, 45);
            this.trackBarWidth.TabIndex = 2;
            this.trackBarWidth.TickStyle = System.Windows.Forms.TickStyle.Both;
            this.trackBarWidth.Scroll += new System.EventHandler(this.trackBarWidth_Scroll);
            // 
            // trackBarHeight
            // 
            this.trackBarHeight.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.trackBarHeight.Location = new System.Drawing.Point(209, 148);
            this.trackBarHeight.Maximum = 150;
            this.trackBarHeight.Minimum = -150;
            this.trackBarHeight.Name = "trackBarHeight";
            this.trackBarHeight.Size = new System.Drawing.Size(104, 45);
            this.trackBarHeight.TabIndex = 3;
            this.trackBarHeight.TickStyle = System.Windows.Forms.TickStyle.Both;
            this.trackBarHeight.Scroll += new System.EventHandler(this.trackBarHeight_Scroll);
            // 
            // buttonHelp
            // 
            this.buttonHelp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonHelp.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonHelp.Location = new System.Drawing.Point(404, 164);
            this.buttonHelp.Name = "buttonHelp";
            this.buttonHelp.Size = new System.Drawing.Size(30, 30);
            this.buttonHelp.TabIndex = 4;
            this.buttonHelp.Text = "❓";
            this.buttonHelp.UseVisualStyleBackColor = true;
            this.buttonHelp.Click += new System.EventHandler(this.buttonHelp_Click);
            // 
            // BlumClick
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(434, 195);
            this.Controls.Add(this.buttonHelp);
            this.Controls.Add(this.trackBarHeight);
            this.Controls.Add(this.trackBarWidth);
            this.Controls.Add(this.buttonRun);
            this.Controls.Add(this.pictureBoxScreen);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(350, 0);
            this.Name = "BlumClick";
            this.Text = "Blum Click";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxScreen)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarWidth)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarHeight)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBoxScreen;
        private System.Windows.Forms.Button buttonRun;
        private System.Windows.Forms.TrackBar trackBarWidth;
        private System.Windows.Forms.TrackBar trackBarHeight;
        private System.Windows.Forms.Button buttonHelp;
    }
}

