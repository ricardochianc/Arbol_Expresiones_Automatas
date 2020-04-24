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

            Console.WriteLine("\nAl final de la expresión se le agregó:     .#");
            Console.WriteLine("el estado de aceptación");

            var arbol = new ArbolExpresiones.ArbolExpresiones();
            arbol.CrearArbol(expresionPostfija);

            Console.WriteLine("\nEl árbol se genera automaticamente, pero no se muestra en pantalla\nPara verlo hay que poner un punto de interrupción en en main y buscar el arbol");

            var recorrido = "";
            arbol.PostOrden(arbol.Raiz, ref recorrido);

            Console.WriteLine("\n\nRecorrido postOrden: " + recorrido);

            Console.ReadKey();
        }
    }
}
