﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics; // pour utiliser Process.Start(".bmp") ==> afficher l'image directement sur la console

namespace Projet_Scientifique_Info_Kevin_Brieuc
{
    public class Program
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
                    case 3:
                        retour = Fractales();
                        break;
                    default:
                        Console.WriteLine("Veuillez choisir le type de traitement souhaité svp");
                        break;
                }
            }
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
                    case "Nuances de gris":
                        image.NuancesDeGris();
                        Console.WriteLine("Transformation de l'image en nuances de gris effectuée.");
                        Console.WriteLine("Appuyer sur une touche pour continuer");
                        Console.ReadKey();
                        Console.Clear();
                        break;
                    case "Noir et blanc":
                        image.ImageNoirEtBlanc();
                        Console.WriteLine("Transformation de l'image en noir et blanc effectuée.");
                        Console.WriteLine("Appuyer sur une touche pour continuer");
                        Console.ReadKey();
                        Console.Clear();
                        break;
                    case "Rotation": // 2 types de rotation : la première en commentaire ne fait que des rotations à 90 degrés et la deuxième fonction ert à faire sur tous les angles 
                        /*
                        Console.WriteLine("Combien de rotations à 90 degrés souhaitez vous ?");
                        int NbrRotations = Convert.ToInt32(Console.ReadLine());
                        for (int i = 0; i < NbrRotations; i++) image.Rotation();
                        Console.WriteLine("Transformation de l'image avec " + NbrRotations +  " effectuée.");
                        */
                        Console.WriteLine("Entrez un angle de rotation pour votre image svp.");
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
                        image.Retrecir(IndiceRétrécissement);
                        Console.WriteLine("Rétrécissecement de l'image avec un coefficent de " + IndiceRétrécissement + " effectuée.");
                        Console.WriteLine("Appuyer sur une touche pour continuer");
                        Console.ReadKey();
                        Console.Clear();
                        break;
                    case "Détection de contour":
                        double[,] kernel1 = new double [,] {{ -1, -1, -1 }, { -1, 8, -1 }, { -1, -1, -1 } };
                        image.Convolution(kernel1);
                        image.NuancesDeGris();
                        Console.WriteLine("Dectection de contour effectuée.");
                        Console.WriteLine("Appuyer sur une touche pour continuer");
                        Console.ReadKey();
                        Console.Clear();
                        break;
                    case "Renforcement des bords":
                        double[,] kernel2 = new double[,] { { -1, -1, -1 }, { -1, 9, -1 }, { -1, -1, -1 } };
                        image.Convolution(kernel2);
                        Console.WriteLine("Renforcement des bords effectué.");
                        Console.WriteLine("Appuyer sur une touche pour continuer");
                        Console.ReadKey();
                        Console.Clear();
                        break;
                    case "Flou":
                        double[,] kernel3 = new double[,] {
                            { 0, 0, 1/13.0, 0, 0 },
                            { 0, 1/13.0, 1/13.0, 1/13.0, 0 },
                            { 1/13.0, 1/13.0, 1/13.0, 1/13.0, 1/13.0 },
                            { 0, 1/13.0, 1/13.0, 1/13.0, 0 },
                            { 0, 0, 1/13.0, 0, 0 } };
                        image.Convolution(kernel3);
                        Console.WriteLine("Flou effectué.");
                        Console.WriteLine("Appuyer sur une touche pour continuer");
                        Console.ReadKey();
                        Console.Clear();
                        break;
                    case "Repoussage":
                        double[,] kernel = new double[,] { { -2, -1, 0 }, { -1,1,1}, {0,1,2 } };
                        image.Convolution(kernel);
                        Console.WriteLine("Repoussage effectué.");
                        Console.WriteLine("Appuyer sur une touche pour continuer");
                        Console.ReadKey();
                        Console.Clear();
                        break;
                    case "Histogramme":
                        Console.WriteLine("Veuillez patienter, cela peut prendre quelques instants...");
                        image.Histogramme();
                        Console.WriteLine("Histogramme créé");
                        Console.WriteLine("Appuyer sur une touche pour continuer");
                        Console.ReadKey();
                        Console.Clear();
                        break;
                    case "Cacher une image":
                        Console.WriteLine("Choisissez l'image à cacher");
                        string choixImage2 = menuImage.DeplacerMenuImages();
                        string nouvelleImage = "";
                        switch (choixImage2)
                        {
                            case "coco.bmp":
                                nouvelleImage = "coco.bmp";
                                break;
                            case "test001.bmp":
                                nouvelleImage = "test001.bmp";
                                break;
                            case "lac.bmp":
                                nouvelleImage = "lac.bmp";
                                break;
                            case "lena.bmp":
                                nouvelleImage = "lena.bmp";
                                break;
                        }
                        MyImage ImageACacher = new MyImage(nouvelleImage);
                        image.Cacher_Image(ImageACacher);
                        Console.WriteLine("Appuyer sur une touche pour continuer");
                        Console.ReadKey();
                        Console.Clear();
                        break;
                    case "Decrypter une image":
                        image.Decrypter_Image();
                        Console.WriteLine("Decryptage fini");
                        Console.WriteLine("Appuyer sur une touche pour continuer");
                        Console.ReadKey();
                        Console.Clear();
                        break;
                    case "Négatif (fonction innovation)":
                        image.Negatif();
                        Console.WriteLine("Negatif fini");
                        Console.WriteLine("Appuyer sur une touche pour continuer");
                        Console.ReadKey();
                        Console.Clear();
                        break;
                    case "Sepia (fonction innovation)":
                        image.Sepia();
                        Console.WriteLine("Sepia fini");
                        Console.WriteLine("Appuyer sur une touche pour continuer");
                        Console.ReadKey();
                        Console.Clear();
                        break;
                    case "Diminue luminosité (fonction innovation)":
                        Console.WriteLine("Entrez un facteur de luminosité svp.");
                        int facteur = Convert.ToInt32(Console.ReadLine());
                        image.DiminuerLuminosite(facteur);
                        Console.WriteLine("Diminution de la luminosité terminée");
                        Console.WriteLine("Appuyer sur une touche pour continuer");
                        Console.ReadKey();
                        Console.Clear();
                        break;
                    case "Augmenter luminosité (fonction innovation)":
                        Console.WriteLine("Entrez un facteur de luminosité svp.");
                        int facteur1 = Convert.ToInt32(Console.ReadLine());
                        image.AjouterLuminosite(facteur1);
                        Console.WriteLine("Augmentation de la luminosité terminée");
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
        public static bool Fractales() //Partie fractale
        {
            bool fini = true;
            Console.WriteLine("Ecrivez le nom du fichier que vous voulez donner à la fractale svp.");
            string fichier = Convert.ToString(Console.ReadLine());
            fichier += ".bmp";
            MyImage image = new MyImage();
            image.FractaleMandelbrot();
            image.From_Image_To_FileNouvelleImage(fichier);
            Console.WriteLine("Création de la fractale finie.");
            Console.WriteLine("Appuyer sur une touche pour arrêter et afficher la fractale.");
            Console.ReadKey();
            Process.Start(fichier);
            return fini;
        }
        public static bool QRCodes() // Partie QR code 
        {
            bool fini = true;
            Console.WriteLine("Entrez un mot ou une suite de caractères svp.");
            string phrase = Console.ReadLine();
            phrase = phrase.ToUpper();
            int longueur = phrase.Length;
            if (longueur != 0 && longueur < 48)
            {
                MyImage QRCode = new MyImage(phrase, longueur);
                QRCode.Agrandir(100);
                QRCode.MiroirHorizontal();
                Console.WriteLine("Entrez le nom du nouveau fichier qui va être créé svp");
                string fichier = Console.ReadLine();
                fichier += ".bmp";
                QRCode.From_Image_To_FileNouvelleImage(fichier);
                Console.WriteLine("Création du QR code finie.");
                Console.WriteLine("Appuyer sur une touche pour arrêter et afficher le QR code.");
                Console.ReadKey();
                Process.Start(fichier);
            }
            else
            {
                fini = false;
                Console.WriteLine("Mot null ou chaîne de caractère trop long");
            }
            Console.ReadKey();
            return fini;
        }



    }
}
