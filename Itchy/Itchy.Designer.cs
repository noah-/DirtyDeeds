﻿namespace Itchy
{
    partial class Itchy
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Itchy));
            this.attachButton = new System.Windows.Forms.Button();
            this.detachButton = new System.Windows.Forms.Button();
            this.clientsComboBox = new System.Windows.Forms.ComboBox();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.statusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.testButton = new System.Windows.Forms.Button();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // attachButton
            // 
            this.attachButton.Location = new System.Drawing.Point(12, 12);
            this.attachButton.Name = "attachButton";
            this.attachButton.Size = new System.Drawing.Size(75, 23);
            this.attachButton.TabIndex = 0;
            this.attachButton.Text = "Attach";
            this.attachButton.UseVisualStyleBackColor = true;
            this.attachButton.Click += new System.EventHandler(this.attachButton_Click);
            // 
            // detachButton
            // 
            this.detachButton.Location = new System.Drawing.Point(93, 12);
            this.detachButton.Name = "detachButton";
            this.detachButton.Size = new System.Drawing.Size(75, 23);
            this.detachButton.TabIndex = 3;
            this.detachButton.Text = "Detach";
            this.detachButton.UseVisualStyleBackColor = true;
            this.detachButton.Click += new System.EventHandler(this.detachButton_Click);
            // 
            // clientsComboBox
            // 
            this.clientsComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.clientsComboBox.FormattingEnabled = true;
            this.clientsComboBox.Location = new System.Drawing.Point(258, 12);
            this.clientsComboBox.Name = "clientsComboBox";
            this.clientsComboBox.Size = new System.Drawing.Size(121, 21);
            this.clientsComboBox.TabIndex = 4;
            this.clientsComboBox.DropDown += new System.EventHandler(this.clientsComboBox_DropDown);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusLabel});
            this.statusStrip1.Location = new System.Drawing.Point(0, 240);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(391, 22);
            this.statusStrip1.TabIndex = 5;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // statusLabel
            // 
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(0, 17);
            // 
            // testButton
            // 
            this.testButton.Location = new System.Drawing.Point(12, 58);
            this.testButton.Name = "testButton";
            this.testButton.Size = new System.Drawing.Size(75, 23);
            this.testButton.TabIndex = 6;
            this.testButton.Text = "Test";
            this.testButton.UseVisualStyleBackColor = true;
            this.testButton.Click += new System.EventHandler(this.testButton_Click);
            // 
            // Itchy
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(391, 262);
            this.Controls.Add(this.testButton);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.clientsComboBox);
            this.Controls.Add(this.detachButton);
            this.Controls.Add(this.attachButton);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Itchy";
            this.Text = "Itchy";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Itchy_FormClosing);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button attachButton;
        private System.Windows.Forms.Button detachButton;
        private System.Windows.Forms.ComboBox clientsComboBox;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel statusLabel;
        private System.Windows.Forms.Button testButton;
    }
}

