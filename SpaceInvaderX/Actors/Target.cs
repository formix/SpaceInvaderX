using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpaceInvaderX.Engine;

namespace SpaceInvaderX.Actors
{
    public class Target : Asset
    {
        public Target(Stage stage) : base(stage)
        {
        }


        public override Region HitBox
        {
            get
            {
                var rect = CreateRect();
                return new Region(rect);
            }
        }

        public override void Collide(Asset other)
        {
            Dead = true;
        }

        public override void Draw(Graphics g)
        {
            var rect = CreateRect();
            g.FillRectangle(Brushes.Yellow, rect);
        }

        private Rectangle CreateRect()
        {
            return new Rectangle(X, Y, 10, 10);
        }
    }
}
