namespace TaterNotify
{
    partial class SettingsForm
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
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.cboUserOwner = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cbPlaySounds = new System.Windows.Forms.CheckBox();
            this.cbSoundsOnlyForTeam = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // cboUserOwner
            // 
            this.cboUserOwner.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboUserOwner.FormattingEnabled = true;
            this.cboUserOwner.Location = new System.Drawing.Point(114, 13);
            this.cboUserOwner.MaxDropDownItems = 13;
            this.cboUserOwner.Name = "cboUserOwner";
            this.cboUserOwner.Size = new System.Drawing.Size(147, 21);
            this.cboUserOwner.TabIndex = 0;
            this.cboUserOwner.SelectedIndexChanged += new System.EventHandler(this.cboUserOwner_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(9, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(99, 23);
            this.label1.TabIndex = 1;
            this.label1.Text = "Your Team Name:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cbPlaySounds
            // 
            this.cbPlaySounds.AutoSize = true;
            this.cbPlaySounds.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.cbPlaySounds.Location = new System.Drawing.Point(40, 40);
            this.cbPlaySounds.Name = "cbPlaySounds";
            this.cbPlaySounds.Size = new System.Drawing.Size(88, 17);
            this.cbPlaySounds.TabIndex = 2;
            this.cbPlaySounds.Text = "Play Sounds:";
            this.cbPlaySounds.UseVisualStyleBackColor = true;
            this.cbPlaySounds.CheckedChanged += new System.EventHandler(this.cbPlaySounds_CheckedChanged);
            // 
            // cbSoundsOnlyForTeam
            // 
            this.cbSoundsOnlyForTeam.AutoSize = true;
            this.cbSoundsOnlyForTeam.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.cbSoundsOnlyForTeam.Enabled = false;
            this.cbSoundsOnlyForTeam.Location = new System.Drawing.Point(143, 40);
            this.cbSoundsOnlyForTeam.Name = "cbSoundsOnlyForTeam";
            this.cbSoundsOnlyForTeam.Size = new System.Drawing.Size(118, 17);
            this.cbSoundsOnlyForTeam.TabIndex = 3;
            this.cbSoundsOnlyForTeam.Text = "Only For My Team?";
            this.cbSoundsOnlyForTeam.UseVisualStyleBackColor = true;
            this.cbSoundsOnlyForTeam.CheckedChanged += new System.EventHandler(this.cbSoundsOnlyForTeam_CheckedChanged);
            // 
            // SettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(286, 69);
            this.Controls.Add(this.cbSoundsOnlyForTeam);
            this.Controls.Add(this.cbPlaySounds);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cboUserOwner);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SettingsForm";
            this.Text = "TaterNotify Settings";
            this.Load += new System.EventHandler(this.SettingsForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.ComboBox cboUserOwner;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox cbPlaySounds;
        private System.Windows.Forms.CheckBox cbSoundsOnlyForTeam;
    }
}