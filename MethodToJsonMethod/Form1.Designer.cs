namespace WindowsFormsApplication2
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
            this.button3 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // sourceTextBox
            // 
            this.sourceTextBox.Location = new System.Drawing.Point(76, 54);
            this.sourceTextBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.sourceTextBox.Multiline = true;
            this.sourceTextBox.Name = "sourceTextBox";
            this.sourceTextBox.Size = new System.Drawing.Size(1648, 487);
            this.sourceTextBox.TabIndex = 0;
            this.sourceTextBox.Text = "Navn\r\nPlacering\r\nModul\r\nKostKomponent\r\nKostHjaelpeMat\r\nKostLoen\r\nDb\r\nBruttoPris\r\n" +
    "DaekningsBidrag\r\nMontageMinutter";
            // 
            // destinationTextBox
            // 
            this.destinationTextBox.Location = new System.Drawing.Point(76, 642);
            this.destinationTextBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.destinationTextBox.Multiline = true;
            this.destinationTextBox.Name = "destinationTextBox";
            this.destinationTextBox.Size = new System.Drawing.Size(1648, 487);
            this.destinationTextBox.TabIndex = 0;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(1472, 597);
            this.button1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(112, 35);
            this.button1.TabIndex = 1;
            this.button1.Text = "Generer";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(1578, 583);
            this.button2.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(112, 35);
            this.button2.TabIndex = 2;
            this.button2.Text = "Nulstill";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // buttonAccessors
            // 
            this.buttonAccessors.Location = new System.Drawing.Point(76, 583);
            this.buttonAccessors.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.buttonAccessors.Name = "buttonAccessors";
            this.buttonAccessors.Size = new System.Drawing.Size(112, 35);
            this.buttonAccessors.TabIndex = 3;
            this.buttonAccessors.Text = "Accessors";
            this.buttonAccessors.UseVisualStyleBackColor = true;
            this.buttonAccessors.Click += new System.EventHandler(this.buttonAccessors_Click);
            // 
            // buttonForm
            // 
            this.buttonForm.Location = new System.Drawing.Point(258, 583);
            this.buttonForm.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.buttonForm.Name = "buttonForm";
            this.buttonForm.Size = new System.Drawing.Size(112, 35);
            this.buttonForm.TabIndex = 3;
            this.buttonForm.Text = "Form";
            this.buttonForm.UseVisualStyleBackColor = true;
            this.buttonForm.Click += new System.EventHandler(this.buttonForm_Click);
            // 
            // buttonHtml
            // 
            this.buttonHtml.Location = new System.Drawing.Point(421, 583);
            this.buttonHtml.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.buttonHtml.Name = "buttonHtml";
            this.buttonHtml.Size = new System.Drawing.Size(112, 35);
            this.buttonHtml.TabIndex = 3;
            this.buttonHtml.Text = "HTML";
            this.buttonHtml.UseVisualStyleBackColor = true;
            this.buttonHtml.Click += new System.EventHandler(this.buttonHtml_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(621, 583);
            this.button3.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(164, 35);
            this.button3.TabIndex = 3;
            this.button3.Text = "GetFromDTO";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1827, 1380);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.buttonHtml);
            this.Controls.Add(this.buttonForm);
            this.Controls.Add(this.buttonAccessors);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.destinationTextBox);
            this.Controls.Add(this.sourceTextBox);
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
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
        private System.Windows.Forms.Button button3;
    }
}

