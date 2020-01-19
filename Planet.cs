using System;
using System.Drawing;

namespace HomeWork_2_1
{
    class Planet : BaseObject
    {
        Image image = Image.FromFile(@"C:\Users\shipo\source\repos\HomeWork_2.1\HomeWork_2.1\147577722413943995.jpg");
        public Planet(Point pos, Point dir, Size size) : base(pos, dir, size)
        {

        }
        public override void Update()
        {
            Pos.X = Pos.X + Dir.X;
            if (Pos.X < 0)
            {
                Pos.X = Game.Width + Size.Width;
            }
        }

        public override void Draw()
        {
            Game.Buffer.Graphics.DrawEllipse(Pens.Green, Pos.X, Pos.Y, Size.Width, Size.Height);
            Game.Buffer.Graphics.DrawImage(image, Pos.X, Pos.Y, Size.Width, Size.Height);
        }
    }
}
