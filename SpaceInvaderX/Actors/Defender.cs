using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using SpaceInvaderX.Engine;
using System.Drawing.Drawing2D;

namespace SpaceInvaderX.Actors
{
    public class Defender : Asset
    {
        private Rectangle[] _rectangles;
        private int _dx;
        private int _dy;

        private Bullet _lastBullet;

        public Defender(Stage stage) : base(stage)
        {
            _rectangles = new Rectangle[7];
            _rectangles[0] = new Rectangle(-1, 0, 2, 1);
            _rectangles[1] = new Rectangle(-3, 1, 6, 2);
            _rectangles[2] = new Rectangle(-5, 3, 10, 1);
            _rectangles[3] = new Rectangle(-3, 4, 6, 2);
            _rectangles[4] = new Rectangle(-7, 6, 14, 1);
            _rectangles[5] = new Rectangle(-5, 7, 10, 1);
            _rectangles[6] = new Rectangle(-7, 8, 14, 2);

            _dx = 0;
            _dy = 0;

            _lastBullet = null;

            Stage.KeyDown += Stage_KeyDown;
            stage.KeyUp += Stage_KeyUp;

            Animate();
        }

        public override Region HitBox
        {
            get
            {
                var path = new GraphicsPath();
                var points = new Point[] {
                    new Point(X - 7, Y + 9),
                    new Point(X - 7, Y + 5),
                    new Point(X - 3, Y + 2),
                    new Point(X - 1, Y),
                    new Point(X, Y),
                    new Point(X + 3, Y + 2),
                    new Point(X + 7, Y + 5),
                    new Point(X + 7, Y + 9),
                };
                path.AddPolygon(points);
                return new Region(path);
            }
        }

        public override void Collide(Asset other)
        {
            throw new NotImplementedException();
        }

        public override void Draw(Graphics g)
        {
            foreach (var rect in _rectangles)
            {
                g.FillRectangle(Brushes.Cyan, X + rect.X, Y + rect.Y, rect.Width, rect.Height);
            }
        }

        public void Animate()
        {
            new Thread(() =>
            {
                while (!Dead && !Stage.IsDisposed)
                {
                    if (_dx != 0 || _dy != 0)
                    {
                        X += _dx;
                        Y += _dy;
                        Thread.Sleep(25);
                    }
                    else
                    {
                        Thread.Sleep(5);
                    }
                }
            }).Start();

        }

        private void Stage_KeyUp(object sender, KeyEventArgs e)
        {
            if (!Stage.IsStarted)
            {
                return;
            }

            UpdateAxisSpeeds(e.KeyCode, -1);
        }

        private void Stage_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (!Stage.IsStarted)
            {
                return;
            }

            if (e.KeyCode == Keys.Space)
            {
                Shoot();
            }
            else
            {
                UpdateAxisSpeeds(e.KeyCode, 1);
            }
        }

        private void UpdateAxisSpeeds(Keys keyCode, int dir)
        {
            var speed = 1 * dir;
            switch (keyCode)
            {
                case Keys.D:
                    _dx += speed;
                    break;
                case Keys.W:
                    _dy -= speed;
                    break;
                case Keys.A:
                    _dx -= speed;
                    break;
                case Keys.S:
                    _dy += speed;
                    break;
            }
        }

        private void Shoot()
        {
            if (_lastBullet != null && !_lastBullet.Dead)
            {
                return;
            }

            _lastBullet = Stage.Create<Bullet>();
            _lastBullet.X = X;
            _lastBullet.Y = Y - 1;
            Stage.AddAsset(_lastBullet);
            _lastBullet.Animate();
        }
    }
}
