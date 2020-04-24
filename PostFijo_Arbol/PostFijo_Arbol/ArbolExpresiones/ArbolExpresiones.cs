using System;
using System.Collections.Generic;
using System.Text;

namespace PostFijo_Arbol.ArbolExpresiones
{
    public class ArbolExpresiones
    {
        private List<char> Operadores { get; set; }
        private Nodo Raiz { get; set; }

        public ArbolExpresiones()
        {
            Operadores = new List<char>();
            Operadores.Add('(');
            Operadores.Add('*');
            Operadores.Add('|');
            Operadores.Add('.');
            Operadores.Add('+');

            Raiz = null;
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

                    pila.Push(nuevoNodo);
                }
                else if (Operadores.Contains(expresionPostfija[0]) && expresionPostfija[0] != '#' ) //Si ES operador
                {
                    if (pila.Count >= 2  && expresionPostfija[0] != '*' && expresionPostfija[0] != '+')
                    {
                        nuevoNodo.ItemExpresion = expresionPostfija[0].ToString();

                        nuevoNodo.DrchNodo = pila.Pop();
                        nuevoNodo.IzqNodo = pila.Pop();
                    }
                    else
                    {
                        nuevoNodo.ItemExpresion = expresionPostfija[0].ToString();
                        
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

                    Raiz = pila.Pop();
                    Raiz.DrchNodo = nuevoNodo;
                }

                expresionPostfija = expresionPostfija.Remove(0, 1);
            }
        }
    }
}
