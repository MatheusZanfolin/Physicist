namespace Physicist
{
    partial class Form2
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
            this.lblDesenhaveis = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblDesenhaveis
            // 
            this.lblDesenhaveis.AutoSize = true;
            this.lblDesenhaveis.Location = new System.Drawing.Point(341, 13);
            this.lblDesenhaveis.Name = "lblDesenhaveis";
            this.lblDesenhaveis.Size = new System.Drawing.Size(0, 13);
            this.lblDesenhaveis.TabIndex = 0;
            // 
            // Form2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(855, 588);
            this.Controls.Add(this.lblDesenhaveis);
            this.Name = "Form2";
            this.Text = "Physicist - Simulação";
            this.Load += new System.EventHandler(this.Form2_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblDesenhaveis;
    }
}