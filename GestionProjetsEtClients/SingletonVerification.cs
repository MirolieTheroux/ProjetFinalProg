﻿using Org.BouncyCastle.Asn1.Crmf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GestionProjetsEtClients
{
    internal class SingletonVerification
    {
        static SingletonVerification instance;

        public SingletonVerification() { }

        public static SingletonVerification getInstance()
        {
            if (instance == null)
                instance = new SingletonVerification();

            return instance;
        }

        public bool isTexteNonVideEtNonNum(string valeur)
        {
            string nonNumeric = "^[0-9]";
            Regex reg = new Regex(nonNumeric, RegexOptions.IgnoreCase);

            if (!string.IsNullOrEmpty(valeur.Trim()) && !reg.IsMatch(valeur))
                return true;
            else
                return false;
        }

        public bool isNumTelValide(string valeur)
        {
            string numTel = "^\\(?([0-9]{3})\\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$";
            Regex reg = new Regex(numTel, RegexOptions.IgnoreCase);

            if (!string.IsNullOrEmpty(valeur.Trim()) && reg.IsMatch(valeur))
                return true;
            else
                return false;
        }

        public bool isTexteNonVideEtNum(string valeur)
        {
            string nonNumeric = "^[0-9]";
            Regex reg = new Regex(nonNumeric, RegexOptions.IgnoreCase);

            if (!string.IsNullOrEmpty(valeur.Trim()) && reg.IsMatch(valeur))
                return true;
            else
                return false;
        }

        public bool isChampValide(string adresse)
        {
            if (!string.IsNullOrEmpty(adresse.Trim()))
                return true;
            else
                return false;
        }

        public bool isDateValide(string date)
        {
            if (date == "")
                return false;

            int annee = Convert.ToInt32(date.Substring(0, 4));
            int difference = DateTime.Now.Year - annee;
            if (difference >= 18 && difference <= 65)
                return true;
            else
                return false;
        }
        public bool isDateEmbaucheValide(string date)
        {
            if (date == "")
                return false;
            if (DateTime.Now > Convert.ToDateTime(date))
                return true;
            else
                return false;

        }
        public bool isDateDebutValide(string date)
        {
            if (date == "")
                return false;
            else if(DateTime.Today <= Convert.ToDateTime(date) && DateTime.Today.AddYears(1) >= Convert.ToDateTime(date))
                return true;
            else
                return false;
        }

        public bool isCourrielValide(string courriel)
        {
            string email = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
            Regex reg2 = new Regex(email, RegexOptions.None);

            if (!string.IsNullOrEmpty(courriel.Trim()) && reg2.IsMatch(courriel))
                return true;
            else
                return false;
        }
        public bool isTauxHValide(double th)
        {

            if (th >= 15 && th <= 100)
                return true;
            else
                return false;
        }

    public bool isLienValide(string lien)
    {
        if (Uri.IsWellFormedUriString(lien, UriKind.Absolute))
            return true;
        else
            return false;
    }

    public bool isStatutValide(int index)
    {
        if (index >= 0)
            return true;
        else
            return false;
    }
    public bool isDataTooLong(string data, int longueur)
    {
        if (data.Length >= longueur)
            return true;
        else
            return false;
    }

}
}
