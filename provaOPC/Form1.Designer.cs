namespace provaOPC
{
    partial class Form1
    {
        /// <summary>
        /// Variabile di progettazione necessaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Pulire le risorse in uso.
        /// </summary>
        /// <param name="disposing">ha valore true se le risorse gestite devono essere eliminate, false in caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Codice generato da Progettazione Windows Form

        /// <summary>
        /// Metodo necessario per il supporto della finestra di progettazione. Non modificare
        /// il contenuto del metodo con l'editor di codice.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.butScrivi = new System.Windows.Forms.Button();
            this.butPath = new System.Windows.Forms.Button();
            this.textBoxPath = new System.Windows.Forms.TextBox();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.textBoxTopic = new System.Windows.Forms.TextBox();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.SuspendLayout();
            // 
            // butScrivi
            // 
            this.butScrivi.Enabled = false;
            this.butScrivi.Location = new System.Drawing.Point(267, 41);
            this.butScrivi.Name = "butScrivi";
            this.butScrivi.Size = new System.Drawing.Size(53, 23);
            this.butScrivi.TabIndex = 0;
            this.butScrivi.Text = "Scrivi";
            this.butScrivi.UseVisualStyleBackColor = true;
            this.butScrivi.Click += new System.EventHandler(this.butScrivi_Click);
            // 
            // butPath
            // 
            this.butPath.Location = new System.Drawing.Point(288, 12);
            this.butPath.Name = "butPath";
            this.butPath.Size = new System.Drawing.Size(32, 22);
            this.butPath.TabIndex = 1;
            this.butPath.Text = "...";
            this.butPath.UseVisualStyleBackColor = true;
            this.butPath.Click += new System.EventHandler(this.butPath_Click);
            // 
            // textBoxPath
            // 
            this.textBoxPath.BackColor = System.Drawing.Color.LightGreen;
            this.textBoxPath.Location = new System.Drawing.Point(13, 13);
            this.textBoxPath.Name = "textBoxPath";
            this.textBoxPath.Size = new System.Drawing.Size(269, 22);
            this.textBoxPath.TabIndex = 2;
            this.textBoxPath.Text = "Selezionare il file .CSV da scrivere";
            this.textBoxPath.TextChanged += new System.EventHandler(this.textBoxPath_TextChanged);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // textBoxTopic
            // 
            this.textBoxTopic.Enabled = false;
            this.textBoxTopic.Location = new System.Drawing.Point(12, 41);
            this.textBoxTopic.Name = "textBoxTopic";
            this.textBoxTopic.Size = new System.Drawing.Size(248, 22);
            this.textBoxTopic.TabIndex = 4;
            this.textBoxTopic.Text = "Creg_OPC_Topic";
            this.textBoxTopic.Click += new System.EventHandler(this.textBoxTopic_Click);
            this.textBoxTopic.TextChanged += new System.EventHandler(this.textBoxTopic_TextChanged);
            this.textBoxTopic.Leave += new System.EventHandler(this.textBoxTopic_Leave);
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(13, 67);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(307, 23);
            this.progressBar1.Step = 1;
            this.progressBar1.TabIndex = 5;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(332, 102);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.textBoxTopic);
            this.Controls.Add(this.textBoxPath);
            this.Controls.Add(this.butPath);
            this.Controls.Add(this.butScrivi);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximumSize = new System.Drawing.Size(350, 147);
            this.MinimumSize = new System.Drawing.Size(350, 147);
            this.Name = "Form1";
            this.Text = "OPC Writer";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button butScrivi;
        private System.Windows.Forms.Button butPath;
        private System.Windows.Forms.TextBox textBoxPath;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.TextBox textBoxTopic;
        private System.Windows.Forms.ProgressBar progressBar1;
    }
}

