using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sintaxis_1
{
    public class Program : Token
    {
        static void Main(string[] args)
        {
            
            try
            {
                using (Lenguaje l  = new Lenguaje("prueba.cpp"))
                
                    
                   /* while (!l.finArchivo())
                    {
                        l.nextToken( );   
                    }
                    */
                   
                    l.Programa();
                
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: " + e.Message);
            }
        }
    }
}


