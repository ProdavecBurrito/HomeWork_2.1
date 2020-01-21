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
    }
}
