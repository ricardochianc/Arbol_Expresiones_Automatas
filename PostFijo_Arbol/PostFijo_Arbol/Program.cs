using System;

namespace PostFijo_Arbol
{
    class Program
    {
        static void Main(string[] args)
        {
            var convertidor = new ExpresionPostfija();

            Console.WriteLine("Ingrese expresión para convertir a Postfijo: ");
            convertidor.ConvertirPostfijo(Console.ReadLine());

            var expresionPostfija = convertidor.Postfija;
            Console.WriteLine("PostFija: " + expresionPostfija);
            Console.WriteLine("Al final de la expresión se le agregó:     .#");
            Console.WriteLine("el estado de aceptación");

            var arbol = new ArbolExpresiones.ArbolExpresiones();
            arbol.CrearArbol(expresionPostfija);

            Console.ReadKey();
        }
    }
}
