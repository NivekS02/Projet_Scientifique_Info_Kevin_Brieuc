using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Projet_Scientifique_Info_Kevin_Brieuc
{
    internal class MyImage
    {
        #region attributs
        private string typeImage;
        private int tailleFichier;
        private int tailleOffset;
        private int hauteur;
        private int largeur;
        private int nbrDeBitsParCouleur;
        private Pixel[,] image;
        private string fileName;

        private int[] IndicateurNombreCaractere;
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
        #endregion
        
        public MyImage(string myfile)
        {
            byte[] file = File.ReadAllBytes(myfile);
            //Console.WriteLine("Nombre de bytes : " + file.Length);
            this.fileName = myfile;
            //myfile est un vecteur composé d'octets représentant les métadonnées et les données de l'image

            //lecture du header et assignement des variables d'instance et des bits par pixels 
            if (file[0] == 66 && file[1] == 77 && file[28] == 24 && file[29] == 0)
            {
                this.typeImage = "BM";
                this.nbrDeBitsParCouleur = 24;
            }
            else
            {
                this.typeImage = "Non BM";
            }

            //Conversion de la taille du fichier Little Endian en entier
            byte[] TailleFichier = new byte[4];
            int TF = 2;
            for (int i = 0; i < 4; i++)
            {
                TailleFichier[i] = file[TF];
                TF++;
                
            }
            this.tailleFichier = Convert_Endian_To_Int(TailleFichier);

            //Console.WriteLine(file[18] + " " + file[19] + " " + file[20] + " " + file[21]);
            //Conversion de la largeur 
            byte[] Largeur = new byte[4];
            int TL = 18;
            for (int i = 0; i < 4; i++)
            {
                Largeur[i] = file[TL];
                TL++;
            }
            this.largeur = Convert_Endian_To_Int(Largeur);
            //Console.WriteLine("Largeur initiale : " + largeur);

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
        
        public MyImage(string ChaîneDeCaracteres, int longueur)
        {
            IndicateurNombreCaractere = ConvertirIntEnBinaire(longueur);
            if(longueur <= 25)
            {

            }
            else
            {

            }
        }
        public MyImage()
        {
            this.typeImage = "BM";
            this.tailleOffset = 54;
            this.hauteur = 40;
            this.largeur = 40;
            Pixel[,] image = new Pixel[hauteur, largeur];
            for(int i = 0;i<hauteur; i++)
            {
                for(int j=0; j<largeur;j++)
                {
                    image[i, j] = new Pixel(0, 0, 0);
                }
            }
        }

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

        public void From_Image_To_FileFractale(string file)
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
            this.image = imageAgrandie;
        }
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
            this.image = imageRetrecie;
        }
        public void Rotation() //uniquement pour les angles à 90/180/270 degrés en sens antihoraire
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
        public void Rotation2(double angle)
        {
            angle = angle * Math.PI / 180;
            //x/PI = angle/180 => x = PI*angle/180

            /*
            //On cherche la largeur de la nouvelle matrice
            int nouvelleLargeur = PolaireAjoutAngleRemiseCartesienne(CartésienneEnPolaire(largeur - 1, hauteur -1) , angle)[1] + 1;
            //On cherche désormais la hauteur de la nouvelle matrice 
            int nouvelleHauteur = PolaireAjoutAngleRemiseCartesienne(CartésienneEnPolaire(0, hauteur-1), angle)[0] + 1;
            //Création de la matrice
            */

            //double AngleInitial = Math.Atan2(image.GetLength(0)-1, image.GetLength(1)-1);
            //double alpha = AngleInitial - angle  ;
            //double Hypoténuse = Math.Sqrt((0 - image.GetLength(0))* (0 - image.GetLength(0)) + (0 - image.GetLength(1)) * (0 - image.GetLength(1)));

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
                    /*
                    if (nouveauI>0 && nouveauJ>0 && nouveauI<nouvelleHauteur && nouveauJ<nouvelleLargeur)
                    {
                        
                    }
                    */
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
        public void toString()
        {
            Console.WriteLine("Taille fichier : " + tailleFichier + "\n" +
                "Type d'image :" + typeImage + "\n" +
                "Hauteur : " + hauteur + "\n" +
                "Largeur : " + largeur + "\n" +
                "Taille Offset : " + tailleOffset + "\n" +
                "Nb de Bits par couleur : " + nbrDeBitsParCouleur + "\n" +
                "Filename : " + fileName);
        }
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
        public void FractaleMandelbrot ()
        {
            Pixel[,] ImageCopie = new Pixel[hauteur, largeur];
            for (int i = 0; i < hauteur; i++)
            {
                for (int j = 0; j < largeur; j++)
                {
                    ImageCopie[i, j] = new Pixel(0, 0, 0);
                }
            }

            

            
            for (int i = 0; i<hauteur; i++)
            {
                for(int j = 0; j<largeur; j++)
                {
                    double X0 = i;
                    double Y0 = j;

                    double X = i;
                    double Y = j;

                    double NouveauRéel = ComplexeAuCaréeReel(X, Y) + X0;
                    double NouveauImaginaire = ComplexeAuCaréeImaginaire(X, Y) + Y0;

                    double module = Math.Sqrt(NouveauRéel * NouveauRéel + NouveauImaginaire * NouveauImaginaire);

                    if (module > 2)
                    {
                        ImageCopie[i,j] = new Pixel(0, 0, 255);
                    }
                    else
                    {
                        for (int k = 0; k < 15; k++)
                        {
                            X = NouveauRéel;
                            Y = NouveauImaginaire;

                            NouveauRéel = ComplexeAuCaréeReel(X, Y) + X0;
                            NouveauImaginaire = ComplexeAuCaréeImaginaire(X, Y) + Y0;

                            module = Math.Sqrt(NouveauRéel * NouveauRéel + NouveauImaginaire * NouveauImaginaire);
                            if (module > 2)
                            {
                                ImageCopie[i,j] = new Pixel(0, 0, 255);
                                k = 15;
                            }
                        }
                        if (ImageCopie[i, j].R != 255)
                        {
                            ImageCopie[i, j] = new Pixel(0, 0, 0);
                        }
                    }
                }
            }
            
            this.image = ImageCopie;
            
        }

        public double ComplexeAuCaréeReel(double X, double Y)
        {
            double re = X * X - (Y * Y);
            return re;
        }
        public double ComplexeAuCaréeImaginaire(double X, double Y)
        {
            double im = 2 * X * Y;
            return im;
        }
        public double Module(double X, double Y)
        {
            return Math.Sqrt(X * X + Y * Y);
        }
        public void Histogramme() // pas encore fini
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

            }
            else
            {
                Console.WriteLine("L'image à cacher est trop grande pour être cachée");
            }
        }
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
        public int[] FusionTableauBinaire (int [] tabNormal, int [] tabCaché)
        {
            int[] tabFinal = new int[8];
            for (int i = 0; i < 4; i++) tabFinal[i] = tabNormal[i];
            for (int i = 4; i < 8; i++) tabFinal[i] = tabCaché[i-4];
            return tabFinal;
        }
        public int[] ConvertirIntEnBinaire (int longueur)
        {
            int[] tab = new int[9];
            int puissance = 256 ;
            for (int i = 0; i < 8; i++)
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
    }
}
