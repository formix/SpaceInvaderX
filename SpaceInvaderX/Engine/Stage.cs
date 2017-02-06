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
using System.Drawing.Drawing2D;

namespace SpaceInvaderX.Engine
{
    public partial class Stage : UserControl
    {
        private Dictionary<Keys, KeyStates> _keyStates;

        private Bitmap _stageView;
        private Graphics _stageGraphics;
        private long _frame;
        private ICollection<Asset> _assets;
        private ICollection<Asset> _newAssets;

        public Stage()
        {
            InitializeComponent();
            DoubleBuffered = true;
            _keyStates = new Dictionary<Keys, KeyStates>();
            _stageView = new Bitmap(320, 240);
            _stageGraphics = Graphics.FromImage(_stageView);
            _frame = 0;
            _assets = new HashSet<Asset>();
            _newAssets = new LinkedList<Asset>();
            FrameDuration = 15;
            IsStarted = false;
        }

        public long Frame
        {
            get
            {
                return _frame;
            }
        }

        public int FrameDuration { get; set; }

        public bool IsStarted { get; set; }

        public void Start()
        {
            _frame = 0;
            IsStarted = true;
            StartAnimationLoop();
        }


        private void StartAnimationLoop()
        {
            Task.Run(() =>
            {
                while (IsStarted)
                {
                    var begin = DateTime.Now;
                    CleanUp();
                    CheckCollisions();
                    AnimateAssets();
                    ImportNewAssets();
                    UpdateStageView();
                    var elapsedTime = DateTime.Now - begin;
                    int sleepTime = ComputeSleepTime(elapsedTime);
                    Thread.Sleep(sleepTime);
                    Invalidate();
                    _frame++;
                }
            });
        }

        private void CheckCollisions()
        {
            var collidables = _assets
                .Where(a => a is ICollidable)
                .Select(a => (CollidableAsset)a)
                .AsEnumerable();
            CheckCollisions(collidables);
        }

        private void CheckCollisions(IEnumerable<CollidableAsset> collidables)
        {
            foreach (var collidable in collidables)
            {
                CheckCollisions(collidables, collidable);
            }
        }

        private void CheckCollisions(IEnumerable<CollidableAsset> collidables, CollidableAsset asset1)
        {
            foreach (var asset2 in collidables)
            {
                if ((asset1 != asset2) && (asset1.Z == asset2.Z) && 
                    !asset1.IsDisposed && !asset2.IsDisposed)
                {
                    CheckCollision(asset1, asset2);
                }
            }
        }

        private void CheckCollision(CollidableAsset asset1, CollidableAsset asset2)
        {
            using (var path1 = asset1.CreateHitBox())
            {
                using (var path2 = asset2.CreateHitBox())
                {
                    using (var region1 = new Region(path1))
                    {
                        using (var region2 = new Region(path2))
                        {
                            region1.Intersect(region2);
                            if (region1.GetRegionScans(new Matrix(1, 0, 0, 1, 0, 0)).Length > 0)
                            {
                                asset1.Collide(asset2);
                                asset2.Collide(asset1);
                            }
                        }
                    }
                }
            }
        }

        private int ComputeSleepTime(TimeSpan elapsedTime)
        {
            if (elapsedTime.TotalMilliseconds < FrameDuration)
            {
                return (int)(FrameDuration - elapsedTime.TotalMilliseconds);
            }
            return 0;
        }

        private void ImportNewAssets()
        {
            foreach (var asset in _newAssets)
            {
                _assets.Add(asset);
            }
            _newAssets.Clear();
        }

        private void AnimateAssets()
        {
            foreach (var asset in _assets)
            {
                asset.Animate();
            }
        }

        private void CleanUp()
        {
            if (_frame % 50 != 0)
            {
                return;
            }

            var livingAssets = new List<Asset>();
            foreach (var asset in _assets)
            {
                if (!asset.IsDisposed)
                {
                    livingAssets.Add(asset);
                }
            }
            _assets = livingAssets;
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
            _newAssets.Add(asset);
        }

        private void UpdateStageView()
        {
            lock (_stageView)
            {
                _stageGraphics.FillRectangle(Brushes.Black, 0, 0, _stageView.Width, _stageView.Height);
                var orderedAsset = _assets.OrderBy(a => -a.Z);
                foreach (var asset in orderedAsset)
                {
                    if (!asset.IsDisposed)
                    {
                        asset.Draw(_stageGraphics);
                    }
                }
            }
        }

        public T Create<T>() where T : Asset
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
