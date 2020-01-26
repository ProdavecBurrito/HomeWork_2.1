using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceGame_Shipov
{
    interface IDestroy
    {
        /// <summary>
        /// Возвращает обьект на изначальную позицию
        /// </summary>
        void Destroy();
    }
}
