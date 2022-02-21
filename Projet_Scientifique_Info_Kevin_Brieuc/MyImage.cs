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
        public MyImage(MyImage image, int[,] matriceConvolution)
        {
            //Création de l'image modifiée
            this.largeur = image.largeur;
            this.hauteur = image.hauteur;
            Pixel[,] imageConv = image.image;
            this.typeImage = image.typeImage;
            this.tailleFichier = image.tailleFichier;
            this.tailleOffset = image.tailleOffset;
            this.nbrDeBitsParCouleur = image.nbrDeBitsParCouleur;
            this.fileName = "RésultatConv.bmp";

            //marche seulement pour des matrices de convolution 3x3
            for (int i = 0; i < hauteur; i++)
            {
                for (int j = 0; j < largeur; j++)
                {
                    if (i != 0 && j != 0 && i != hauteur - 1 && j != largeur - 1)
                    {
                        imageConv[i,j].R = imageConv[i - 1, j - 1].R * matriceConvolution[0,0] + imageConv[i - 1, j].R * matriceConvolution[0,1] + imageConv[i - 1, j + 1].R * matriceConvolution[0,2]
                        + imageConv[i, j - 1].R * matriceConvolution[1,0] + imageConv[i, j + 1].R * matriceConvolution[1,2]
                        + imageConv[i + 1, j - 1].R * matriceConvolution[2,0] + imageConv[i + 1, j].R * matriceConvolution[2,1]+ imageConv[i + 1, j + 1].R * matriceConvolution[2,2];
            
                        imageConv[i,j].B = imageConv[i - 1, j - 1].B * matriceConvolution[0,0] + imageConv[i - 1, j].B * matriceConvolution[0,1] + imageConv[i - 1, j + 1].B * matriceConvolution[0,2]
                        + imageConv[i, j - 1].B * matriceConvolution[1,0] + imageConv[i, j + 1].B * matriceConvolution[1,2]
                        + imageConv[i + 1, j - 1].B * matriceConvolution[2,0] + imageConv[i + 1, j].B * matriceConvolution[2,1]+ imageConv[i + 1, j + 1].B * matriceConvolution[2,2];                    

                        imageConv[i,j].V = imageConv[i - 1, j - 1].V * matriceConvolution[0,0] + imageConv[i - 1, j].V * matriceConvolution[0,1] + imageConv[i - 1, j + 1].V * matriceConvolution[0,2]
                        + imageConv[i, j - 1].V * matriceConvolution[1,0] + imageConv[i, j + 1].V * matriceConvolution[1,2]
                        + imageConv[i + 1, j - 1].V * matriceConvolution[2,0] + imageConv[i + 1, j].V * matriceConvolution[2,1]+ imageConv[i + 1, j + 1].V * matriceConvolution[2,2];                    
                    }    
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

        public void ImageNoirEtBlanc ()
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
        public void Rotation() //uniquement pour les angles à 90/180/270 degrés
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

            //On cherche la largeur de la nouvelle matrice
            int nouvelleLargeur = PolaireAjoutAngleRemiseCartesienne(CartésienneEnPolaire(largeur - 1, hauteur -1) , angle)[1] + 1;


            //On cherche désormais la hauteur de la nouvelle matrice 
            int nouvelleHauteur = PolaireAjoutAngleRemiseCartesienne(CartésienneEnPolaire(0, hauteur-1), angle)[0] + 1;

            //Création de la matrice
            Pixel[,] ImageRotation = new Pixel[nouvelleHauteur, nouvelleLargeur];

            for (int i = 0; i < image.GetLength(0); i++)
            {
                for (int j = 0; j < image.GetLength(1); j++)
                {
                    int[] nouvellesCoor = PolaireAjoutAngleRemiseCartesienne(CartésienneEnPolaire(j, i), angle);
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
                        ImageRotation[i, j].B = 255;
                        ImageRotation[i, j].V = 255;
                        ImageRotation[i, j].R = 255;
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

            double r = Math.Sqrt(xRelatif * xRelatif + yRelatif * yRelatif);
            double alpha = Math.Atan2(yRelatif, xRelatif);

            return new double[] { r, alpha };
        }

        public int[] PolaireAjoutAngleRemiseCartesienne(double[] coorPolaire, double angle)
        {
            double r = coorPolaire[0];
            double alpha  = coorPolaire[1] + angle;

            double xRelatif = r * Math.Cos(alpha);
            double yRelatif = r * Math.Sin(alpha);

            double xMilieu = (largeur - 1) / 2.0;
            double yMilieu = (hauteur - 1) / 2.0;

            int i = (int)(yRelatif + yMilieu);
            int j = (int)(xRelatif + xMilieu);

            return new int[] {  i,  j  };
        }
        public void Miroir()
        {
            Pixel[,] ImageMiroir = new Pixel[hauteur, largeur];
            for(int i=0; i<image.GetLength(0); i++)
            {
                for(int j=0; j<image.GetLength(1); j++)
                {
                    ImageMiroir[i, j] = image[i, (image.GetLength(1) - 1 - j) % image.GetLength(1)];
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
    }
}
