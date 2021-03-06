﻿#region License
/*
Copyright © Joan Charmant 2009.
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
using System.Drawing.Imaging;
using System.Reflection;
using System.Resources;
using System.Threading;
using System.Windows.Forms;

using AForge.Imaging;
using AForge.Imaging.Filters;
using Kinovea.ScreenManager.Languages;
using Kinovea.Services;

namespace Kinovea.ScreenManager
{
    public class VideoFilterAutoLevels : AdjustmentFilter
    {
        #region Properties
        public override string Name {
            get { return ScreenManagerLang.VideoFilterAutoLevels_FriendlyName; }
        }
        public override Bitmap Icon {
            get { return Properties.Resources.chart_bar; }
        }
        public override ImageProcessor ImageProcessor {
            get { return ProcessSingleImage; }
        }
        #endregion
        
        private void ProcessSingleImage(Bitmap source)
        {
            ImageStatistics stats = new ImageStatistics(source);
            LevelsLinear levelsLinear = new LevelsLinear {
                InRed = stats.Red.GetRange( 0.87 ),
                InGreen = stats.Green.GetRange( 0.87 ),
                InBlue  = stats.Blue.GetRange( 0.87 )
            };
            
            levelsLinear.ApplyInPlace(source);
        }
    }
}

