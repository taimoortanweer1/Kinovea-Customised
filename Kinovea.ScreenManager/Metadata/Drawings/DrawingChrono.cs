#region License
/*
Copyright � Joan Charmant 2008-2009.
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
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Reflection;
using System.Resources;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;

using Kinovea.ScreenManager.Languages;
using Kinovea.Services;

namespace Kinovea.ScreenManager
{
    [XmlType ("Chrono")]
    public class DrawingChrono : AbstractDrawing, IDecorable, IKvaSerializable
    {
        #region Properties
        public override string ToolDisplayName
        {
            get {  return ScreenManagerLang.ToolTip_DrawingToolChrono; }
        }
        public override int ContentHash
        {
            get 
            { 
                int iHash = visibleTimestamp.GetHashCode();
                iHash ^= invisibleTimestamp.GetHashCode();
                iHash ^= startCountingTimestamp.GetHashCode();
                iHash ^= stopCountingTimestamp.GetHashCode();
                iHash ^= clockOriginTimestamp.GetHashCode();

                iHash ^= styleHelper.ContentHash;
                iHash ^= showLabel.GetHashCode();
    
                return iHash;
            }
        } 
        public DrawingStyle DrawingStyle
        {
            get { return style;}
        }
        public override InfosFading  InfosFading
        {
            // Fading is not modifiable from outside for chrono.
            // The chrono visibility uses its own mechanism.
            get { return null; }
            set { }
        }
        public override DrawingCapabilities Caps
        {
            get { return DrawingCapabilities.ConfigureColorSize | DrawingCapabilities.CopyPaste; }
        }
        public override List<ToolStripItem> ContextMenu
        {
            get
            {
                List<ToolStripItem> contextMenu = new List<ToolStripItem>();

                mnuVisibility.Text = "Visibility";
                mnuHideBefore.Text = "Hide before this point";
                mnuShowBefore.Text = "Show before this point";
                mnuHideAfter.Text = "Hide after this point";
                mnuShowAfter.Text = "Show after this point";

                if (styleHelper.Clock)
                {
                    mnuMarkOrigin.Text = "Mark current time as time origin for this clock";
                    contextMenu.AddRange(new ToolStripItem[] { mnuVisibility, mnuMarkOrigin });
                }
                else
                {
                    mnuStart.Text = ScreenManagerLang.mnuChronoStart;
                    mnuStop.Text = ScreenManagerLang.mnuChronoStop;
                    contextMenu.AddRange(new ToolStripItem[] { mnuVisibility, mnuStart, mnuStop });
                }

                mnuShowLabel.Text = "Show label";
                mnuShowLabel.Checked = showLabel;
                contextMenu.Add(mnuShowLabel);

                return contextMenu;
            }
        }
        public Metadata ParentMetadata
        {
            get { return parentMetadata; }    // unused.
            set { parentMetadata = value; }
        }
        public long TimeStart
        {
            get { return startCountingTimestamp; }
        }
        public long TimeStop
        {
            get { return stopCountingTimestamp; }
        }
        #endregion

        #region Members
        // Core
        private long visibleTimestamp;               	// chrono becomes visible.
        private long invisibleTimestamp;             	// chrono stops being visible.
        private long startCountingTimestamp;         	// chrono starts counting.
        private long stopCountingTimestamp;          	// chrono stops counting. 
        private long clockOriginTimestamp;              // time origin for clock mode.
        private string timecode;
        private bool showLabel;
        // Decoration
        private StyleHelper styleHelper = new StyleHelper();
        private DrawingStyle style;
        private InfosFading infosFading;
        private static readonly int allowedFramesOver = 12;  // Number of frames the chrono stays visible after the 'Hiding' point.
        private RoundedRectangle mainBackground = new RoundedRectangle();
        private RoundedRectangle lblBackground = new RoundedRectangle();

        private ToolStripMenuItem mnuVisibility = new ToolStripMenuItem();
        private ToolStripMenuItem mnuHideBefore = new ToolStripMenuItem();
        private ToolStripMenuItem mnuShowBefore = new ToolStripMenuItem();
        private ToolStripMenuItem mnuHideAfter = new ToolStripMenuItem();
        private ToolStripMenuItem mnuShowAfter = new ToolStripMenuItem();

        private ToolStripMenuItem mnuStart = new ToolStripMenuItem();
        private ToolStripMenuItem mnuStop = new ToolStripMenuItem();
        private ToolStripMenuItem mnuMarkOrigin = new ToolStripMenuItem();

        private ToolStripMenuItem mnuShowLabel = new ToolStripMenuItem();

        private Metadata parentMetadata;
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        #region Constructors
        public DrawingChrono(PointF p, long start, long averageTimeStampsPerFrame, DrawingStyle preset = null)
        {
            // Core
            visibleTimestamp = 0;
            startCountingTimestamp = long.MaxValue;
            stopCountingTimestamp = long.MaxValue;
            invisibleTimestamp = long.MaxValue;
            clockOriginTimestamp = long.MaxValue;
            mainBackground.Rectangle = new RectangleF(p, SizeF.Empty);

            timecode = "error";

            styleHelper.Bicolor = new Bicolor(Color.Black);
            styleHelper.Font = new Font("Arial", 16, FontStyle.Bold);
            styleHelper.Clock = false;
            if (preset == null)
                preset = ToolManager.GetStylePreset("Chrono");
            
            style = preset.Clone();
            BindStyle();
            
            // We use the InfosFading utility to fade the chrono away.
            // The refererence frame will be the frame at which fading start.
            // Must be updated on "Hide" menu.
            infosFading = new InfosFading(invisibleTimestamp, averageTimeStampsPerFrame);
            infosFading.FadingFrames = allowedFramesOver;
            infosFading.UseDefault = false;

            mnuVisibility.Image = Properties.Drawings.persistence;
            mnuShowBefore.Image = Properties.Drawings.showbefore;
            mnuShowAfter.Image = Properties.Drawings.showafter;
            mnuHideBefore.Image = Properties.Drawings.hidebefore;
            mnuHideAfter.Image = Properties.Drawings.hideafter;
            mnuShowBefore.Click += MnuShowBefore_Click;
            mnuShowAfter.Click += MnuShowAfter_Click;
            mnuHideBefore.Click += MnuHideBefore_Click;
            mnuHideAfter.Click += MnuHideAfter_Click;
            mnuVisibility.DropDownItems.AddRange(new ToolStripItem[] { mnuShowBefore, mnuShowAfter, new ToolStripSeparator(),  mnuHideBefore, mnuHideAfter });

            mnuStart.Image = Properties.Drawings.chronostart;
            mnuStop.Image = Properties.Drawings.chronostop;
            mnuMarkOrigin.Image = Properties.Resources.marker;
            mnuStart.Click += mnuStart_Click;
            mnuStop.Click += mnuStop_Click;
            mnuMarkOrigin.Click += mnuMarkOrigin_Click;

            mnuShowLabel.Click += mnuShowLabel_Click;
        }

        public DrawingChrono(XmlReader xmlReader, PointF scale, TimestampMapper timestampMapper, Metadata metadata)
            : this(PointF.Empty, 0, 1, null)
        {
            ReadXml(xmlReader, scale, timestampMapper);
        }
        #endregion

        #region AbstractDrawing Implementation
        public override void Draw(Graphics canvas, DistortionHelper distorter, IImageToViewportTransformer transformer, bool selected, long currentTimestamp)
        {
            if (currentTimestamp < visibleTimestamp)
                return;
            
            double opacityFactor = 1.0;
            if (currentTimestamp > invisibleTimestamp)
                opacityFactor = infosFading.GetOpacityFactor(currentTimestamp);

            if (opacityFactor <= 0)
                return;

            timecode = GetTimecode(currentTimestamp);
            string text = timecode;

            using (SolidBrush brushBack = styleHelper.GetBackgroundBrush((int)(opacityFactor * 128)))
            using (SolidBrush brushText = styleHelper.GetForegroundBrush((int)(opacityFactor * 255)))
            using (Font fontText = styleHelper.GetFont((float)transformer.Scale))
            {
                SizeF textSize = canvas.MeasureString(text, fontText);
                Point bgLocation = transformer.Transform(mainBackground.Rectangle.Location);
                Size bgSize = new Size((int)textSize.Width, (int)textSize.Height);

                SizeF untransformed = transformer.Untransform(textSize);
                mainBackground.Rectangle = new RectangleF(mainBackground.Rectangle.Location, untransformed);

                Rectangle rect = new Rectangle(bgLocation, bgSize);
                int roundingRadius = fontText.Height / 4;
                RoundedRectangle.Draw(canvas, rect, brushBack, roundingRadius, false, false, null);
                canvas.DrawString(text, fontText, brushText, rect.Location);

                if (showLabel && name.Length > 0)
                {
                    using (Font fontLabel = styleHelper.GetFont((float)transformer.Scale * 0.5f))
                    {
                        // Note: the alignment here assumes fixed margins of the rounded rectangle class.
                        SizeF lblTextSize = canvas.MeasureString(name, fontLabel);
                        int labelRoundingRadius = fontLabel.Height / 3;
                        int top = rect.Location.Y - (int)lblTextSize.Height - roundingRadius - labelRoundingRadius;
                        Rectangle lblRect = new Rectangle(rect.Location.X, top, (int)lblTextSize.Width, (int)lblTextSize.Height);
                        RoundedRectangle.Draw(canvas, lblRect, brushBack, labelRoundingRadius, true, false, null);
                        canvas.DrawString(name, fontLabel, brushText, lblRect.Location);
                    }
                }
            }
        }
        public override int HitTest(PointF point, long currentTimestamp, DistortionHelper distorter, IImageToViewportTransformer transformer, bool zooming)
        {
            // Convention: miss = -1, object = 0, handle = n.
            int result = -1;
            long maxHitTimeStamps = invisibleTimestamp;
            if (maxHitTimeStamps != long.MaxValue)
                maxHitTimeStamps += (allowedFramesOver * parentMetadata.AverageTimeStampsPerFrame);

            if (currentTimestamp >= visibleTimestamp && currentTimestamp <= maxHitTimeStamps)
            {
                using (Font fontText = styleHelper.GetFont(1.0f))
                {
                    int roundingRadius = fontText.Height / 4;
                    result = mainBackground.HitTest(point, true, (int)(roundingRadius * 1.8f), transformer);
                }
               
                if(result < 0) 
                    result = lblBackground.HitTest(point, false, 0, transformer);
            }

            return result;
        }
        public override void MoveHandle(PointF point, int handleNumber, Keys modifiers)
        {
            // Invisible handler to change font size.
            int targetHeight = (int)(point.Y - mainBackground.Rectangle.Location.Y);
            StyleElementFontSize elem = style.Elements["font size"] as StyleElementFontSize;
            elem.ForceSize(targetHeight, timecode, styleHelper.Font);
            UpdateLabelRectangle();
        }
        public override void MoveDrawing(float dx, float dy, Keys modifierKeys, bool zooming)
        {
            mainBackground.Move(dx, dy);
            lblBackground.Move(dx, dy);
        }
        public override PointF GetCopyPoint()
        {
            return mainBackground.Rectangle.Center();
        }
        #endregion

        #region KVA Serialization
        public void WriteXml(XmlWriter w, SerializationFilter filter)
        {
            if (ShouldSerializeCore(filter))
            {
                w.WriteElementString("Position", XmlHelper.WritePointF(mainBackground.Rectangle.Location));

                w.WriteStartElement("Values");
                
                w.WriteElementString("Visible", (visibleTimestamp == long.MaxValue) ? "-1" : visibleTimestamp.ToString());
                w.WriteElementString("Invisible", (invisibleTimestamp == long.MaxValue) ? "-1" : invisibleTimestamp.ToString());
                w.WriteElementString("StartCounting", (startCountingTimestamp == long.MaxValue) ? "-1" : startCountingTimestamp.ToString());
                w.WriteElementString("StopCounting", (stopCountingTimestamp == long.MaxValue) ? "-1" : stopCountingTimestamp.ToString());
                w.WriteElementString("ClockOrigin", (clockOriginTimestamp == long.MaxValue) ? "-1" : clockOriginTimestamp.ToString());

                if (ShouldSerializeAll(filter))
                {
                    // Spreadsheet support
                    string userDuration = "0";
                    if (!styleHelper.Clock && startCountingTimestamp != long.MaxValue && stopCountingTimestamp != long.MaxValue)
                        userDuration = parentMetadata.TimeCodeBuilder(stopCountingTimestamp - startCountingTimestamp, TimeType.Absolute, TimecodeFormat.Unknown, false);
                    w.WriteElementString("UserDuration", userDuration);
                }

                // </values>
                w.WriteEndElement();
            }

            if (ShouldSerializeStyle(filter))
            {
                // Label
                w.WriteStartElement("Label");
                w.WriteElementString("Show", showLabel.ToString().ToLower());
                w.WriteEndElement();

                w.WriteStartElement("DrawingStyle");
                style.WriteXml(w);
                w.WriteEndElement();
            }
        }
        public void ReadXml(XmlReader xmlReader, PointF scale, TimestampMapper timestampMapper)
        {
            if (xmlReader.MoveToAttribute("id"))
                identifier = new Guid(xmlReader.ReadContentAsString());

            if (xmlReader.MoveToAttribute("name"))
                name = xmlReader.ReadContentAsString();

            xmlReader.ReadStartElement();
            
            while(xmlReader.NodeType == XmlNodeType.Element)
            {
                switch(xmlReader.Name)
                {
                    case "Position":
                        PointF p = XmlHelper.ParsePointF(xmlReader.ReadElementContentAsString());
                        mainBackground.Rectangle = new RectangleF(p.Scale(scale.X, scale.Y), SizeF.Empty);
                        break;
                    case "Values":
                        ParseWorkingValues(xmlReader, timestampMapper);
                        break;
                    case "DrawingStyle":
                        style = new DrawingStyle(xmlReader);
                        BindStyle();
                        break;
                    case "Label":
                        ParseLabel(xmlReader);
                        break;
                    default:
                        string unparsed = xmlReader.ReadOuterXml();
                        log.DebugFormat("Unparsed content in KVA XML: {0}", unparsed);
                        break;
                }
            }
            
            xmlReader.ReadEndElement();
            SanityCheckValues();
        }
        private void ParseWorkingValues(XmlReader xmlReader, TimestampMapper timestampMapper)
        {
            if(timestampMapper == null)
            {
                xmlReader.ReadOuterXml();
                return;                
            }
            
            xmlReader.ReadStartElement();
            
            while(xmlReader.NodeType == XmlNodeType.Element)
            {
                switch(xmlReader.Name)
                {
                    case "Visible":
                        visibleTimestamp = timestampMapper(xmlReader.ReadElementContentAsLong(), false);
                        break;
                    case "Invisible":
                        long hide = xmlReader.ReadElementContentAsLong();
                        invisibleTimestamp = (hide == -1) ? long.MaxValue : timestampMapper(hide, false);                        
                        break;
                    case "StartCounting":
                        long start = xmlReader.ReadElementContentAsLong(); 
                        startCountingTimestamp = (start == -1) ? long.MaxValue : timestampMapper(start, false);
                        break;
                    case "StopCounting":
                        long stop = xmlReader.ReadElementContentAsLong();
                        stopCountingTimestamp = (stop == -1) ? long.MaxValue : timestampMapper(stop, false);
                        break;
                    case "ClockOrigin":
                        long origin = xmlReader.ReadElementContentAsLong();
                        clockOriginTimestamp = (origin == -1) ? long.MaxValue : timestampMapper(origin, false);
                        break;
                    case "UserDuration":
                        xmlReader.ReadOuterXml();
                        break;
                    default:
                        string unparsed = xmlReader.ReadOuterXml();
                        log.DebugFormat("Unparsed content in KVA XML: {0}", unparsed);
                        break;
                }
            }
            
            xmlReader.ReadEndElement();
        }
        private void SanityCheckValues()
        {
            visibleTimestamp = Math.Max(visibleTimestamp, 0);
            invisibleTimestamp = Math.Max(invisibleTimestamp, 0);
            startCountingTimestamp = Math.Max(startCountingTimestamp, 0);
            stopCountingTimestamp = Math.Max(stopCountingTimestamp, 0);

            if (styleHelper.Clock)
                return;

            if (stopCountingTimestamp < startCountingTimestamp)
                stopCountingTimestamp = long.MaxValue;
        }
        private void ParseLabel(XmlReader xmlReader)
        {
            xmlReader.ReadStartElement();
            
            while(xmlReader.NodeType == XmlNodeType.Element)
            {
                switch(xmlReader.Name)
                {
                    // In older versions chronos used to have a label property. It was later merged with the general name property of drawings.
                    // 0.8.27 should already store the label in the name property.
                    // For older formats we import it here.
                    // The current format doesn't export this "Text" tag at all.
                    case "Text":
                        string label = xmlReader.ReadElementContentAsString();
                        if (!string.IsNullOrEmpty(label) && label != name)
                            name = label;
                        break;
                    case "Show":
                        showLabel = XmlHelper.ParseBoolean(xmlReader.ReadElementContentAsString());
                        break;
                    default:
                        string unparsed = xmlReader.ReadOuterXml();
                        log.DebugFormat("Unparsed content in KVA XML: {0}", unparsed);
                        break;
                }
            }
            
            xmlReader.ReadEndElement();
        }
        #endregion

        #region Specific context menu
        private void MnuShowBefore_Click(object sender, EventArgs e)
        {
            visibleTimestamp = 0;
            InvalidateFromMenu(sender);
        }

        private void MnuShowAfter_Click(object sender, EventArgs e)
        {
            invisibleTimestamp = long.MaxValue;
            infosFading.ReferenceTimestamp = invisibleTimestamp;
            InvalidateFromMenu(sender);
        }

        private void MnuHideBefore_Click(object sender, EventArgs e)
        {
            visibleTimestamp = CurrentTimestampFromMenu(sender);
            InvalidateFromMenu(sender);
        }

        private void MnuHideAfter_Click(object sender, EventArgs e)
        {
            invisibleTimestamp = CurrentTimestampFromMenu(sender);
            infosFading.ReferenceTimestamp = invisibleTimestamp;
            InvalidateFromMenu(sender);
        }

        private void mnuStart_Click(object sender, EventArgs e)
        {
            startCountingTimestamp = CurrentTimestampFromMenu(sender);

            if (stopCountingTimestamp < startCountingTimestamp)
                stopCountingTimestamp = long.MaxValue;

            InvalidateFromMenu(sender);
            UpdateFramesMarkersFromMenu(sender);
        }

        private void mnuStop_Click(object sender, EventArgs e)
        {
            stopCountingTimestamp = CurrentTimestampFromMenu(sender);

            if (stopCountingTimestamp <= startCountingTimestamp)
                startCountingTimestamp = stopCountingTimestamp;

            if (stopCountingTimestamp > invisibleTimestamp)
                invisibleTimestamp = stopCountingTimestamp;

            InvalidateFromMenu(sender);
            UpdateFramesMarkersFromMenu(sender);
        }

        private void mnuMarkOrigin_Click(object sender, EventArgs e)
        {
            clockOriginTimestamp = CurrentTimestampFromMenu(sender);
            InvalidateFromMenu(sender);
        }

        private void mnuShowLabel_Click(object sender, EventArgs e)
        {
            showLabel = !mnuShowLabel.Checked;
            InvalidateFromMenu(sender);
        }
        #endregion

        #region Lower level helpers
        private void BindStyle()
        {
            DrawingStyle.SanityCheck(style, ToolManager.GetStylePreset("Chrono"));
            style.Bind(styleHelper, "Bicolor", "color");
            style.Bind(styleHelper, "Font", "font size");
            style.Bind(styleHelper, "Clock", "clock");
        }
        private void UpdateLabelRectangle()
        {
            using(Font f = styleHelper.GetFont(0.5F))
            {
                SizeF size = TextHelper.MeasureString(name, f);
                lblBackground.Rectangle = new RectangleF(
                    mainBackground.X, mainBackground.Y - lblBackground.Rectangle.Height, size.Width + 11, size.Height);
            }
        }
        private string GetTimecode(long currentTimestamp)
        {
            if (styleHelper.Clock)
            {
                // Relative clock mode: video time relative to origin.
                // Origin is either the drawing-specific origin, or this hasn't been set yet, the video-wide origin.
                if (clockOriginTimestamp == long.MaxValue)
                    return parentMetadata.TimeCodeBuilder(currentTimestamp, TimeType.UserOrigin, TimecodeFormat.Unknown, true);
                else
                    return parentMetadata.TimeCodeBuilder(currentTimestamp - clockOriginTimestamp, TimeType.Absolute, TimecodeFormat.Unknown, true);
            }

            // Stopwatch mode.
            long durationTimestamps;
            if (currentTimestamp <= startCountingTimestamp)
            {
                // Before start. Keep min value.
                durationTimestamps = 0;
            }
            else if (currentTimestamp > stopCountingTimestamp)
            {
                // After stop. Keep max value.
                durationTimestamps = stopCountingTimestamp - startCountingTimestamp;
            }
            else
            {
                durationTimestamps = currentTimestamp - startCountingTimestamp;
            }

            return parentMetadata.TimeCodeBuilder(durationTimestamps, TimeType.Absolute, TimecodeFormat.Unknown, true);
        }
        #endregion
    }
}
