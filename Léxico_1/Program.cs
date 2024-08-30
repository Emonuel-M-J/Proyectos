
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
            try
            {
             using (Lecturas L = new Lecturas())
                {
                    Token  token = new();
                    T.setContenido("HOLA");
                 T.setClasificacion(Token.Tipos.Identificador);

                    Console.Writeline(true.getContenido()+" = "+T.getClasificacion());

                 T.setContenido("123");
                    T.setClasificacion(Token.Tipos.Numero);

                    Console.Writeline(true.getContenido()+ " = " +T.getClasificacion());
                }
            }
            catch(Exception e)
            {
                Console.WriteLine("Error : " + e.Message);
            }
        }
    }
}