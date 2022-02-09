using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projet_Scientifique_Info_Kevin_Brieuc
{
    internal class Program
    {
        public static int Convert_Endian_To_Int(byte[] tab)
        {
            int taille = tab.Length;
            int entier = 0;
            int nbr = 1;
            for (int i = 0; i < taille; i++)
            {
                entier += tab[i] * nbr;
                nbr = nbr * 256;
            }
            return entier;
        }
        public static byte[] Convert_Int_To_Endian(int val)
        {
            int reste = val;
            byte[] tab = new byte[4];
            int x = 16777216;
            for (int i = 3; i >= 0; i--)
            {
                if (reste >= x)
                {
                    tab[i] = Convert.ToByte(reste / x);
                    reste = reste % x;
                    x = x / 256;
                }
                else
                {
                    tab[i] = 0;
                }
            }
            return tab;
        }
        static void Main(string[] args)
        {
            
            string fichier = "coco.bmp";
            MyImage image = new MyImage(fichier);
            string test = "Test.bmp";
            image.From_Image_To_File(test);
            Console.WriteLine("Taille fichier : " + image.TailleFichier);
            Console.WriteLine("Hauteur : " + image.Hauteur);
            Console.WriteLine("Largeur : " + image.Largeur);
            Console.WriteLine("Taille Offset : " + image.TailleOffset);
            Console.WriteLine("Nb de Bits par couleur : " + image.NbrDeBitsParCouleur);
            Console.WriteLine("Filename : " + image.FileName);

            image.AfficherMatrice();










            Console.ReadKey();


            /* TEST CONVERSIONS
            byte[] tab = { 230, 4, 0, 0 };
            Console.WriteLine(Convert_Endian_To_Int(tab));
            byte[] tab2 = Convert_Int_To_Endian(1254);
            for (int i = 0; i < tab2.Length; i++)
            {
                Console.Write(tab[i] + " ");
            }
            Console.ReadKey();
            */
        }
    }
}
