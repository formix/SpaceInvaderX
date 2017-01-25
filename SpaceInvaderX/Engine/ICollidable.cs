using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceInvaderX.Engine
{
    public interface ICollidable
    {
        GraphicsPath CreateHitBox();
        void Collide(ICollidable other);
    }
}
