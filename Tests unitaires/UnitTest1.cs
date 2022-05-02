using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Projet_Scientifique_Info_Kevin_Brieuc;

namespace Tests_unitaires
{
    public class Tests
    {
        [Test]
        public void QRCode()
        {
            MyImage QRCode = new MyImage("HELLO WORLD", 11);         
            byte[] solomon = ReedSolomonAlgorithm.Encode(QRCode.chaineByte, 7, ErrorCorrectionCodeType.QRCode);
            byte[] reedsolomon = new byte[] {209,239,196,207,78,195,109};


            Assert.AreEqual(QRCode.chaineDeCaractereBinaire.Length, 152); // On vérifie que la chaine de caractère corrigée sans reed solomon fait bien la bonne longueur
            Assert.AreEqual(QRCode.chaineDeCaractereBinaire, "00100000010110110000101101111000110100010111001011011100010011010100001101000000111011000001" +
            "000111101100000100011110110000010001111011000001000111101100"); // on vérifie que cette même chaine est cohérente par rapport à celle donnée dans l'énoncé
            
            Assert.AreEqual(solomon, reedsolomon); // On vérifie que le reedsolomon trouvé pour "HELLO WORLD" correspond à celui donné dans le sujet

            Assert.AreEqual(QRCode.chaineBinaireCorrige, "001000000101101100001011011110001101000101110010110111000100110101000011" +
            "0100000011101100000100011110110000010001111011000001000111101100000100011110110011010001111011111100010011001111" +
            "010011101100001101101101"); // On vérifie que la chaine binaire finale est la même que celle donnée dans l'énoncé
        }

        [Test]
        public void Images()
        {
            MyImage image = new MyImage("coco.bmp");
            //image.ImageNoirEtBlanc();
            //image.NuancesDeGris();
            //image.Miroir();
            //image.MiroirHorizontal();
            //image.Negatif();
            //image.Sepia();
            image.From_Image_To_File("resultat.bmp");

            //On vérifie que l'image a les bonnes dimensions
            Assert.AreEqual(image.Hauteur, 200);
            Assert.AreEqual(image.Largeur, 320);
            Assert.AreNotEqual(image.Hauteur, 1200);
            Assert.AreNotEqual(image.Largeur, 80);

            //Vérification du format de l'image
            Assert.AreEqual(image.TypeImage, "BM");
            Assert.AreNotEqual(image.TypeImage, "AM");
            Assert.AreEqual(image.NbrDeBitsParCouleur, 24);
            Assert.AreNotEqual(image.NbrDeBitsParCouleur, 67);

            //Vérification de la taille de l'image
            Assert.AreEqual(image.TailleFichier, 192054);
            Assert.AreNotEqual(image.TailleFichier, 192004);
            Assert.AreEqual(image.TailleOffset, 54);
            Assert.AreNotEqual(image.TailleOffset, 35);


        }


    }
}