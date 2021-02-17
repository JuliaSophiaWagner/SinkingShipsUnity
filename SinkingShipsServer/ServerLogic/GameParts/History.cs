using System;
using System.Collections.Generic;
using System.Text;

namespace ServerLogic.GameParts
{
    public class History
    {
        public History()
        {

        }

        public History(string name, int firstPlayerPoints, int secondPlayerPoints)
        {
            this.Name = name;
            this.FirstPlayerPoints = firstPlayerPoints;
            this.SecondPlayerPoints = secondPlayerPoints;
        }

        public string Name { get; set; }

        public int FirstPlayerPoints { get; set; }

        public int SecondPlayerPoints { get; set; }
    }
}
