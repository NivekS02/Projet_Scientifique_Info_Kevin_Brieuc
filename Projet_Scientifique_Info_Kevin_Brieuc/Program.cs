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
            bool retour = false;
            while(retour == false)
            {
                int choix = menu.DeplacerMenu();
                switch (choix)
                {
                    case 1:
                        retour = Images();
                        break;
                    case 2:
                        retour = QRCodes();
                        break;
                    default:
                        Console.WriteLine("Veuillez choisir le type de traitement souhaité svp");
                        break;
                }
            }
            
            //image.AfficherMatrice();
            Console.ReadKey();
        }
        public static bool Images()
        {
            bool imageModifiée = true;
            Menu menuImage = new Menu();
            string choixImage = menuImage.DeplacerMenuImages(); // on choisit le fichier
            if (choixImage == "Retour") return false;
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
                        Console.WriteLine("Transformation de l'image en noir et blanc effectuée.");
                        Console.WriteLine("Appuyer sur une touche pour continuer");
                        Console.ReadKey();
                        Console.Clear();
                        break;
                    case "Rotation":
                        /*
                        Console.WriteLine("Combien de rotations à 90 degrés souhaitez vous ?");
                        int NbrRotations = Convert.ToInt32(Console.ReadLine());
                        for (int i = 0; i < NbrRotations; i++) image.Rotation();
                        Console.WriteLine("Transformation de l'image avec " + NbrRotations +  " effectuée.");
                        */
                        double angle = Convert.ToInt32(Console.ReadLine());
                        image.Rotation2(angle);
                        Console.WriteLine("Appuyer sur une touche pour continuer");
                        Console.ReadKey();
                        Console.Clear();
                        break;
                    case "Miroir":
                        image.Miroir();
                        Console.WriteLine("Transformation de l'image miroir effectuée.");
                        Console.WriteLine("Appuyer sur une touche pour continuer");
                        Console.ReadKey();
                        Console.Clear();
                        break;
                    case "Agrandissement":
                        Console.WriteLine("Entrez un indice d'agrandissement");
                        int IndiceAgrandissement = Convert.ToInt32(Console.ReadLine());
                        image.Agrandir(IndiceAgrandissement);
                        Console.WriteLine("Agrandissement de l'image avec un coefficent de " + IndiceAgrandissement + " effectuée.");
                        Console.WriteLine("Appuyer sur une touche pour continuer");
                        Console.ReadKey();
                        Console.Clear();
                        break;
                    case "Retrécissement":
                        Console.WriteLine("Entrez un indice de rétrécissement");
                        int IndiceRétrécissement = Convert.ToInt32(Console.ReadLine());
                        image.Agrandir(IndiceRétrécissement);
                        Console.WriteLine("Rétrécissecement de l'image avec un coefficent de " + IndiceRétrécissement + " effectuée.");
                        Console.WriteLine("Appuyer sur une touche pour continuer");
                        Console.ReadKey();
                        Console.Clear();
                        break;
                    case "Informations sur l'image":
                        image.toString();
                        Console.WriteLine("Appuyer sur une touche pour continuer");
                        Console.ReadKey();
                        Console.Clear();
                        break;
                    case "Enregistrer l'image" :
                        fini = true;
                        Console.WriteLine("Image sauvegardée.");
                        Console.WriteLine("Appuyer sur une touche pour fermer la console.");
                        Console.ReadKey();
                        Console.Clear();
                        break;
                    case "Retour":
                        Images();
                        fini = true;
                        imageModifiée = false;
                        break;
                    default:
                        Console.WriteLine("Veuillez choisir le type de traitement souhaité svp");
                        break;
                }
            }
            if (imageModifiée)
            {
                image.From_Image_To_File(fichier);
                Process.Start(fichier);
            }
            Console.ReadKey();
            return fini;
        }
        public static bool QRCodes()
        {
            bool fini = true;
            Console.WriteLine("Rien du tout pour l'instant...");
            Console.ReadKey();
            return fini;
        }
    }
}
