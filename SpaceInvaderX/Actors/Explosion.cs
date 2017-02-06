using SpaceInvaderX.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Threading;

namespace SpaceInvaderX.Actors
{
    public class Explosion : Asset
    {
        private object _mutex;
        private Timer _timer;
        private int _step;
        private Brush _brush;


        public Explosion(Stage stage) : base(stage)
        {
            _mutex = new object();
            _timer = new Timer(state => UpdateStep(), null, 5, 20);
            _step = 0;
            _brush = new SolidBrush(Color.OrangeRed);
        }

        public Color Color
        {
            get { return ((SolidBrush)_brush).Color; }
            set
            {
                _brush.Dispose();
                _brush = new SolidBrush(value);
            }
        }

        public override void Draw(Graphics g)
        {
            var radius = 0f;
            lock(_mutex)
            {
                radius = (_step + 1) * 1.25f;
                if (IsDisposed)
                {
                    return;
                }
            }
            g.FillEllipse(_brush, X - radius, Y - radius, radius * 2, radius * 2);
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _brush.Dispose();
            }
            base.Dispose(disposing);
        }

        private void UpdateStep()
        {
            lock(_mutex)
            {
                if (_step == 5)
                {
                    _timer.Dispose();
                    Dispose();
                    return;
                }
                _step++;
            }
        }
    }
}
