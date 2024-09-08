using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Léxico_1
{
    public class Archivonovalido : Exception
    {
        public Archivonovalido(string message, StreamWriter log ) : base(message)
        {
            log.WriteLine("Archivo no valido:" + message);
        }
        
    }
}