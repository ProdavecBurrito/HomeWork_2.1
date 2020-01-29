using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;


namespace SpaceGame_Shipov
{
    class Ship : BaseObject
    {
        Image image;
        private int _energy = 100;
        public int Energy => _energy;

        public Ship(Image _image, Point pos, Point dir, Size size) : base(pos, dir, size)
        {
            image = Image.FromFile(@"../../Images/Star_ship.png");
        }

        public override void Draw()
        {
            Game.Buffer.Graphics.DrawImage(this.image, Pos.X, Pos.Y, Size.Width, Size.Height);
        }

        public override void Update()
        {
        }

        public void Up()
        {
            if (Pos.Y > 0)
            {
                Pos.Y = Pos.Y - Dir.Y;
            }
        }

        public void Down()
        {
            if (Pos.Y < Game.Height)
            {
                Pos.Y = Pos.Y + Dir.Y;
            }
        }
        public void Die()
        {
            MessageDie?.Invoke();
        }

        /// <summary>
        /// Понижение энергии
        /// </summary>
        /// <param name="n"></param>
        public void EnergyLow(int n)
        {
            _energy -= n;
        }

        /// <summary>
        /// Повышение энергии
        /// </summary>
        /// <param name="n"></param>
        public void EnegryUp(int n)
        {
            _energy += n;
            if (_energy > 100)
            {
                _energy = 100;
            }
        }

        public static event Message MessageDie;
    }

}
