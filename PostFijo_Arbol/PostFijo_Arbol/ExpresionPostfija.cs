using System;
using System.Collections.Generic;
using System.Text;

namespace PostFijo_Arbol
{
    public class ExpresionPostfija
    {
        public List<char> Operadores { get; set; }

        public Stack<char> PilaOperdores { get; set; }

        public string Infija { get; set; } //Expresión normal, ejemplo: (A|B)*|A

        public string Postfija { get; set; }

        public ExpresionPostfija()
        {
            Operadores = new List<char>();
            Operadores.Add('(');
            Operadores.Add(')');
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
        public int EvaluarPrioridad(int Posicion, char operador)
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
    }
}
