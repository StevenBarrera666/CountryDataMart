using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security;
using Microsoft.VisualBasic;
namespace ADO
{
    public class pproteccBasecs
    {
        private string patron_busqueda = @"uvfgA\BHIJ8xbcR7Q@VXZ02Wza3Un5Llm{6MY4FTNS1GDEKkiOhoypqtjrd9CePws .!#$%/()=¿*-+|[],;:_?'¡";
        private string Patron_encripta = @"#QV¡\XPwsa[()3K+J8|zL$l;m6,MY_*4S=1GDku-vfgABHI x%b!?cR7iZ02/WEpqtjrd9CeO:ho¿yUn5FTN.{]@'";

        public string EncriptarCadena(string cadena)
        {
            int idx;
            string result = "";
            for (idx = 0; idx <= cadena.Length - 1; idx++)
                result += EncriptarCaracter(cadena.Substring(idx, 1), cadena.Length, idx);
            return result;
        }
        public string EncriptarSHA512(string cadena)
        {
            byte[] data = new byte[81];
            System.Text.Encoding enc = new System.Text.ASCIIEncoding();
            data = enc.GetBytes(cadena);
            byte[] result;
            SHA512Managed shaM = new SHA512Managed();
            result = shaM.ComputeHash(data);
            return Convert.ToBase64String(result);
        }

        //public string EncriptarSHA1(string cadena)
        //{
        //    object sha;

        //    sha = new SHA1CryptoServiceProvider();
        //    byte[] bytesToHash;
        //    bytesToHash = System.Text.Encoding.UTF8.GetBytes(cadena);
        //    bytesToHash = sha.ComputeHash(bytesToHash);

        //    string encPassword = "";
        //    foreach (byte b in bytesToHash)
        //        encPassword += b.ToString("x2");

        //    return encPassword;
        //}

        private string EncriptarCaracter(string caracter, int variable, int a_indice)
        {
            string caracterEncriptado = "";
            int indice;
            if (patron_busqueda.IndexOf(caracter) != -1)
            {
                indice = (patron_busqueda.IndexOf(caracter) + variable + a_indice) % patron_busqueda.Length;
                return Patron_encripta.Substring(indice, 1);
            }
            return caracter;
        }

        public string DesEncriptarCadena(string cadena)
        {
            int idx;
            string result = "";
            for (idx = 0; idx <= cadena.Length - 1; idx++)
                result += DesEncriptarCaracter(cadena.Substring(idx, 1), cadena.Length, idx);
            return result;
        }

        private string DesEncriptarCaracter(string caracter, int variable, int a_indice)
        {
            int indice;
            if (Patron_encripta.IndexOf(caracter) != -1)
            {
                if ((Patron_encripta.IndexOf(caracter) - variable - a_indice) > 0)
                    indice = (Patron_encripta.IndexOf(caracter) - variable - a_indice) % Patron_encripta.Length;
                else
                    indice = (patron_busqueda.Length) + ((Patron_encripta.IndexOf(caracter) - variable - a_indice) % Patron_encripta.Length);
                indice = indice % Patron_encripta.Length;
                return patron_busqueda.Substring(indice, 1);
            }
            else
                return caracter;
        }
    }
}


