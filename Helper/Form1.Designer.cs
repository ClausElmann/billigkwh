namespace Helper
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
            this.sourceTextBox = new System.Windows.Forms.TextBox();
            this.destinationTextBox = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.buttonAccessors = new System.Windows.Forms.Button();
            this.buttonForm = new System.Windows.Forms.Button();
            this.buttonHtml = new System.Windows.Forms.Button();
            this.textBoxDto = new System.Windows.Forms.TextBox();
            this.buttonAccessorsReflection = new System.Windows.Forms.Button();
            this.buttonFormReflection = new System.Windows.Forms.Button();
            this.buttonHTMLReflection = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // sourceTextBox
            // 
            this.sourceTextBox.Location = new System.Drawing.Point(84, 180);
            this.sourceTextBox.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.sourceTextBox.Multiline = true;
            this.sourceTextBox.Name = "sourceTextBox";
            this.sourceTextBox.Size = new System.Drawing.Size(1831, 496);
            this.sourceTextBox.TabIndex = 0;
            this.sourceTextBox.Text = "Navn\r\nPlacering\r\nModul\r\nKostKomponent\r\nKostHjaelpeMat\r\nKostLoen\r\nDb\r\nBruttoPris\r\n" +
    "DaekningsBidrag\r\nMontageMinutter";
            // 
            // destinationTextBox
            // 
            this.destinationTextBox.Location = new System.Drawing.Point(84, 802);
            this.destinationTextBox.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.destinationTextBox.Multiline = true;
            this.destinationTextBox.Name = "destinationTextBox";
            this.destinationTextBox.Size = new System.Drawing.Size(1831, 608);
            this.destinationTextBox.TabIndex = 0;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(1636, 746);
            this.button1.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(124, 44);
            this.button1.TabIndex = 1;
            this.button1.Text = "Generer";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(1753, 729);
            this.button2.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(124, 44);
            this.button2.TabIndex = 2;
            this.button2.Text = "Nulstill";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // buttonAccessors
            // 
            this.buttonAccessors.Location = new System.Drawing.Point(84, 729);
            this.buttonAccessors.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.buttonAccessors.Name = "buttonAccessors";
            this.buttonAccessors.Size = new System.Drawing.Size(124, 44);
            this.buttonAccessors.TabIndex = 3;
            this.buttonAccessors.Text = "Accessors";
            this.buttonAccessors.UseVisualStyleBackColor = true;
            this.buttonAccessors.Click += new System.EventHandler(this.buttonAccessors_Click);
            // 
            // buttonForm
            // 
            this.buttonForm.Location = new System.Drawing.Point(226, 729);
            this.buttonForm.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.buttonForm.Name = "buttonForm";
            this.buttonForm.Size = new System.Drawing.Size(124, 44);
            this.buttonForm.TabIndex = 3;
            this.buttonForm.Text = "Form";
            this.buttonForm.UseVisualStyleBackColor = true;
            this.buttonForm.Click += new System.EventHandler(this.buttonForm_Click);
            // 
            // buttonHtml
            // 
            this.buttonHtml.Location = new System.Drawing.Point(370, 729);
            this.buttonHtml.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.buttonHtml.Name = "buttonHtml";
            this.buttonHtml.Size = new System.Drawing.Size(124, 44);
            this.buttonHtml.TabIndex = 3;
            this.buttonHtml.Text = "HTML";
            this.buttonHtml.UseVisualStyleBackColor = true;
            this.buttonHtml.Click += new System.EventHandler(this.buttonHtml_Click);
            // 
            // textBoxDto
            // 
            this.textBoxDto.Location = new System.Drawing.Point(84, 15);
            this.textBoxDto.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.textBoxDto.Multiline = true;
            this.textBoxDto.Name = "textBoxDto";
            this.textBoxDto.Size = new System.Drawing.Size(249, 59);
            this.textBoxDto.TabIndex = 0;
            this.textBoxDto.Text = "ElTavleDto";
            // 
            // buttonAccessorsReflection
            // 
            this.buttonAccessorsReflection.Location = new System.Drawing.Point(383, 15);
            this.buttonAccessorsReflection.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.buttonAccessorsReflection.Name = "buttonAccessorsReflection";
            this.buttonAccessorsReflection.Size = new System.Drawing.Size(124, 44);
            this.buttonAccessorsReflection.TabIndex = 3;
            this.buttonAccessorsReflection.Text = "Accessors";
            this.buttonAccessorsReflection.UseVisualStyleBackColor = true;
            this.buttonAccessorsReflection.Click += new System.EventHandler(this.buttonAccessorsReflection_Click);
            // 
            // buttonFormReflection
            // 
            this.buttonFormReflection.Location = new System.Drawing.Point(533, 15);
            this.buttonFormReflection.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.buttonFormReflection.Name = "buttonFormReflection";
            this.buttonFormReflection.Size = new System.Drawing.Size(124, 44);
            this.buttonFormReflection.TabIndex = 3;
            this.buttonFormReflection.Text = "Form";
            this.buttonFormReflection.UseVisualStyleBackColor = true;
            this.buttonFormReflection.Click += new System.EventHandler(this.buttonFormReflection_Click);
            // 
            // buttonHTMLReflection
            // 
            this.buttonHTMLReflection.Location = new System.Drawing.Point(687, 15);
            this.buttonHTMLReflection.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.buttonHTMLReflection.Name = "buttonHTMLReflection";
            this.buttonHTMLReflection.Size = new System.Drawing.Size(124, 44);
            this.buttonHTMLReflection.TabIndex = 3;
            this.buttonHTMLReflection.Text = "HTML";
            this.buttonHTMLReflection.UseVisualStyleBackColor = true;
            this.buttonHTMLReflection.Click += new System.EventHandler(this.buttonHTMLReflection_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(2030, 1725);
            this.Controls.Add(this.buttonHTMLReflection);
            this.Controls.Add(this.buttonHtml);
            this.Controls.Add(this.buttonFormReflection);
            this.Controls.Add(this.buttonAccessorsReflection);
            this.Controls.Add(this.buttonForm);
            this.Controls.Add(this.buttonAccessors);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.destinationTextBox);
            this.Controls.Add(this.textBoxDto);
            this.Controls.Add(this.sourceTextBox);
            this.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox sourceTextBox;
        private System.Windows.Forms.TextBox destinationTextBox;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button buttonAccessors;
        private System.Windows.Forms.Button buttonForm;
        private System.Windows.Forms.Button buttonHtml;
        private System.Windows.Forms.TextBox textBoxDto;
        private System.Windows.Forms.Button buttonAccessorsReflection;
        private System.Windows.Forms.Button buttonFormReflection;
        private System.Windows.Forms.Button buttonHTMLReflection;
    }
}

