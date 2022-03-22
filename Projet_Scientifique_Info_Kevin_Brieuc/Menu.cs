using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projet_Scientifique_Info_Kevin_Brieuc
{
    internal class Menu
    {
        // Fonctions à ajouter encore !
        // QR code à faire plus tard 
        // Ajouter les touches avec des nombres pour la selection 
        // Ajouter les consignes pour utiliser la console 
        // Régler l'affichage des infos de l'image modifiée 


        string[] menu;
        string[] deplacement;
        string[] menuImages;
        string[] deplacementImages;
        string[] menuImages2;
        string[] deplacementImages2;

        public Menu()
        {
            this.deplacement = new string[2] { "1", "0"};
            this.menu = new string[2] { "Images", "QR codes" };
            this.deplacementImages = new string[5] {"1", "0", "0", "0" ,"0"};
            this.menuImages = new string[5] { "coco.bmp", "test001.bmp", "lac.bmp", "lena.bmp" , "Retour"};
            this.deplacementImages2 = new string[13] { "1", "0", "0", "0","0" , "0", "0", "0","0","0","0","0","0"};
            this.menuImages2 = new string[13] { "Noir et blanc", "Nuances de gris","Rotation", "Miroir", "Agrandissement","Retrécissement", "Détection de contour","Renforcement des bords", "Flou", "Repoussage","Informations sur l'image",  "Enregistrer l'image" , "Retour"};
        }
        public void AfficherMenu()
        {
            Console.WriteLine("\n\n" + new string(' ', Console.WindowWidth / 2 - 10) + "||Traitement d'images||" + "\n\n");
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.Write(new string('-', Console.WindowWidth));
            Console.ResetColor();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(); 
            Console.WriteLine(new string(' ', Console.WindowWidth / 2 - 2) + "MENU");
            Console.WriteLine();
            for (int i = 0; i < menu.Length; i++)
            {
                Console.Write(new string(' ', Console.WindowWidth / 2 - menu[i].Length / 2));
                if (deplacement[i] == "1")
                {
                    Console.BackgroundColor = ConsoleColor.White;
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.WriteLine(menu[i]);
                    Console.ResetColor();
                }
                else
                {
                    Console.ResetColor();
                    Console.WriteLine(menu[i]);
                }
            }
            Console.WriteLine("\n Utilisez les flèches directionnelles et appuyez sur entré pour valider svp.");
            Console.WriteLine("\n\n\n\n\n\n\n\n\n\n\n\n\n");
            Console.WriteLine("Kévin Kui/ Brieuc Le Guével");
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.Write(new string('-', Console.WindowWidth));
            Console.ResetColor();
        }
        public int DeplacerMenu()
        {
            int exo = 0;
            int i = 0;
            bool entré = false;
            while (!entré)
            {
                AfficherMenu();
                ConsoleKeyInfo cki = Console.ReadKey();
                Console.WriteLine();
                switch (cki.Key)
                {
                    case ConsoleKey.UpArrow:
                        deplacement[i] = "0";
                        i = (i - 1 + 2) % 2;
                        AfficherMenu();
                        break;
                    case ConsoleKey.DownArrow:
                        deplacement[i] = "0";
                        i = (i + 1) % 2;
                        AfficherMenu();
                        break;
                    case ConsoleKey.Enter:
                        entré = true;
                        exo = i;
                        break;
                }

                deplacement[i] = "1";
                Console.Clear();
            }
            return exo + 1;
        }

        public void AfficherMenuImages()
        {
            Console.WriteLine("\n\n" + new string(' ', Console.WindowWidth / 2 - 10) + "||Choix de l'image||" + "\n\n");
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.Write(new string('-', Console.WindowWidth));
            Console.ResetColor();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine();
            Console.WriteLine(new string(' ', Console.WindowWidth / 2 - 5) + "MENU IMAGE");
            Console.WriteLine();
            for (int i = 0; i < menuImages.Length; i++)
            {
                Console.Write(new string(' ', Console.WindowWidth / 2 - menuImages[i].Length / 2));
                if (deplacementImages[i] == "1")
                {
                    Console.BackgroundColor = ConsoleColor.White;
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.WriteLine(menuImages[i]);
                    Console.ResetColor();
                }
                else
                {
                    Console.ResetColor();
                    Console.WriteLine(menuImages[i]);
                }
            }
            Console.WriteLine("\n Utilisez les flèches directionnelles et appuyez sur entré pour valider svp.");
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.Write(new string('-', Console.WindowWidth));
            Console.ResetColor();
        }
        public string DeplacerMenuImages()
        {
            string imageChoisie = "";
            int i = 0;
            bool entré = false;
            while (!entré)
            {
                AfficherMenuImages();
                ConsoleKeyInfo cki = Console.ReadKey();
                Console.WriteLine();
                switch (cki.Key)
                {
                    case ConsoleKey.UpArrow:
                        deplacementImages[i] = "0";
                        i = (i - 1 + 5) % 5;
                        AfficherMenuImages();
                        break;
                    case ConsoleKey.DownArrow:
                        deplacementImages[i] = "0";
                        i = (i + 1) % 5;
                        AfficherMenuImages();
                        break;
                    case ConsoleKey.Enter:
                        entré = true;
                        imageChoisie = menuImages[i];
                        break;
                }
                deplacementImages[i] = "1";
                Console.Clear();
            }
            return imageChoisie;
        }
        public void AfficherMenuImages2()
        {
            Console.WriteLine("\n\n" + new string(' ', Console.WindowWidth / 2 - 10) + "||Choix du traitement||" + "\n\n");
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.Write(new string('-', Console.WindowWidth));
            Console.ResetColor();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine();
            Console.WriteLine(new string(' ', Console.WindowWidth / 2 - 7) + "MENU TRAITEMENT");
            Console.WriteLine();
            for (int i = 0; i < menuImages2.Length; i++)
            {
                Console.Write(new string(' ', Console.WindowWidth / 2 - menuImages2[i].Length / 2));
                if (deplacementImages2[i] == "1")
                {
                    Console.BackgroundColor = ConsoleColor.White;
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.WriteLine(menuImages2[i]);
                    Console.ResetColor();
                }
                else
                {
                    Console.ResetColor();
                    Console.WriteLine(menuImages2[i]);
                }
            }
            Console.WriteLine("\n Utilisez les flèches directionnelles et appuyez sur entré pour valider svp.");
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.Write(new string('-', Console.WindowWidth));
            Console.ResetColor();
        }
        public string DeplacerMenuImages2()
        {
            string traitement = "";
            int i = 0;
            bool entré = false;
            while (!entré)
            {
                AfficherMenuImages2();
                ConsoleKeyInfo cki = Console.ReadKey();
                Console.WriteLine();
                switch (cki.Key)
                {
                    case ConsoleKey.UpArrow:
                        deplacementImages2[i] = "0";
                        i = (i - 1 + 13) % 13;
                        AfficherMenuImages2();
                        break;
                    case ConsoleKey.DownArrow:
                        deplacementImages2[i] = "0";
                        i = (i + 1) % 13;
                        AfficherMenuImages2();
                        break;
                    case ConsoleKey.Enter:
                        entré = true;
                        traitement = menuImages2[i];
                        break;
                }
                deplacementImages2[i] = "1";
                Console.Clear();
            }
            deplacementImages2[i] = "0";
            deplacementImages2[0] = "1";
            
            return traitement;
        }

    }
}
