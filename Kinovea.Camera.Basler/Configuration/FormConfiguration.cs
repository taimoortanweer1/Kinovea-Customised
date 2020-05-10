﻿#region License
/*
Copyright © Joan Charmant 2013.
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
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Kinovea.Camera;
using PylonC.NET;
using Kinovea.Services;
using Kinovea.Camera.Languages;
using System.Globalization;

namespace Kinovea.Camera.Basler
{
    public partial class FormConfiguration : Form
    {
        public bool AliasChanged
        {
            get { return iconChanged || tbAlias.Text != summary.Alias;}
        }
        
        public string Alias
        { 
            get { return tbAlias.Text; }
        }
        
        public Bitmap PickedIcon
        { 
            get { return (Bitmap)btnIcon.BackgroundImage; }
        }
        
        public bool SpecificChanged
        {
            get { return specificChanged; }
        }

        public GenApiEnum SelectedStreamFormat
        {
            get { return selectedStreamFormat; }
        }

        public Bayer8Conversion Bayer8Conversion
        {
            get { return bayer8Conversion; }
        }
        
        public Dictionary<string, CameraProperty> CameraProperties
        {
            get { return cameraProperties; }
        } 
        
        private CameraSummary summary;
        private PYLON_DEVICE_HANDLE deviceHandle;
        private bool iconChanged;
        private bool specificChanged;
        private GenApiEnum selectedStreamFormat;
        private Bayer8Conversion bayer8Conversion;
        private Dictionary<string, CameraProperty> cameraProperties = new Dictionary<string, CameraProperty>();
        private Dictionary<string, AbstractCameraPropertyView> propertiesControls = new Dictionary<string, AbstractCameraPropertyView>();
        private Action disconnect;
        private Action connect;
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        
        public FormConfiguration(CameraSummary summary, Action disconnect, Action connect)
        {
            this.summary = summary;
            this.disconnect = disconnect;
            this.connect = connect;

            InitializeComponent();
            tbAlias.AutoSize = false;
            tbAlias.Height = 20;
            
            tbAlias.Text = summary.Alias;
            lblSystemName.Text = summary.Name;
            btnIcon.BackgroundImage = summary.Icon;
            btnReconnect.Text = CameraLang.FormConfiguration_Reconnect;

            SpecificInfo specific = summary.Specific as SpecificInfo;
            if (specific == null || specific.Handle == null || !specific.Handle.IsValid)
                return;

            deviceHandle = specific.Handle;
            cameraProperties = CameraPropertyManager.Read(specific.Handle, summary.Identifier);
            if (cameraProperties.Count != specific.CameraProperties.Count)
                specificChanged = true;

            bayer8Conversion = specific.Bayer8Conversion;

            Populate();
            this.Text = CameraLang.FormConfiguration_Title;
            btnApply.Text = CameraLang.Generic_Apply;
            UpdateResultingFramerate();
        }
        
        private void Populate()
        {
            try
            {
                PopulateStreamFormat();
                PopulateBayerConversion();
                PopulateCameraControls();
            }
            catch
            {
                string error = PylonHelper.GetLastError();
                if (string.IsNullOrEmpty(error))
                    error = "Unknown";

                log.ErrorFormat("Error while populating configuration options. Pylon error: {0}", error);
            }
        }
        
        private void BtnIconClick(object sender, EventArgs e)
        {
            FormIconPicker fip = new FormIconPicker(IconLibrary.Icons, 5);
            FormsHelper.Locate(fip);
            if(fip.ShowDialog() == DialogResult.OK)
            {
                btnIcon.BackgroundImage = fip.PickedIcon;
                iconChanged = true;
            }
            
            fip.Dispose();
        }

        private void PopulateStreamFormat()
        {
            lblColorSpace.Text = CameraLang.FormConfiguration_Properties_StreamFormat;

            bool readable = Pylon.DeviceFeatureIsReadable(deviceHandle, "PixelFormat");
            if (!readable)
            {
                cmbFormat.Enabled = false;
                return;
            }

            List<GenApiEnum> streamFormats = PylonHelper.ReadEnum(deviceHandle, "PixelFormat");
            if (streamFormats == null)
            {
                cmbFormat.Enabled = false;
                return;
            }

            string currentValue = Pylon.DeviceFeatureToString(deviceHandle, "PixelFormat");
            cmbFormat.Items.Clear();

            foreach (GenApiEnum streamFormat in streamFormats)
            {
                cmbFormat.Items.Add(streamFormat);
                if (currentValue == streamFormat.Symbol)
                {
                    selectedStreamFormat = streamFormat;
                    cmbFormat.SelectedIndex = cmbFormat.Items.Count - 1;
                }
            }
        }

        private void PopulateBayerConversion()
        {
            cmbBayer8Conversion.Items.Clear();

            lblBayerConversion.Text = CameraLang.FormConfiguration_Properties_BayerFormatConversion;
            cmbBayer8Conversion.Items.Add("Raw");
            cmbBayer8Conversion.Items.Add("Mono");
            cmbBayer8Conversion.Items.Add("Color");
            cmbBayer8Conversion.SelectedIndex = (int)bayer8Conversion;
            
            SetBayerComboVisibility();
        }

        private void SetBayerComboVisibility()
        {
            EPylonPixelType pixelType = Pylon.PixelTypeFromString(selectedStreamFormat.Symbol);
            bool isBayer8 = PylonHelper.IsBayer8(pixelType);
            cmbBayer8Conversion.Enabled = isBayer8;
            lblBayerConversion.Enabled = isBayer8;
        }

        private void PopulateCameraControls()
        {
            int top = lblAuto.Bottom;
            AddCameraProperty("width", CameraLang.FormConfiguration_Properties_ImageWidth, top);
            AddCameraProperty("height", CameraLang.FormConfiguration_Properties_ImageHeight, top + 30);
            AddCameraProperty("framerate", CameraLang.FormConfiguration_Properties_Framerate, top + 60);
            AddCameraProperty("exposure", CameraLang.FormConfiguration_Properties_ExposureMicro, top + 90);
            AddCameraProperty("gain", CameraLang.FormConfiguration_Properties_Gain, top + 120);
        }

        private void RemoveCameraControls()
        {
            foreach (var pair in propertiesControls)
            {
                pair.Value.ValueChanged -= cpvCameraControl_ValueChanged;
                gbProperties.Controls.Remove(pair.Value);
            }

            propertiesControls.Clear();
        }

        private void AddCameraProperty(string key, string text, int top)
        {
            if (!cameraProperties.ContainsKey(key))
                return;

            CameraProperty property = cameraProperties[key];

            AbstractCameraPropertyView control = null;
             
            switch (property.Representation)
            {
                case CameraPropertyRepresentation.LinearSlider:
                    control = new CameraPropertyViewLinear(property, text, null);
                    break;
                case CameraPropertyRepresentation.LogarithmicSlider:
                    control = new CameraPropertyViewLogarithmic(property, text, null);
                    break;
                case CameraPropertyRepresentation.Checkbox:
                    control = new CameraPropertyViewCheckbox(property, text);
                    break;

                default:
                    break;
            }

            if (control == null)
                return;

            control.Tag = key;
            control.ValueChanged += cpvCameraControl_ValueChanged;
            control.Left = 20;
            control.Top = top;
            gbProperties.Controls.Add(control);
            propertiesControls.Add(key, control);
        }

        private void cpvCameraControl_ValueChanged(object sender, EventArgs e)
        {
            AbstractCameraPropertyView control = sender as AbstractCameraPropertyView;
            if (control == null)
                return;

            string key = control.Tag as string;
            if (string.IsNullOrEmpty(key) || !cameraProperties.ContainsKey(key))
                return;

            CameraPropertyManager.Write(deviceHandle, cameraProperties[key]);
            UpdateResultingFramerate();
            specificChanged = true;
        }
        
        private void cmbFormat_SelectedIndexChanged(object sender, EventArgs e)
        {
            GenApiEnum selected = cmbFormat.SelectedItem as GenApiEnum;
            if (selected == null || selectedStreamFormat.Symbol == selected.Symbol)
                return;

            selectedStreamFormat = selected;
            specificChanged = true;
            UpdateResultingFramerate();
            SetBayerComboVisibility();
        }

        private void cmbBayerConversion_SelectedIndexChanged(object sender, EventArgs e)
        {
            EPylonPixelType pixelType = Pylon.PixelTypeFromString(selectedStreamFormat.Symbol);
            bool isBayer8 = PylonHelper.IsBayer8(pixelType);

            Bayer8Conversion selected = (Bayer8Conversion)cmbBayer8Conversion.SelectedIndex;
            if (selected == bayer8Conversion)
                return;

            bayer8Conversion = (Bayer8Conversion)cmbBayer8Conversion.SelectedIndex;
            specificChanged = true;
            UpdateResultingFramerate();
        }

        private void BtnReconnect_Click(object sender, EventArgs e)
        {
            if (SelectedStreamFormat == null)
            {
                // This happens when we load the config window and the camera isn't connected.
                return;
            }

            SpecificInfo info = summary.Specific as SpecificInfo;
            if (info == null)
                return;

            info.StreamFormat = this.SelectedStreamFormat.Symbol;
            info.CameraProperties = this.CameraProperties;
            summary.UpdateDisplayRectangle(Rectangle.Empty);
            CameraTypeManager.UpdatedCameraSummary(summary);

            disconnect();
            connect();

            SpecificInfo specific = summary.Specific as SpecificInfo;
            if (specific == null || specific.Handle == null || !specific.Handle.IsValid)
                return;

            deviceHandle = specific.Handle;
            cameraProperties = CameraPropertyManager.Read(specific.Handle, summary.Identifier);

            RemoveCameraControls();
            PopulateCameraControls();
            UpdateResultingFramerate();
        }

        private void UpdateResultingFramerate()
        {
            float resultingFramerate = PylonHelper.GetResultingFramerate(deviceHandle);
            lblResultingFramerateValue.Text = string.Format("{0:0.##}", resultingFramerate);

            bool discrepancy = false;
            if (cameraProperties.ContainsKey("framerate") && cameraProperties["framerate"].Supported)
            {
                float framerate;
                bool parsed = float.TryParse(cameraProperties["framerate"].CurrentValue, NumberStyles.Any, CultureInfo.InvariantCulture, out framerate);
                if (parsed && Math.Abs(framerate - resultingFramerate) > 1)
                    discrepancy = true;
            }

            lblResultingFramerateValue.ForeColor = discrepancy ? Color.Red : Color.Black;
        }
    }
}
