using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        Viewer viewer;

        public Form1()
        {
            InitializeComponent();

            this.ClientSize = new Size(800, 600);
            viewer = new Viewer();
            if (viewer.InitializeGraphics(this))
                timer1.Enabled = true;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            viewer.Update();
            viewer.Render();
        }
    }
}
