using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceGame_Shipov
{
    // Класс для ведения счета
    class Score
    {
        private int _score = 0;
        public int GetScore => _score;

        public void AddScore(int a)
        {
            _score += a;
        }
    }
}
