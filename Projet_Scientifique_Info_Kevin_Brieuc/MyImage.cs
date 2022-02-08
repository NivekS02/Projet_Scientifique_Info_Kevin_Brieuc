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
	private int bitsParCouleur;
	private byte[,] image;


	public MyImage(string myfile)
    {
        byte[] file = File.ReadAllBytes(myfile);
        //myfile est un vecteur composé d'octets représentant les métadonnées et les données de l'image


        //lecture du header et assignement des variables d'instance       
        if (file[0] == 66 && file[1] == 77)
        {
            this.typeImage = "BM";
        }
        this.tailleFichier = file[2] * 1 + file[3] * 256 + file[4] * 65536 + file[5] * 16777216;
        this.largeur = file[18] * 1 + file[19] * 256 + file[20] * 65536 + file[21] * 16777216;
        this.hauteur = file[22] * 1 + file[23] * 256 + file[24] * 65536 + file[25] * 16777216;
        this.tailleOffset = file[14] * 1 + file[15] * 256 + file[16] * 65536 + file[17] * 16777216;
        this.bitsParCouleur = file[28] * 1 + file[29] * 256;

        int k = 54;
        for (int i = 0; i < this.hauteur; i++)
        {
            for (int j = 0; j < this.largeur; j++)
            {
                this.image[i, j] = file[k];
                k++;
            }
        }
    }

    
    public void From_Image_To_File(string file)
    {
           
    }
	public int Convertir_Endian_To_Int(byte[] tab)
    {

    }
	public byte[] Convert_Int_To_Endian(int val)
    {
		
    }
    



    }
}
