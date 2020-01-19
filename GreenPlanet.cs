using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace HomeWork_2_1
{
    class GreenPlanet : Planet
    {
        public GreenPlanet(Point pos, Point dir, Size size) : base(pos, dir, size)
        {

        }
        public override void Draw()
        {
            Game.Buffer.Graphics.DrawEllipse(Pens.Green, Pos.X, Pos.Y, Size.Width, Size.Height);
            Game.Buffer.Graphics.FillEllipse(Brushes.Green, new Rectangle(Pos.X, Pos.Y, Size.Width, Size.Height));
        }
    }
}
