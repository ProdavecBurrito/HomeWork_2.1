using System;
using System.Windows.Forms;
using System.Drawing;
namespace SpaceGame_Shipov
{
    static class Game
    {
        private static BufferedGraphicsContext _context;
        public static BufferedGraphics Buffer;

        public static BaseObject[] _objs;
        private static Bullet _bullet;
        private static Asteroid[] _asteroids;
        private static Planet[] _planets;

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
            // Связываем буфер в памяти с графическим объектом, чтобы рисовать в буфере
            Buffer = _context.Allocate(g, new Rectangle(0, 0, Width, Height));

            Load();

            Timer timer = new Timer { Interval = 100 };
            timer.Start();
            timer.Tick += Timer_Tick;
        }

        public static void Draw()
        {
            Buffer.Graphics.Clear(Color.Black);

            foreach (BaseObject obj in _objs)
            {
                obj.Draw();
            }

            foreach (Asteroid obj in _asteroids)
            {
                obj.Draw();
            }

            foreach (Planet obj in _planets)
            {
                obj.Draw();
            }

            _bullet.Draw();

            Buffer.Render();
        }

        public static void Update()
        {
            foreach (BaseObject obj in _objs)
            {
                obj.Update();
            }

            foreach (Asteroid ast in _asteroids)
            {
                ast.Update();
                if (ast.Collision(_bullet))
                { 
                    System.Media.SystemSounds.Hand.Play();
                    ast.Destroy();
                    _bullet.Destroy();
                }
            }

            foreach (Planet obj in _planets)
            {
                obj.Update();
            }

            _bullet.Update();
        }

        public static void Load()
        {
            _objs = new BaseObject[30];
            _planets = new Planet[6];
            _asteroids = new Asteroid[20];
            _bullet = new Bullet(new Point(0, 200), new Point(5, 0), new Size(10, 10));

            var rnd = new Random();

            for (var i = 0; i < _objs.Length; i++)
            {
                int r = rnd.Next(5, 50);
                _objs[i] = new Star(new Point(1000, rnd.Next(0, Game.Height)), new Point(-r, r), new Size(10, 10));
            }
            for (var i = 0; i < _asteroids.Length; i++)
            {
                int r = rnd.Next(5, 50);
                _asteroids[i] = new Asteroid(new Point(1000, rnd.Next(0, Game.Height)), new Point(-r / 5, r), new Size(r, r));
            }
            for (int i = 0; i < _planets.Length; i +=3)
            {
                int r = rnd.Next(5, 50);
                _planets[i] = new Planet(Image = Image.FromFile(@"C:\Users\shipo\source\repos\SpaceGame_Shipov\Images\Red_Planet.jpg"), new Point(1000, rnd.Next(0, Game.Height)), new Point(-r / 2, r), new Size(20, 20));
                r = rnd.Next(5, 50);
                _planets[i + 1] = new Planet(Image = Image.FromFile(@"C:\Users\shipo\source\repos\SpaceGame_Shipov\Images\Gas_Giant.jpg"), new Point(1000, rnd.Next(0, Game.Height)), new Point(-r / 2, r), new Size(60, 60));
                r = rnd.Next(5, 50);
                _planets[i + 2] = new Planet(Image = Image.FromFile(@"C:\Users\shipo\source\repos\SpaceGame_Shipov\Images\Earth.jpg"), new Point(1000, rnd.Next(0, Game.Height)), new Point(-r / 2, r), new Size(30, 15));
            }
        }

        private static void Timer_Tick(object sender, EventArgs e)
        {
            Draw();
            Update();
        }
    }
}
