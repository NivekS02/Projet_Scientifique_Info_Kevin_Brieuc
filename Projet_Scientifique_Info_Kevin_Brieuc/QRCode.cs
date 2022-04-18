using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Projet_Scientifique_Info_Kevin_Brieuc
{
    internal class QRCode
    {
        //Alphanumeric Mode
        //mode character capacities : 25
        //mode Indicator : 0010        
        //exemple : je veux coder "Hello World" --> 11 caractères --> 11 en binaire = 1011 
        //--> étendre à 9 bits : 000001011 --> ajouter le mode indicator : 0010 000001011
        //Ensuite on code le mot en lui même en se référant à la table alphanumérique des lettres
        /*
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
        space 36
        $ 37
        % 38
        * 39
        + 40
        - 41
        . 42
        / 43
        : 44
        */
        // On prend les 2 premières lettres 
        // multiply the first number by 45, then add that to the second number
        // Convertir le résultat en binaire sur 11 bits ou 6 bits si le nombre est pair
    }
}