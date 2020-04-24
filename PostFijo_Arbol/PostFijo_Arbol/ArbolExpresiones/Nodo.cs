using System;
using System.Collections.Generic;
using System.Text;

namespace PostFijo_Arbol.ArbolExpresiones
{
    public class Nodo
    {
        public string ItemExpresion { get; set; }
        public List<Nodo> First { get; set; }
        public List<Nodo> Last { get; set; }
        public List<Nodo> Follow { get; set; }
        public Nodo IzqNodo { get; set; } //Hijo izquierdo
        public Nodo DrchNodo { get; set; } //Hijo derecho
        public bool EsHoja { get; set; }
        public int NumNodo { get; set; } //Servirá para guardar la numeración del nodo en caso de ser hoja
        public bool Nulo { get; set; } //TRUE si es NULL y FALSE si NOT NULL
        

        public Nodo()
        {
            ItemExpresion = string.Empty;

            First = new List<Nodo>();
            Last = new List<Nodo>();
            Follow = new List<Nodo>();

            IzqNodo = null;
            DrchNodo = null;

            EsHoja = false;
            NumNodo = -1;
        }
        public void AsignarNulabilidad()
        {
            if (EsHoja)
            {
                Nulo = false;
            }
            else if (ItemExpresion == "*")
            {
                Nulo = true;
            }
            else if (ItemExpresion == "+")
            {
                Nulo = false;
            }
            else if(ItemExpresion == ".")
            {
                if (DrchNodo != null)
                {
                    if (IzqNodo.Nulo && DrchNodo.Nulo)
                    {
                        Nulo = true;
                    }
                }
            }
            else if(ItemExpresion == "|")
            {
                if (IzqNodo.Nulo || DrchNodo.Nulo)
                {
                    Nulo = true;
                }
            }
        }
    }
}
