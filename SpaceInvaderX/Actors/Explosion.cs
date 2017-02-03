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


        public Explosion(Stage stage) : base(stage)
        {
            _mutex = new object();
            _timer = new Timer(state => UpdateStep(), null, 0, 20);
            _step = 0;
        }

        public override void Draw(Graphics g)
        {
            var radius = 0f;
            lock(_mutex)
            {
                radius = (_step + 1) * 1.25f;
                if (Dead)
                {
                    return;
                }
            }
            g.FillEllipse(Brushes.Red, X - radius, Y - radius, radius * 2, radius * 2);
        }

        private void UpdateStep()
        {
            lock(_mutex)
            {
                if (_step == 5)
                {
                    _timer.Dispose();
                    Dead = true;
                    return;
                }
                _step++;
            }
        }
    }
}
