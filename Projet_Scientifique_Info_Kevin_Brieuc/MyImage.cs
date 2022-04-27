using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Numerics;

namespace Projet_Scientifique_Info_Kevin_Brieuc
{
    internal class MyImage
    {
        #region attributs
        //Partie traitement d'image
        private string typeImage;
        private int tailleFichier;
        private int tailleOffset;
        private int hauteur;
        private int largeur;
        private int nbrDeBitsParCouleur;
        private Pixel[,] image;
        private string fileName;

        //Partie QR Code
        private string IndicateurNombreCaractere; //Binaire en fonction de la longueur de la chaine de caractere 
        private string CaractereBinaire;
        private string IndicateurDeMode = "0010";
        private string ChaineDeCaractereBinaire;
        private byte[] ChaineByte; // 
        private int[] ChaineInt; // Chaine binaire en Int afin de pouvoir le convertir en bytes
        private string ChaineBinaireCorrige; // Chaine de caractère en binaire
        private char[,] FinalQR;

        // A = noir ; B = Blanc ; C = à remplir 
        // Pixel blanc = 0 ; pixel noir = 1
        //111011111000100
        private char[,] CodeQR1 = new char[21, 21] //QR code version 1
        { 
        {'A','A','A','A','A','A','A','B','B', 'C', 'C', 'C', 'C', 'B','A', 'A','A','A','A','A', 'A' },
        {'A','B','B','B','B','B','A','B','B', 'C', 'C', 'C', 'C', 'B','A', 'B','B','B','B','B', 'A' },
        {'A','B','A','A','A','B','A','B','A', 'C', 'C', 'C', 'C', 'B','A', 'B','A','A','A','B', 'A' },
        {'A','B','A','A','A','B','A','B','B', 'C', 'C', 'C', 'C', 'B','A', 'B','A','A','A','B', 'A' },
        {'A','B','A','A','A','B','A','B','B', 'C', 'C', 'C', 'C', 'B','A', 'B','A','A','A','B', 'A' },
        {'A','B','B','B','B','B','A','B','B', 'C', 'C', 'C', 'C', 'B','A', 'B','B','B','B','B', 'A' },
        {'A','A','A','A','A','A','A','B','A', 'B', 'A', 'B', 'A', 'B','A', 'A','A','A','A','A', 'A' },
        {'B','B','B','B','B','B','B','B','A', 'C', 'C', 'C', 'C', 'B','B', 'B','B','B','B','B', 'B' },
        {'A','A','A','B','A','A','A','A','A', 'C', 'C', 'C', 'C', 'A','A', 'B','B','B','A','B', 'B' },
        {'C','C','C','C','C','C','B','C','C', 'C', 'C', 'C', 'C', 'C','C', 'C','C','C','C','C', 'C' },
        {'C','C','C','C','C','C','A','C','C', 'C', 'C', 'C', 'C', 'C','C', 'C','C','C','C','C', 'C' },
        {'C','C','C','C','C','C','B','C','C', 'C', 'C', 'C', 'C', 'C','C', 'C','C','C','C','C', 'C' },
        {'C','C','C','C','C','C','A','C','C', 'C', 'C', 'C', 'C', 'C','C', 'C','C','C','C','C', 'C' },
        {'B','B','B','B','B','B','B','B','A', 'C', 'C', 'C', 'C', 'C','C', 'C','C','C','C','C', 'C' },
        {'A','A','A','A','A','A','A','B','A', 'C', 'C', 'C', 'C', 'C','C', 'C','C','C','C','C', 'C' },
        {'A','B','B','B','B','B','A','B','A', 'C', 'C', 'C', 'C', 'C','C', 'C','C','C','C','C', 'C' },
        {'A','B','A','A','A','B','A','B','A', 'C', 'C', 'C', 'C', 'C','C', 'C','C','C','C','C', 'C' },
        {'A','B','A','A','A','B','A','B','B', 'C', 'C', 'C', 'C', 'C','C', 'C','C','C','C','C', 'C' },
        {'A','B','A','A','A','B','A','B','A', 'C', 'C', 'C', 'C', 'C','C', 'C','C','C','C','C', 'C' },
        {'A','B','B','B','B','B','A','B','A', 'C', 'C', 'C', 'C', 'C','C', 'C','C','C','C','C', 'C' },
        {'A','A','A','A','A','A','A','B','A', 'C', 'C', 'C', 'C', 'C','C', 'C','C','C','C','C', 'C' },
        };

        private char[,] CodeQR2 = new char[25, 25] //QR code version 2
        {
        {'A','A','A','A','A','A','A','B','B','C','C','C','C','C','C','C','C','B','A','A','A','A','A','A','A' },
        {'A','B','B','B','B','B','A','B','B','C','C','C','C','C','C','C','C','B','A','B','B','B','B','B','A' },
        {'A','B','A','A','A','B','A','B','A','C','C','C','C','C','C','C','C','B','A','B','A','A','A','B','A' },
        {'A','B','A','A','A','B','A','B','B','C','C','C','C','C','C','C','C','B','A','B','A','A','A','B','A' },
        {'A','B','A','A','A','B','A','B','B','C','C','C','C','C','C','C','C','B','A','B','A','A','A','B','A' },
        {'A','B','B','B','B','B','A','B','B','C','C','C','C','C','C','C','C','B','A','B','B','B','B','B','A' },
        {'A','A','A','A','A','A','A','B','A','B','A','B','A','B','A','B','A','B','A','A','A','A','A','A','A' },
        {'B','B','B','B','B','B','B','B','A','C','C','C','C','C','C','C','C','B','B','B','B','B','B','B','B' },
        {'A','A','A','B','A','A','A','A','A','C','C','C','C','C','C','C','C','A','A','B','B','B','A','B','B' },
        {'C','C','C','C','C','C','B','C','C','C','C','C','C','C','C','C','C','C','C','C','C','C','C','C','C' },
        {'C','C','C','C','C','C','A','C','C','C','C','C','C','C','C','C','C','C','C','C','C','C','C','C','C' },
        {'C','C','C','C','C','C','B','C','C','C','C','C','C','C','C','C','C','C','C','C','C','C','C','C','C' },
        {'C','C','C','C','C','C','A','C','C','C','C','C','C','C','C','C','C','C','C','C','C','C','C','C','C' },
        {'C','C','C','C','C','C','B','C','C','C','C','C','C','C','C','C','C','C','C','C','C','C','C','C','C' },
        {'C','C','C','C','C','C','A','C','C','C','C','C','C','C','C','C','C','C','C','C','C','C','C','C','C' },
        {'C','C','C','C','C','C','B','C','C','C','C','C','C','C','C','C','C','C','C','C','C','C','C','C','C' },
        {'C','C','C','C','C','C','A','C','C','C','C','C','C','C','C','C','A','A','A','A','A','C','C','C','C' },
        {'B','B','B','B','B','B','B','B','A','C','C','C','C','C','C','C','A','B','B','B','A','C','C','C','C' },
        {'A','A','A','A','A','A','A','B','A','C','C','C','C','C','C','C','A','B','A','B','A','C','C','C','C' },
        {'A','B','B','B','B','B','A','B','A','C','C','C','C','C','C','C','A','B','B','B','A','C','C','C','C' },
        {'A','B','A','A','A','B','A','B','A','C','C','C','C','C','C','C','A','A','A','A','A','C','C','C','C' },
        {'A','B','A','A','A','B','A','B','B','C','C','C','C','C','C','C','C','C','C','C','C','C','C','C','C' },
        {'A','B','A','A','A','B','A','B','A','C','C','C','C','C','C','C','C','C','C','C','C','C','C','C','C' },
        {'A','B','B','B','B','B','A','B','A','C','C','C','C','C','C','C','C','C','C','C','C','C','C','C','C' },
        {'A','A','A','A','A','A','A','B','A','C','C','C','C','C','C','C','C','C','C','C','C','C','C','C','C' }
        };
        #endregion
        #region Propriétés
        public string TypeImage
            {
            get { return typeImage; }
            set { typeImage = value; }
            }
        public int TailleFichier
        {
            get { return tailleFichier; }
            set { tailleFichier = value; }
        }
        public int TailleOffset
        {
            get { return tailleOffset; }
            set { tailleOffset = value; }
        }
        public int Hauteur
        {
            get { return hauteur; }
            set { hauteur = value; }
        }
        public int Largeur
        {
            get { return largeur; }
            set { largeur = value; }
        }
        public int NbrDeBitsParCouleur
        {
            get { return nbrDeBitsParCouleur; }
            set { nbrDeBitsParCouleur = value; }
        }
        public string FileName
        {
            get { return fileName; }
            set { fileName = value; }
        }
        public Pixel[,] Image
        {
            get { return image; }
            set { image = value; }
        }

        public string chaineBinaireCorrige
        {
            get { return ChaineBinaireCorrige; }
            set { ChaineBinaireCorrige = value; }
        }

        #endregion
        #region Constructeurs
        public MyImage(string myfile) // Contructeur pour le traitement d'image 
        {
            byte[] file = File.ReadAllBytes(myfile);
            this.fileName = myfile; //myfile est un vecteur composé d'octets représentant les métadonnées et les données de l'image
            if (file[0] == 66 && file[1] == 77 && file[28] == 24 && file[29] == 0) //lecture du header et assignement des variables d'instance et des bits par pixels 
            {
                this.typeImage = "BM";
                this.nbrDeBitsParCouleur = 24;
            }
            else this.typeImage = "Non BM";

            //Conversion de la taille du fichier Little Endian en entier
            byte[] TailleFichier = new byte[4];
            int TF = 2;
            for (int i = 0; i < 4; i++)
            {
                TailleFichier[i] = file[TF];
                TF++;
                
            }
            this.tailleFichier = Convert_Endian_To_Int(TailleFichier);

            //Conversion de la largeur 
            byte[] Largeur = new byte[4];
            int TL = 18;
            for (int i = 0; i < 4; i++)
            {
                Largeur[i] = file[TL];
                TL++;
            }
            this.largeur = Convert_Endian_To_Int(Largeur);

            //Conversion de la hauteur
            byte[] Hauteur = new byte[4];
            int TH = 22;
            for (int i = 0; i < 4; i++)
            {
                Hauteur[i] = file[TH];
                TH++;
            }
            this.hauteur = Convert_Endian_To_Int(Hauteur);

            //Conversion de la taille offset 
            byte[] TailleOffset = new byte[4];
            int TO = 10;
            for (int i = 0; i < 4; i++)
            {
                TailleOffset[i] = file[TO];
                TO++;
            }
            this.tailleOffset = Convert_Endian_To_Int(TailleOffset);

            Pixel[,] image = new Pixel[hauteur ,largeur];
            int k = 54;
            int l = 0;
            for (int i = 0; i < this.hauteur; i++)
            {
                for (int j = 0; j < this.largeur; j++)
                {
                    Pixel pixel = new Pixel(file[k + l], file[k + l + 1], file[k + l + 2]);
                    image[i, j] = pixel;
                    l += 3;
                }
            }
            this.image = image;
        }
        public MyImage() // Constructeur pour la fractale 
        {
            this.typeImage = "BM";
            this.tailleOffset = 54;
            this.hauteur = 1000;
            this.largeur = 1000;
            Pixel[,] image = new Pixel[hauteur, largeur];
            for (int i = 0; i < hauteur; i++)
            {
                for (int j = 0; j < largeur; j++)
                {
                    image[i, j] = new Pixel(0, 0, 0);
                }
            }
        }
        public MyImage(string ChaîneDeCaracteres, int longueur) // Constructeur pour le QR code 
        {
            string nbcar = ""; //On initialise une variable stockant les bits de nombre de la longueur de la chaîne de caractère
            int[] id =  ConvertirLongueurEnBinaire(longueur); // On convertit la longueur de la chaine de caractère en binaire
            for (int i = 0; i < id.Length; i++) nbcar = nbcar + id[i]; // On le concatène
            this.IndicateurNombreCaractere = nbcar;
            this.CaractereBinaire = ConvertirChaineDeCaractereEnBinaire(ChaîneDeCaracteres); // On convertit toute la chaîne de caractère en binaire 

            if (longueur <= 25) //Version 1 ==> Nombre d’octets pour la gestion EC = 7
            {
                this.hauteur = 21;
                this.largeur = 21;
                this.ChaineDeCaractereBinaire = FinitionChaineBinaire(this.CaractereBinaire, 152); // On concatène le mode + la longueur de la chaine en binaire + la chaine en binaire
                this.ChaineInt = StringToTabInt(ChaineDeCaractereBinaire); // On convertit ce dernier en un tableau de int afin de faciliter les calculs 
                ChaineByte = new byte[19];
                int compteur = 0;
                for (int i = 0; i < 19; i++)
                {
                    int[] tab = new int[8];
                    for(int j = 0;j< 8; j++)
                    {
                        tab[j] = ChaineInt[compteur];
                        compteur++;
                    }
                    ChaineByte[i] = BinaireToByte(tab);
                }
                byte[] solomon = ReedSolomonAlgorithm.Encode(ChaineByte, 7, ErrorCorrectionCodeType.QRCode);
                // concaténation de la chaine solomon convertie en binaire et mise en string avec
                //  le string ChaineDeCaractereBinaire qui donnent la ChaineBinaireCorrige
                string BinaireSolomon = "";              
                for (int i = 0; i < solomon.Length; i++)
                {
                    for (int j = 0; j < 8; j++)
                    {
                        BinaireSolomon = BinaireSolomon + ByteToBinaire(solomon[i])[j];
                    }
                }
                this.ChaineBinaireCorrige = ChaineDeCaractereBinaire + BinaireSolomon;
                //PlacementBitsQRB();
                PlacementBitsQRK();
                RecadrageEnBlanc(CodeQR1); //On rajoute les contours blancs 
                LireQRCode(FinalQR); // On créé l'image du QR code en couleurs
            }
            else //Version 2 ==> Nombre d’octets pour la gestion EC = 10
            {
                this.largeur = 25;
                this.hauteur = 25;
                this.ChaineDeCaractereBinaire = FinitionChaineBinaire(this.CaractereBinaire, 272);
                this.ChaineInt = StringToTabInt(ChaineDeCaractereBinaire);
                ChaineByte = new byte[34];
                int compteur = 0;
                for (int i = 0; i < 34; i++)
                {
                    int[] tab = new int[8];
                    for (int j = 0; j < 8; j++)
                    {
                        tab[j] = ChaineInt[compteur];
                        compteur++;
                    }
                    ChaineByte[i] = BinaireToByte(tab);
                }
                byte[] solomon = ReedSolomonAlgorithm.Encode(ChaineByte, 10, ErrorCorrectionCodeType.QRCode);
                // concaténation de la chaine solomon convertie en binaire et mise en string avec
                //  le string ChaineDeCaractereBinaire qui donnent la ChaineBinaireCorrige
                string BinaireSolomon = "";
                for (int i = 0; i < solomon.Length; i++)
                {
                    for (int j = 0; j < 8; j++)
                    {
                        BinaireSolomon = BinaireSolomon + ByteToBinaire(solomon[i])[j];
                    }
                }
                this.ChaineBinaireCorrige = ChaineDeCaractereBinaire + BinaireSolomon;
                this.ChaineBinaireCorrige = ChaineBinaireCorrige + "0000000"; //on ajoute des bits de remplissage
                //PlacementBitsQRB();
                PlacementBitsQRK();
                RecadrageEnBlanc(CodeQR2);
                LireQRCode(FinalQR);
            }
        }
        #endregion
        #region Méthodes Traitement d'images
        #region TD2 Lire et écrire une image à partir d'un format .bmp (From image to file)
        /// <summary>
        /// Méthode permettant de 
        /// </summary>
        /// <param name="file"></param>
        public void From_Image_To_File(string file)
        {
            List<byte> FileSave = new List<byte>();
            List<byte> FileCopy = new List<byte>(File.ReadAllBytes(fileName)); // On copie tous les bytes dans une liste
            for (int i = 0; i < 54; i++)    //Construction du header
            {
                FileSave.Add(FileCopy[i]);
            }
            for (int i = 0; i < 4; i++) // On modifie les données sur les dimensions de l'image et la taille du fichier
            {
                FileSave[2 + i] = Convert_Int_To_Endian((image.Length * 3) + 54)[i]; // Nouvelle taille du fichier
                FileSave[18 + i] = Convert_Int_To_Endian(image.GetLength(1))[i]; // Nouvelle largeur de l'image 
                FileSave[22 + i] = Convert_Int_To_Endian(image.GetLength(0))[i]; // Nouvelle hauteur de l'image
                FileSave[35 + i] = Convert_Int_To_Endian(image.Length)[i]; // Nouvelle taille de l'image
            }
            // Lecture de l'image elle même
            
            for (int i = 0; i < image.GetLength(0); i++)
            {
                int k = 0;
                for (int j = 0; j < image.GetLength(1); j++)
                {
                    FileSave.Add(this.image[i, j].B);
                    FileSave.Add(this.image[i, j].V);
                    FileSave.Add(this.image[i, j].R);
                    k++;
                }
                while(k%4 != 0) // Si la largeur n'est pas un multiple de 4, on rajoute des 0
                {
                    FileSave.Add(0);
                    k++;
                }
            }
            // Ecriture dans le fichier
            File.WriteAllBytes(file, FileSave.ToArray());
        }
        
        /// <summary>
        /// Méthode permettant de
        /// </summary>
        /// <param name="file"></param>
        public void From_Image_To_FileNouvelleImage(string file)
        {
            List<byte> FileSave = new List<byte>();
            //Construction du header
            FileSave.Add(66); // format BMP
            FileSave.Add(77);
            for (int i = 0; i < 4; i++) FileSave.Add(Convert_Int_To_Endian((image.Length * 3) + 54)[i]); // Nouvelle taille du fichier
            for (int i = 0; i < 4; i++) FileSave.Add(0); //Ajout de bits de stockage 
            FileSave.Add(54); //taille de l'en-tête (header)
            for (int i = 0; i < 3; i++) FileSave.Add(0); // On rajoute des 0 pour les 4 octets du header
            FileSave.Add(40); //taille du header info
            for (int i = 0; i < 3; i++) FileSave.Add(0); // On rajoute des 0 pour les 4 octets du header info
            for (int i = 0; i < 4; i++) FileSave.Add(Convert_Int_To_Endian(image.GetLength(1))[i]); //Largeur de l'image
            for (int i = 0; i < 4; i++) FileSave.Add(Convert_Int_To_Endian(image.GetLength(0))[i]); //hauteur de l'image
            FileSave.Add(1); //Le nombre de plans (toujours 1 0 pour nous)
            FileSave.Add(0);
            FileSave.Add(24); 
            FileSave.Add(0);
            for (int i = 0; i < 4; i++) FileSave.Add(0);
            for (int i = 0; i < 4; i++) FileSave.Add(Convert_Int_To_Endian(image.Length)[i]);
            for (int i = 0; i < 16; i++) FileSave.Add(0);

            // Lecture de l'image elle-même en ajoutant les bytes des différents pixels dans la liste 
            for (int i = 0; i < image.GetLength(0); i++)
            {
                int k = 0;
                for (int j = 0; j < image.GetLength(1); j++)
                {
                    FileSave.Add(this.image[i, j].B);
                    FileSave.Add(this.image[i, j].V);
                    FileSave.Add(this.image[i, j].R);
                    k++;
                }
                while (k % 4 != 0) // Si la largeur n'est pas un multiple de 4, on rajoute des 0
                {
                    FileSave.Add(0);
                    k++;
                }
            }
            // Ecriture dans le fichier
            File.WriteAllBytes(file, FileSave.ToArray());
        }

        /// <summary>
        /// Méthode permettant de convertir un tableau de bytes ,codé en little endian, en entier
        /// </summary>
        /// <param name="tab"></param>
        /// <returns></returns>
        public int Convert_Endian_To_Int(byte[] tab)
        {
                int taille = tab.Length;
                int entier = 0;
                int nbr = 1;
                for(int i = 0; i<taille; i++)
                {
                    entier += tab[i] * nbr;
                    nbr = nbr * 256;
                }
                return entier;
        }
        /// <summary>
        /// Méthode permettant de convertir un entier en tableau de bytes codé en little endian
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
	    public byte[] Convert_Int_To_Endian(int val)
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
                        x = x / 256;
                    }
                }
            return tab;
        }

        /// <summary>
        /// Permet d'afficher les bytes contenus dans chaque pixel de l'image
        /// </summary>
        public void AfficherMatrice()
        {
            for(int i = 0; i<hauteur; i++)
            {
                for(int j = 0; j<largeur; j++)
                {
                    Console.Write(Image[i, j].B + ",");
                    Console.Write(Image[i, j].V + ",");
                    Console.Write(Image[i, j].R + ",");
                    Console.Write(" ");
                }
                Console.WriteLine();
                Console.WriteLine();
            }
        }
        /// <summary>
        /// Permet d'afficher les informations principales de l'image dans la console
        /// </summary>
        public void toString() // Info sur l'image
        {
            Console.WriteLine("Taille fichier : " + tailleFichier + "\n" +
                "Type d'image :" + typeImage + "\n" +
                "Hauteur : " + hauteur + "\n" +
                "Largeur : " + largeur + "\n" +
                "Taille Offset : " + tailleOffset + "\n" +
                "Nb de Bits par couleur : " + nbrDeBitsParCouleur + "\n" +
                "Filename : " + fileName);
        }
        #endregion
        #region TD3 Traiter une image (nuance de gris, noir et blanc, agrandir/retrecir, rotation, miroir)
        /// <summary>
        /// Permet de changer chaque pixel de l'image en nuance de gris
        /// </summary>
        public void NuancesDeGris()
        {            
            for (int i = 0; i < image.GetLength(0); i++)
            {
                for (int j = 0; j < image.GetLength(1); j++)
                {
                    byte gris = Convert.ToByte((image[i, j].B + image[i, j].V + image[i, j].R) / 3);
                    image[i, j].B = gris;
                    image[i, j].V = gris;
                    image[i, j].R = gris;
                }
            }          
        }

        /// <summary>
        /// Permet de changer chaque pixel de l'image en noir ou en blanc suivant les valeurs des bytes de ce pixel
        /// </summary>
        public void ImageNoirEtBlanc()
        {
            for (int i = 0; i < image.GetLength(0); i++)
            {
                for (int j = 0; j < image.GetLength(1); j++)
                {
                    byte gris = Convert.ToByte((image[i, j].B + image[i, j].V + image[i, j].R) / 3);
                    if (gris <= 128)
                    {
                        image[i, j].B = 0;
                        image[i, j].V = 0;
                        image[i, j].R = 0;
                    }
                    else
                    {
                        image[i, j].B = 255;
                        image[i, j].V = 255;
                        image[i, j].R = 255;
                    }
                }
            }
        }

        /// <summary>
        /// Permet d'agrandir l'image du ratio voulu par l'utilisateur
        /// </summary>
        /// <param name="ratio"></param>
        public void Agrandir(int ratio)
        {
            Pixel[,] imageAgrandie = new Pixel[hauteur * ratio, largeur * ratio]; 
            for (int i = 0; i < imageAgrandie.GetLength(0); i++)
            {
                for (int j = 0; j < imageAgrandie.GetLength(1); j++)
                {
                    imageAgrandie[i,j] = image[i/ratio,j/ratio];
                }
            }
            hauteur = hauteur * ratio;
            largeur = largeur * ratio;
            this.image = imageAgrandie;
        }

        /// <summary>
        /// Permet rétrecir l'image du ratio voulu par l'utilisateur
        /// </summary>
        /// <param name="ratio"></param>
        public void Retrecir(int ratio)
        {
            Pixel[,] imageRetrecie = new Pixel[hauteur / ratio, largeur / ratio]; 
            for (int i = 0; i < imageRetrecie.GetLength(0); i++)
            {
                for (int j = 0; j < imageRetrecie.GetLength(1); j++)
                {
                    imageRetrecie[i,j] = image[i*ratio,j*ratio];
                }
            }
            hauteur = hauteur / ratio;
            largeur = largeur / ratio;
            this.image = imageRetrecie;
        }
        /// <summary>
        /// Permet d'effectuer une rotation de l'image d'un angle de 90° dans le sens antihoraire
        /// </summary>
        public void Rotation()
        {
            Pixel[,] ImageFinale = new Pixel[largeur, hauteur];
            int l = image.GetLength(1) - 1;
            int k = 0;
            for(int i=0; i<image.GetLength(1); i++)
            {
                for(int j=0; j<image.GetLength(0); j++)
                {
                    ImageFinale[i, j] = image[k, l];
                    k++;
                }
                k = 0;
                l--;
            }
            this.image = ImageFinale;
            this.hauteur = image.GetLength(0);
            this.largeur = image.GetLength(1);
        }

        /// <summary>
        /// Permet d'effectuer une rotation de l'image de l'angle souhaité par l'utilisateur dans le sens anti-horaire
        /// </summary>
        /// <param name="angle"></param>
        public void Rotation2(double angle)
        {
            angle = angle * Math.PI / 180; //x/PI = angle/180 => x = PI*angle/180
            double [] coorPol = CartésienneEnPolaire(image.GetLength(0)-1,image.GetLength(1)-1);
            int nouvelleLargeur = (int)(Math.Sin(coorPol[1]) * coorPol[0]*3);
            coorPol = CartésienneEnPolaire(0,image.GetLength(1)-1);
            int nouvelleHauteur = (int)Math.Abs(Math.Cos(coorPol[1]) * coorPol[0]*3);
            while (nouvelleLargeur % 4 != 0)
                nouvelleLargeur++;
            Pixel[,] ImageRotation = new Pixel[nouvelleHauteur, nouvelleLargeur];
            for (int i = 0; i < image.GetLength(0); i++)
            {
                for (int j = 0; j < image.GetLength(1); j++)
                {
                    int[] nouvellesCoor = PolaireAjoutAngleRemiseCartesienne(CartésienneEnPolaire(j, i), angle, nouvelleLargeur, nouvelleHauteur);
                    int nouveauI = nouvellesCoor[0];
                    int nouveauJ = nouvellesCoor[1];
                    ImageRotation[nouveauI, nouveauJ] = image[i, j];
                }
            }
            for(int i =0; i < ImageRotation.GetLength(0); i++)
            {
                for(int j=0; j<ImageRotation.GetLength(1); j++)
                {
                    if(ImageRotation [i,j]== null)
                    {
                        ImageRotation[i, j] = new Pixel(255, 255, 255);
                    }
                }
            }
            this.image = ImageRotation;
            this.hauteur = image.GetLength(0);
            this.largeur = image.GetLength(1);
        }

        /// <summary>
        /// Permet de passer de coordonneés cartésiennes à coordonnées polaire 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public double [] CartésienneEnPolaire (int x, int y)
        {
            double xMilieu = (largeur-1) / 2.0;
            double yMilieu = (hauteur-1) / 2.0;
            double xRelatif = x - xMilieu;
            double yRelatif = y - yMilieu;

            double r = Math.Sqrt(xRelatif * xRelatif + yRelatif * yRelatif); // r² = x² + y²
            double alpha = Math.Atan2(yRelatif, xRelatif); // alpha = arctan2(x,y)

            return new double[] { r, alpha }; // coordonnées cylindriques
        }

        /// <summary>
        /// Permet de passer de coordonnées polaire à cartésiennes en ayant ajouté l'angle entré par l'utilisateur
        /// </summary>
        /// <param name="coorPolaire"></param>
        /// <param name="angle"></param>
        /// <param name="nouvelleLargeur"></param>
        /// <param name="nouvelleHauteur"></param>
        /// <returns></returns>
        public int[] PolaireAjoutAngleRemiseCartesienne(double[] coorPolaire, double angle, int nouvelleLargeur, int nouvelleHauteur)
        {
            double r = coorPolaire[0];
            double alpha  = coorPolaire[1] + angle;

            double xRelatif = r * Math.Cos(alpha);
            double yRelatif = r * Math.Sin(alpha);

            double xMilieu = (largeur - 1) / 2.0;
            double yMilieu = (hauteur - 1) / 2.0;
            if (nouvelleLargeur !=0 && nouvelleHauteur != 0)
            {
                xMilieu = (nouvelleLargeur - 1) / 2.0;
                yMilieu = (nouvelleHauteur - 1) / 2.0;
            }

            int i = (int)(yRelatif + yMilieu);
            int j = (int)(xRelatif + xMilieu);

            return new int[] {  i,  j  };
        }

        /// <summary>
        /// Permet d'inverser la droite et la gauche de l'image (effet miroir)
        /// </summary>
        public void Miroir()
        {
            Pixel[,] ImageMiroir = new Pixel[hauteur, largeur];
            /*
            for(int i=0; i<image.GetLength(0); i++)
            {
                for(int j=0; j<image.GetLength(1); j++)
                {
                    ImageMiroir[i, j] = image[i, (image.GetLength(1) - 1 - j) % image.GetLength(1)];
                }
            }
            */
            for (int i = 0; i < image.GetLength(0); i++)
            {
                for (int j = 0; j < image.GetLength(1)/2; j++)
                {
                    ImageMiroir[i, j] = image[i, image.GetLength(1) - 1 - j];
                    ImageMiroir[i, ImageMiroir.GetLength(1) - 1 - j] = image[i, j];
                }
            }
            this.image = ImageMiroir;
        }

        /// <summary>
        /// Permet d'inverser le haut et le bas de l'image 
        /// </summary>
        public void MiroirHorizontal()
        {
            Pixel[,] ImageMiroir = new Pixel[hauteur, largeur];
            
            for(int i=0; i<hauteur; i++)
            {
                for(int j=0; j<largeur; j++)
                {
                    
                        ImageMiroir[i, j] = image[hauteur - 1 - i, j];
                    
                }
            }
            this.image = ImageMiroir;
        }
        #endregion
        #region TD4 Appliquer un filtre (matrice de convolution)
        /// <summary>
        /// Permet de faire la somme des produits de la matrice de convolution avec la matrice de kernel 
        /// </summary>
        /// <param name="matriceCopie"></param>
        /// <param name="i"></param>
        /// <param name="j"></param>
        /// <param name="kernel"></param>
        /// <returns></returns>
        public Pixel CalculConvolution(Pixel[,] matriceCopie, int i, int j, double[,] kernel)
        {
            double[] pixel = new double[]{ 0,0,0};
            for(int x = 0; x<kernel.GetLength(0); x++)
            {
                for(int y = 0; y<kernel.GetLength(1); y++)
                {
                    //Coordonnées des voisins du pixel actuel en face des éléments du noyau
                    int CoorI = i - kernel.GetLength(0) / 2 + x;
                    int CoorJ = j - kernel.GetLength(1) / 2 + y;

                    //Seuillage à 0 (en gros si ça dépasse on le ramène au bord où il a dépassé
                    if(CoorI < 0)
                        CoorI = 0;
                    if(CoorI >= matriceCopie.GetLength(0))
                        CoorI = matriceCopie.GetLength(0) - 1;
                    if(CoorJ < 0)
                        CoorJ = 0;
                    if(CoorJ >= matriceCopie.GetLength(1))
                        CoorJ = matriceCopie.GetLength(1) - 1;

                    pixel[0] += matriceCopie[CoorI,CoorJ].B * kernel[x, y];
                    pixel[1] += matriceCopie[CoorI,CoorJ].V * kernel[x, y];
                    pixel[2] += matriceCopie[CoorI,CoorJ].R * kernel[x, y];
                }
            }
            //On mets toutes les valeurs en bytes (en cas de dépassement on les remet à la limite)
            for(int k=0;k<3;k++)
                if(pixel[k]<0)
                    pixel[k] = 0;
                else if (pixel[k] > 255)
                    pixel[k] = 255;
            
            return new Pixel((byte)pixel[0], (byte)pixel[1], (byte)pixel[2]);
        }
        /// <summary>
        /// Permet d'appliquer la matrice de convolution entrée par l'utilisateur à l'image
        /// </summary>
        /// <param name="kernel"></param>
        public void Convolution(double [,] kernel)
        {
            Pixel[,] matriceCopie = new Pixel[image.GetLength(0) , image.GetLength(1)];

            for(int i = 0; i < image.GetLength(0) ; i++)
                for(int j = 0; j < image.GetLength(1) ; j++)
                    matriceCopie[i,j] = image[i,j];

            for (int i = 0; i < image.GetLength(0); i++)
            {
                for (int j = 0; j < image.GetLength(1); j++)
                {
                    image[i, j] = CalculConvolution(matriceCopie, i, j, kernel);
                }
            }
        }
        #endregion
        #region TD5 Créer ou extraire une nouvelle image (fractale de Mandelbrot, histogramme, Coder et décoder dans une image)
        public int SuiteDeMandelBrot(int iteration, Complex c, Complex z, int n = 0)
        {
            if (z.Magnitude > 2 || iteration <= 0)
            {
                return n;
            }
            iteration--;
            n++;
            return SuiteDeMandelBrot(iteration, c, z * z + c, n);
        }
        /// <summary>
        /// Permet de créer la fractale de Mandelbrot
        /// </summary>
        public void FractaleMandelbrot()
        {
            Pixel[,] ImageCopie = new Pixel[hauteur, largeur];
            for (int i = 0; i < hauteur; i++)
            {
                for (int j = 0; j < largeur; j++)
                {
                    ImageCopie[i, j] = new Pixel(0, 0, 0);
                }
            }
            Pixel[] couleurs = { //ALLEZ CHERCHER DES COULEURS (tapez color picker sur google)
                new Pixel(255, 255, 255),//Du plus éloigné au plus proche
                new Pixel(3, 1, 26),
                new Pixel(9, 1, 47),
                new Pixel(4, 4, 73),
                new Pixel(0, 7, 100),
                new Pixel(12, 44, 138),
                new Pixel(24, 82, 177),
                new Pixel(57, 125, 209),
                new Pixel(134, 181, 229),
                new Pixel(211, 236, 248),
                new Pixel(241, 233, 191),
                new Pixel(248, 201, 95),
                new Pixel(255, 170, 0),
                new Pixel(204, 128, 0),
                new Pixel(153, 87 ,0),
                new Pixel(255, 255, 255) //ça c'est le milieu j'ai en blanc pour que vous sachiez quoi changer mais changez c'est moche
            };
            int iteration = 50;
            for (int i = 0; i < hauteur; i++)
                for (int j = 0; j < largeur; j++)
                {
                    Complex z = new Complex(0, 0);
                    Complex p = new Complex(3 * (j - 3 * largeur / 4) / (double)largeur, 3 * (i - hauteur / 2) / (double)largeur); //le 3* et le /4 c'est pour centrer au milieu
                    int color = 15 * (iteration - SuiteDeMandelBrot(iteration, p, z)) / iteration;
                    ImageCopie[i, j] = couleurs[15 - color];
                }
            this.image = ImageCopie;
        }
        /// <summary>
        /// Permet de créer un histogramme de l'image 
        /// </summary>
        public void Histogramme() 
        {
        Pixel[,] histogramme = new Pixel[256, 256];
        for (int i = 0; i < histogramme.GetLength(0); i++)
        {
            for (int j = 0; j < histogramme.GetLength(1); j++)
            {
                histogramme[i, j] = new Pixel(0, 0, 0); // on crée donc une matrice image avec seulement des pixels noirs
            }
        }

        //matrice de 3 lignes (ROuge Vert Bleu de 256 colonnes) avec le nombre de pixel ayant tel valeur de V B R au bon endroit
        int[,] tableauHistogramme = new int[3,256];
        
        // calcul du nombre de pixel ayant la même valeur de B puis de R puis de V 
        for (int k = 0; k < 256; k++)
        {
            int CompteurBleu = 0;
            int CompteurRouge = 0;
            int CompteurVert = 0;
            for (int i = 0; i < hauteur; i++)
            {
                for(int j = 0; j < largeur; j++)
                {
                    if(image[i, j].B == k)
                    {
                        CompteurBleu++;
                    }
                    if (image[i, j].V == k)
                    {                                     
                        CompteurVert++;            
                    }
                    if (image[i, j].R == k)
                    {                 
                        CompteurRouge++;                
                    }

                }
            }
            tableauHistogramme[0, k] = CompteurBleu ;
            tableauHistogramme[1, k] = CompteurVert ;
            tableauHistogramme[2, k] = CompteurRouge;  
        }   
        // Recherche du maximum dans tableauHistogramme
        int max = 0;
        for (int i = 0; i < tableauHistogramme.GetLength(0); i++)
        {
            for (int j = 0; j < tableauHistogramme.GetLength(1);j++)
            {
                if (tableauHistogramme[i,j] > max)
                {
                    max = tableauHistogramme[i,j];
                }
            }
        }
        //Création de l'histogramme
        for (int i = 0; i < 256; i++)
        {
            for (int j = 0; j < 256 * tableauHistogramme[0,i]/max ; j++)
            {
                histogramme[j,i].B = 255;
            }
            for (int j = 0; j < 256 * tableauHistogramme[1,i]/max ; j++)
            {
                histogramme[j,i].V = 255;
            }
            for (int j = 0; j < 256 * tableauHistogramme[2,i]/max ; j++)
            {
                histogramme[j,i].R = 255;
            }
        }
        
        image = histogramme;            
        }
        /// <summary>
        /// Permet de cacher une petite image dans une image plus grande
        /// </summary>
        /// <param name="image_a_cacher"></param>
        public void Cacher_Image(MyImage image_a_cacher)
        {
            if(image_a_cacher.image.Length <= image.Length)
            {
                for(int i=0; i<image_a_cacher.hauteur;i++)
                {
                    for(int j=0; j<image_a_cacher.largeur; j++)
                    {
                        image[i, j].B = BinaireToByte(FusionTableauBinaire(ByteToBinaire(image[i, j].B), ByteToBinaire(image_a_cacher.image[i, j].B)));
                        image[i, j].R = BinaireToByte(FusionTableauBinaire(ByteToBinaire(image[i, j].R), ByteToBinaire(image_a_cacher.image[i, j].R)));
                        image[i, j].V = BinaireToByte(FusionTableauBinaire(ByteToBinaire(image[i, j].V), ByteToBinaire(image_a_cacher.image[i, j].V)));
                    }
                }
                Console.WriteLine("L'image a été cachée");
            }
            else Console.WriteLine("L'image à cacher est trop grande pour être cachée");
        }


        /// <summary>
        /// Permet de faire apparaitre l'image cachée dans l'image principale
        /// </summary>
        public void Decrypter_Image()
        {
            for (int i = 0; i < image.GetLength(0); i++)
            {
                for (int j = 0; j < image.GetLength(1); j++)
                {
                    image[i, j].B = BinaireToByte(InversionTableauBinaire(ByteToBinaire(image[i, j].B)));
                    image[i, j].R = BinaireToByte(InversionTableauBinaire(ByteToBinaire(image[i, j].R)));
                    image[i, j].V = BinaireToByte(InversionTableauBinaire(ByteToBinaire(image[i, j].V)));
                }
            }
        }


        /// <summary>
        /// Permet de convertir un byte en tableau d'entiers binaires
        /// </summary>
        /// <param name="pixel"></param>
        /// <returns></returns>
        public int [] ByteToBinaire (byte pixel)
        {
            int pxl = (int)(pixel);
            int[] tab = new int[8];
            int puissance = 128;
            for (int i = 0; i < 8; i++)
            {
                if (pxl / puissance != 0)
                {
                    pxl -= puissance;
                    tab[i] = 1;
                    puissance = puissance / 2;

                }
                else
                {
                    tab[i] = 0;
                    puissance = puissance / 2;
                }
            }
            return tab;
        }

        /// <summary>
        /// Permet de convertir un tableau d'entiers binaires en byte
        /// </summary>
        /// <param name="tab"></param>
        /// <returns></returns>
        public byte BinaireToByte(int[]tab)
        {
            int somme = 0;
            int puissance = 128;
            for(int i=0; i<8;i++)
            {
                somme += tab[i] * puissance;
                puissance = puissance / 2;
            }
            return (byte)(somme);
        }

        /// <summary>
        /// Permet de fusionner deux tableaux de 4 entiers binaires en un tableau de 8 entiers binaires
        /// </summary>
        /// <param name="tabNormal"></param>
        /// <param name="tabCaché"></param>
        /// <returns></returns>
        public int[] FusionTableauBinaire (int [] tabNormal, int [] tabCaché)
        {
            int[] tabFinal = new int[8];
            for (int i = 0; i < 4; i++) tabFinal[i] = tabNormal[i];
            for (int i = 4; i < 8; i++) tabFinal[i] = tabCaché[i-4];
            return tabFinal;
        }

        /// <summary>
        /// Permet d'inverser les bits de poids forts et de poids faible d'un tableau de 8 entiers binaire
        /// </summary>
        /// <param name="tab"></param>
        /// <returns></returns>
        public int[] InversionTableauBinaire(int[] tab)
        {
            int[] retour = new int[8];
            for (int i = 0; i < 4; i++) retour[i] = tab[i+ 4];
            for (int i = 4; i < 8; i++) retour[i] = tab[i - 4];
            return retour;
        }

        #endregion
        #endregion
        #region Méthodes QRCode
        #region Fonctions principales
        /// <summary>
        /// Méthode permettant de convertir la longueur de la chaine de caractère en binaire
        /// </summary>
        /// <param name="longueur"></param>
        /// <returns> tableau d'entiers </returns>
        public int[] ConvertirLongueurEnBinaire (int longueur)
        {
            int[] tab = new int[9];
            int puissance = 256 ;
            for (int i = 0; i < 9; i++)
            {
                if (longueur / puissance != 0)
                {
                    longueur -= puissance;
                    tab[i] = 1;
                    puissance = puissance / 2;

                }
                else
                {
                    tab[i] = 0;
                    puissance = puissance / 2;
                }
            }
            return tab;
        }

        /// <summary>
        /// Méthode permettant de convertir un nombre en binaire sur 6 bits 
        /// </summary>
        /// <param name="nbr"></param>
        /// <returns> string contenant le nombre entré converti en binaire sur 6 bits </returns>

        public string ConvertirIntEn6Bits(int nbr)
        {
            string bits = "";
            int puissance = 32;
            for (int i = 0; i < 6; i++)
            {
                if (nbr / puissance != 0)
                {
                    nbr -= puissance;
                    bits += 1;
                    puissance = puissance / 2;

                }
                else
                {
                    bits += 0;
                    puissance = puissance / 2;
                }
            }
            return bits;
        }

        /// <summary>
        /// Méthode permettant de convertir un nombre en binaire sur 11 bits
        /// </summary>
        /// <param name="nbr"></param>
        /// <returns> string contenant le nombre entré converti en binaire sur 11 bits </returns>
        public string ConvertirIntEn11Bits(int nbr)
        {
            string bits = "";
            int puissance = 1024;
            for (int i = 0; i < 11; i++)
            {
                if (nbr / puissance != 0)
                {
                    nbr -= puissance;
                    bits += 1;
                    puissance = puissance / 2;

                }
                else
                {
                    bits += 0;
                    puissance = puissance / 2;
                }
            }
            return bits;
        }

        /// <summary>
        /// Méthode permettant, à partir de la chaine de caractère entrée par l'utilisateur, de retourner une chaine de caractère contenant des '0' et des '1', 
        /// qui correspondent, suivant le code alphanumérique, à chaque caractère de la chaine
        /// </summary>
        /// <param name="ChaîneDeCaracteres"></param>
        /// <returns> string contenant la chaine entrée convertie en binaire </returns>
        public string ConvertirChaineDeCaractereEnBinaire(string ChaîneDeCaracteres)
        {
            string [] binaire;
            string chaineBinaire = "";
            if (ChaîneDeCaracteres.Length%2 != 0)
            {
                binaire = new string[ChaîneDeCaracteres.Length / 2 + 1];
            }
            else
            {
                binaire = new string[ChaîneDeCaracteres.Length/2];
            }

            int compteur = 0;
            for (int i = 0; i < ChaîneDeCaracteres.Length; i+=2)
            {
                if (ChaîneDeCaracteres.Length%2 != 0 && i == ChaîneDeCaracteres.Length - 1) // si on arrive au bout de la chaine de caractère à longueur impaire
                {
                    binaire[compteur] = ConvertirCaractèreEnBinaire(Convert.ToString(ChaîneDeCaracteres[i]));
                }
                else
                {
                    binaire[compteur] = ConvertirCaractèreEnBinaire(Convert.ToString(ChaîneDeCaracteres[i]) + Convert.ToString(ChaîneDeCaracteres[i + 1]));
                    compteur++;
                }
            }

            foreach (string str in binaire)
            {
                chaineBinaire = chaineBinaire + str;
            }
            return chaineBinaire;
        }

        /// <summary>
        /// Méthode permettant de convertir un string contenant un caractère en string de bits correspondant
        /// </summary>
        /// <param name="caractère"></param>
        /// <returns></returns>

        public string ConvertirCaractèreEnBinaire(string caractère)
        {
            string binaire = "";
            if(caractère.Length%2 != 0)
            {
                binaire = ConvertirIntEn6Bits(Alphanumérique(caractère[0]));
            }
            else
            {
                binaire = ConvertirIntEn11Bits(45 * Alphanumérique(caractère[0]) + Alphanumérique(caractère[1]));
            }
            return binaire;
        }

        /// <summary>
        /// Retourne un entier correspondant au caractère "lettre" rentré, suivant la table alphanumérique
        /// </summary>
        /// <param name="lettre"></param>
        /// <returns></returns>
        public int Alphanumérique(char lettre)
        {
            int alpha = 0;
            if((int)lettre >= 48 && (int)lettre <=57) alpha = (int)lettre - 48; // pour les chiffres
            else if ((int)lettre >= 65 && (int)lettre <= 90) alpha = (int)lettre - 55; // Pour les lettres (attention : seuls les majuscules marchent)
            else if ((int)lettre == 32 || (int)lettre == 37 ) alpha = (int)lettre + 4;// space ; - 
            else if ((int)lettre == 36) alpha = (int)lettre + 1;// $ 
            else if ((int)lettre == 42 || (int)lettre == 43) alpha = (int)lettre - 3;// * ; +
            else if ((int)lettre == 45 || (int)lettre == 46 ||  (int)lettre == 47) alpha = (int)lettre - 4; // - ; . ; /
            return alpha;
        }
        /// <summary>
        /// Ajoute l'indicateur de mode, l'indicateur de longueur de la chaîne de caractère initialement rentrée, 
        /// puis ajuste la taille de cet élément pour atteindre 152 bits pour la version 1 et 272 pour la version 2 suivant des règles précises
        /// Retourne cette nouvelle chaîne de caractère
        /// </summary>
        /// <param name="chaineBinaire"></param>
        /// <param name="longueur"></param>
        /// <returns></returns>
        public string FinitionChaineBinaire(string chaineBinaire,int longueur)
        {
            string retour =  IndicateurDeMode + IndicateurNombreCaractere + chaineBinaire;

            if (retour.Length < longueur - 3)
            {
                retour += "0000";
            }
            else
            {
                while (retour.Length != longueur) retour += "0";
            }
            while(retour.Length%8 != 0)
            {
                retour += "0";
            }

            while (retour.Length != longueur)
            {
                retour = retour + "11101100";
                if (retour.Length != longueur)
                {
                    retour = retour + "00010001";
                }
            }
            return retour;
        }
        public int[] StringToTabInt(string chaineBinaire)
        {
            int[] tabInt = new int[chaineBinaire.Length];
            for (int i = 0; i< chaineBinaire.Length; i++)
            {
                if (chaineBinaire[i] == '0')
                {
                    tabInt[i] = 0;
                }
                else
                {
                    tabInt[i] = 1;
                }

            }
            return tabInt;
        }
        
        /// <summary>
        /// Méthode permettant de placer des '0' et des '1' dans la matrice de caractères CodeQR1 ou CodeQR2 suivant le mode utilisé (respectivement 1 ou 2)
        /// </summary>
        public void PlacementBitsQRK()
        {
            int i = hauteur - 1;
            int j = largeur - 1;
            int longueur = 0;
            int AllerRetour;
            char[,] QR;
            char masque = '0';

            if (hauteur == 21)
            {
                AllerRetour = 5;// pour un QR code version 1
                QR = CodeQR1;
            }
            else
            {
                AllerRetour = 6;// pour un QR code version 2
                QR = CodeQR2;
            }

            for (int n = 0; n < AllerRetour; n++)
            {
                
                for (int k = 0; k < hauteur; k++)
                {
                    if (QR[i, j] == 'C')
                    {
                        if ((i + j) % 2 == 0) masque = '1';
                        else masque = '0';
                        if (ChaineBinaireCorrige[longueur] == masque) QR[i, j] = '0';
                        else QR[i, j] = '1';
                        j--;
                        longueur++;
                        if ((i + j) % 2 == 0) masque = '1';
                        else masque = '0';
                        if (ChaineBinaireCorrige[longueur] == masque) QR[i, j] = '0';
                        else QR[i, j] = '1';
                        longueur++;
                        j++;
                    }
                    else if (QR[i, j - 1] == 'C')
                    {
                        j--;
                        if ((i + j) % 2 == 0) masque = '1';
                        else masque = '0';
                        if (ChaineBinaireCorrige[longueur] == masque) QR[i, j] = '0';
                        else QR[i, j] = '1';
                        longueur++;
                        j++;
                    }
                    i--;
                }
                i++;
                j -= 2;
                if (j == 6) j--;
                for (int k = 0; k < hauteur; k++)
                {
                    if (QR[i, j] == 'C')
                    {
                        if ((i + j) % 2 == 0) masque = '1';
                        else masque = '0';
                        if (ChaineBinaireCorrige[longueur] == masque) QR[i, j] = '0';
                        else QR[i, j] = '1';
                        j--;
                        longueur++;
                        if ((i + j) % 2 == 0) masque = '1';
                        else masque = '0';
                        if (ChaineBinaireCorrige[longueur] == masque) QR[i, j] = '0';
                        else QR[i, j] = '1';
                        longueur++;
                        j++;
                    }
                    else if (QR[i, j - 1] == 'C')
                    {
                        j--;
                        if ((i + j) % 2 == 0) masque = '1';
                        else masque = '0';
                        if (ChaineBinaireCorrige[longueur] == masque) QR[i, j] = '0';
                        else QR[i, j] = '1';
                        longueur++;
                        j++;
                    }
                    i++;
                }
                j -= 2;
                i--;
            }
            if (hauteur == 21) CodeQR1 = QR;
            else CodeQR2 = QR;
        }

        /// <summary>
        /// Méthode permettant de placer des '0' et des '1' dans la matrice de caractères CodeQR1 ou CodeQR2 suivant le mode utilisé (respectivement 1 ou 2)
        /// </summary>
        public void PlacementBitsQRB()
        {          
            char[,] QR;
            int compteur = 0; //position dans la chaine binaire
            char masque;

            if (hauteur == 21 && largeur == 21) 
            {
                QR = CodeQR1; //Version 1
            }
            else 
            {
                QR = CodeQR2; //Version 2
            }

            for (int j = 0; j < largeur; j += 4)
            {
                for (int i = 0; i < hauteur; i++)
                {
                    if ((hauteur - 1 - i + largeur - 1 - j) % 2 == 0) masque = '0';
                    else masque = '1';
                    if (QR[hauteur - 1 - i, largeur - 1 - j] == 'C' && masque == ChaineBinaireCorrige[compteur])
                    {
                        QR[hauteur - 1 - i, largeur - 1 - j] = '1';
                        compteur++;
                    }
                    else if(QR[hauteur - 1 - i, largeur - 1 - j] == 'C' && masque != ChaineBinaireCorrige[compteur])
                    {
                        QR[hauteur - 1 - i, largeur - 1 - j] = '0';
                        compteur++;
                    }

                    if ((hauteur - 1 - i + largeur - 2 - j) % 2 == 0) masque = '0';
                    else masque = '1';
                    if (QR[hauteur - 1 - i, largeur - 2 - j] == 'C' && masque == ChaineBinaireCorrige[compteur])
                    {
                        QR[hauteur - 1 - i, largeur - 2 - j] = '1';
                        compteur++;
                    }
                    else if(QR[hauteur - 1 - i, largeur - 2 - j] == 'C' && masque != ChaineBinaireCorrige[compteur])
                    {
                        QR[hauteur - 1 - i, largeur - 2 - j] = '0';
                        compteur++;
                    }
                }

                if (j == largeur - 9) //Permet d'éviter la 7e colonnne (d'indide 6)
                {
                    j++;
                }

                for (int i = 0; i < hauteur; i++)
                {
                    if ((hauteur - 1 - i + largeur - 3 - j) % 2 == 0) masque = '0';
                    else masque = '1';
                    if (QR[i, largeur - 3 - j] == 'C' && masque == ChaineBinaireCorrige[compteur])
                    { 
                        QR[i,largeur - 3 - j] = '1';
                        compteur++;
                    }
                    else if (QR[i, largeur - 3 - j] == 'C' && masque != ChaineBinaireCorrige[compteur])
                    {
                        QR[i, largeur - 3 - j] = '0';
                        compteur++;
                    }

                    if ((hauteur - 1 - i + largeur - 4 - j) % 2 == 0) masque = '0';
                    else masque = '1';
                    if (QR[i, largeur - 4 - j] == 'C' && masque == ChaineBinaireCorrige[compteur])
                    {
                        QR[i, largeur - 4 - j] = '1';
                        compteur++;
                    }
                    else if (QR[i, largeur - 4 - j] == 'C' && masque != ChaineBinaireCorrige[compteur])
                    {
                        QR[i, largeur - 4 - j] = '0';
                        compteur++;
                    }
                }
            }
            if (hauteur == 25 && largeur == 25) CodeQR2 = QR; //Version 2
            else CodeQR1 = QR; //Version 1          
        }
        /// <summary>
        /// Méthode permettant d'ajouter un bord blanc autour du QRCode pour faciliter la lecture de celui-ci
        /// Prend en paramètre la matrice QR composée de caractères '0' et '1'
        /// </summary>
        /// <param name="QR"></param>
        /// /// <summary>
        /// Permet de lire notre matrice de QR code en char et de les convertir en pixel pour créer une matrice de pixel image
        /// </summary>
        /// <param name="QR"></param>
        #endregion
        #region Fonctions secondaires de mise en forme du QR code
        public void LireQRCode(char[,] QR)
        {
            Pixel[,] Picture = new Pixel[QR.GetLength(0), QR.GetLength(1)];
            for (int i = 0; i < QR.GetLength(0); i++)
            {
                for (int j = 0; j < QR.GetLength(1); j++)
                {
                    if (QR[i, j] == 'A' || QR[i, j] == '1')
                    {
                        Picture[i, j] = new Pixel(0, 0, 0);
                    }
                    else if (QR[i, j] == 'B' || QR[i, j] == '0')
                    {
                        Picture[i, j] = new Pixel(255, 255, 255);
                    }
                    else if (QR[i, j] == 'C')
                    {
                        Picture[i, j] = new Pixel(128, 128, 128);
                    }
                }
            }
            this.image = Picture;
        }
        public void RecadrageEnBlanc(char[,] QR)
        {
            char[,] NewTab = new char[hauteur + 2, largeur + 2];
            for(int i = 0; i<hauteur+2; i++)
            {
                for (int j = 0; j < largeur + 2; j++)
                {
                    NewTab[i, j] = 'B';
                }
            }
            
            for (int i = 0; i < hauteur ; i++)
            {
                for (int j = 0; j < largeur; j++)
                {
                    NewTab[i+1, j+1] = QR[i,j];
                }
            }
            this.hauteur = this.hauteur + 2;
            this.largeur = this.largeur + 2;
            this.FinalQR = NewTab; 
        }
        /*
         ====> -55 pour les lettres et -48 pour les chiffres
        0 0
        1 1
        2 2
        3 3
        4 4
        5 5
        6 6
        7 7
        8 8
        9 9
        A 10
        B 11
        C 12
        D 13
        E 14
        F 15
        G 16
        H 17
        I 18
        J 19
        K 20
        L 21
        M 22
        N 23
        O 24
        P 25
        Q 26
        R 27
        S 28
        T 29
        U 30
        V 31
        W 32
        X 33
        Y 34
        Z 35
        space 36   ==> il faut ajouter 4 ==> ca marche 
        $ 37 ===> ajouter 1  ==> ca marche 
        % 38 ===> ajouter 1
        * 39 ===> soustraire 3 ==> ca marche 
        + 40 ===> soustraire 3 ==> ca marche
        - 41 ===> ajouter 4 ==> ca marche 
        . 42 ===> soustraire 4 ==> ca marche
        / 43 ===> soustraire 4 ==> ca marche 
        : 44 ===> soustraire 14
        */
        #endregion
        #endregion
        #region Fonctions bonus
        /// <summary>
        /// Passe l'image en négatif
        /// </summary>
        public void Negatif()
        {
            for (int i = 0; i < image.GetLength(0); i++)
            {
                for (int j = 0; j < image.GetLength(1); j++)
                {
                    image[i, j].B = (byte)(255 - image[i, j].B);
                    image[i, j].V = (byte)(255 - image[i, j].V);
                    image[i, j].R = (byte)(255 - image[i, j].R);
                }
            }
        }

        /// <summary>
        /// Passe l'image en sépia
        /// </summary>
        public void Sepia()
        {
            int s = 100;
            for (int i = 0; i < image.GetLength(0); i++)
            {
                for (int j = 0; j < image.GetLength(1); j++)
                {
                    int m = (image[i, j].B + image[i, j].V + image[i, j].R) / 3;
                    if (m < s)
                    {
                        int c = m / s;
                        image[i, j].B = (byte)(18 * c);
                        image[i, j].V = (byte)(38 * c);
                        image[i, j].R = (byte)(94 * c);
                    }
                    else
                    {
                        int c = (m - s) / (255 - s);
                        image[i, j].B = (byte)(18 + c * (255 - 18));
                        image[i, j].V = (byte)(38 + c * (255 - 38));
                        image[i, j].R = (byte)(94 + c * (255 - 94));
                    }
                }
            }
        }

        /// <summary>
        /// Ajoute de la luminosité à l'image
        /// </summary>
        public void AjouterLuminosite(int intensité)
        {
            for (int i = 0; i < image.GetLength(0); i++)
            {
                for (int j = 0; j < image.GetLength(1); j++)
                {
                    if (image[i, j].B < 255 - intensité && image[i, j].V < 255 - intensité && image[i, j].R < 255 - intensité)
                    {
                        image[i, j].B = (byte)(intensité + image[i, j].B);
                        image[i, j].V = (byte)(intensité + image[i, j].V);
                        image[i, j].R = (byte)(intensité + image[i, j].R);
                    }
                }
            }
        }

        /// <summary>
        /// Diminue la luminosité de l'image
        /// </summary>
        public void DiminuerLuminosite(int intensité)
        {
            for (int i = 0; i < image.GetLength(0); i++)
            {
                for (int j = 0; j < image.GetLength(1); j++)
                {
                    if (image[i, j].B > intensité - 1 && image[i, j].V > intensité - 1 && image[i, j].R > intensité - 1)
                    {
                        image[i, j].B = (byte)(image[i, j].B - intensité);
                        image[i, j].V = (byte)(image[i, j].V - intensité);
                        image[i, j].R = (byte)(image[i, j].R - intensité);
                    }
                }
            }
        }
        #endregion
    }
}

