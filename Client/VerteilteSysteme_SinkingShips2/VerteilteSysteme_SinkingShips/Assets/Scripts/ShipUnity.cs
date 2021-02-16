using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets
{
    public class ShipUnity
    {
        private List<(int, int)> fields;
        private List<(int, int)> fieldTemp;
        private int posX;
        private int posY;

        public ShipUnity(string name, int size, int posX, int posY, bool rotated = false)
        {
            this.Size = size;
            this.Name = name;
            this.posX = posX;
            this.posY = posY;
            this.Rotated = rotated;
            this.fields = new List<(int, int)>();
            this.fieldTemp = new List<(int, int)>();
        }

        public List<(int, int)> FieldTemp 
        { 
            get
            {
                this.CalculateFieldTemp();
                return this.fieldTemp;
            }
        }

        public int Size { get; set; }
        public string Name { get; set; }
        public int PosX 
        {
            get
            {
                return this.posX;
            }
            set
            {
                this.posX = value;
            }
        }
        public int PosY
        {
            get
            {
                return this.posY;
            }
            set
            {
                this.posY = value;
            }
        }

        public bool Rotated { get; set; }
        public List<(int, int)> Field
        {
            get
            {
                this.CalculateField();
                return this.fields;
            }
        }
        public int Uneven
        {
            get
            {
                return this.Size % 2 - 1;
            }
        }

        public int Offset
        {
            get
            {
                return (int)Math.Floor((double)(this.Size / 2));
            }
        }

        private void CalculateField()
        {
            if (this.PosX < 0 || this.PosY < 0)
            {
                return;
            }
            
            this.fields.Clear();

            if (this.Rotated)
            {
                int posTemp = this.PosX - Offset;
             
                if (Uneven != 0)
                {
                    posTemp++;
                }

                
                for (int i = 0; i < this.Size; i++)
                {
                    this.fields.Add((posTemp + i, PosY));
                }
            }
            else
            {
                int posTemp = this.PosY - Offset;
                for (int i = 0; i < this.Size; i++)
                {
                    this.fields.Add((PosX, posTemp + i));
                }
            }
        }

        private void CalculateFieldTemp()
        {
            if (this.PosX < 0 || this.PosY < 0)
            {
                return;
            }

            this.fieldTemp.Clear();
            if (this.Rotated)
            {
                this.fieldTemp.AddRange(Field);
                this.fieldTemp.Add((this.fieldTemp.Last().Item1 + 1, this.fieldTemp.Last().Item2));
                this.fieldTemp.Add((this.fieldTemp.First().Item1 - 1, this.fieldTemp.First().Item2));

                for (int i = 0; i < this.Size + 2; i++)
                {
                    this.fieldTemp.Add((this.fieldTemp[i].Item1, PosY + 1));
                    this.fieldTemp.Add((this.fieldTemp[i].Item1, PosY - 1));
                }

                return;
            }

            this.fieldTemp.AddRange(Field);
            this.fieldTemp.Add((this.fieldTemp.Last().Item1, this.fieldTemp.Last().Item2 + 1));
            this.fieldTemp.Add((this.fieldTemp.First().Item1, this.fieldTemp.First().Item2 - 1));

            for (int i = 0; i < this.Size + 2; i++)
            {
                this.fieldTemp.Add((PosX + 1, this.fieldTemp[i].Item2));
                this.fieldTemp.Add((PosX - 1, this.fieldTemp[i].Item2));
            }
        }
    }
}