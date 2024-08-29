using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;

namespace Proyectos
{
    public class Lecturas : IDisposable
    {
        StreamReader archivo;
        StreamWriter log;
        public Lecturas()
        {   Console.WriteLine("Constructor");

            archivo = new StreamReader("C:/Users/PEPE/Documents/Desktop/Nueva carpeta/Proyectos/prueba.cpp");
             log     = new StreamWriter("C:/Users/PEPE/Documents/Desktop/Nueva carpeta/Proyectos/prueba.log");
        }
        public Lecturas(string nombre)
        {
            archivo = new StreamReader(nombre);
           
        }
        
        public void Dispose()
        {
            archivo.Close();
            log.Close();
        }
        public void Display()
        {
            while (!archivo.EndOfStream)
            {
                Console.Write((char)archivo.Read());
            }
        }
        public void Copy()
        {
            while (!archivo.EndOfStream)
            {
                log.Write((char)archivo.Read());
            }
        }
        public void Encrypt()
        {       char c;
            while (!archivo.EndOfStream)
            {
                c=(char)archivo.Read();
                if(char.IsLetter(c))
                {
                    log.Write((char)(c+2));
                }
                else{
                    log.Write(c);
                }
            
            }
        }
        public void DesEncrypt()
        {       char c;
            while (!archivo.EndOfStream)
            {
                c=(char)archivo.Read();
                if(char.IsLetter(c))
                {
                    log.Write((char)(c-2));
                }
                else{
                    log.Write(c);
                }
            
            }
        }
        public void Encrypt2(char o)
        {       char [] vocales={'a','e','i','o', 'u'};
                char v;
                bool verificacion; 
            while (!archivo.EndOfStream)
            {   
                v=(char)archivo.Read();
        verificacion=vocales.Contains(v);
                if(verificacion)
                {

                    log.Write(o);
                }
                else{
                    log.Write(v);
                }
            
            }
        }
        public int ContarLetras()
        {       
                char c;
                int contador=0;
                 
            while (!archivo.EndOfStream)
            {   
                c=(char)archivo.Read();
        
                if(char.IsLetter((char)archivo.Read()))
                {

                    contador++;
                }
                
            
            }
            return contador++;
        }
     public int ContarDígitos()
        {       
                char c;
                int contador=0;
                 
            while (!archivo.EndOfStream)
            {   
                c=(char)archivo.Read();
        
                if(char.IsDigit((char)archivo.Read()))
                {

                    contador++;
                }
                
            
            }
            return contador++;
        }  
        public int contarEspacios()
        {       
                char c;
                int contador=0;
                 
            while (!archivo.EndOfStream)
            {   
                c=(char)archivo.Read();
        
                if(char.IsLetter((char)archivo.Read()))
                {

                    contador++;
                }
                
            
            }
            return contador++;
        }

        public void primerCaracter(){
            char v=' ';
            char c;
            while(char.IsWhiteSpace()){
                c=(char)archivo.Read();
                if(!char.IsWhiteSpace()){
                    v=char(c);
                    return v;

                }
                
            }

        }

        public void primerPalabra(){
            string buffer= "";
            char c;
            while(char.IsLetter(c=(char)archivo.Read()) && !archivo.EndOfStream){
                if(char.IsLetter(c)){
                    buffer+=c;


                }
                log.WriteLine(buffer);
                return buffer.ToString();
            }
                
         }


        public bool finArchivo(){
            return archivo.EndOfStream;
        }
    }
}