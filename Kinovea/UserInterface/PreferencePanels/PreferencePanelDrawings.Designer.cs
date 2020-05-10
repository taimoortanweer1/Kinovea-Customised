#region License
/*
Copyright � Joan Charmant 2011.
jcharmant@gmail.com 
 
This file is part of Kinovea.

Kinovea is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License version 2 
as published by the Free Software Foundation.

Kinovea is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with Kinovea. If not, see http://www.gnu.org/licenses/.
*/
#endregion
namespace Kinovea.Root
{
	partial class PreferencePanelDrawings
	{
		/// <summary>
		/// Designer variable used to keep track of non-visual components.
		/// </summary>
		private System.ComponentModel.IContainer components = null;
		
		/// <summary>
		/// Disposes resources used by the control.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing) {
				if (components != null) {
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}
		
		/// <summary>
		/// This method is required for Windows Forms designer support.
		/// Do not change the method contents inside the source code editor. The Forms designer might
		/// not be able to load this method if it was changed manually.
		/// </summary>
		private void InitializeComponent()
		{
      this.tabSubPages = new System.Windows.Forms.TabControl();
      this.tabGeneral = new System.Windows.Forms.TabPage();
      this.chkCustomToolsDebug = new System.Windows.Forms.CheckBox();
      this.chkEnableFiltering = new System.Windows.Forms.CheckBox();
      this.chkDrawOnPlay = new System.Windows.Forms.CheckBox();
      this.tabPersistence = new System.Windows.Forms.TabPage();
      this.lblDefaultOpacity = new System.Windows.Forms.Label();
      this.tabTracking = new System.Windows.Forms.TabPage();
      this.lblDescription = new System.Windows.Forms.Label();
      this.cmbSearchWindowUnit = new System.Windows.Forms.ComboBox();
      this.cmbBlockWindowUnit = new System.Windows.Forms.ComboBox();
      this.tbSearchHeight = new System.Windows.Forms.TextBox();
      this.label5 = new System.Windows.Forms.Label();
      this.tbSearchWidth = new System.Windows.Forms.TextBox();
      this.tbBlockHeight = new System.Windows.Forms.TextBox();
      this.lblSearchWindow = new System.Windows.Forms.Label();
      this.label4 = new System.Windows.Forms.Label();
      this.tbBlockWidth = new System.Windows.Forms.TextBox();
      this.lblObjectWindow = new System.Windows.Forms.Label();
      this.nudFading = new System.Windows.Forms.NumericUpDown();
      this.nudOpaque = new System.Windows.Forms.NumericUpDown();
      this.nudMax = new System.Windows.Forms.NumericUpDown();
      this.lblOpaque = new System.Windows.Forms.Label();
      this.lblFading = new System.Windows.Forms.Label();
      this.lblMax = new System.Windows.Forms.Label();
      this.chkAlwaysVisible = new System.Windows.Forms.CheckBox();
      this.tabSubPages.SuspendLayout();
      this.tabGeneral.SuspendLayout();
      this.tabPersistence.SuspendLayout();
      this.tabTracking.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.nudFading)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.nudOpaque)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.nudMax)).BeginInit();
      this.SuspendLayout();
      // 
      // tabSubPages
      // 
      this.tabSubPages.Controls.Add(this.tabGeneral);
      this.tabSubPages.Controls.Add(this.tabPersistence);
      this.tabSubPages.Controls.Add(this.tabTracking);
      this.tabSubPages.Dock = System.Windows.Forms.DockStyle.Fill;
      this.tabSubPages.Location = new System.Drawing.Point(0, 0);
      this.tabSubPages.Name = "tabSubPages";
      this.tabSubPages.SelectedIndex = 0;
      this.tabSubPages.Size = new System.Drawing.Size(490, 322);
      this.tabSubPages.TabIndex = 0;
      // 
      // tabGeneral
      // 
      this.tabGeneral.Controls.Add(this.chkCustomToolsDebug);
      this.tabGeneral.Controls.Add(this.chkEnableFiltering);
      this.tabGeneral.Controls.Add(this.chkDrawOnPlay);
      this.tabGeneral.Location = new System.Drawing.Point(4, 22);
      this.tabGeneral.Name = "tabGeneral";
      this.tabGeneral.Padding = new System.Windows.Forms.Padding(3);
      this.tabGeneral.Size = new System.Drawing.Size(482, 296);
      this.tabGeneral.TabIndex = 0;
      this.tabGeneral.Text = "General";
      this.tabGeneral.UseVisualStyleBackColor = true;
      // 
      // chkCustomToolsDebug
      // 
      this.chkCustomToolsDebug.AutoSize = true;
      this.chkCustomToolsDebug.Location = new System.Drawing.Point(27, 95);
      this.chkCustomToolsDebug.Name = "chkCustomToolsDebug";
      this.chkCustomToolsDebug.Size = new System.Drawing.Size(148, 17);
      this.chkCustomToolsDebug.TabIndex = 54;
      this.chkCustomToolsDebug.Text = "Custom tools debug mode";
      this.chkCustomToolsDebug.UseVisualStyleBackColor = true;
      this.chkCustomToolsDebug.CheckedChanged += new System.EventHandler(this.chkCustomToolsDebug_CheckedChanged);
      // 
      // chkEnableFiltering
      // 
      this.chkEnableFiltering.AutoSize = true;
      this.chkEnableFiltering.Location = new System.Drawing.Point(27, 62);
      this.chkEnableFiltering.Name = "chkEnableFiltering";
      this.chkEnableFiltering.Size = new System.Drawing.Size(153, 17);
      this.chkEnableFiltering.TabIndex = 53;
      this.chkEnableFiltering.Text = "Enable coordinates filtering";
      this.chkEnableFiltering.UseVisualStyleBackColor = true;
      this.chkEnableFiltering.CheckedChanged += new System.EventHandler(this.chkEnableFiltering_CheckedChanged);
      // 
      // chkDrawOnPlay
      // 
      this.chkDrawOnPlay.AutoSize = true;
      this.chkDrawOnPlay.Location = new System.Drawing.Point(27, 27);
      this.chkDrawOnPlay.Name = "chkDrawOnPlay";
      this.chkDrawOnPlay.Size = new System.Drawing.Size(202, 17);
      this.chkDrawOnPlay.TabIndex = 52;
      this.chkDrawOnPlay.Text = "Show drawings when video is playing";
      this.chkDrawOnPlay.UseVisualStyleBackColor = true;
      this.chkDrawOnPlay.CheckedChanged += new System.EventHandler(this.chkDrawOnPlay_CheckedChanged);
      // 
      // tabPersistence
      // 
      this.tabPersistence.Controls.Add(this.chkAlwaysVisible);
      this.tabPersistence.Controls.Add(this.nudFading);
      this.tabPersistence.Controls.Add(this.nudOpaque);
      this.tabPersistence.Controls.Add(this.nudMax);
      this.tabPersistence.Controls.Add(this.lblOpaque);
      this.tabPersistence.Controls.Add(this.lblFading);
      this.tabPersistence.Controls.Add(this.lblMax);
      this.tabPersistence.Controls.Add(this.lblDefaultOpacity);
      this.tabPersistence.Location = new System.Drawing.Point(4, 22);
      this.tabPersistence.Name = "tabPersistence";
      this.tabPersistence.Padding = new System.Windows.Forms.Padding(3);
      this.tabPersistence.Size = new System.Drawing.Size(482, 296);
      this.tabPersistence.TabIndex = 1;
      this.tabPersistence.Text = "Opacity";
      this.tabPersistence.UseVisualStyleBackColor = true;
      // 
      // lblDefaultOpacity
      // 
      this.lblDefaultOpacity.Location = new System.Drawing.Point(20, 18);
      this.lblDefaultOpacity.Name = "lblDefaultOpacity";
      this.lblDefaultOpacity.Size = new System.Drawing.Size(362, 32);
      this.lblDefaultOpacity.TabIndex = 56;
      this.lblDefaultOpacity.Text = "Default opacity of new drawings:";
      this.lblDefaultOpacity.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // tabTracking
      // 
      this.tabTracking.Controls.Add(this.lblDescription);
      this.tabTracking.Controls.Add(this.cmbSearchWindowUnit);
      this.tabTracking.Controls.Add(this.cmbBlockWindowUnit);
      this.tabTracking.Controls.Add(this.tbSearchHeight);
      this.tabTracking.Controls.Add(this.label5);
      this.tabTracking.Controls.Add(this.tbSearchWidth);
      this.tabTracking.Controls.Add(this.tbBlockHeight);
      this.tabTracking.Controls.Add(this.lblSearchWindow);
      this.tabTracking.Controls.Add(this.label4);
      this.tabTracking.Controls.Add(this.tbBlockWidth);
      this.tabTracking.Controls.Add(this.lblObjectWindow);
      this.tabTracking.Location = new System.Drawing.Point(4, 22);
      this.tabTracking.Name = "tabTracking";
      this.tabTracking.Padding = new System.Windows.Forms.Padding(3);
      this.tabTracking.Size = new System.Drawing.Size(482, 296);
      this.tabTracking.TabIndex = 2;
      this.tabTracking.Text = "Tracking";
      this.tabTracking.UseVisualStyleBackColor = true;
      // 
      // lblDescription
      // 
      this.lblDescription.AutoSize = true;
      this.lblDescription.Location = new System.Drawing.Point(16, 13);
      this.lblDescription.Name = "lblDescription";
      this.lblDescription.Size = new System.Drawing.Size(137, 13);
      this.lblDescription.TabIndex = 67;
      this.lblDescription.Text = "Default tracking parameters";
      // 
      // cmbSearchWindowUnit
      // 
      this.cmbSearchWindowUnit.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.cmbSearchWindowUnit.Location = new System.Drawing.Point(288, 72);
      this.cmbSearchWindowUnit.Name = "cmbSearchWindowUnit";
      this.cmbSearchWindowUnit.Size = new System.Drawing.Size(116, 21);
      this.cmbSearchWindowUnit.TabIndex = 66;
      this.cmbSearchWindowUnit.SelectedIndexChanged += new System.EventHandler(this.cmbSearchWindowUnit_SelectedIndexChanged);
      // 
      // cmbBlockWindowUnit
      // 
      this.cmbBlockWindowUnit.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.cmbBlockWindowUnit.Location = new System.Drawing.Point(288, 45);
      this.cmbBlockWindowUnit.Name = "cmbBlockWindowUnit";
      this.cmbBlockWindowUnit.Size = new System.Drawing.Size(116, 21);
      this.cmbBlockWindowUnit.TabIndex = 65;
      this.cmbBlockWindowUnit.SelectedIndexChanged += new System.EventHandler(this.cmbBlockWindowUnit_SelectedIndexChanged);
      // 
      // tbSearchHeight
      // 
      this.tbSearchHeight.Location = new System.Drawing.Point(241, 72);
      this.tbSearchHeight.Name = "tbSearchHeight";
      this.tbSearchHeight.Size = new System.Drawing.Size(30, 20);
      this.tbSearchHeight.TabIndex = 62;
      this.tbSearchHeight.TextChanged += new System.EventHandler(this.tbSearchHeight_TextChanged);
      // 
      // label5
      // 
      this.label5.AutoSize = true;
      this.label5.Location = new System.Drawing.Point(222, 75);
      this.label5.Name = "label5";
      this.label5.Size = new System.Drawing.Size(13, 13);
      this.label5.TabIndex = 61;
      this.label5.Text = "�";
      // 
      // tbSearchWidth
      // 
      this.tbSearchWidth.Location = new System.Drawing.Point(182, 72);
      this.tbSearchWidth.Name = "tbSearchWidth";
      this.tbSearchWidth.Size = new System.Drawing.Size(30, 20);
      this.tbSearchWidth.TabIndex = 60;
      this.tbSearchWidth.TextChanged += new System.EventHandler(this.tbSearchWidth_TextChanged);
      // 
      // tbBlockHeight
      // 
      this.tbBlockHeight.Location = new System.Drawing.Point(241, 46);
      this.tbBlockHeight.Name = "tbBlockHeight";
      this.tbBlockHeight.Size = new System.Drawing.Size(30, 20);
      this.tbBlockHeight.TabIndex = 59;
      this.tbBlockHeight.TextChanged += new System.EventHandler(this.tbBlockHeight_TextChanged);
      // 
      // lblSearchWindow
      // 
      this.lblSearchWindow.AutoSize = true;
      this.lblSearchWindow.Location = new System.Drawing.Point(52, 76);
      this.lblSearchWindow.Name = "lblSearchWindow";
      this.lblSearchWindow.Size = new System.Drawing.Size(86, 13);
      this.lblSearchWindow.TabIndex = 58;
      this.lblSearchWindow.Text = "Search window :";
      // 
      // label4
      // 
      this.label4.AutoSize = true;
      this.label4.Location = new System.Drawing.Point(222, 49);
      this.label4.Name = "label4";
      this.label4.Size = new System.Drawing.Size(13, 13);
      this.label4.TabIndex = 57;
      this.label4.Text = "�";
      // 
      // tbBlockWidth
      // 
      this.tbBlockWidth.Location = new System.Drawing.Point(182, 46);
      this.tbBlockWidth.Name = "tbBlockWidth";
      this.tbBlockWidth.Size = new System.Drawing.Size(30, 20);
      this.tbBlockWidth.TabIndex = 55;
      this.tbBlockWidth.TextChanged += new System.EventHandler(this.tbBlockWidth_TextChanged);
      // 
      // lblObjectWindow
      // 
      this.lblObjectWindow.AutoSize = true;
      this.lblObjectWindow.Location = new System.Drawing.Point(52, 49);
      this.lblObjectWindow.Name = "lblObjectWindow";
      this.lblObjectWindow.Size = new System.Drawing.Size(83, 13);
      this.lblObjectWindow.TabIndex = 56;
      this.lblObjectWindow.Text = "Object window :";
      // 
      // nudFading
      // 
      this.nudFading.Location = new System.Drawing.Point(264, 167);
      this.nudFading.Maximum = new decimal(new int[] {
            250,
            0,
            0,
            0});
      this.nudFading.Name = "nudFading";
      this.nudFading.Size = new System.Drawing.Size(52, 20);
      this.nudFading.TabIndex = 64;
      this.nudFading.ValueChanged += new System.EventHandler(this.nudFading_ValueChanged);
      // 
      // nudOpaque
      // 
      this.nudOpaque.Location = new System.Drawing.Point(264, 132);
      this.nudOpaque.Maximum = new decimal(new int[] {
            500,
            0,
            0,
            0});
      this.nudOpaque.Name = "nudOpaque";
      this.nudOpaque.Size = new System.Drawing.Size(52, 20);
      this.nudOpaque.TabIndex = 63;
      this.nudOpaque.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
      this.nudOpaque.ValueChanged += new System.EventHandler(this.nudOpaque_ValueChanged);
      // 
      // nudMax
      // 
      this.nudMax.Location = new System.Drawing.Point(264, 98);
      this.nudMax.Name = "nudMax";
      this.nudMax.Size = new System.Drawing.Size(52, 20);
      this.nudMax.TabIndex = 62;
      this.nudMax.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
      this.nudMax.ValueChanged += new System.EventHandler(this.nudMax_ValueChanged);
      // 
      // lblOpaque
      // 
      this.lblOpaque.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.lblOpaque.Location = new System.Drawing.Point(53, 128);
      this.lblOpaque.Name = "lblOpaque";
      this.lblOpaque.Size = new System.Drawing.Size(194, 25);
      this.lblOpaque.TabIndex = 61;
      this.lblOpaque.Text = "Opaque duration (frames):";
      this.lblOpaque.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // lblFading
      // 
      this.lblFading.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.lblFading.Location = new System.Drawing.Point(53, 163);
      this.lblFading.Name = "lblFading";
      this.lblFading.Size = new System.Drawing.Size(153, 25);
      this.lblFading.TabIndex = 60;
      this.lblFading.Text = "Fading duration (frames):";
      this.lblFading.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // lblMax
      // 
      this.lblMax.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.lblMax.Location = new System.Drawing.Point(53, 94);
      this.lblMax.Name = "lblMax";
      this.lblMax.Size = new System.Drawing.Size(153, 25);
      this.lblMax.TabIndex = 59;
      this.lblMax.Text = "Maximum opacity (%):";
      this.lblMax.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // chkAlwaysVisible
      // 
      this.chkAlwaysVisible.AutoSize = true;
      this.chkAlwaysVisible.Location = new System.Drawing.Point(56, 65);
      this.chkAlwaysVisible.Name = "chkAlwaysVisible";
      this.chkAlwaysVisible.Size = new System.Drawing.Size(91, 17);
      this.chkAlwaysVisible.TabIndex = 65;
      this.chkAlwaysVisible.Text = "Always visible";
      this.chkAlwaysVisible.UseVisualStyleBackColor = true;
      this.chkAlwaysVisible.CheckedChanged += new System.EventHandler(this.chkAlwaysVisible_CheckedChanged);
      // 
      // PreferencePanelDrawings
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.tabSubPages);
      this.Name = "PreferencePanelDrawings";
      this.Size = new System.Drawing.Size(490, 322);
      this.tabSubPages.ResumeLayout(false);
      this.tabGeneral.ResumeLayout(false);
      this.tabGeneral.PerformLayout();
      this.tabPersistence.ResumeLayout(false);
      this.tabPersistence.PerformLayout();
      this.tabTracking.ResumeLayout(false);
      this.tabTracking.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.nudFading)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.nudOpaque)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.nudMax)).EndInit();
      this.ResumeLayout(false);

		}
		private System.Windows.Forms.TabControl tabSubPages;
		private System.Windows.Forms.TabPage tabGeneral;
		private System.Windows.Forms.TabPage tabPersistence;
		private System.Windows.Forms.CheckBox chkDrawOnPlay;
        private System.Windows.Forms.TabPage tabTracking;
        private System.Windows.Forms.TextBox tbSearchHeight;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox tbSearchWidth;
        private System.Windows.Forms.TextBox tbBlockHeight;
        private System.Windows.Forms.Label lblSearchWindow;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox tbBlockWidth;
        private System.Windows.Forms.Label lblObjectWindow;
        private System.Windows.Forms.ComboBox cmbSearchWindowUnit;
        private System.Windows.Forms.ComboBox cmbBlockWindowUnit;
        private System.Windows.Forms.Label lblDescription;
        private System.Windows.Forms.Label lblDefaultOpacity;
        private System.Windows.Forms.CheckBox chkEnableFiltering;
        private System.Windows.Forms.CheckBox chkCustomToolsDebug;
        private System.Windows.Forms.CheckBox chkAlwaysVisible;
        private System.Windows.Forms.NumericUpDown nudFading;
        private System.Windows.Forms.NumericUpDown nudOpaque;
        private System.Windows.Forms.NumericUpDown nudMax;
        private System.Windows.Forms.Label lblOpaque;
        private System.Windows.Forms.Label lblFading;
        private System.Windows.Forms.Label lblMax;
    }
}
