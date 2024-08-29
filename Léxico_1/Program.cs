
nusing System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lexico_1
{
    public class Program
    {
        static void Main(string[] args) 
        {
            using (Lecturas L = new Lecturas())
            
                Token  token = new();
            T.setContenido("HOLA");
            T.setClasificacion(Token.Tipos.Identificador);

            Console.Writeline(true.GetContenido()+" = "+true.GetClasificacion());

            T.setContenido("123");
            T.setClasificacion(Token.Tipos.Numero);

            Console.Writeline(true.GetContenido()+" = "+true.GetClasificacion());
        
        }
    }
}