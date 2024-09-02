
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
                    Lexico_1 1 = new Lexico_1();
                    while(!L.finArchivo())
                    {
                        L.nextToken();
                    }
                }
            }
            catch(Exception e)
            {
                Console.WriteLine("Error : " + e.Message);
            }
        }
    }
}