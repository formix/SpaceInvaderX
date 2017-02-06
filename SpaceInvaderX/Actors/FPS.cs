using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SpaceInvaderX.Engine;

namespace SpaceInvaderX.Actors
{
    public class FPS : Asset
    {
        private long _lastFrames;
        private Timer _timer;
        private string _fps;

        public FPS(Stage stage) : base(stage)
        {
            _fps = "0.0 fps";
            _timer = new Timer(s => UpdateFps(), null, 5000, 5000);
            _lastFrames = 0;
        }

        public override void Draw(Graphics g)
        {
            g.DrawString(_fps, SystemFonts.DefaultFont, Brushes.Green, X, Y);
        }

        private void UpdateFps()
        {
            var fps = (Stage.Frame - _lastFrames) / 5.0;
            _fps = $"{fps:0.0} fps";
            _lastFrames = Stage.Frame;
        }
    }
}
