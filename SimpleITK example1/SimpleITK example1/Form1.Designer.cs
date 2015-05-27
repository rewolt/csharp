namespace SimpleITK_example1
{
    partial class Form1
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
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.open = new System.Windows.Forms.Button();
            this.lLokalizacja = new System.Windows.Forms.Label();
            this.bAkcja = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(12, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(612, 401);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // open
            // 
            this.open.Location = new System.Drawing.Point(12, 419);
            this.open.Name = "open";
            this.open.Size = new System.Drawing.Size(75, 23);
            this.open.TabIndex = 1;
            this.open.Text = "Wczytaj";
            this.open.UseVisualStyleBackColor = true;
            this.open.Click += new System.EventHandler(this.open_Click);
            // 
            // lLokalizacja
            // 
            this.lLokalizacja.AutoSize = true;
            this.lLokalizacja.Location = new System.Drawing.Point(111, 452);
            this.lLokalizacja.Name = "lLokalizacja";
            this.lLokalizacja.Size = new System.Drawing.Size(60, 13);
            this.lLokalizacja.TabIndex = 2;
            this.lLokalizacja.Text = "Lokalizacja";
            // 
            // bAkcja
            // 
            this.bAkcja.Location = new System.Drawing.Point(12, 452);
            this.bAkcja.Name = "bAkcja";
            this.bAkcja.Size = new System.Drawing.Size(75, 23);
            this.bAkcja.TabIndex = 3;
            this.bAkcja.Text = "Akcja!";
            this.bAkcja.UseVisualStyleBackColor = true;
            this.bAkcja.Click += new System.EventHandler(this.bAkcja_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(644, 498);
            this.Controls.Add(this.bAkcja);
            this.Controls.Add(this.lLokalizacja);
            this.Controls.Add(this.open);
            this.Controls.Add(this.pictureBox1);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button open;
        private System.Windows.Forms.Label lLokalizacja;
        private System.Windows.Forms.Button bAkcja;
    }
}

