using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Proyectos
{
    class Program
    {
        static void Main(string[] args) 
        {
            using (Lecturas L = new Lecturas())
            {
                L.Encrypt();

                L.Encrypt2('o');

                Console.WriteLine(L.ContarLetras);
                Console.WriteLine(L.ContarDígitos);


                while(L.finArchivo()){

                    L.primerPalabra;
                }

            }
        }
    }
}
