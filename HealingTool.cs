using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceGame_Shipov
{
    class HealingTool : BaseObject
    {
        Image image;
        public HealingTool(Image _image, Point pos, Point dir, Size size) : base(pos, dir, size)
        {
            image = Image.FromFile(@"../../Images/HealingTool.png");
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
