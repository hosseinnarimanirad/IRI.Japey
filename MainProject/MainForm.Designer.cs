namespace MainProject
{
    partial class MainForm
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
            this.voronoi = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.delauanay = new System.Windows.Forms.Button();
            this.convex = new System.Windows.Forms.Button();
            this.import = new System.Windows.Forms.Button();
            this.export = new System.Windows.Forms.Button();
            this.generate = new System.Windows.Forms.Button();
            this.checkCircle = new System.Windows.Forms.Button();
            this.dCheck = new System.Windows.Forms.CheckBox();
            this.vCheck = new System.Windows.Forms.CheckBox();
            this.cCheck = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.mCheck = new System.Windows.Forms.CheckBox();
            this.nCheck = new System.Windows.Forms.CheckBox();
            this.pCheck = new System.Windows.Forms.CheckBox();
            this.button1 = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(734, 66);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(93, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Number of Points:";
            // 
            // voronoi
            // 
            this.voronoi.Location = new System.Drawing.Point(851, 243);
            this.voronoi.Name = "voronoi";
            this.voronoi.Size = new System.Drawing.Size(75, 23);
            this.voronoi.TabIndex = 2;
            this.voronoi.Text = "Voronoi";
            this.voronoi.UseVisualStyleBackColor = true;
            this.voronoi.Click += new System.EventHandler(this.voronoi_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(826, 63);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(100, 21);
            this.textBox1.TabIndex = 3;
            this.textBox1.Text = "20";
            // 
            // delauanay
            // 
            this.delauanay.Location = new System.Drawing.Point(851, 272);
            this.delauanay.Name = "delauanay";
            this.delauanay.Size = new System.Drawing.Size(75, 23);
            this.delauanay.TabIndex = 5;
            this.delauanay.Text = "Delaunay";
            this.delauanay.UseVisualStyleBackColor = true;
            this.delauanay.Click += new System.EventHandler(this.delaunay_Click);
            // 
            // convex
            // 
            this.convex.Location = new System.Drawing.Point(851, 214);
            this.convex.Name = "convex";
            this.convex.Size = new System.Drawing.Size(75, 23);
            this.convex.TabIndex = 6;
            this.convex.Text = "ConvexHull";
            this.convex.UseVisualStyleBackColor = true;
            this.convex.Click += new System.EventHandler(this.convexHull_Click);
            // 
            // import
            // 
            this.import.Enabled = false;
            this.import.Location = new System.Drawing.Point(734, 104);
            this.import.Name = "import";
            this.import.Size = new System.Drawing.Size(86, 23);
            this.import.TabIndex = 7;
            this.import.Text = "Import Points";
            this.import.UseVisualStyleBackColor = true;
            this.import.Click += new System.EventHandler(this.import_Click);
            // 
            // export
            // 
            this.export.Location = new System.Drawing.Point(734, 133);
            this.export.Name = "export";
            this.export.Size = new System.Drawing.Size(86, 23);
            this.export.TabIndex = 8;
            this.export.Text = "Export Points";
            this.export.UseVisualStyleBackColor = true;
            this.export.Click += new System.EventHandler(this.export_Click);
            // 
            // generate
            // 
            this.generate.Location = new System.Drawing.Point(851, 104);
            this.generate.Name = "generate";
            this.generate.Size = new System.Drawing.Size(75, 23);
            this.generate.TabIndex = 9;
            this.generate.Text = "Generate";
            this.generate.UseVisualStyleBackColor = true;
            this.generate.Click += new System.EventHandler(this.generate_Click);
            // 
            // checkCircle
            // 
            this.checkCircle.Location = new System.Drawing.Point(734, 272);
            this.checkCircle.Name = "checkCircle";
            this.checkCircle.Size = new System.Drawing.Size(111, 23);
            this.checkCircle.TabIndex = 11;
            this.checkCircle.Text = "Check empty circle";
            this.checkCircle.UseVisualStyleBackColor = true;
            this.checkCircle.Click += new System.EventHandler(this.checkCircle_Click);
            // 
            // dCheck
            // 
            this.dCheck.AutoSize = true;
            this.dCheck.Checked = true;
            this.dCheck.CheckState = System.Windows.Forms.CheckState.Checked;
            this.dCheck.Location = new System.Drawing.Point(6, 20);
            this.dCheck.Name = "dCheck";
            this.dCheck.Size = new System.Drawing.Size(71, 17);
            this.dCheck.TabIndex = 12;
            this.dCheck.Text = "Delaunay";
            this.dCheck.UseVisualStyleBackColor = true;
            this.dCheck.CheckedChanged += new System.EventHandler(this.dCheck_CheckedChanged);
            // 
            // vCheck
            // 
            this.vCheck.AutoSize = true;
            this.vCheck.Location = new System.Drawing.Point(6, 43);
            this.vCheck.Name = "vCheck";
            this.vCheck.Size = new System.Drawing.Size(62, 17);
            this.vCheck.TabIndex = 13;
            this.vCheck.Text = "Voronoi";
            this.vCheck.UseVisualStyleBackColor = true;
            this.vCheck.CheckedChanged += new System.EventHandler(this.dCheck_CheckedChanged);
            // 
            // cCheck
            // 
            this.cCheck.AutoSize = true;
            this.cCheck.Location = new System.Drawing.Point(6, 66);
            this.cCheck.Name = "cCheck";
            this.cCheck.Size = new System.Drawing.Size(83, 17);
            this.cCheck.TabIndex = 14;
            this.cCheck.Text = "Convex Hull";
            this.cCheck.UseVisualStyleBackColor = true;
            this.cCheck.CheckedChanged += new System.EventHandler(this.dCheck_CheckedChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.mCheck);
            this.groupBox1.Controls.Add(this.nCheck);
            this.groupBox1.Controls.Add(this.dCheck);
            this.groupBox1.Controls.Add(this.cCheck);
            this.groupBox1.Controls.Add(this.vCheck);
            this.groupBox1.Location = new System.Drawing.Point(734, 301);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(192, 141);
            this.groupBox1.TabIndex = 15;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Show Items";
            // 
            // mCheck
            // 
            this.mCheck.AutoSize = true;
            this.mCheck.Location = new System.Drawing.Point(6, 112);
            this.mCheck.Name = "mCheck";
            this.mCheck.Size = new System.Drawing.Size(138, 17);
            this.mCheck.TabIndex = 16;
            this.mCheck.Text = "Minimum Spanning Tree";
            this.mCheck.UseVisualStyleBackColor = true;
            this.mCheck.CheckedChanged += new System.EventHandler(this.dCheck_CheckedChanged);
            // 
            // nCheck
            // 
            this.nCheck.AutoSize = true;
            this.nCheck.Location = new System.Drawing.Point(6, 89);
            this.nCheck.Name = "nCheck";
            this.nCheck.Size = new System.Drawing.Size(65, 17);
            this.nCheck.TabIndex = 15;
            this.nCheck.Text = "network";
            this.nCheck.UseVisualStyleBackColor = true;
            this.nCheck.CheckedChanged += new System.EventHandler(this.dCheck_CheckedChanged);
            // 
            // pCheck
            // 
            this.pCheck.AutoSize = true;
            this.pCheck.Checked = true;
            this.pCheck.CheckState = System.Windows.Forms.CheckState.Checked;
            this.pCheck.Location = new System.Drawing.Point(6, 20);
            this.pCheck.Name = "pCheck";
            this.pCheck.Size = new System.Drawing.Size(142, 17);
            this.pCheck.TabIndex = 16;
            this.pCheck.Text = "Draw points with voronoi";
            this.pCheck.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(851, 650);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 17;
            this.button1.Text = "About Me...";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.pCheck);
            this.groupBox2.Location = new System.Drawing.Point(734, 448);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(192, 66);
            this.groupBox2.TabIndex = 18;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Options";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(826, 109);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(19, 13);
            this.label2.TabIndex = 19;
            this.label2.Text = "Or";
            // 
            // pictureBox1
            // 
            this.pictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox1.Location = new System.Drawing.Point(12, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(716, 661);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseClick);
            this.pictureBox1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseMove);
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(744, 16);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(100, 21);
            this.textBox2.TabIndex = 20;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(944, 685);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.checkCircle);
            this.Controls.Add(this.generate);
            this.Controls.Add(this.export);
            this.Controls.Add(this.import);
            this.Controls.Add(this.convex);
            this.Controls.Add(this.delauanay);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.voronoi);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pictureBox1);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "KNTUniversity; Faculty of Geomatics; GIS Dept.";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button voronoi;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button delauanay;
        private System.Windows.Forms.Button convex;
        private System.Windows.Forms.Button import;
        private System.Windows.Forms.Button export;
        private System.Windows.Forms.Button generate;
        private System.Windows.Forms.Button checkCircle;
        private System.Windows.Forms.CheckBox dCheck;
        private System.Windows.Forms.CheckBox vCheck;
        private System.Windows.Forms.CheckBox cCheck;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox pCheck;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox nCheck;
        private System.Windows.Forms.CheckBox mCheck;
        private System.Windows.Forms.TextBox textBox2;
    }
}