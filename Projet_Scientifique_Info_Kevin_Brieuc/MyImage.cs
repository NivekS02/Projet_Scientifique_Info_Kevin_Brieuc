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
	private string typeImage;
	private int tailleFichier;
	private int tailleOffset;
	private int hauteur;
	private int largeur;
	private int nbrDeBitsParCouleur;
	private Pixel[,] image;
    private string fileName;

        public MyImage(string myfile)
        {
            byte[] file = File.ReadAllBytes(myfile);
            this.fileName = myfile;
            //myfile est un vecteur composé d'octets représentant les métadonnées et les données de l'image

            //lecture du header et assignement des variables d'instance et des bits par pixels 
            if (file[0] == 66 && file[1] == 77 && file[28] == 24 && file[29] == 0)
            {
                this.typeImage = "BM";
                this.nbrDeBitsParCouleur = 24;
            }

            //Conversion de la taille du fichier Little Endian en entier
            byte[] TailleFichier = new byte[4];
            int TF = 2;
            for (int i = 0; i < 3; i++)
            {
                TailleFichier[i] = file[TF];
                TF++;
            }
            this.tailleFichier = Convert_Endian_To_Int(TailleFichier);

            //Conversion de la largeur 
            byte[] Largeur = new byte[4];
            int TL = 18;
            for (int i = 0; i < 3; i++)
            {
                Largeur[i] = file[TL];
                TL++;
            }
            this.largeur = Convert_Endian_To_Int(Largeur);

            //Conversion de la hauteur
            byte[] Hauteur = new byte[4];
            int TH = 22;
            for (int i = 0; i < 3; i++)
            {
                Hauteur[i] = file[TH];
                TH++;
            }
            this.hauteur = Convert_Endian_To_Int(Hauteur);

            //Conversion de la taille offset 
            byte[] TailleOffset = new byte[4];
            int TO = 10;
            for (int i = 0; i < 3; i++)
            {
                TailleOffset[i] = file[TO];
                TO++;
            }
            this.tailleOffset = Convert_Endian_To_Int(TailleOffset);

            Pixel [,] image = new Pixel[hauteur ,largeur] ;
            int k = 54;
            int l = 0;
            for (int i = 0; i < this.hauteur; i++)
            {
                for (int j = 0; j < this.largeur; j++)
                {
                    Pixel pixel = new Pixel(file[k + l], file[k + l + 1], file[k + l + 2]);
                    image[i,j] = pixel;
                    l+=3;
                }
            }
            this.image = image;
            
        }
    public void From_Image_To_File(string file)
    {
            // Lecture du Header
            byte[] FileSave = new byte[image.Length*3 + 54];
            byte[] fileCopy = File.ReadAllBytes(fileName);
            for (int i = 0; i<54; i++)    //Construction du header
            {
                FileSave[i] = fileCopy[i] ;
            }

            for (int i=0; i<3; i++) // On modifie les données sur les dimensions de l'image et la taille du fichier
            {
                FileSave[10 + i] = Convert_Int_To_Endian(image.Length*3+54)[i]; // Nouvelle taille du fichier
                FileSave[18 + i] = Convert_Int_To_Endian(image.GetLength(1))[i]; // Nouvelle largeur de l'image 
                FileSave[22 + i] = Convert_Int_To_Endian(image.GetLength(0))[i]; // Nouvelle hauteur de l'image 
                

            }

            // Lecture de l'image elle même
            int k = 54;
            int l = 0;
            for (int i = 0; i < this.hauteur; i++)
            {
                for (int j = 0; j < this.largeur; j++)
                {
                    FileSave[k + l] = this.image[i,j].B;
                    FileSave[k + l + 1] = this.image[i,j].V;
                    FileSave[k + l + 2] = this.image[i,j].R;
                    l+=3;
                }
            }

            // Ecriture dans le fichier
            File.WriteAllBytes(file, FileSave);



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
                }
            }
        return tab;
    }

    }
}
