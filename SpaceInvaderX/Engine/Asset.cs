using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceInvaderX.Engine
{
    public abstract class Asset : IDisposable
    {
        public Stage Stage { get; private set; }
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }
        public bool IsDisposed { get; private set; }


        public Asset(Stage stage)
        {
            Stage = stage;
            IsDisposed = false;
        }

        ~Asset()
        {
            Dispose(false);
        }

        public virtual void Animate()
        {
        }

        public abstract void Draw(Graphics g);

        protected virtual void Dispose(bool disposing)
        {
            IsDisposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
        }
    }
}
