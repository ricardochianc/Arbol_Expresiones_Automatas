using System;
using System.Collections.Generic;
using System.Text;

namespace PostFijo_Arbol.ArbolExpresiones
{
    public class ArbolExpresiones
    {
        private List<char> Operadores { get; set; }

        public ArbolExpresiones()
        {
            Operadores = new List<char>();
            Operadores.Add('(');
            Operadores.Add('*');
            Operadores.Add('|');
            Operadores.Add('.');
            Operadores.Add('+');
        }
    }
}
