﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceInvaderX.Engine
{
    public abstract class Asset
    {
        public Stage Stage { get; private set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; }
        public bool Dead { get; set; }


        public Asset(Stage stage)
        {
            Stage = stage;
        }

        public virtual void Animate()
        {
        }

        public abstract void Draw(Graphics g);
    }
}
