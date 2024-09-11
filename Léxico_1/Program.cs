
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Léxico_1
{
    public class Program
    {
        static void Main(string[] args)
        {


                



            
            try
            {
                using (Lexico l  = new Lexico())
                {
                    

                   new Lexico("C:/Users/PEPE/Documents/Lenguagesautomat1/Léxico_1/prueba.cpp");
                    while (!l.finArchivo())
                    {
                        
                        
                        l.nextToken();
                        
                    }

                    
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: " + e.Message);
            }
        }
    }
}


/*while(!file.EndOfStream){Mientras el archivo no termne de ller }
c= (char)file.Read Es para leer todo el archivo y cada linea 
file.Peek(); se queda en la primera palabra 



dda*/

