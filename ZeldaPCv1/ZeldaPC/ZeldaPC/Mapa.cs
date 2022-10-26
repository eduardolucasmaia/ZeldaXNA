using System;
using System.IO;

namespace WindowsGame1
{
    class Mapa
    {
        public char[,] array = null;
        int tamx = -1, tamy = -1;

        public Mapa(string nome_Arquivo_mapa)
        {
            try
            {
                bool tdOK = true;
                string[] array_str_temp = File.ReadAllLines(nome_Arquivo_mapa);
                if (array_str_temp[0].Length > 0)
                {
                    for (int i = 0; i < array_str_temp.Length - 1; i++)
                    {
                        if (array_str_temp[i].Length != array_str_temp[i + 1].Length) { tdOK = false; }
                    }
                    if (tdOK)
                    {
                        tamx = array_str_temp[0].Length;
                        tamy = array_str_temp.Length;
                        array = new char[tamx, tamy];
                        for (int b = 0; b < tamy; b++)
                        {
                            char[] array_str = array_str_temp[b].ToCharArray();
                            for (int a = 0; a < tamx; a++)
                            {
                                array[a, b] = array_str[a];
                            }
                        }
                    }
                }
                else
                {
                    array = null;
                }
            }
            catch
            {
                array = null;
            }
        }
        public bool ehValido()
        {
            if (array != null) { return true; }
            else { return false; }
        }
        public int Tamanho_X()
        {
            return tamx;
        }
        public int Tamanho_Y()
        {
            return tamy;
        }
    }
}
