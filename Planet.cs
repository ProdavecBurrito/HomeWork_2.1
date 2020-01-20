using System;
using System.Drawing;

namespace HomeWork_2_1
{
    class Planet : BaseObject
    {
        Image image;
        Pen color;
        public Planet(Pen _color, Image _image, Point pos, Point dir, Size size) : base(pos, dir, size)
        {
            color = _color;
            image = _image;
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
            Game.Buffer.Graphics.DrawEllipse(color, Pos.X, Pos.Y, Size.Width, Size.Height);
            Game.Buffer.Graphics.DrawImage(image, Pos.X, Pos.Y, Size.Width, Size.Height);
        }
    }
}
