using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SpaceInvaderX.Engine
{
    public abstract class Asset : IDisposable
    {
        public Vector3 Position;

        public Asset(Stage stage)
        {
            Stage = stage;
            IsDisposed = false;
            Position = Vector3.Zero;
        }

        ~Asset()
        {
            Dispose(false);
        }

        public Stage Stage { get; private set; }
        public bool IsDisposed { get; private set; }

        public float X {
            get
            {
                return Position.X;
            }
            set
            {
                Position.X = value;
            }
        }

        public float Y
        {
            get
            {
                return Position.Y;
            }
            set
            {
                Position.Y = value;
            }
        }

        public float Z
        {
            get
            {
                return Position.Z;
            }
            set
            {
                Position.Z = value;
            }
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
