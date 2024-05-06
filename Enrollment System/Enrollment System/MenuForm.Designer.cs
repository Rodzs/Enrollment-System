namespace Enrollment_System
{
    partial class MenuForm
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
            this.SubjectEntryButton = new System.Windows.Forms.Button();
            this.SubjectScheduleButton = new System.Windows.Forms.Button();
            this.StudentEntryButton = new System.Windows.Forms.Button();
            this.StudentEnrollmentEntryButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Harlow Solid Italic", 15.75F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(270, 62);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(229, 26);
            this.label1.TabIndex = 1;
            this.label1.Text = "Enrollment System Menu";
            // 
            // SubjectEntryButton
            // 
            this.SubjectEntryButton.Font = new System.Drawing.Font("Harlow Solid Italic", 15.75F, System.Drawing.FontStyle.Italic);
            this.SubjectEntryButton.Location = new System.Drawing.Point(46, 192);
            this.SubjectEntryButton.Name = "SubjectEntryButton";
            this.SubjectEntryButton.Size = new System.Drawing.Size(142, 216);
            this.SubjectEntryButton.TabIndex = 3;
            this.SubjectEntryButton.Text = "Subject Entry";
            this.SubjectEntryButton.UseVisualStyleBackColor = true;
            this.SubjectEntryButton.Click += new System.EventHandler(this.SubjectEntryButton_Click);
            // 
            // SubjectScheduleButton
            // 
            this.SubjectScheduleButton.Font = new System.Drawing.Font("Harlow Solid Italic", 15.75F, System.Drawing.FontStyle.Italic);
            this.SubjectScheduleButton.Location = new System.Drawing.Point(241, 192);
            this.SubjectScheduleButton.Name = "SubjectScheduleButton";
            this.SubjectScheduleButton.Size = new System.Drawing.Size(144, 216);
            this.SubjectScheduleButton.TabIndex = 4;
            this.SubjectScheduleButton.Text = "Subject Schedule";
            this.SubjectScheduleButton.UseVisualStyleBackColor = true;
            this.SubjectScheduleButton.Click += new System.EventHandler(this.SubjectScheduleButton_Click);
            // 
            // StudentEntryButton
            // 
            this.StudentEntryButton.Font = new System.Drawing.Font("Harlow Solid Italic", 15.75F, System.Drawing.FontStyle.Italic);
            this.StudentEntryButton.Location = new System.Drawing.Point(432, 192);
            this.StudentEntryButton.Name = "StudentEntryButton";
            this.StudentEntryButton.Size = new System.Drawing.Size(141, 216);
            this.StudentEntryButton.TabIndex = 5;
            this.StudentEntryButton.Text = "Student Entry";
            this.StudentEntryButton.UseVisualStyleBackColor = true;
            this.StudentEntryButton.Click += new System.EventHandler(this.StudentEntryButton_Click);
            // 
            // StudentEnrollmentEntryButton
            // 
            this.StudentEnrollmentEntryButton.Font = new System.Drawing.Font("Harlow Solid Italic", 15.75F, System.Drawing.FontStyle.Italic);
            this.StudentEnrollmentEntryButton.Location = new System.Drawing.Point(614, 192);
            this.StudentEnrollmentEntryButton.Name = "StudentEnrollmentEntryButton";
            this.StudentEnrollmentEntryButton.Size = new System.Drawing.Size(141, 216);
            this.StudentEnrollmentEntryButton.TabIndex = 6;
            this.StudentEnrollmentEntryButton.Text = "Student Enrollment Entry";
            this.StudentEnrollmentEntryButton.UseVisualStyleBackColor = true;
            // 
            // MenuForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.StudentEnrollmentEntryButton);
            this.Controls.Add(this.StudentEntryButton);
            this.Controls.Add(this.SubjectScheduleButton);
            this.Controls.Add(this.SubjectEntryButton);
            this.Controls.Add(this.label1);
            this.Name = "MenuForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Menu";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Menu_FormClosing);
            this.Load += new System.EventHandler(this.Menu_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button SubjectEntryButton;
        private System.Windows.Forms.Button SubjectScheduleButton;
        private System.Windows.Forms.Button StudentEntryButton;
        private System.Windows.Forms.Button StudentEnrollmentEntryButton;
    }
}