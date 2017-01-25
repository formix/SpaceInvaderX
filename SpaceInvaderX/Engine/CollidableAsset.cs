using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceInvaderX.Engine
{
    public abstract class CollidableAsset : Asset, ICollidable
    {
        public CollidableAsset(Stage stage) : base(stage)
        {
        }

        public abstract void Collide(ICollidable other);
        public abstract GraphicsPath CreateHitBox();
    }
}
