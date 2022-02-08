using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        #region LectureImage
        byte[] myfile = File.ReadAllBytes(myfile);
        //myfile est un vecteur composé d'octets représentant les métadonnées et les données de l'image


        //lecture du header et assignement des variables d'instance       
        if (myfile[0] == 66 && myfile[1] == 77)
        {
            this.typeImage = "BM";
        }
        this.tailleFichier = myfile[2] * 1 + myfile[3] * 256 + myfile[4] * 65536 + myfile[5] * 16777216;
        this.largeur = myfile[18] * 1 + myfile[19] * 256 + myfile[20] * 65536 + myfile[21] * 16777216;
        this.hauteur = myfile[22] * 1 + myfile[23] * 256 + myfile[24] * 65536 + myfile[25] * 16777216;
        this.tailleOffset = myfile[14] * 1 + myfile[15] * 256 + myfile[16] * 65536 + myfile[17] * 16777216;
        this.bitsParCouleur = myfile[28] * 1 + myfile[29] * 256;

        int k = 54;
        for (int i = 0; i < this.hauteur; i++)
        {
            for (int j = 0; j < this.largeur; j++)
            {
                this.image[i, j] = myfile[k];
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
