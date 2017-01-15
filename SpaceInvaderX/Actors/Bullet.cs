using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SpaceInvaderX.Engine;

namespace SpaceInvaderX.Actors
{
    public class Bullet : Asset
    {
        public Bullet(Stage stage) : base(stage)
        {
        }


        public Asset Source { get; set; }

        public override void Animate()
        {
            if (Y > -6)
            {
                Y -= 3;
            }
            else
            {
                Dead = true;
            }
        }

        public override Region HitBox
        {
            get
            {
                var rect = CreateBulletRectangle();
                return new Region(rect);
            }
        }

        public override void Collide(Asset other)
        {
        }

        public override void Draw(Graphics g)
        {
            var rect = CreateBulletRectangle();
            g.FillRectangle(Brushes.OrangeRed, rect);
        }

        private Rectangle CreateBulletRectangle()
        {
            return new Rectangle(X - 1, Y - 6, 2, 6);
        }
    }
}
