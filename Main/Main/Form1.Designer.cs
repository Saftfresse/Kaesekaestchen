namespace Main
{
    partial class Form1
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.panel1 = new System.Windows.Forms.Panel();
            this.button_triggerLeft = new System.Windows.Forms.Button();
            this.canvas = new System.Windows.Forms.PictureBox();
            this.panel_left = new System.Windows.Forms.Panel();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.canvas)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.panel1.Controls.Add(this.button_triggerLeft);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1088, 42);
            this.panel1.TabIndex = 0;
            // 
            // button_triggerLeft
            // 
            this.button_triggerLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.button_triggerLeft.FlatAppearance.BorderSize = 0;
            this.button_triggerLeft.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_triggerLeft.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_triggerLeft.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(247)))));
            this.button_triggerLeft.Location = new System.Drawing.Point(0, 0);
            this.button_triggerLeft.Name = "button_triggerLeft";
            this.button_triggerLeft.Size = new System.Drawing.Size(42, 42);
            this.button_triggerLeft.TabIndex = 0;
            this.button_triggerLeft.Text = "≡";
            this.button_triggerLeft.UseVisualStyleBackColor = true;
            this.button_triggerLeft.Click += new System.EventHandler(this.button_triggerLeft_Click);
            // 
            // canvas
            // 
            this.canvas.Dock = System.Windows.Forms.DockStyle.Fill;
            this.canvas.Location = new System.Drawing.Point(0, 42);
            this.canvas.Name = "canvas";
            this.canvas.Size = new System.Drawing.Size(1088, 563);
            this.canvas.TabIndex = 1;
            this.canvas.TabStop = false;
            // 
            // panel_left
            // 
            this.panel_left.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(73)))), ((int)(((byte)(73)))), ((int)(((byte)(73)))));
            this.panel_left.Location = new System.Drawing.Point(1, 42);
            this.panel_left.Name = "panel_left";
            this.panel_left.Size = new System.Drawing.Size(199, 563);
            this.panel_left.TabIndex = 2;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(19)))), ((int)(((byte)(19)))), ((int)(((byte)(19)))));
            this.ClientSize = new System.Drawing.Size(1088, 605);
            this.Controls.Add(this.panel_left);
            this.Controls.Add(this.canvas);
            this.Controls.Add(this.panel1);
            this.Name = "Form1";
            this.Text = "Käsekästchen";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.Resize += new System.EventHandler(this.Form1_Resize);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.canvas)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button button_triggerLeft;
        private System.Windows.Forms.PictureBox canvas;
        private System.Windows.Forms.Panel panel_left;
    }
}

