using System;
using System.Collections.Generic;
using System.Linq;
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

                        if (pila.Count > 0)
                        {
                            nuevoNodo.IzqNodo = pila.Pop();
                        }

                        nuevoNodo.AsignarNulabilidad();
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
                            Hojas[nodo.NumNodo - 1].Follow.Sort(Nodo.OrdenarPorNodo);
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
                            Hojas[nodo.NumNodo - 1].Follow.Sort(Nodo.OrdenarPorNodo);
                        }
                    }
                }
            }
        }

        public void GenerarTransiciones()
        {
            //El estado incial es el FIRST de la raíz
            Estados.Add(new Estado(Raiz.First));

            //Se plantean todos los caracteres que se van a analizar, o sea se muestra el valor de todas las hojas sin repetirse
            foreach (var nodoHoja in Hojas)
            {
                if (!TablaTransiciones.ContainsKey(nodoHoja.ItemExpresion) && nodoHoja.ItemExpresion != "#")
                {
                    TablaTransiciones.Add(nodoHoja.ItemExpresion, new List<Estado>());
                }
            }

            //Como hay una lista de estados, se hace el procedimiento por cada estado de la lista y se le aplica a cada "columna/caracter/Hoja"
            for (int i = 0; i < Estados.Count; i++)
            {
                //Por cada "columna/caracter/Hoja", se hace...
                foreach (var columna in TablaTransiciones)
                {
                    //Variable que sirve para llevar el control si algún "nodo" del estado que se está evaluando coincide
                    var match = false;

                    //Por cada "nodo" del estado que se evalúa actualmente
                    foreach (var nodoEstado in Estados[i].ListaNodos)
                    {
                        //si algún nodo coincide con la "columna/caracter/Hoja"...
                        if (nodoEstado.ItemExpresion == columna.Key)
                        {
                            //se marca que sí coincide y...
                            match = true;

                            //se manda a traer los FOLLOW de ese nodo que se analiza.

                            //Luego para que no hayan datos repetidos en el estado que se está creando, se hace que....

                            //Por cada nodo del FOLLOW se verifica si ya se ingresó previamente al nuevo estado
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