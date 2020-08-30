namespace Käsekästchen
{
    partial class ServerView
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
            this.tb_log = new System.Windows.Forms.TextBox();
            this.lv_players = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SuspendLayout();
            // 
            // tb_log
            // 
            this.tb_log.BackColor = System.Drawing.Color.White;
            this.tb_log.Location = new System.Drawing.Point(12, 12);
            this.tb_log.Multiline = true;
            this.tb_log.Name = "tb_log";
            this.tb_log.ReadOnly = true;
            this.tb_log.Size = new System.Drawing.Size(465, 506);
            this.tb_log.TabIndex = 0;
            // 
            // lv_players
            // 
            this.lv_players.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2});
            this.lv_players.HideSelection = false;
            this.lv_players.Location = new System.Drawing.Point(494, 12);
            this.lv_players.Name = "lv_players";
            this.lv_players.Size = new System.Drawing.Size(361, 296);
            this.lv_players.TabIndex = 1;
            this.lv_players.UseCompatibleStateImageBehavior = false;
            this.lv_players.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Name";
            this.columnHeader1.Width = 250;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Origin";
            this.columnHeader2.Width = 100;
            // 
            // ServerView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(869, 532);
            this.Controls.Add(this.lv_players);
            this.Controls.Add(this.tb_log);
            this.Name = "ServerView";
            this.Text = "ServerView";
            this.Load += new System.EventHandler(this.ServerView_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        public System.Windows.Forms.TextBox tb_log;
        public System.Windows.Forms.ListView lv_players;
    }
}