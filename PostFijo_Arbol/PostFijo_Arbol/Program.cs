using System;
using System.IO;

namespace PostFijo_Arbol
{
    class Program
    {
        static void Main(string[] args)
        {
            var convertidor = new ExpresionPostfija();

            //L.(L|D)*|D.D*
            //((A|C)+|D*)+

            Console.WriteLine("Ingrese expresión infija para convertir a Postfijo: ");
            convertidor.ConvertirPostfijo(Console.ReadLine());

            var expresionPostfija = convertidor.Postfija;
            Console.WriteLine("Expresión postFija: " + expresionPostfija);

            Console.WriteLine("\nAl final de la expresión se le agregó:     .# (estado de aceptación), para hacer el árbol de expresiones");
            
            var arbol = new ArbolExpresiones.ArbolExpresiones();

            arbol.CrearArbol(expresionPostfija); //Línea operatoria

            Console.WriteLine("\nEl árbol se genera automaticamente, pero no se muestra en pantalla\nPara verlo hay que poner un punto de interrupción y buscar el arbol");

            var recorrido = string.Empty;

            arbol.PostOrdenOperaciones(arbol.Raiz, ref recorrido); //Linea operatoria
            arbol.GenerarTransiciones();

            Console.WriteLine("\n\nRecorrido postOrden: " + recorrido);
            Console.ReadKey();
        }
    }
}
