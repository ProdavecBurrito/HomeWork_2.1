using System;
using System.Windows.Forms;
using System.Drawing;
namespace HomeWork_2_1
{
    static class Game
    {
        private static BufferedGraphicsContext _context;
        public static BufferedGraphics Buffer;

        static Random rand = new Random();
        public static BaseObject[] _objs;
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

            Timer timer = new Timer { Interval = 50 };
            timer.Start();
            timer.Tick += Timer_Tick;
        }

        public static void Draw()
        {
            // Проверяем вывод графики
            //Buffer.Graphics.Clear(Color.Black);
            //Buffer.Graphics.DrawRectangle(Pens.White, new Rectangle(100, 100, 200, 200));
            //Buffer.Graphics.FillEllipse(Brushes.Wheat, new Rectangle(100, 100, 200, 200));
            //Buffer.Render();

            Buffer.Graphics.Clear(Color.Black);
            foreach (BaseObject obj in _objs)
            {
                obj.Draw();
            }
            Buffer.Render();
        }

        public static void Update()
        {
            foreach (BaseObject obj in _objs)
            {
                obj.Update();
            }
        }

        public static void Load()
        {
            _objs = new BaseObject[60];
            int objCount = _objs.Length;
            int k = 0;
            for (int j = 0; j < _objs.Length / 10; j++)
            {
                for (int i = 0; i < 7; i++)
                {
                    _objs[_objs.Length - objCount] = new Star(new Point(rand.Next(100, 700), rand.Next(1,30) * rand.Next(15, 30)), new Point(-k, -k), new Size(rand.Next(5, 10), rand.Next(5, 10)));
                    objCount -= 1;
                    k++;
                }
                _objs[_objs.Length - objCount] = new Planet(new Point(rand.Next(200, 700), rand.Next(1, 30) * rand.Next(15, 30)), new Point(-k, -k), new Size(40, 20));
                objCount -= 1;
                k++;
                _objs[_objs.Length - objCount] = new RedPlanet(new Point(rand.Next(200, 700), rand.Next(1, 30) * rand.Next(15, 30)), new Point(-k, -k), new Size(25, 25));
                objCount -= 1;
                k++;
                _objs[_objs.Length - objCount] = new GreenPlanet(new Point(rand.Next(200, 700), rand.Next(1, 30) * rand.Next(15, 30)), new Point(-k, -k), new Size(15, 15));
                objCount -= 1;
                k++;
            }


            //    _objs = new BaseObject[30];
            //for (int i = 0; i < _objs.Length / 2; i++)
            //    _objs[i] = new BaseObject(new Point(rand.Next(200, 700), i * 20), new Point(-i, -i), new Size(10, 10));
            //for (int i = _objs.Length; i < _objs.Length; i++)
            //    _objs[i] = new Star(new Point(rand.Next(200, 700), i * rand.Next(15, 30)), new Point(-i, 0), new Size(rand.Next(5, 10), rand.Next(5, 10)));
        }

        private static void Timer_Tick(object sender, EventArgs e)
        {
            Draw();
            Update();
        }
    }
}
