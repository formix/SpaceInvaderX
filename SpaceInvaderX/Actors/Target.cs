using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpaceInvaderX.Engine;
using System.Drawing.Drawing2D;

namespace SpaceInvaderX.Actors
{
    public class Target : CollidableAsset
    {
        public Target(Stage stage) : base(stage)
        {
        }

        public override void Draw(Graphics g)
        {
            var rect = CreateRect();
            g.FillRectangle(Brushes.Yellow, rect);
        }

        public override void Collide(ICollidable other)
        {
            var explosion = Stage.Create<Explosion>();
            explosion.X = X + 5;
            explosion.Y = Y + 5;
            explosion.Color = Color.Orange;
            Stage.AddAsset(explosion);
            Dispose();
        }

        public override GraphicsPath CreateHitBox()
        {
            var rect = CreateRect();
            var path = new GraphicsPath();
            path.AddRectangle(rect);
            return path;
        }

        private RectangleF CreateRect()
        {
            return new RectangleF(X, Y, 10, 10);
        }
    }
}
