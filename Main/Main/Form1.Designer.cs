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
            this.btn_playerColor = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.button_triggerLeft = new System.Windows.Forms.Button();
            this.canvas = new System.Windows.Forms.PictureBox();
            this.panel_left = new System.Windows.Forms.Panel();
            this.btn_join = new System.Windows.Forms.Button();
            this.btn_host = new System.Windows.Forms.Button();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.colorDialog1 = new System.Windows.Forms.ColorDialog();
            this.btn_symbol = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.canvas)).BeginInit();
            this.panel_left.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.panel1.Controls.Add(this.btn_symbol);
            this.panel1.Controls.Add(this.btn_playerColor);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.button_triggerLeft);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1469, 46);
            this.panel1.TabIndex = 0;
            // 
            // btn_playerColor
            // 
            this.btn_playerColor.FlatAppearance.BorderSize = 0;
            this.btn_playerColor.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_playerColor.Location = new System.Drawing.Point(1423, 0);
            this.btn_playerColor.Name = "btn_playerColor";
            this.btn_playerColor.Size = new System.Drawing.Size(46, 46);
            this.btn_playerColor.TabIndex = 2;
            this.btn_playerColor.UseVisualStyleBackColor = true;
            this.btn_playerColor.Click += new System.EventHandler(this.btn_playerColor_Click);
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Quicksand Book", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(55, 4);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(535, 37);
            this.label1.TabIndex = 1;
            this.label1.Text = "Käsekästchen";
            // 
            // button_triggerLeft
            // 
            this.button_triggerLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.button_triggerLeft.FlatAppearance.BorderSize = 0;
            this.button_triggerLeft.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_triggerLeft.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_triggerLeft.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(247)))));
            this.button_triggerLeft.Location = new System.Drawing.Point(0, 0);
            this.button_triggerLeft.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.button_triggerLeft.Name = "button_triggerLeft";
            this.button_triggerLeft.Size = new System.Drawing.Size(46, 46);
            this.button_triggerLeft.TabIndex = 0;
            this.button_triggerLeft.Text = "≡";
            this.button_triggerLeft.UseVisualStyleBackColor = true;
            this.button_triggerLeft.Click += new System.EventHandler(this.button_triggerLeft_Click);
            // 
            // canvas
            // 
            this.canvas.Location = new System.Drawing.Point(264, 56);
            this.canvas.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.canvas.Name = "canvas";
            this.canvas.Size = new System.Drawing.Size(571, 511);
            this.canvas.TabIndex = 1;
            this.canvas.TabStop = false;
            this.canvas.Paint += new System.Windows.Forms.PaintEventHandler(this.canvas_Paint);
            this.canvas.MouseClick += new System.Windows.Forms.MouseEventHandler(this.canvas_MouseClick);
            this.canvas.MouseMove += new System.Windows.Forms.MouseEventHandler(this.canvas_MouseMove);
            // 
            // panel_left
            // 
            this.panel_left.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(73)))), ((int)(((byte)(73)))), ((int)(((byte)(73)))));
            this.panel_left.Controls.Add(this.btn_join);
            this.panel_left.Controls.Add(this.btn_host);
            this.panel_left.Location = new System.Drawing.Point(0, 46);
            this.panel_left.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.panel_left.Name = "panel_left";
            this.panel_left.Size = new System.Drawing.Size(252, 563);
            this.panel_left.TabIndex = 2;
            // 
            // btn_join
            // 
            this.btn_join.FlatAppearance.BorderSize = 0;
            this.btn_join.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_join.Font = new System.Drawing.Font("Roboto Lt", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_join.Location = new System.Drawing.Point(3, 52);
            this.btn_join.Name = "btn_join";
            this.btn_join.Size = new System.Drawing.Size(246, 38);
            this.btn_join.TabIndex = 1;
            this.btn_join.Text = "Spiel Beitreten";
            this.btn_join.UseVisualStyleBackColor = true;
            this.btn_join.Click += new System.EventHandler(this.btn_join_Click);
            // 
            // btn_host
            // 
            this.btn_host.FlatAppearance.BorderSize = 0;
            this.btn_host.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_host.Font = new System.Drawing.Font("Roboto Lt", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_host.Location = new System.Drawing.Point(3, 8);
            this.btn_host.Name = "btn_host";
            this.btn_host.Size = new System.Drawing.Size(246, 38);
            this.btn_host.TabIndex = 0;
            this.btn_host.Text = "Spiel Hosten";
            this.btn_host.UseVisualStyleBackColor = true;
            this.btn_host.Click += new System.EventHandler(this.btn_host_Click);
            // 
            // listBox1
            // 
            this.listBox1.Font = new System.Drawing.Font("Roboto Lt", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.listBox1.FormattingEnabled = true;
            this.listBox1.ItemHeight = 15;
            this.listBox1.Location = new System.Drawing.Point(896, 66);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(553, 244);
            this.listBox1.TabIndex = 2;
            this.listBox1.SelectedIndexChanged += new System.EventHandler(this.listBox1_SelectedIndexChanged);
            // 
            // btn_symbol
            // 
            this.btn_symbol.FlatAppearance.BorderSize = 0;
            this.btn_symbol.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_symbol.Location = new System.Drawing.Point(1371, 0);
            this.btn_symbol.Name = "btn_symbol";
            this.btn_symbol.Size = new System.Drawing.Size(46, 46);
            this.btn_symbol.TabIndex = 3;
            this.btn_symbol.UseVisualStyleBackColor = true;
            this.btn_symbol.Click += new System.EventHandler(this.btn_symbol_Click);
            this.btn_symbol.Paint += new System.Windows.Forms.PaintEventHandler(this.btn_symbol_Paint);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(19)))), ((int)(((byte)(19)))), ((int)(((byte)(19)))));
            this.ClientSize = new System.Drawing.Size(1469, 609);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.panel_left);
            this.Controls.Add(this.canvas);
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(247)))));
            this.Location = new System.Drawing.Point(-1800, 300);
            this.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Käsekästchen";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.Resize += new System.EventHandler(this.Form1_Resize);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.canvas)).EndInit();
            this.panel_left.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button button_triggerLeft;
        private System.Windows.Forms.PictureBox canvas;
        private System.Windows.Forms.Panel panel_left;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btn_host;
        private System.Windows.Forms.Button btn_join;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.Button btn_playerColor;
        private System.Windows.Forms.ColorDialog colorDialog1;
        private System.Windows.Forms.Button btn_symbol;
    }
}

