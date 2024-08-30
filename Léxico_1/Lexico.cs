using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;

/*
    Requerimiento 1= Sobrecargar el constructor Lexico para quereciba 
            como argument0 el nombre del archivo a compilar
    Requerimiento 2: Tener un contador de lineas.
*/
namespace 
{
    public class Lexico : Token, IDisposable
    {
        StreamReader archivo;
        StreamWriter log;
        StreamWriter asm;;
        int linea;
        public Lexico()
        {
            linea = 1;
            log = new StreamWriter("prueba.log");
            asm = new StreamWriter("prueba.asm");
            log.AutoFlush=true;
            asm.AutoFlush=true;
            if(File.Exists("prueba.cpp"))
            {
                archivo  = new StreamReader("prueba.cpp");
            }
            else
            {
                throw new Error("El archivo prueba.cpp no exite", log);

            }
            

        }
        /*public Lexico(string nombre)
        {
            

                Si nombre = suma.cpp
                LOG = suma.log
                ASM = suma.asm
                Y validar la extension del nombre del archivo

            
        }
        */
        public void Dispose()
        {
            archivo.close();
            log.close();
            asm.close();
          
         }
         public void nextToken()
         {
            char c;
            string buffer= "";
            while(char.IsWhiteSpace(c = (char)archivo.Read()))
            {

            }
            if(char.IsLetter(c))
            {
            SeyClasificacion(Tipos.Identificador);
             }
             else if(char.IsDigit(c))
             {
                setClasificacion(Tipos.Numero);
             }
             else
             {
                setClasificacion(Tipos.Caracter);
             }
             setContenido(buffer);
            log.Writeline(()+ " = " + getClasificacion());


         }


    }
}