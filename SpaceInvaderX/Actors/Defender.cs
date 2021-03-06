﻿using System;
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
    public class Defender : CollidableAsset
    {
        private Rectangle[] _rectangles;
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

            Speed = 1.33f;

            _lastBullet = null;
        }

        public float Speed { get; set; }

        public override GraphicsPath CreateHitBox()
        {
            var path = new GraphicsPath();
            var points = new PointF[] {
                new PointF(X - 7, Y + 9),
                new PointF(X - 7, Y + 5),
                new PointF(X - 3, Y + 2),
                new PointF(X - 1, Y),
                new PointF(X, Y),
                new PointF(X + 3, Y + 2),
                new PointF(X + 7, Y + 5),
                new PointF(X + 7, Y + 9),
            };
            path.AddPolygon(points);
            return path;
        }

        public override void Collide(ICollidable other)
        {
            if (other is Bullet)
            {
                if (((Bullet)other).Source == this)
                {
                    return;
                }
            }

            var explosion = Stage.Create<Explosion>();
            explosion.X = X;
            explosion.Y = Y + 5;
            explosion.Color = Color.DarkBlue;
            Stage.AddAsset(explosion);

            Dispose();
        }

        public override void Draw(Graphics g)
        {
            foreach (var rect in _rectangles)
            {
                g.FillRectangle(Brushes.Cyan, X + rect.X, Y + rect.Y, rect.Width, rect.Height);
            }
        }

        public override void Animate()
        {
            if (!IsDisposed)
            {
                if (Stage.GetKeyState(Keys.Space) == KeyStates.Down)
                {
                    Shoot();
                }

                if (Stage.Frame % 2 != 0)
                {
                    return;
                }

                if (Stage.GetKeyState(Keys.A) == KeyStates.Down)
                {
                    X -= Speed;
                }
                if (Stage.GetKeyState(Keys.D) == KeyStates.Down)
                {
                    X += Speed;
                }
                if (Stage.GetKeyState(Keys.W) == KeyStates.Down)
                {
                    Y -= Speed;
                }
                if (Stage.GetKeyState(Keys.S) == KeyStates.Down)
                {
                    Y += Speed;
                }
            }
        }


        private void Shoot()
        {
            if (_lastBullet != null && !_lastBullet.IsDisposed)
            {
                return;
            }

            _lastBullet = Stage.Create<Bullet>();
            _lastBullet.Source = this;
            _lastBullet.X = X;
            _lastBullet.Y = Y;
            Stage.AddAsset(_lastBullet);
        }
    }
}
