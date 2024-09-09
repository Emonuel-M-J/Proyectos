using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using System.Security.Cryptography.X509Certificates;

/*  
    Requerimiento 1: Sobrecargar el constructor Lexico para que reciba como
                     argumento el nombre del archvo a compilar
    Requerimiento 2: Tener un contador de lineas 
    Requerimiento 3: Agregar operador relacional y Operador Lógico
                    ==, >,=>,<,<=,<>,!=,<=,<        &&,||,!
*/
namespace Léxico_1
{
    public class Lexico : Token, IDisposable
    {
        StreamReader archivo;
        StreamWriter log;
        StreamWriter asm;
        
        
        public Lexico()
        {

            
            log     = new StreamWriter("prueba.log");
            asm     = new StreamWriter("prueba.asm");
            log.AutoFlush=true;
            asm.AutoFlush=true;
            if (File.Exists("prueba.cpp"))
            {
                 archivo = new StreamReader("prueba.cpp");
            }
            else
            {
                throw new Error("El archivo prueba.cpp no existe",log);
            }
        }
        public Lexico(string nombre)
        {   
            log = new StreamWriter(nombre + ".log");
            log.AutoFlush=true;
            if(System.IO.Path.GetExtension(nombre).ToLower() == ".cpp" )
            {
                asm  = new StreamWriter(nombre + ".asm"); 
                asm.AutoFlush=true;
                
                if(File.Exists(nombre))
                {
                     archivo = new StreamReader("nombre");
                     
                }
                else
                {
                    throw new Error("El archivo prueba .cpp no existe", log);
                }
            }
                else 
                { 
                    throw new Error("El archivo no es correcto", log);
                }
                
        }
        
       /* public Lexico(string nombre)
        {
            
                if( nombre = suma.cpp)
                {
                    LOG = suma.log
                    ASM = suma.asm
                    //Y validar la extension del nombre del archivo

                }
        }*/
        
        public void Dispose()
        {
            //Contador de lineas 
            int contadorlinea = File.ReadAllLines("prueba.cpp").Length;
            contadorlinea = contadorlinea +1;
            log.WriteLine("Numero de lineas: " + contadorlinea   );
            archivo.Close();
            log.Close();
            asm.Close();
            
        }
        public void nextToken()
        {
            char c;
            string buffer = "";
            
            
            

            while (char.IsWhiteSpace(c = (char)archivo.Read()))
            {
            }
            buffer+=c;
            
            

            if (char.IsLetter(c))
            {
                setClasificacion(Tipos.Identificador);
                while (char.IsLetterOrDigit(c=(char)archivo.Peek()))
                {
                    buffer+=c;
                    archivo.Read();
                }
            }
            else if (char.IsDigit(c))
            {
                setClasificacion(Tipos.Numero);
                while (char.IsDigit(c=(char)archivo.Peek()))
                {
                    buffer+=c;
                    archivo.Read();
                }
            }
            else if (c=='=')
            {
                setClasificacion(Tipos.Asignacion);
                if((c=(char)archivo.Peek()) =='=' || c=='=' )
                {
                    setClasificacion(Tipos.OperadorRelacional);
                    buffer+=c;
                    archivo.Read();
                    

                }
            }
            
            else if (c==';')
            {
                setClasificacion(Tipos.FinSentencia);
            }
            else if (c=='{')
            {
                setClasificacion(Tipos.InicioBloque);
            }
            else if (c=='}')
            {
                setClasificacion(Tipos.FinBloque);
            }
            else if (c=='?' || c==':')
            {
                setClasificacion(Tipos.OperadorTernario);
            }
            else if (c=='+')
            {
                setClasificacion(Tipos.OperadorTermino);
                if((c=(char)archivo.Peek()) =='+' || c=='=' )
                {
                    setClasificacion(Tipos.IncrementoTermino);
                    buffer+=c;
                    archivo.Read();
                    

                }
            }
            else if (c=='-')
            {
                setClasificacion(Tipos.OperadorTermino);
                if((c=(char)archivo.Peek()) =='-' || c=='=' )
                {
                    setClasificacion(Tipos.IncrementoTermino);
                    buffer+=c;
                    archivo.Read();
                    

                }
                else if(c=='>')
                {
                    setClasificacion(Tipos.Puntero);
                    buffer+=c;
                    archivo.Read();
                }
            }
            else if (c=='*' || c=='%' || c== '/')
            {
                setClasificacion(Tipos.OperadorFactor);
            }

            else if (c== '*')
            {
                setClasificacion(Tipos.IncrementoFactor);
                if((c=(char)archivo.Peek()) =='*' || c=='=' )
                {
                    setClasificacion(Tipos.IncrementoFactor);
                    buffer+=c;
                    archivo.Read();
                    

                }
            }
            else if (c=='%')
            {
                setClasificacion(Tipos.IncrementoFactor);
                if((c=(char)archivo.Peek()) =='%' || c=='=' )
                {
                    setClasificacion(Tipos.IncrementoFactor);
                    buffer+=c;
                    archivo.Read();
                    

                }
            }
             
            else if (c=='/')
            {
                setClasificacion(Tipos.IncrementoFactor);
                if((c=(char)archivo.Peek()) =='/' || c=='=' )
                {
                    setClasificacion(Tipos.IncrementoFactor);
                    buffer+=c;
                    archivo.Read();
                    

                }
            }
            
        
            else if (c =='$')
            {
                setClasificacion(Tipos.Caracter);
                
                if(char.IsDigit(c=(char)archivo.Peek()))
                {
                    setClasificacion(Tipos.Moneda);
                    while(char.IsDigit(c=(char)archivo.Peek()))
                    {   
                        buffer+=c;
                        archivo.Read();
                    }
                }
            }

                    //Nuevos tokens 

                else if (c == '=')
            {
                setClasificacion(Tipos.Asignacion);
                if((c=(char)archivo.Peek()) == '=')
                {
                    setClasificacion(Tipos.OperadorRelacional);
                    buffer += c;
                    archivo.Read();
                    
                }
            }
                 else if (c=='<' || c == '>' )
            {
                  setClasificacion(Tipos.OperadorRelacional);
                  if((c=(char)archivo.Peek()) == '=')
                {
                    setClasificacion(Tipos.OperadorRelacional);
                    buffer += c;
                    archivo.Read();
                    
                }
                else if(c== '<'){
                if((c=(char)archivo.Peek()) == '>')
                {
                    setClasificacion(Tipos.OperadorRelacional);
                    buffer += c;
                    archivo.Read();
                    
                }
            }
            }
            else if(c=='!'){
                setClasificacion(Tipos.OperadorLogico);
                if((c=(char)archivo.Peek()) == '=')
                {
                    setClasificacion(Tipos.OperadorRelacional);
                    buffer += c;
                    archivo.Read();
                    
                }
            }
            
            else if (c == '&')
            
            {
                setClasificacion(Tipos.Caracter);
                if((c=(char)archivo.Peek()) == '&')
                {
                    setClasificacion(Tipos.OperadorLogico);
                    buffer += c;
                    archivo.Read();
                    
                }
            }
            else if (c == '|')
            {
                if((c=(char)archivo.Peek()) == '|')
                {
                    setClasificacion(Tipos.OperadorLogico);
                    buffer += c;
                    archivo.Read();
                    
                }
            }

            
                 else
                {
                    setClasificacion(Tipos.Caracter);
                 }
                 if(!finArchivo())
                 {
                     setContenido(buffer);
                    log.WriteLine(getContenido() + " = " + getClasificacion()); 
                  
                     
                 }

                 

         
        
                   
        } 
        
        public bool finArchivo()
        {
             
            return archivo.EndOfStream;
        }
        
        
        

                  
            
        
    }
}