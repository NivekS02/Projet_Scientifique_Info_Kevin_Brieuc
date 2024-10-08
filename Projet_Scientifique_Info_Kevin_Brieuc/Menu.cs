﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projet_Scientifique_Info_Kevin_Brieuc
{
    public class Menu
    {
        // Ajouter les touches avec des nombres pour la selection 

        string[] menu;
        string[] deplacement;
        string[] menuImages;
        string[] deplacementImages;
        string[] menuImages2;
        string[] deplacementImages2;

        public Menu()
        {
            this.deplacement = new string[3] { "1", "0", "0" }; // menu choix du traitement
            this.menu = new string[3] { "Images", "QR codes", "Fractales" };

            this.deplacementImages = new string[6] { "1", "0", "0", "0", "0", "0" }; // Menu choix de l'image à traiter
            this.menuImages = new string[6] { "coco.bmp", "test001.bmp", "lac.bmp", "lena.bmp", "tigre.bmp", "Retour" };

            this.deplacementImages2 = new string[20] { "1", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0" , "0", "0", "0", "0", "0", "0" }; // Menu choix de la fonction de traitement 
            this.menuImages2 = new string[20] { "Noir et blanc", "Nuances de gris","Rotation", "Miroir", "Agrandissement","Retrécissement",
                                                "Détection de contour","Renforcement des bords", "Flou", "Repoussage","Histogramme","Cacher une image","Decrypter une image",
                                                "Négatif (fonction innovation)", "Sepia (fonction innovation)", "Diminue luminosité (fonction innovation)", "Augmenter luminosité (fonction innovation)", 
                                                "Informations sur l'image",  "Enregistrer l'image" , "Retour"};
        }
        #region Menu principal
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
                        i = (i - 1 + 3) % 3;
                        AfficherMenu();
                        break;
                    case ConsoleKey.DownArrow:
                        deplacement[i] = "0";
                        i = (i + 1) % 3;
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
        #endregion
        #region Menu Choix image
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
                        i = (i - 1 + 6) % 6;
                        AfficherMenuImages();
                        break;
                    case ConsoleKey.DownArrow:
                        deplacementImages[i] = "0";
                        i = (i + 1) % 6;
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
            deplacementImages[i] = "0";
            deplacementImages[0] = "1";
            return imageChoisie;
        }
        #endregion
        #region Menu choix type de traitement
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
                        i = (i - 1 + 20) % 20;
                        AfficherMenuImages2();
                        break;
                    case ConsoleKey.DownArrow:
                        deplacementImages2[i] = "0";
                        i = (i + 1) % 20;
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
        #endregion
    }
}
