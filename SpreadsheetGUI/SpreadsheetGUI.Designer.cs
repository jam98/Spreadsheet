using System.Windows.Forms;

namespace SpreadsheetGUI
{
    partial class SSGUI
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
            this.SSPanel = new SS.SpreadsheetPanel();
            this.ButtonStrip = new System.Windows.Forms.MenuStrip();
            this.FileButton = new System.Windows.Forms.ToolStripMenuItem();
            this.New = new System.Windows.Forms.ToolStripMenuItem();
            this.Open = new System.Windows.Forms.ToolStripMenuItem();
            this.Save = new System.Windows.Forms.ToolStripMenuItem();
            this.SaveAsButton = new System.Windows.Forms.ToolStripMenuItem();
            this.Close = new System.Windows.Forms.ToolStripMenuItem();
            this.HelpButton = new System.Windows.Forms.ToolStripMenuItem();
            this.ContentsTextbox = new System.Windows.Forms.ToolStripTextBox();
            this.ValueTextbox = new System.Windows.Forms.ToolStripTextBox();
            this.CellTextbox = new System.Windows.Forms.ToolStripTextBox();
            this.OpenDialog = new System.Windows.Forms.OpenFileDialog();
            this.SaveAsDialog = new System.Windows.Forms.SaveFileDialog();
            this.ButtonStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // SSPanel
            // 
            this.SSPanel.AutoSize = true;
            this.SSPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.SSPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SSPanel.Location = new System.Drawing.Point(0, 27);
            this.SSPanel.Name = "SSPanel";
            this.SSPanel.Size = new System.Drawing.Size(768, 492);
            this.SSPanel.TabIndex = 0;
            this.SSPanel.SelectionChanged += new SS.SelectionChangedHandler(this.DisplaySelection);
            // 
            // ButtonStrip
            // 
            this.ButtonStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.FileButton,
            this.HelpButton,
            this.ContentsTextbox,
            this.ValueTextbox,
            this.CellTextbox});
            this.ButtonStrip.Location = new System.Drawing.Point(0, 0);
            this.ButtonStrip.Name = "ButtonStrip";
            this.ButtonStrip.Size = new System.Drawing.Size(768, 27);
            this.ButtonStrip.TabIndex = 1;
            // 
            // FileButton
            // 
            this.FileButton.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.New,
            this.Open,
            this.Save,
            this.SaveAsButton,
            this.Close});
            this.FileButton.Name = "FileButton";
            this.FileButton.Size = new System.Drawing.Size(37, 23);
            this.FileButton.Text = "File";
            // 
            // New
            // 
            this.New.Name = "New";
            this.New.Size = new System.Drawing.Size(152, 22);
            this.New.Text = "New";
            this.New.Click += new System.EventHandler(this.LaunchNewSpreadsheet);
            // 
            // Open
            // 
            this.Open.Name = "Open";
            this.Open.Size = new System.Drawing.Size(152, 22);
            this.Open.Text = "Open";
            this.Open.Click += new System.EventHandler(this.OpenClick);
            // 
            // Save
            // 
            this.Save.Name = "Save";
            this.Save.Size = new System.Drawing.Size(152, 22);
            this.Save.Text = "Save";
            this.Save.Click += new System.EventHandler(this.SaveClick);
            // 
            // SaveAsButton
            // 
            this.SaveAsButton.Name = "SaveAsButton";
            this.SaveAsButton.Size = new System.Drawing.Size(152, 22);
            this.SaveAsButton.Text = "Save As...";
            this.SaveAsButton.Click += new System.EventHandler(this.SaveAsClick);
            // 
            // Close
            // 
            this.Close.Name = "Close";
            this.Close.Size = new System.Drawing.Size(152, 22);
            this.Close.Text = "Close";
            this.Close.Click += new System.EventHandler(this.CloseSheet);
            // 
            // HelpButton
            // 
            this.HelpButton.Name = "HelpButton";
            this.HelpButton.Size = new System.Drawing.Size(44, 23);
            this.HelpButton.Text = "Help";
            this.HelpButton.Click += new System.EventHandler(this.ShowHelp);
            // 
            // ContentsTextbox
            // 
            this.ContentsTextbox.AcceptsReturn = true;
            this.ContentsTextbox.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.ContentsTextbox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ContentsTextbox.MergeIndex = 3;
            this.ContentsTextbox.Name = "ContentsTextbox";
            this.ContentsTextbox.Size = new System.Drawing.Size(300, 23);
            this.ContentsTextbox.Text = "Contents";
            this.ContentsTextbox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.ContentTextInput);
            // 
            // ValueTextbox
            // 
            this.ValueTextbox.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.ValueTextbox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ValueTextbox.MergeIndex = 2;
            this.ValueTextbox.Name = "ValueTextbox";
            this.ValueTextbox.ReadOnly = true;
            this.ValueTextbox.Size = new System.Drawing.Size(150, 23);
            this.ValueTextbox.Text = "Value";
            this.ValueTextbox.TextBoxTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // CellTextbox
            // 
            this.CellTextbox.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.CellTextbox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.CellTextbox.Name = "CellTextbox";
            this.CellTextbox.ReadOnly = true;
            this.CellTextbox.Size = new System.Drawing.Size(50, 23);
            this.CellTextbox.Text = "Cell";
            this.CellTextbox.TextBoxTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // OpenDialog
            // 
            this.OpenDialog.DefaultExt = "ss";
            this.OpenDialog.RestoreDirectory = true;
            this.OpenDialog.InitialDirectory = Application.StartupPath;

            // 
            // SaveAsDialog
            // 
            this.SaveAsDialog.DefaultExt = "ss";
            this.SaveAsDialog.Filter = "Spreadsheet files|*.ss|All files|*.*";
            this.SaveAsDialog.InitialDirectory = Application.StartupPath;
            // 
            // SSGUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(768, 519);
            this.Controls.Add(this.SSPanel);
            this.Controls.Add(this.ButtonStrip);
            this.MainMenuStrip = this.ButtonStrip;
            this.Name = "SSGUI";
            this.Text = "Spreadsheet";
            this.ButtonStrip.ResumeLayout(false);
            this.ButtonStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private SS.SpreadsheetPanel SSPanel;
        private System.Windows.Forms.MenuStrip ButtonStrip;
        private System.Windows.Forms.ToolStripMenuItem FileButton;
        private System.Windows.Forms.ToolStripMenuItem New;
        private System.Windows.Forms.ToolStripMenuItem Save;
        private System.Windows.Forms.ToolStripMenuItem Open;
        private System.Windows.Forms.ToolStripMenuItem Close;
        private System.Windows.Forms.ToolStripMenuItem HelpButton;
        private System.Windows.Forms.ToolStripTextBox CellTextbox;
        private System.Windows.Forms.ToolStripTextBox ValueTextbox;
        private System.Windows.Forms.ToolStripTextBox ContentsTextbox;
        private System.Windows.Forms.ToolStripMenuItem SaveAsButton;
        private System.Windows.Forms.OpenFileDialog OpenDialog;
        private System.Windows.Forms.SaveFileDialog SaveAsDialog;
    }
}

