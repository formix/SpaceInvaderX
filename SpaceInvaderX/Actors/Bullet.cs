using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SpaceInvaderX.Engine;
using System.Drawing.Drawing2D;

namespace SpaceInvaderX.Actors
{
    public class Bullet : CollidableAsset
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

        public override GraphicsPath CreateHitBox()
        {
            var rect = CreateBulletRectangle();
            var path = new GraphicsPath();
            path.AddRectangle(rect);
            return path;
        }

        public override void Collide(ICollidable other)
        {
            Dead = true;
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
