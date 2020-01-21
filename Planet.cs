using System;
using System.Drawing;

namespace SpaceGame_Shipov
{
    class Planet : BaseObject
    {
        Image image;
        public Planet(Image _image, Point pos, Point dir, Size size) : base(pos, dir, size)
        {
            image = _image;
        }

        public override void Draw()
        {
            Game.Buffer.Graphics.DrawImage(image, Pos.X, Pos.Y, Size.Width, Size.Height);
        }

        public override void Update()
        {
            Pos.X = Pos.X + Dir.X;
            if (Pos.X < 0)
            {
                Pos.X = Game.Width + Size.Width;
            }
        }
    }
}
