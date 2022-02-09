﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projet_Scientifique_Info_Kevin_Brieuc
{
    internal class Pixel
    {
        private byte rouge; // B G R 
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

        public Pixel(byte bleu, byte vert, byte rouge)            
        {
            this.rouge = rouge;
            this.vert = vert;
            this.bleu = bleu;
        }

        public string AfficherPixel()
        {
            string pixel;
            for (int i = 0; i < 3;i++ )
            {
                pixel = pixel + Convert.ToInt32(this.B) + "," + Convert.ToInt32(this.G) + "," + Convert.ToInt32(this.R);
            }
            return pixel;
        }
    }
}
