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
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            ClientSize = new Size(960, 720);
            var defender = stage1.Create<Defender>();
            defender.X = 200;
            defender.Y = 200;
            stage1.AddAsset(defender);
            stage1.Start();
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            stage1.Stop();
        }
    }
}
