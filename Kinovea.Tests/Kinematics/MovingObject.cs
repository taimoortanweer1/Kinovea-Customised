﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kinovea.Tests.Kinematics
{
    public class MovingObject
    {
        public int Radius { get; set; }
        public double SpeedX { get; set; }
        public double SpeedY { get; set; }
        public double NoiseX { get; set; }
        public double NoiseY { get; set; }

        public MovingObject()
        {
           this.Radius = 5;
        }
    }
}
