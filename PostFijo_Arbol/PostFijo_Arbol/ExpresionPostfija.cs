using System;
using System.Collections.Generic;
using System.Text;

namespace PostFijo_Arbol
{
    public class ExpresionPostfija
    {
        private List<char> Operadores { get; set; }

        private Stack<char> PilaOperdores { get; set; }

        private string Infija { get; set; } //Expresión normal, ejemplo: (A|B)*|A

        public string Postfija { get; set; }

        public ExpresionPostfija()
        {
            Operadores = new List<char>();
            Operadores.Add('(');
            Operadores.Add('*');
            Operadores.Add('|');
            Operadores.Add('.');
            Operadores.Add('+');

            PilaOperdores = new Stack<char>();
            Infija = string.Empty;
            Postfija = string.Empty;
        }

        /// <summary>
        /// Método para evaluar la prioridad del operador, mientras más grande sea el número más importante será
        /// </summary>
        /// <param name="Posicion"> Se refiere a si se evalúa la prioridad del operador afuera de la pila o dentro de ella
        /// 0 = prioridad dentro de la pila
        /// 1 = prioridad fuera de la pila </param>
        /// <returns>Prioridad del operador evaluado</returns>
        private int EvaluarPrioridad(int Posicion, char operador)
        {
            var prioridad = -1;

            if (Posicion == 0) //Dentro de la pila
            {
                switch (operador)
                {
                    case '*':
                        prioridad = 3;
                        break;

                    case '|':
                        prioridad = 2;
                        break;

                    case '.':
                        prioridad = 1;
                        break;

                    case '(':
                        prioridad = 0;
                        break;
                }
            }

            if (Posicion == 1) //Fuera de la pila
            {
                switch (operador)
                {
                    case '*':
                        prioridad = 4;
                        break;

                    case '|':
                        prioridad = 2;
                        break;

                    case '.':
                        prioridad = 1;
                        break;

                    case '(':
                        prioridad = 5;
                        break;
                }
            }


            return prioridad;
        }

        public void ConvertirPostfijo(string infijo)
        {
            Infija = infijo;

            while(Infija != "")
            {
                if (!Operadores.Contains(Infija[0]) && Infija[0] != ')') //Si es operando y diferente a paréntisis derecho
                {
                    Postfija += Infija[0];
                }
                else if (Operadores.Contains(Infija[0]) && Infija[0] != ')') //Si es operador y no es un paréntesis derecho
                {
                    var continuar = false;

                    while (continuar == false)
                    {
                        if (PilaOperdores.Count == 0) //Si la pila está vacía
                        {
                            PilaOperdores.Push(Infija[0]);
                            continuar = true;
                        }
                        else //Si no está vacía
                        {
                            if (EvaluarPrioridad(1, Infija[0]) > EvaluarPrioridad(0, PilaOperdores.Peek()))
                            {
                                PilaOperdores.Push(Infija[0]);
                                continuar = true;
                            }
                            else
                            {
                                Postfija += PilaOperdores.Pop();
                            }
                        }
                    }
                }
                else if(Infija[0] == ')')
                {
                    Postfija += PilaOperdores.Pop();

                    if (PilaOperdores.Peek() == '(')
                    {
                        PilaOperdores.Pop();
                    }
                    else
                    {
                        while (PilaOperdores.Peek() != '(')
                        {
                            Postfija += PilaOperdores.Pop();
                        }

                        PilaOperdores.Pop();
                    }
                }

                Infija = Infija.Remove(0, 1);
            }

            if (PilaOperdores.Count > 0)
            {
                while (PilaOperdores.Count > 0)
                {
                    Postfija += PilaOperdores.Pop();
                }
            }

            Postfija += "#"; //Se le agrega para mostrar que se llegó al estado de aceptación
        }
    }
}
