using System;
using System.Collections.Generic;
using System.Text;

namespace PostFijo_Arbol.ArbolExpresiones
{
    public class ArbolExpresiones
    {
        private List<char> Operadores { get; set; }
        public Nodo Raiz { get; set; }

        //Lista que tendrá solo los nodos hoja, para que al momento de calcular
        //la tabla de transiciones se más fácil y no hay que recorrer todo el árbol
        private List<Nodo> Hojas { get; set; } 
        

        public ArbolExpresiones()
        {
            Operadores = new List<char>();
            Operadores.Add('(');
            Operadores.Add('*');
            Operadores.Add('|');
            Operadores.Add('.');
            Operadores.Add('+');

            Raiz = null;

            Hojas = new List<Nodo>();
        }

        public void CrearArbol(string expresionPostfija)
        {
            var pila = new Stack<Nodo>(); //Pila que guardará momentanemente los nodos para ir formando en orden el árbol
            var contadorHojas = 1;

            while (expresionPostfija.Length != 0 )
            {
                var nuevoNodo = new Nodo();

                if (!Operadores.Contains(expresionPostfija[0]) && expresionPostfija[0] != '#') //Si No es un operador y si NO es el estado de ACEPTACIÓN
                {
                    nuevoNodo.ItemExpresion = expresionPostfija[0].ToString();
                    nuevoNodo.NumNodo = contadorHojas;
                    nuevoNodo.EsHoja = true;
                    contadorHojas ++;

                    nuevoNodo.AsignarNulabilidad();

                    pila.Push(nuevoNodo);
                }
                else if (Operadores.Contains(expresionPostfija[0]) && expresionPostfija[0] != '#' ) //Si ES operador
                {
                    if (pila.Count >= 2  && expresionPostfija[0] != '*' && expresionPostfija[0] != '+')
                    {
                        nuevoNodo.ItemExpresion = expresionPostfija[0].ToString();

                        nuevoNodo.DrchNodo = pila.Pop();
                        nuevoNodo.IzqNodo = pila.Pop();

                        nuevoNodo.AsignarNulabilidad();
                    }
                    else
                    {
                        nuevoNodo.ItemExpresion = expresionPostfija[0].ToString();

                        nuevoNodo.AsignarNulabilidad();
                        
                        if(pila.Count > 0)
                        {
                            nuevoNodo.IzqNodo = pila.Pop();
                        }
                    }
                    pila.Push(nuevoNodo);
                }
                else if(expresionPostfija[0] == '#')
                {
                    nuevoNodo.ItemExpresion = expresionPostfija[0].ToString();
                    nuevoNodo.NumNodo = contadorHojas;
                    nuevoNodo.EsHoja = true;

                    nuevoNodo.AsignarNulabilidad();

                    Raiz = pila.Pop();
                    Raiz.DrchNodo = nuevoNodo;
                    Raiz.AsignarNulabilidad();
                }

                expresionPostfija = expresionPostfija.Remove(0, 1);
            }
        }

        public void PostOrden(Nodo raiz, ref string recorrido)
        {
            if (raiz != null)
            {
                PostOrden(raiz.IzqNodo, ref recorrido);
                PostOrden(raiz.DrchNodo, ref recorrido);
                recorrido += raiz.ItemExpresion;

                CalcularFirst(raiz);

                if (raiz.EsHoja)
                {
                    Hojas.Add(raiz);
                }
            }
        }

        public void CalcularFirst(Nodo raiz)
        {
            if (raiz.EsHoja)
            {
                raiz.First.Add(raiz);
            }
            else if(raiz.ItemExpresion == "|")
            {
                raiz.First.AddRange(raiz.IzqNodo.First);
                raiz.First.AddRange(raiz.DrchNodo.First);
            }
            else if(raiz.ItemExpresion == ".")
            {
                if (raiz.IzqNodo.Nulo)
                {
                    raiz.First.AddRange(raiz.IzqNodo.First);
                    raiz.First.AddRange(raiz.DrchNodo.First);
                }
                else
                {
                    raiz.First = (raiz.IzqNodo.First);
                }
            }
            else if(raiz.ItemExpresion == "*")
            {
                raiz.First= raiz.IzqNodo.First;
            }
        }
    }
}
