using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SpaceInvaderX.Actors;

namespace SpaceInvaderX
{
    public partial class Form1 : Form
    {
        private Random _rnd;
        
        public Form1()
        {
            InitializeComponent();
            _rnd = new Random();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            ClientSize = new Size(960, 720);
            var defender = stage1.Create<Defender>();
            defender.X = 50;
            defender.Y = 180;
            stage1.AddAsset(defender);

            Target target1 = stage1.Create<Target>();
            target1.X = 70;
            target1.Y = 60;
            stage1.AddAsset(target1);


            Target target2 = stage1.Create<Target>();
            target2.X = 100;
            target2.Y = 100;
            stage1.AddAsset(target2);


            Target target3 = stage1.Create<Target>();
            target3.X = 140;
            target3.Y = 150;
            stage1.AddAsset(target3);


            Target target4 = stage1.Create<Target>();
            target4.X = 230;
            target4.Y = 30;
            stage1.AddAsset(target4);


            FPS fps = stage1.Create<FPS>();
            fps.X = 275;
            fps.Y = 225;
            stage1.AddAsset(fps);


            _timer.Start();


            stage1.Start();
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            stage1.Stop();
            _timer.Stop();
        }

        private void _timer_Tick(object sender, EventArgs e)
        {
            Target target = stage1.Create<Target>();
            target.X = _rnd.Next(310);
            target.Y = _rnd.Next(150);
            stage1.AddAsset(target);
        }
    }
}
