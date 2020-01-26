using System;
using System.Windows.Forms;
namespace SpaceGame_Shipov
{
    class Program
    {
        static void Main(string[] args)
        {
            Form form = new Form();
            form.Width = 900;
            form.Height = 700;
            Game.Init(form);
            form.Show();
            Game.Draw();
            Application.Run(form);
        }
    }
}