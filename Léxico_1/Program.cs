
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Léxico_1
{
    public class Programs
    {
        static void Main(string[] args)
        {


                StreamReader file= new("./test.cpp");
                StreamWriter logger =new("./new.log");
                file.Close();
                logger.Close();



            
            try
            {
                using (Lexico l  = new ())
                {
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

