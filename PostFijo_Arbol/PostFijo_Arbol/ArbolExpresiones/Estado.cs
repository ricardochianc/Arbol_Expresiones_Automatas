using System;
using System.Collections.Generic;
using System.Text;

namespace PostFijo_Arbol.ArbolExpresiones
{
    public class Estado
    {
        public string Nombre { get; set; } //Tendrá el valor q0, q1, q2, ..., qn
        public List<Nodo> ListaNodos { get; set; }
        public bool EsAceptacion { get; set; }

        /// <summary>
        /// Constructor para el resto de estados
        /// </summary>
        /// <param name="numeroEstado"></param>
        public Estado(int numeroEstado)
        {
            Nombre = "q" + numeroEstado;
            ListaNodos = new List<Nodo>();
            EsAceptacion = false;
        }

        /// <summary>
        /// Constructor para estado inicial
        /// </summary>
        /// <param name="fistRaiz"></param>
        public Estado(List<Nodo> fistRaiz)
        {
            Nombre = "q0";
            ListaNodos = fistRaiz;
            EsAceptacion = false;
        }

    }
}
