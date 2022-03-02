using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projet_Scientifique_Info_Kevin_Brieuc
{
    internal class Pixel
    {
        private byte rouge; // B V R 
        private byte vert;
        private byte bleu;

        public byte R
        {
            get { return rouge; }
            set { rouge = value; }
        }
        public byte V
        {
            get { return vert; }
            set { vert = value; }
        }
        public byte B
        {
            get { return bleu; }
            set { bleu = value; }
        }
        public int IntR
        {
            get { return Convert.ToInt32(rouge); }
        }
        public int IntB
        {
            get { return Convert.ToInt32(bleu); }
        }
        public int IntV
        {
            get { return Convert.ToInt32(vert); }
        }

        public Pixel(byte bleu, byte vert, byte rouge)            
        {
            this.rouge = rouge;
            this.vert = vert;
            this.bleu = bleu;
        }

        public string AfficherPixel()
        {         
            string pixel = Convert.ToInt32(this.bleu) + "," + Convert.ToInt32(this.vert) + "," + Convert.ToInt32(this.rouge);
            return pixel;
        }
    }
}
