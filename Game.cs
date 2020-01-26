using System;
using System.Windows.Forms;
using System.Drawing;
namespace SpaceGame_Shipov
{
    static class Game
    {
        private static BufferedGraphicsContext _context;
        public static BufferedGraphics Buffer;

        private static Timer _timer;
        public static Random Rnd = new Random();


        public static BaseObject[] _objs;
        private static Bullet _bullet;
        private static Asteroid[] _asteroids;
        private static Planet[] _planets;
        private static Ship _ship;
        static HealingTool[] _healings;


        static Image Image;
        // Свойства
        // Ширина и высота игрового поля
        public static int Width { get; set; }
        public static int Height { get; set; }
        static Game()
        {
        }

        public static void Init(Form form)
        {
            // Графическое устройство для вывода графики
            Graphics g;
            // Предоставляет доступ к главному буферу графического контекста для текущего приложения
            _context = BufferedGraphicsManager.Current;
            g = form.CreateGraphics();
            // Создаем объект (поверхность рисования) и связываем его с формой
            // Запоминаем размеры формы
            Width = form.ClientSize.Width;
            Height = form.ClientSize.Height;

            // Исключение превышения размеров
            if (form.ClientSize.Width > 1000 || form.ClientSize.Width < 0)
            {
                throw new ArgumentOutOfRangeException("Width", "Введенны неверные данные");
            }
            else if (form.ClientSize.Height >= 1001 || form.ClientSize.Height <= 0)
            {
                throw new ArgumentOutOfRangeException("Heigth", "Введенны неверные данные");
            }
            // Связываем буфер в памяти с графическим объектом, чтобы рисовать в буфере
            Buffer = _context.Allocate(g, new Rectangle(0, 0, Width, Height));

            Load();

            _timer = new Timer { Interval = 100 };
            _timer.Start();
            _timer.Tick += Timer_Tick;

            form.KeyDown += Form_KeyDown;

            Ship.MessageDie += Finish;
        }

        public static void Draw()
        {
            Buffer.Graphics.Clear(Color.Black);

            foreach (BaseObject obj in _objs)
            {
                obj.Draw();
            }

            foreach (Asteroid a in _asteroids)
            {
                a?.Draw();
            }

            _bullet?.Draw();

            _ship?.Draw();

            if (_ship != null)
            {
                Buffer.Graphics.DrawString("Energy:" + _ship.Energy, SystemFonts.DefaultFont, Brushes.White, 0, 0);
            }

            foreach (Planet obj in _planets)
            {
                obj.Draw();
            }

            foreach(HealingTool ht in _healings)
            {
                ht?.Draw();
            }

            Buffer.Render();
        }


        public static void Update()
        {
            foreach (BaseObject obj in _objs)
            {
                obj.Update();
            }

            _bullet?.Update();

            for (int i = 0; i < _healings.Length; i++)
            {
                if (_healings[i] == null)
                {
                    continue;
                }

                _healings[i].Update();

                if (_healings[i] != null && _ship.Collision(_healings[i]) && _ship?.Energy < 100)
                {
                    System.Media.SystemSounds.Hand.Play();
                    _healings[i] = null;
                    var rnd = new Random();
                    _ship.EnegryUp(rnd.Next(15, 25));
                    continue;
                }
            }

            for (var i = 0; i < _asteroids.Length; i++)
            {
                if (_asteroids[i] == null)
                {
                    continue;
                }

                _asteroids[i].Update();

                if (_bullet != null && _bullet.Collision(_asteroids[i]))
                {
                    System.Media.SystemSounds.Hand.Play();
                    _asteroids[i] = null;
                    _bullet = null;
                    continue;
                }

                if (!_ship.Collision(_asteroids[i]))
                {
                    continue;
                }

                var rnd = new Random();
                _ship?.EnergyLow(rnd.Next(15, 25));
                _asteroids[i] = null;
                System.Media.SystemSounds.Asterisk.Play();

                if (_ship.Energy <= 0)
                {
                    _ship?.Die();
                }
            }

            foreach (Planet obj in _planets)
            {
                obj.Update();
            }

        }

        public static void Load()
        {
            _objs = new BaseObject[30];
            _planets = new Planet[6];
            _asteroids = new Asteroid[20];
            _healings = new HealingTool[2];

            var rnd = new Random();

            // Инициализация хилки
            for (var i = 0; i < _healings.Length; i++)
            {
                int r = rnd.Next(5, 50);
                _healings[i] = new HealingTool(Image, new Point(1000, rnd.Next(0, Game.Height)), new Point(-r, r), new Size(40, 40));
            }

            // Инициализация корабля
            _ship = new Ship(Image, new Point(10, 400), new Point(5, 5), new Size(40, 40));

            // Инициализация планет
            try
            {
                for (int i = 0; i < _planets.Length; i += 3)
                {
                    int r = rnd.Next(5, 50);
                    _planets[i] = new Planet(Image = Image.FromFile(@"../../Images/Red_Planet.png"), new Point(1000, rnd.Next(0, Game.Height)), new Point(-r / 2, r), new Size(30, 30));
                    r = rnd.Next(5, 50);
                    _planets[i + 1] = new Planet(Image = Image.FromFile(@"../../Images/Gas_Giant.png"), new Point(1000, rnd.Next(0, Game.Height)), new Point(-r / 2, r), new Size(50, 50));
                    r = rnd.Next(5, 50);
                    _planets[i + 2] = new Planet(Image = Image.FromFile(@"../../Images/Earth.png"), new Point(1000, rnd.Next(0, Game.Height)), new Point(-r / 2, r), new Size(40, 40));
                }
            }
            catch (GameObjectException mes)
            {
                Console.WriteLine("Ошибка: ", mes.Message);
            }

            // Инициализация звезд
            for (var i = 0; i < _objs.Length; i++)
            {
                int r = rnd.Next(5, 50);
                _objs[i] = new Star(new Point(1000, rnd.Next(0, Game.Height)), new Point(-r, r), new Size(10, 10));
            }

            // Инициализация астероидов
            try
            {
                for (var i = 0; i < _asteroids.Length; i++)
                {
                    {
                        int r = rnd.Next(5, 50);
                        _asteroids[i] = new Asteroid(Image, new Point(1000, rnd.Next(0, Game.Height)), new Point(-r / 5, r), new Size(r, r));
                    }
                }
            }
            catch (GameObjectException mes)
            {
                Console.WriteLine("Ошибка: ", mes.Message);
            }
        }
        private static void Timer_Tick(object sender, EventArgs e)
        {
            Draw();
            Update();
        }

        private static void Form_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.ControlKey) _bullet = new Bullet(new Point(_ship.Rect.X + 40, _ship.Rect.Y + 20), new Point(4, 0), new Size(6, 2));
            if (e.KeyCode == Keys.Up) _ship.Up();
            if (e.KeyCode == Keys.Down) _ship.Down();
        }

        public static void Finish()
        {
            _timer.Stop();
            Buffer.Graphics.DrawString("The End", new Font(FontFamily.GenericSansSerif, 60, FontStyle.Underline), Brushes.White, 200, 100);
            Buffer.Render();

        }
    }
}
