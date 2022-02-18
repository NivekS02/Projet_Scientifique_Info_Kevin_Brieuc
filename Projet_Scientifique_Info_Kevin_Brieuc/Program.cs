using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics; // pour utiliser Process.Start(".bmp") ==> afficher l'image directement sur la console

namespace Projet_Scientifique_Info_Kevin_Brieuc
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Menu menu = new Menu();
            int choix = menu.DeplacerMenu();
            switch (choix)
            {
                case 1:
                    Images();
                    break;
                case 2:
                    QRCodes();
                    break;
                default:
                    Console.WriteLine("Veuillez choisir le type de traitement souhaité svp");
                    break;
            }

            /*
            
            Console.WriteLine("Taille fichier : " + image.TailleFichier);
            Console.WriteLine("Type d'image :" + image.TypeImage);
            Console.WriteLine("Hauteur : " + image.Hauteur);
            Console.WriteLine("Largeur : " + image.Largeur);
            Console.WriteLine("Taille Offset : " + image.TailleOffset);
            Console.WriteLine("Nb de Bits par couleur : " + image.NbrDeBitsParCouleur);
            Console.WriteLine("Filename : " + image.FileName);

            //image.AfficherMatrice();
            Console.ReadKey();
            */
        }
        public static void Images()
        {
            Menu menuImage = new Menu();
            string choixImage = menuImage.DeplacerMenuImages(); // on choisit le fichier
            MyImage image = new MyImage(choixImage);
            Console.WriteLine("Ecrivez le nom de votre nouvelle image qui sera sauvegardée");
            string fichier = Convert.ToString(Console.ReadLine());
            fichier += ".bmp";
            bool fini = false;
            Console.Clear();
            while(!fini)
            {
                string choix = menuImage.DeplacerMenuImages2();
                switch (choix)
                {
                    case "Noir et blanc":
                        image.ImageNoirEtBlanc();
                        break;
                    case "Rotation":
                        Console.WriteLine("Combien de rotations à 90 degrés souhaitez vous ?");
                        int NbrRotations = Convert.ToInt32(Console.ReadLine());
                        for (int i = 0; i < NbrRotations; i++) image.Rotation();
                        break;
                    case "Miroir":
                        image.Miroir();
                        break;
                    case "Agrandissement":
                        Console.WriteLine("Entrez un indice d'agrandissement");
                        int IndiceAgrandissement = Convert.ToInt32(Console.ReadLine());
                        image.Agrandir(IndiceAgrandissement);
                        break;
                    case "Retrécissement":
                        Console.WriteLine("Entrez un indice de rétrécissement");
                        int IndiceRétrécissement = Convert.ToInt32(Console.ReadLine());
                        image.Agrandir(IndiceRétrécissement);
                        break;
                    case "Informations sur l'image":
                        Console.WriteLine("Taille fichier : " + image.TailleFichier);
                        Console.WriteLine("Type d'image :" + image.TypeImage);
                        Console.WriteLine("Hauteur : " + image.Hauteur);
                        Console.WriteLine("Largeur : " + image.Largeur);
                        Console.WriteLine("Taille Offset : " + image.TailleOffset);
                        Console.WriteLine("Nb de Bits par couleur : " + image.NbrDeBitsParCouleur);
                        Console.WriteLine("Filename : " + image.FileName);
                        break;
                    case "Enregistrer l'image" :
                        fini = true;
                        break;
                    default:
                        Console.WriteLine("Veuillez choisir le type de traitement souhaité svp");
                        break;
                }
            }
            image.From_Image_To_File(fichier);
            Process.Start(fichier);
            Console.ReadKey();
        }
        public static void QRCodes()
        {
            Console.WriteLine("Rien du tout pour l'instant...");
            Console.ReadKey();
        }
    }
}
