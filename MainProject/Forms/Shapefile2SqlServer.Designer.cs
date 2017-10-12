namespace MainProject.Forms
{
    partial class Shapefile2SqlServer
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
            this.fileName = new System.Windows.Forms.TextBox();
            this.browse = new System.Windows.Forms.Button();
            this.connectionString = new System.Windows.Forms.TextBox();
            this.go = new System.Windows.Forms.Button();
            this.tableName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.sdfFileLocation = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.insertRandom = new System.Windows.Forms.Button();
            this.read = new System.Windows.Forms.Button();
            this.goToPostgres = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // fileName
            // 
            this.fileName.Location = new System.Drawing.Point(12, 25);
            this.fileName.Name = "fileName";
            this.fileName.Size = new System.Drawing.Size(585, 20);
            this.fileName.TabIndex = 0;
            this.fileName.Text = "C:\\Users\\Hossein\\Desktop\\education.shp";
            // 
            // browse
            // 
            this.browse.Location = new System.Drawing.Point(603, 23);
            this.browse.Name = "browse";
            this.browse.Size = new System.Drawing.Size(75, 23);
            this.browse.TabIndex = 1;
            this.browse.Text = "Browse";
            this.browse.UseVisualStyleBackColor = true;
            this.browse.Click += new System.EventHandler(this.browse_Click);
            // 
            // connectionString
            // 
            this.connectionString.Location = new System.Drawing.Point(12, 72);
            this.connectionString.Name = "connectionString";
            this.connectionString.Size = new System.Drawing.Size(671, 20);
            this.connectionString.TabIndex = 2;
            this.connectionString.Text = "server = YA-MORTAZA\\MSSQLSERVER2012; integrated security = true;user id=sa; passw" +
    "ord=sa123456; database = TestSpatialDatabase";
            // 
            // go
            // 
            this.go.Location = new System.Drawing.Point(608, 170);
            this.go.Name = "go";
            this.go.Size = new System.Drawing.Size(75, 23);
            this.go.TabIndex = 3;
            this.go.Text = "Go";
            this.go.UseVisualStyleBackColor = true;
            this.go.Click += new System.EventHandler(this.go_Click);
            // 
            // tableName
            // 
            this.tableName.Location = new System.Drawing.Point(83, 172);
            this.tableName.Name = "tableName";
            this.tableName.Size = new System.Drawing.Size(100, 20);
            this.tableName.TabIndex = 4;
            this.tableName.Text = "education";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(51, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Shapefile";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 56);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(91, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Connection String";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 175);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Table Name";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(10, 111);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(159, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = "SQL Server Compact Database:";
            // 
            // sdfFileLocation
            // 
            this.sdfFileLocation.Location = new System.Drawing.Point(10, 127);
            this.sdfFileLocation.Name = "sdfFileLocation";
            this.sdfFileLocation.Size = new System.Drawing.Size(587, 20);
            this.sdfFileLocation.TabIndex = 8;
            this.sdfFileLocation.Text = "E:\\Narimani\\108. Others\\100. MainProject\\IRI.Tol\\IRI.NGO.GonbadNegar\\bin\\Release\\" +
    "Data\\IRIDatabaseGonbadNegar.sdf";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(603, 124);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 10;
            this.button1.Text = "Browse";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(522, 170);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 11;
            this.button2.Text = "Go (SDF)";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // insertRandom
            // 
            this.insertRandom.Location = new System.Drawing.Point(370, 170);
            this.insertRandom.Name = "insertRandom";
            this.insertRandom.Size = new System.Drawing.Size(146, 23);
            this.insertRandom.TabIndex = 12;
            this.insertRandom.Text = "Insert Random Mountains";
            this.insertRandom.UseVisualStyleBackColor = true;
            this.insertRandom.Click += new System.EventHandler(this.insertRandom_Click);
            // 
            // read
            // 
            this.read.Location = new System.Drawing.Point(306, 169);
            this.read.Name = "read";
            this.read.Size = new System.Drawing.Size(58, 23);
            this.read.TabIndex = 13;
            this.read.Text = "Read";
            this.read.UseVisualStyleBackColor = true;
            this.read.Click += new System.EventHandler(this.read_Click);
            // 
            // goToPostgres
            // 
            this.goToPostgres.Location = new System.Drawing.Point(221, 170);
            this.goToPostgres.Name = "goToPostgres";
            this.goToPostgres.Size = new System.Drawing.Size(79, 23);
            this.goToPostgres.TabIndex = 14;
            this.goToPostgres.Text = "Go Postgres";
            this.goToPostgres.UseVisualStyleBackColor = true;
            this.goToPostgres.Click += new System.EventHandler(this.goToPostgres_Click);
            // 
            // Shapefile2SqlServer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(690, 205);
            this.Controls.Add(this.goToPostgres);
            this.Controls.Add(this.read);
            this.Controls.Add(this.insertRandom);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.sdfFileLocation);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tableName);
            this.Controls.Add(this.go);
            this.Controls.Add(this.connectionString);
            this.Controls.Add(this.browse);
            this.Controls.Add(this.fileName);
            this.Name = "Shapefile2SqlServer";
            this.Text = "Shapefile2SqlServer";
            this.Load += new System.EventHandler(this.Shapefile2SqlServer_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox fileName;
        private System.Windows.Forms.Button browse;
        private System.Windows.Forms.TextBox connectionString;
        private System.Windows.Forms.Button go;
        private System.Windows.Forms.TextBox tableName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox sdfFileLocation;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button insertRandom;
        private System.Windows.Forms.Button read;
        private System.Windows.Forms.Button goToPostgres;

    }
}