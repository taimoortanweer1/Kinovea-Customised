﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using Kinovea.Services;
using System.IO;
using System.Globalization;
using Kinovea.ScreenManager.Languages;

namespace Kinovea.ScreenManager
{
    public partial class FormTrackAnalysis : Form
    {
        private Metadata metadata;
        private TrajectoryKinematics kinematics;
        private Color color;
        private PlotHelper plotHelper;
        private bool manualUpdate;
        private PlotObject currentPlot;

        public FormTrackAnalysis(Metadata metadata, DrawingTrack track)
        {
            this.metadata = metadata;
            this.kinematics = track.TrajectoryKinematics;
            this.color = track.MainColor;

            InitializeComponent();
            
            plotHelper = new PlotHelper(plotView);
            Localize();
            CreatePlots();
        }

        private void Localize()
        {
            Text = ScreenManagerLang.DataAnalysis;
            pagePlot.Text = ScreenManagerLang.DataAnalysis_PagePlot;
            gbSource.Text = ScreenManagerLang.DataAnalysis_DataSource;
            lblData.Text = ScreenManagerLang.DataAnalysis_DataLabel;
            //cmbDataSource

            gbLabels.Text = ScreenManagerLang.DataAnalysis_Labels;
            lblTitle.Text = ScreenManagerLang.DataAnalysis_Title;
            lblXAxis.Text = ScreenManagerLang.DataAnalysis_XaxisLabel;
            lblYAxis.Text = ScreenManagerLang.DataAnalysis_YaxisLabel;
            tbTitle.Text = ScreenManagerLang.DataAnalysis_ScatterPlot;
            //tbXAxis.Text = ScreenManagerLang.;
            //tbYAxis.Text = "Y axis";

            gbExportGraph.Text = ScreenManagerLang.DataAnalysis_ExportGraph;
            lblPixels.Text = ScreenManagerLang.DataAnalysis_Pixels;
            btnImageCopy.Text = ScreenManagerLang.mnuCopyToClipboard;
            btnExportGraph.Text = ScreenManagerLang.DataAnalysis_SaveToFile;

            gbExportData.Text = ScreenManagerLang.DataAnalysis_ExportData;
            btnDataCopy.Text = ScreenManagerLang.mnuCopyToClipboard;
            btnExportData.Text = ScreenManagerLang.DataAnalysis_SaveToFile;

            lblCutoffFrequencies.Text = "Selected cutoff frequencies:";

            if (kinematics.CanFilter)
            {
                LocalizeInfo();
                CreateDurbinWatsonPlot();
            }
            else
            {
                tabControl.TabPages.Remove(pageAbout);
            }
        }

        private void LocalizeInfo()
        {
            Font fontHeader = new Font("Microsoft Sans Serif", 9, FontStyle.Bold);
            Font fontText = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular);

            rtbInfo1.BackColor = Color.White;
            rtbInfo1.SelectionFont = fontHeader;
            rtbInfo1.AppendText("Filtering\n");

            rtbInfo1.SelectionFont = fontText;
            StringBuilder b = new StringBuilder();
            b.AppendLine("The digitized coordinates are passed through a low pass filter to remove noise. The filter does two passes of a second-order Butterworth filter. The two passes (one forward, one backward) are used to reset the phase shift (Winter, 2009).");
            b.AppendLine("To initialize the filter the trajectory is extrapolated for 10 data points each side using reflected values around the end points. The extrapolated points are then removed from the filtered results (Smith, 1989).");
            rtbInfo1.AppendText(b.ToString());
    
            rtbInfo1.SelectionFont = fontHeader;
            rtbInfo1.AppendText("\nCutoff frequency\n");

            rtbInfo1.SelectionFont = fontText;
            b = new StringBuilder();
            b.AppendLine("The filter is tested on the data at various cutoff frequencies between 0.5Hz and the Nyquist frequency.");
            b.AppendLine("The best cutoff frequency is computed by estimating the autocorrelation of residuals and finding the frequency yielding the residuals that are the least autocorrelated. The filtered data set corresponding to this cutoff frequency is kept as the final result (Challis, 1999).");
            b.AppendLine("The autocorrelation of residuals is estimated using the Durbin-Watson statistic.");
            rtbInfo1.AppendText(b.ToString());

            rtbInfo2.BackColor = Color.White;
            rtbInfo2.SelectionFont = fontHeader;
            rtbInfo2.AppendText("\nReferences\n");
            rtbInfo2.SelectionFont = fontText;
            b = new StringBuilder();
            b.AppendLine("1. Smith G. (1989). Padding point extrapolation techniques for the butterworth digital filter. J. Biomech. Vol. 22, No. s/9, pp. 967-971.");
            b.AppendLine("2. Challis J. (1999). A procedure for the automatic determination of filter cutoff frequency for the processing of biomechanical data., Journal of Applied Biomechanics, Volume 15, Issue 3.");
            b.AppendLine("3. Winter, D. A. (2009). Biomechanics and motor control of human movements (4th ed.). Hoboken, New Jersey: John Wiley & Sons, Inc.");
            rtbInfo2.AppendText(b.ToString());
        }

        private void CreateDurbinWatsonPlot()
        {
            if (kinematics.Xs == null || kinematics.Ys == null)
            {
                plotDurbinWatson.Visible = false;
                return;
            }

            PlotModel model = new PlotModel();
            model.TitleFontSize = 12;
            model.Title = "Residuals autocorrelation";

            LinearAxis xAxis = new LinearAxis();
            xAxis.Position = AxisPosition.Bottom;
            xAxis.MajorGridlineStyle = LineStyle.Solid;
            xAxis.MinorGridlineStyle = LineStyle.Dot;
            xAxis.Title = "Cutoff frequency (Hz)";
            xAxis.TitleFontSize = 10;
            model.Axes.Add(xAxis);

            LinearAxis yAxis = new LinearAxis();
            yAxis.Position = AxisPosition.Left;
            yAxis.MajorGridlineStyle = LineStyle.Solid;
            yAxis.MinorGridlineStyle = LineStyle.Dot;
            yAxis.Title = "Autocorrelation (norm.)";
            yAxis.TitleFontSize = 10; 
            model.Axes.Add(yAxis);

            LineSeries xseries = new LineSeries();
            xseries.Color = OxyColors.Green;
            xseries.MarkerType = MarkerType.None;
            xseries.Smooth = true;


            LineSeries yseries = new LineSeries();
            yseries.Color = OxyColors.Tomato;
            yseries.MarkerType = MarkerType.None;
            yseries.Smooth = true;
            
            foreach (FilteringResult r in kinematics.FilterResultXs)
                xseries.Points.Add(new DataPoint(r.CutoffFrequency, r.DurbinWatson));

            foreach (FilteringResult r in kinematics.FilterResultYs)
                yseries.Points.Add(new DataPoint(r.CutoffFrequency, r.DurbinWatson));

            model.Series.Add(xseries);
            model.Series.Add(yseries);
            plotDurbinWatson.Model = model;
            plotDurbinWatson.BackColor = Color.White;

            lblCutoffX.Text = string.Format("X: {0:0.000} Hz", kinematics.FilterResultXs[kinematics.XCutoffIndex].CutoffFrequency);
            lblCutoffY.Text = string.Format("Y: {0:0.000} Hz", kinematics.FilterResultYs[kinematics.YCutoffIndex].CutoffFrequency);
        }

        private void CreatePlots()
        {
            // TODO: localize title and time.

            string d = metadata.CalibrationHelper.GetLengthAbbreviation();
            string v = metadata.CalibrationHelper.GetSpeedAbbreviation();
            string a = metadata.CalibrationHelper.GetAccelerationAbbreviation();
            string da = metadata.CalibrationHelper.GetAngleAbbreviation();
            string va = metadata.CalibrationHelper.GetAngularVelocityAbbreviation();

            AddPlot(kinematics.Xs, ScreenManagerLang.dlgConfigureTrajectory_ExtraData_HorizontalPosition, d, false);
            AddPlot(kinematics.Ys, ScreenManagerLang.dlgConfigureTrajectory_ExtraData_VerticalPosition, d, false);
            AddPlot(kinematics.TotalDistance, ScreenManagerLang.dlgConfigureTrajectory_ExtraData_TotalDistance, d, false);
            AddPlot(kinematics.Speed, ScreenManagerLang.dlgConfigureTrajectory_ExtraData_Speed, v, true);
            AddPlot(kinematics.HorizontalVelocity, ScreenManagerLang.dlgConfigureTrajectory_ExtraData_HorizontalVelocity, v, false);
            AddPlot(kinematics.VerticalVelocity, ScreenManagerLang.dlgConfigureTrajectory_ExtraData_VerticalPosition, v, false);
            AddPlot(kinematics.Acceleration, ScreenManagerLang.dlgConfigureTrajectory_ExtraData_Acceleration, a, false);
            AddPlot(kinematics.HorizontalAcceleration, ScreenManagerLang.dlgConfigureTrajectory_ExtraData_HorizontalAcceleration, a, false);
            AddPlot(kinematics.VerticalAcceleration, ScreenManagerLang.dlgConfigureTrajectory_ExtraData_VerticalAcceleration, a, false);
            AddPlot(kinematics.DisplacementAngle, ScreenManagerLang.dlgConfigureTrajectory_ExtraData_AngularDisplacement, da, false);
            AddPlot(kinematics.AngularVelocity, ScreenManagerLang.dlgConfigureTrajectory_ExtraData_AngularVelocity, va, false);
            //AddPlot(kinematics.AngularAcceleration, ScreenManagerLang.dlgConfigureTrajectory_ExtraData_AngularAcceleration, false);
            //AddPlot(kinematics.CentripetalAcceleration, ScreenManagerLang.dlgConfigureTrajectory_ExtraData_CentripetalAcceleration, false);
        }

        private void AddPlot(double[] data, string title, string abbreviation, bool selected)
        {
            string t = ScreenManagerLang.DataAnalysis_TimeAxisMilliseconds;
            PlotModel model = CreatePlot(data, title, abbreviation, t);

            cmbDataSource.Items.Add(new PlotObject(title, model, data, abbreviation));
            if (selected)
                cmbDataSource.SelectedItem = cmbDataSource.Items[cmbDataSource.Items.Count-1];
        }
        
        private PlotModel CreatePlot(double[] data, string title, string abbreviation, string xLegend)
        {
            if (data == null)
                return null;

            PlotModel model = new PlotModel();
            model.PlotType = PlotType.XY;

            model.Title = title;

            LinearAxis xAxis = new LinearAxis();
            xAxis.Position = AxisPosition.Bottom;
            xAxis.MajorGridlineStyle = LineStyle.Solid;
            xAxis.MinorGridlineStyle = LineStyle.Dot;
            xAxis.MinimumPadding = 0.02;
            xAxis.MaximumPadding = 0.05;
            xAxis.Title = xLegend;
            model.Axes.Add(xAxis);

            LinearAxis yAxis = new LinearAxis();
            yAxis.Position = AxisPosition.Left;
            yAxis.MajorGridlineStyle = LineStyle.Solid;
            yAxis.MinorGridlineStyle = LineStyle.Dot;
            yAxis.MinimumPadding = 0.05;
            yAxis.MaximumPadding = 0.1;
            yAxis.Title = string.Format("{0} ({1})", title, abbreviation);
            model.Axes.Add(yAxis);

            LineSeries series = new LineSeries();
            series.Title = title;
            series.Color = OxyColor.FromArgb(color.A, color.R, color.G, color.B);
            series.MarkerType = MarkerType.None;
            series.Smooth = true;

            for (int i = 0; i < data.Length; i++)
            {
                double value = data[i];
                double time = TimestampToMilliseconds(kinematics.Times[i]);
                if (!double.IsNaN(value))
                    series.Points.Add(new DataPoint(time, value));
            }

            model.Series.Add(series);
            return model;
        }

        private double TimestampToMilliseconds(long ts)
        {
            long relative = ts - metadata.SelectionStart;
            double seconds = (double)relative / metadata.AverageTimeStampsPerSecond;
            double milliseconds = (seconds * 1000) / metadata.HighSpeedFactor;
            return milliseconds;
        }

        private void LabelsChanged(object sender, EventArgs e)
        {
            if (manualUpdate || plotView == null || plotView.Model == null || plotView.Model.Axes.Count != 2)
                return;

            plotView.Model.Title = tbTitle.Text;
            plotView.Model.Axes[0].Title = tbXAxis.Text;
            plotView.Model.Axes[1].Title = tbYAxis.Text;

            plotView.InvalidatePlot(false);
        }

        private void btnExportGraph_Click(object sender, EventArgs e)
        {
            plotHelper.ExportGraph((int)nudWidth.Value, (int)nudHeight.Value);
        }

        private void btnImageCopy_Click(object sender, EventArgs e)
        {
            plotHelper.CopyGraph((int)nudWidth.Value, (int)nudHeight.Value);
        }

        private void cmbDataSource_SelectedIndexChanged(object sender, EventArgs e)
        {
            PlotObject p = cmbDataSource.SelectedItem as PlotObject;
            if (p == null)
                return;

            manualUpdate = true;

            plotView.Model = p.PlotModel;
            currentPlot = p;
            tbTitle.Text = plotView.Model.Title;
            tbXAxis.Text = plotView.Model.Axes[0].Title;
            tbYAxis.Text = plotView.Model.Axes[1].Title;

            manualUpdate = false;
        }

        private void btnDataCopy_Click(object sender, EventArgs e)
        {
            StringBuilder b = new StringBuilder();
            string separator = CultureInfo.CurrentCulture.TextInfo.ListSeparator;

            b.AppendLine(string.Format("{0}{1}{2}", currentPlot.PlotModel.Axes[0].Title, separator, currentPlot.PlotModel.Axes[1].Title));

            double[] data = currentPlot.Data;
            for (int i = 0; i < data.Length; i++)
            {
                double value = Math.Round(data[i], 3);
                double time = Math.Round(TimestampToMilliseconds(kinematics.Times[i]));
                if (!double.IsNaN(value))
                    b.AppendLine(string.Format("{0}{1}{2}", time, separator, value));
            }

            string text = b.ToString();
            Clipboard.SetText(text);
        }

        private void btnExportData_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Title = ScreenManagerLang.DataAnalysis_ExportData;
            saveFileDialog.Filter = "Comma Separated Values (*.csv)|*.csv";
            saveFileDialog.FilterIndex = 1;
            saveFileDialog.RestoreDirectory = true;

            if (saveFileDialog.ShowDialog() != DialogResult.OK || string.IsNullOrEmpty(saveFileDialog.FileName))
                return;

            string separator = CultureInfo.CurrentCulture.TextInfo.ListSeparator;

            using (StreamWriter w = File.CreateText(saveFileDialog.FileName))
            {
                w.WriteLine(string.Format("{0}{1}{2}", currentPlot.PlotModel.Axes[0].Title, separator, currentPlot.PlotModel.Axes[1].Title));

                double[] data = currentPlot.Data;
                for (int i = 0; i < data.Length; i++)
                {
                    double value = Math.Round(data[i], 3);
                    double time = Math.Round(TimestampToMilliseconds(kinematics.Times[i]));
                    if (!double.IsNaN(value))
                        w.WriteLine(string.Format("{0}{1}{2}", time, separator, value));
                }
            }
        }

        private class PlotObject
        {
            public string Label { get; private set; }
            public PlotModel PlotModel { get; private set; }
            public double[] Data { get; private set; }
            public string Abbreviation { get; private set; }
            public PlotObject(string label, PlotModel plotModel, double[] data, string abbreviation)
            {
                this.Label = label;
                this.PlotModel = plotModel;
                this.Data = data;
                this.Abbreviation = abbreviation;
            }

            public override string ToString()
            {
                return Label;
            }
        }
    }
}
