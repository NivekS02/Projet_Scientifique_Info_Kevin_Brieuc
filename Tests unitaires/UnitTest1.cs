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



            Assert.AreEqual(QRCode.chaineDeCaractereBinaire.Length, 152); // On v�rifie que la chaine de caract�re corrig�e sans reed solomon fait bien la bonne longueur
            Assert.AreEqual(QRCode.chaineDeCaractereBinaire, "00100000010110110000101101111000110100010111001011011100010011010100001101000000111011000001" +
            "000111101100000100011110110000010001111011000001000111101100"); // on v�rifie que cette m�me chaine est coh�rente par rapport � celle donn�e dans l'�nonc�
            
            Assert.AreEqual(solomon, reedsolomon); // On v�rifie que le reedsolomon trouv� pour "HELLO WORLD" correspond � celui donn� dans le sujet

            Assert.AreEqual(QRCode.chaineBinaireCorrige, "001000000101101100001011011110001101000101110010110111000100110101000011" +
            "0100000011101100000100011110110000010001111011000001000111101100000100011110110011010001111011111100010011001111" +
            "010011101100001101101101"); // On v�rifie que la chaine binaire finale est la m�me que celle donn�e dans l'�nonc�
        }

        [Test]
        public void Images()
        {

        }


    }
}