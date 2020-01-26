using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace SpaceGame_Shipov
{
    class Bullet : BaseObject, IDestroy
    {
        public Bullet(Point pos, Point dir, Size size) : base(pos, dir, size)
        {

        }
        public override void Draw()
        {
            Game.Buffer.Graphics.DrawRectangle(Pens.OrangeRed, Pos.X, Pos.Y, Size.Width, Size.Height);
            if (Size.Width > 10 || Size.Height > 10)
            {
                throw new GameObjectException("Недопустимый размер");
            }
        }

        public override void Update()
        {
            Pos.X = Pos.X + 15;
        }

        public void Destroy()
        {
            Pos.X = 0;
        }
    }
}
