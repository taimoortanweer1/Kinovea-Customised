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
using System.Drawing;

namespace Kinovea.ScreenManager
{
    public class ViewerSelectorOption
    {
        public Bitmap Image { get; private set;}
        public string Text { get; private set;}
        public object Data { get; private set;}
        
        public ViewerSelectorOption(Bitmap image, string text, object data)
        {
            this.Image = image;
            this.Text = text;
            this.Data = data;
        }
    }
}
