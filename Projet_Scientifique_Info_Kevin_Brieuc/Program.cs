using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projet_Scientifique_Info_Kevin_Brieuc
{
    internal class Program
    {
        static void Main(string[] args)
        {
            
            string fichier = "lac.bmp";
            MyImage image = new MyImage(fichier);
            string test = "Résultat.bmp";
            image.From_Image_To_File(test);
            Console.WriteLine("Taille fichier : " + image.TailleFichier);
            Console.WriteLine("Type d'image :" + image.TypeImage);
            Console.WriteLine("Hauteur : " + image.Hauteur);
            Console.WriteLine("Largeur : " + image.Largeur);
            Console.WriteLine("Taille Offset : " + image.TailleOffset);
            Console.WriteLine("Nb de Bits par couleur : " + image.NbrDeBitsParCouleur);
            Console.WriteLine("Filename : " + image.FileName);

            //image.AfficherMatrice();

            //image.ImageNoirEtBlanc();
            //image.Rotation();
            //image.Rotation();
            //image.Rotation();
            image.Miroir();
            image.From_Image_To_File(test);




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
