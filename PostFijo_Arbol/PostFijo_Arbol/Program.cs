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
            Console.WriteLine(convertidor.Postfija);
            Console.WriteLine("Al final de la expresión se le agregó:     .#");
            Console.WriteLine("el estado de aceptación");
            Console.ReadKey();
        }
    }
}
