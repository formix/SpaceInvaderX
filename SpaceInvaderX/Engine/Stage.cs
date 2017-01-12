using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace SpaceInvaderX.Engine
{
    public partial class Stage : UserControl
    {
        private Dictionary<Keys, KeyStates> _keyStates;

        private Bitmap _stageView;
        private Graphics _stageGraphics;
        private long _frameCount;
        private HashSet<Asset> _collidable;

        public Stage()
        {
            InitializeComponent();
            DoubleBuffered = true;
            _keyStates = new Dictionary<Keys, KeyStates>();
            _stageView = new Bitmap(320, 240);
            _stageGraphics = Graphics.FromImage(_stageView);
            _frameCount = 0;
            _collidable = new HashSet<Asset>();
            Assets = new List<Asset>();
            IsStarted = false;
        }

        public long FrameCount
        {
            get
            {
                return _frameCount;
            }
        }

        public List<Asset> Assets { get; private set; }

        public bool IsStarted { get; set; }

        public void Start()
        {
            _frameCount = 0;
            IsStarted = true;
            StartAnimationLoop();
            StartDeadBodyCollectionLoop();
        }


        public IEnumerable<Asset> GetCollidables()
        {
            lock (_collidable)
            {
                return new LinkedList<Asset>(_collidable);
            }
        }

        private void StartAnimationLoop()
        {
            Task.Run(() =>
            {
                while (IsStarted)
                {
                    var begin = DateTime.Now;
                    UpdateStageView();
                    Invalidate();
                    var duration = DateTime.Now - begin;
                    var sleepTime = (int)(duration.TotalMilliseconds < 15 ? 15 - duration.TotalMilliseconds : 0);
                    Thread.Sleep(sleepTime);
                    _frameCount++;
                }
            });
        }

        private void StartDeadBodyCollectionLoop()
        {
            Task.Run(() =>
            {
                while (IsStarted)
                {
                    Thread.Sleep(1000);

                    lock (Assets)
                    {
                        var livingAssets = new List<Asset>();
                        foreach (var asset in Assets)
                        {
                            if (!asset.Dead)
                            {
                                livingAssets.Add(asset);
                            }
                            else
                            {
                                // Calling remove if the asset is not 
                                // present do not break.
                                lock (_collidable)
                                {
                                    _collidable.Remove(asset);
                                }
                            }
                        }
                        Assets = livingAssets;
                    }
                }
            });
        }

        public void Stop()
        {
            IsStarted = false;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            lock (_stageView)
            {
                e.Graphics.DrawImage(_stageView,
                    new Rectangle(0, 0, Width, Height),
                    new Rectangle(0, 0, _stageView.Width, _stageView.Height),
                    GraphicsUnit.Pixel);
            }
        }

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            Stop();
            _stageGraphics.Dispose();
            base.Dispose(disposing);
        }

        public void AddAsset(Asset asset)
        {
            lock (Assets)
            {
                Assets.Add(asset);
                if (asset.HitBox != null)
                {
                    lock (_collidable)
                    {
                        _collidable.Add(asset);
                    }
                }
            }
        }

        private void UpdateStageView()
        {
            lock (_stageView)
            {
                _stageGraphics.FillRectangle(Brushes.Black, 0, 0, _stageView.Width, _stageView.Height);
                lock (Assets)
                {
                    foreach (var asset in Assets)
                    {
                        if (!asset.Dead)
                        {
                            asset.Draw(_stageGraphics);
                        }
                    }
                }
            }
        }

        public T Create<T>() where T: Asset
        {
            var type = typeof(T);
            var ctor = type.GetConstructor(new Type[] { typeof(Stage) });
            var asset = (Asset)ctor.Invoke(new object[] { this });
            return (T)asset;
        }


        public KeyStates GetKeyState(Keys keyCode)
        {
            if (!_keyStates.ContainsKey(keyCode))
            {
                return KeyStates.Up;
            }
            return _keyStates[keyCode];
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (!_keyStates.ContainsKey(e.KeyCode) || _keyStates[e.KeyCode] == KeyStates.Up)
            {
                _keyStates[e.KeyCode] = KeyStates.Down;
                base.OnKeyDown(e);
            }
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            _keyStates[e.KeyCode] = KeyStates.Up;
            base.OnKeyUp(e);
        }
    }
}
