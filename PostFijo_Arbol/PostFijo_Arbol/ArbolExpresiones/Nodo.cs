using System;
using System.Collections.Generic;
using System.Text;

namespace PostFijo_Arbol.ArbolExpresiones
{
    public class Nodo
    {
        public string ItemExpresion { get; set; }
        public List<string> First { get; set; }
        public List<string> Last { get; set; }
        public List<string> Follow { get; set; }
        public Nodo IzqNodo { get; set; } //Hijo izquierdo
        public Nodo DrchNodo { get; set; } //Hijo derecho
        public bool EsHoja { get; set; }
        public int NumNodo { get; set; } //Servirá para guardar la numeración del nodo en caso de ser hoja

        public Nodo()
        {
            ItemExpresion = string.Empty;

            First = new List<string>();
            Last = new List<string>();
            Follow = new List<string>();

            IzqNodo = null;
            DrchNodo = null;

            EsHoja = true;
            NumNodo = -1;
        }
    }
}
