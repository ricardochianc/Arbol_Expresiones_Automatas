﻿using System;
using System.Collections.Generic;
using System.Text;

namespace PostFijo_Arbol.ArbolExpresiones
{
    public class ArbolExpresiones
    {
        private List<char> Operadores { get; set; }
        public Nodo Raiz { get; set; }

        //Lista que tendrá solo los nodos hoja, para que al momento de calcular los follow y
        //la tabla de transiciones sea más fácil y no hay que recorrer todo el árbol
        private List<Nodo> Hojas { get; set; }

        //Parte "izquierda" de la tabla de transiciones
        public List<Estado> Estados { get; set; }

        //Parte "derecha" de la tabla de transiciones
        //               <  D   ,[q1,( 5,6 )]>
        public Dictionary<string, List<Estado>> TablaTransiciones { get; set; }

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
            Estados = new List<Estado>();
            TablaTransiciones = new Dictionary<string, List<Estado>>();
        }

        /// <summary>
        /// Método para la creación del árbol basado en la notación postfija
        /// </summary>
        /// <param name="expresionPostfija"></param>
        public void CrearArbol(string expresionPostfija)
        {
            var pila = new Stack<Nodo>(); //Pila que guardará momentanemente los nodos para ir formando en orden el árbol
            var contadorHojas = 1;

            while (expresionPostfija.Length != 0)
            {
                var nuevoNodo = new Nodo();

                if (!Operadores.Contains(expresionPostfija[0]) && expresionPostfija[0] != '#') //Si No es un operador y si NO es el estado de ACEPTACIÓN
                {
                    nuevoNodo.ItemExpresion = expresionPostfija[0].ToString();
                    nuevoNodo.NumNodo = contadorHojas;
                    nuevoNodo.Follow = new List<Nodo>();
                    nuevoNodo.EsHoja = true;
                    contadorHojas++;

                    nuevoNodo.AsignarNulabilidad();

                    pila.Push(nuevoNodo);
                }
                else if (Operadores.Contains(expresionPostfija[0]) && expresionPostfija[0] != '#') //Si ES operador
                {
                    if (pila.Count >= 2 && expresionPostfija[0] != '*' && expresionPostfija[0] != '+')
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

                        if (pila.Count > 0)
                        {
                            nuevoNodo.IzqNodo = pila.Pop();
                        }
                    }
                    pila.Push(nuevoNodo);
                }
                else if (expresionPostfija[0] == '#')
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

        /// <summary>
        /// Método que recorre el árbol de manera PostOrden (izquierdo-derecho-raiz)
        /// y opera FIRST, LAST y FOLLOW
        /// </summary>
        /// <param name="raiz">Nodo árbol</param>
        /// <param name="recorrido">Variable que guarda el recorrido</param>
        public void PostOrdenOperaciones(Nodo raiz, ref string recorrido)
        {
            if (raiz != null)
            {
                PostOrdenOperaciones(raiz.IzqNodo, ref recorrido);
                PostOrdenOperaciones(raiz.DrchNodo, ref recorrido);
                recorrido += raiz.ItemExpresion;

                CalcularFirst(raiz);
                CalcularLast(raiz);

                if (raiz.EsHoja)
                {
                    Hojas.Add(raiz);
                }

                CalcularFollows(raiz);
            }
        }

        private void CalcularFirst(Nodo raiz)
        {
            if (raiz.EsHoja)
            {
                raiz.First.Add(raiz);
            }
            else if (raiz.ItemExpresion == "|")
            {
                raiz.First.AddRange(raiz.IzqNodo.First);
                raiz.First.AddRange(raiz.DrchNodo.First);
            }
            else if (raiz.ItemExpresion == ".")
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
            else if (raiz.ItemExpresion == "*" || raiz.ItemExpresion == "+")
            {
                raiz.First = raiz.IzqNodo.First;
            }
        }

        private void CalcularLast(Nodo raiz)
        {
            if (raiz.EsHoja)
            {
                raiz.Last.Add(raiz);
            }
            else if (raiz.ItemExpresion == "|")
            {
                raiz.Last.AddRange(raiz.IzqNodo.Last);
                raiz.Last.AddRange(raiz.DrchNodo.Last);
            }
            else if (raiz.ItemExpresion == ".")
            {
                if (raiz.DrchNodo.Nulo)
                {
                    raiz.Last.AddRange(raiz.IzqNodo.Last);
                    raiz.Last.AddRange(raiz.DrchNodo.Last);
                }
                else
                {
                    raiz.Last = (raiz.DrchNodo.Last);
                }
            }
            else if (raiz.ItemExpresion == "*" || raiz.ItemExpresion == "+")
            {
                raiz.Last = raiz.IzqNodo.Last;
            }
        }

        private void CalcularFollows(Nodo raiz)
        {
            if (raiz.ItemExpresion == ".")
            {
                //Para               todo last de C1 
                foreach (var nodo in raiz.IzqNodo.Last)
                {
                    //                             El FIRST de C2
                    foreach (var nodoFirst in raiz.DrchNodo.First)
                    {
                        //Si ya está entre los Follow no ingresarlo, sino se repetirá en la "tabla" (lista Hojas) de FOLLOWS
                        if (!Hojas[nodo.NumNodo - 1].Follow.Contains(nodoFirst))
                        {
                            Hojas[nodo.NumNodo - 1].Follow.Add(nodoFirst);
                        }
                    }
                }
            }
            else if (raiz.ItemExpresion == "*" || raiz.ItemExpresion == "+")
            {
                //Para               todo last de C1 
                foreach (var nodo in raiz.IzqNodo.Last)
                {
                    //                                      El FIRST de C1
                    foreach (var nodoFirst in raiz.IzqNodo.First)
                    {
                        if (!Hojas[nodo.NumNodo - 1].Follow.Contains(nodoFirst))
                        {
                            Hojas[nodo.NumNodo - 1].Follow.Add(nodoFirst);
                        }
                    }
                }
            }
        }

        public void GenerarTransiciones()
        {
            //El estado incial es el FIRST de la raíz
            Estados.Add(new Estado(Raiz.First));

            foreach (var nodoHoja in Hojas)
            {
                if (!TablaTransiciones.ContainsKey(nodoHoja.ItemExpresion) && nodoHoja.ItemExpresion != "#")
                {
                    TablaTransiciones.Add(nodoHoja.ItemExpresion, new List<Estado>());
                }
            }

            for (int i = 0; i < Estados.Count; i++)
            {
                foreach (var columna in TablaTransiciones)
                {
                    var match = false;

                    foreach (var nodoEstado in Estados[i].ListaNodos)
                    {
                        if (nodoEstado.ItemExpresion == columna.Key)
                        {
                            match = true;
                            //Por cada nodo del FOLLOW | De los FOLLOW de ese nodo perteneciente al estado     
                            foreach (var nodoFollow in Hojas[nodoEstado.NumNodo - 1].Follow)
                            {
                                if (columna.Value.Count == i)
                                {
                                    columna.Value.Add(new Estado(-1));
                                }

                                if (!columna.Value[i].ListaNodos.Contains(nodoFollow))
                                {
                                    columna.Value[i].ListaNodos.Add(nodoFollow);
                                }
                            }

                            var existe = false;
                            var casilla = 0;

                            for (int j = 0; j < Estados.Count; j++)
                            {
                                var contador = 0;

                                foreach (var itemEstado in Estados[j].ListaNodos)
                                {
                                    if (itemEstado.NumNodo != columna.Value[i].ListaNodos[contador].NumNodo)
                                    {
                                        existe = false;
                                    }
                                    else
                                    {
                                        existe = true;
                                        contador++;
                                    }

                                    if (existe == false)
                                    {
                                        break;
                                    }
                                }

                                if (existe == true)
                                {
                                    columna.Value[i].Nombre = Estados[j].Nombre;
                                    break;
                                }
                            }

                            if (existe == false)
                            {
                                var estado = new Estado(Estados.Count);
                                estado.ListaNodos = columna.Value[i].ListaNodos;

                                VerificarAceptacion(ref estado);
                                Estados.Add(estado);

                                columna.Value[i].Nombre = estado.Nombre;
                            }
                        }
                    }
                    if (match == false)
                    {
                        var estadoAux = new Estado(-1);
                        estadoAux.Nombre = "-";

                        columna.Value.Add(estadoAux);
                    }
                }
            }
        }

        private void VerificarAceptacion(ref Estado estado)
        {
            foreach (var nodo in estado.ListaNodos)
            {
                if (nodo.ItemExpresion == "#")
                {
                    estado.EsAceptacion = true;
                }
            }
        }
    }
}