using System;
using System.Windows.Forms;
using System.Drawing;
using System.IO;
using System.Collections.Generic;

namespace SpaceGame_Shipov
{
    static class Game
    {
        private static BufferedGraphicsContext _context;
        public static BufferedGraphics Buffer;

        private static Timer _timer;
        public static Random Rnd = new Random();

        delegate void Message(string mes);

        static Score _score = new Score();

        public static BaseObject[] _objs;
        private static List<Bullet> _bullets = new List<Bullet>();

        private static List<Asteroid> _asteroids;
        static int asteroidsCounter = 20;

        private static Planet[] _planets;
        private static Ship _ship;
        static HealingTool[] _healings;
        // Файл для записи лога
        static StreamWriter sw = new StreamWriter(@"../../ShipLog.txt");

        // Создание переменных делегата
        static Message message = Console.WriteLine;
        static Message fileMessage = sw.WriteLine;

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
            form.AutoScaleMode = AutoScaleMode.Font;
            Width = form.ClientSize.Width;
            Height = form.ClientSize.Height;
            form.WindowState = FormWindowState.Normal;
            form.StartPosition = FormStartPosition.CenterScreen;
            form.MaximumSize = new Size(Width, Height);
            form.MinimumSize = new Size(Width, Height);
            form.FormBorderStyle = FormBorderStyle.FixedSingle;


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

            if (_bullets.Count != 0)
            {
                foreach (Bullet b in _bullets)
                {
                    b.Draw();
                }
            }

            _ship?.Draw();

            if (_ship != null)
            {
                Buffer.Graphics.DrawString("Energy:" + _ship.Energy, SystemFonts.DefaultFont, Brushes.White, 0, 0);
                Buffer.Graphics.DrawString("Score:" + _score.GetScore, SystemFonts.DefaultFont, Brushes.Blue, 0, 20);
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

            foreach (Bullet b in _bullets)
            {
                b.Update();
            }

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
                    _score.AddScore(25);
                    message(@"Подобранна хилка. Востановленно rnd.Next(15, 25) энергии");
                    fileMessage(@"Подобранна хилка. Востановленно rnd.Next(15, 25) энергии");
                    continue;
                }
            }

            for (var i = 0; i < _asteroids.Count; i++)
            {
                if(_asteroids.Count == 0)
                {
                    Console.WriteLine("krk");
                    asteroidsCounter += 1;
                    for (int j= 0; j < asteroidsCounter; j++)
                    {
                        int r = new Random().Next(5, 50);
                        _asteroids.Add(new Asteroid(Image, new Point(1000, new Random().Next(0, Game.Height)), new Point(-r / 5, r), new Size(r, r)));
                    }
                }

                if (_asteroids[i] == null)
                {
                    continue;
                }

                _asteroids[i].Update();

                for (int j = 0; j < _bullets.Count; j++)
                {
                    // Проверка, попала ли пуля в астероид
                    if (_bullets[j].Collision(_asteroids[i]))
                    {
                        System.Media.SystemSounds.Hand.Play();
                        _asteroids.RemoveAt(i);
                        _bullets.RemoveAt(j);
                        j--;
                        _score.AddScore(100);
                        message("Сбит астероид. +100 очков");
                        fileMessage("Сбит астероид. +100 очков");
                    }
                    continue;
                }

                // Проверка, врезался ли корабль в астероид
                if (!_ship.Collision(_asteroids[i]))
                {
                    continue;
                }

                var rnd = new Random();
                _ship?.EnergyLow(rnd.Next(15, 25));
                _asteroids[i] = null;
                _score.AddScore(-30);
                message($"Корабль врезался в астероид. Полученно {rnd.Next(15,25)} урона. -30 очков");
                fileMessage($"Корабль врезался в астероид. Полученно {rnd.Next(15, 25)} урона. -30 очков");
                System.Media.SystemSounds.Asterisk.Play();

                // Смерть кора
                if (_ship.Energy <= 0)
                {
                    _ship?.Die();
                    message("Корабль погиб. Нажмите на F, что бы отдать честь");
                    fileMessage("Корабль погиб. Нажмите на F, что бы отдать честь");
                    char r = Console.ReadKey().KeyChar;
                    Console.WriteLine();
                    if (r == 'f')
                    {
                        message("Вы почтили память космического скитальца");
                        fileMessage("Вы почтили память космического скитальца");
                    }
                    else
                    {
                        message("Вам не стыдно?");
                        fileMessage("Вам не стыдно?");
                    }
                    sw.Close();
                }
            }

            foreach (Planet obj in _planets)
            {
                obj.Update();
            }

        }

        public static void Load()
        {
            _objs = new BaseObject[50];
            _planets = new Planet[6];
            _healings = new HealingTool[2];
            _asteroids = new List<Asteroid>();

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
                    _planets[i] = new Planet(Image = Image.FromFile(@"../../Images/Red_Planet.png"), new Point(1000, rnd.Next(0, Game.Height)), new Point(-r / 2, r), new Size(20, 20));
                    r = rnd.Next(5, 50);
                    _planets[i + 1] = new Planet(Image = Image.FromFile(@"../../Images/Gas_Giant.png"), new Point(1000, rnd.Next(0, Game.Height)), new Point(-r / 2, r), new Size(30, 30));
                    r = rnd.Next(5, 50);
                    _planets[i + 2] = new Planet(Image = Image.FromFile(@"../../Images/Earth.png"), new Point(1000, rnd.Next(0, Game.Height)), new Point(-r / 2, r), new Size(25, 25));
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
                _objs[i] = new Star(new Point(1000, rnd.Next(0, Game.Height)), new Point(-r, r), new Size(3, 3));
            }

            // Инициализация астероидов
            try
            {
                for (var i = 0; i < asteroidsCounter; i++)
                {
                    {
                        int r = rnd.Next(5, 50);
                        _asteroids.Add(new Asteroid(Image, new Point(1000, rnd.Next(0, Game.Height)), new Point(-r / 5, r), new Size(r, r)));
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

        // Нажатые клавиши
        private static void Form_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.ControlKey) _bullets.Add(new Bullet(new Point(_ship.Rect.X + 40, _ship.Rect.Y + 20), new Point(4, 0), new Size(6, 2)));
            if (e.KeyCode == Keys.Up) _ship.Up();
            if (e.KeyCode == Keys.Down) _ship.Down();
            if (e.KeyCode == Keys.B) Console.WriteLine(_asteroids.Count);
            // Что бы убить себя
            if (e.KeyCode == Keys.Delete) _ship?.Die();
            if (e.KeyCode == Keys.N)
            {
                _ship?.EnergyLow(25);
                if (_ship.Energy <= 0)
                {
                    _ship?.Die();
                }
            }
        }

        public static void Finish()
        {
            _timer.Stop();
            Buffer.Graphics.DrawString("The End", new Font(FontFamily.GenericSansSerif, 60, FontStyle.Underline), Brushes.White, 200, 100);
            Buffer.Graphics.DrawString($"Score: {_score.GetScore}", new Font(FontFamily.GenericSansSerif, 30, FontStyle.Underline), Brushes.Blue, 275, 200);
            Buffer.Render();

        }
    }
}
