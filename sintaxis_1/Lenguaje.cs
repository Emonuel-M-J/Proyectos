using System;
using System.Collections.Generic;
using System.Formats.Asn1;
using System.Linq;
using System.Threading.Tasks;
/*
    REQUERIMIENTOS
    - Indicar en el error lexico o sintactico el numero de linea y caracter 
        El numero de linea
    - En el log colocar el nombre al archivo a compilar la fecha y la hora 
        Ya  
    - Agregar el resto de asignaciones
        Y  a 
    -Emular el Console.Write() y Console.WriteLine( ) 
    - Emular el Console.Read() y Console.ReadLine()
*/

namespace sintaxis_1
{
    public class Lenguaje: Sintaxis
    {
        public Lenguaje(): base()
        {
            log.WriteLine("Constructor Lenguaje");

        }
        public Lenguaje (String name) : base (name)
        {
            log.WriteLine("Constructor Lenguaje");
        }
        // Cerradura epsilon
        //Programa  -> Librerias? Variables? Main
        public void Programa()
        {   
            if(getContenido() == "using")
            {
                Librerias();
            }
            if(getClasificacion() ==   Tipos.TipoDato)
            {
                Variables();
            }
            
            Main();
        }
        //Librerias -> using ListaLibrerias; Librerias?
        private void Librerias()
        {
            match("using");
            ListaLibrerias();
            match(";");
            if(getContenido() == "using")
            {
                Librerias();
            }
            
        }
        //Variables -> tipo_dato Lista_identificadores; Variables?
        private void Variables()
        {
            match(Tipos.TipoDato);
            ListaIdentificadores();
            match(";");
            if(getClasificacion()==Tipos.TipoDato)
            {
                Variables();
            }            
        }
        //ListaLibrerias -> identificador (.ListaLibrerias)?
        private void ListaLibrerias()
        {
            match(Tipos.Identificador);
            
            if(getContenido() == ".")
            {
                match(".");
                ListaLibrerias();
            }
        }
        //ListaIdentificadores -> identificador (,ListaIdentificadores)?
        private void ListaIdentificadores()
        {
            match(Tipos.Identificador);
            if(getContenido() == ",")
            {
                match(",");
                ListaIdentificadores();
            }
        }

        //BloqueInstrucciones -> { listaIntrucciones? }
        private void BloqueInstrucciones()
        {
            match("{");
            if(getContenido() != "}") 
            {
                ListaInstrucciones();
            } 
            else 
            {
                match("}");
            }


        }          
            //ListaInstrucciones -> Instruccion ListaInstrucciones?
        private void ListaInstrucciones()
        {
            Instruccion();
            if(getContenido() != "}")
            {
                ListaInstrucciones();
            }
            else
            {
                match("}");
            }

        }
            //Instruccion -> Console | If | While | do | For | Variables | Asignación
        private void Instruccion()
        {
            if(getContenido()== "Console")
            {
                console();
            }
            else if(getContenido() == "if")
            {
                If();
            }
            else if(getContenido() == "while")
            {
                While();
            }
            else if(getContenido() == "do")
            {
                Do();
            }
            else if(getContenido() == "for")
            {
                For();
            }
            else if(getClasificacion() == Tipos.TipoDato)
            {
                Variables();
            }
            else
            {
                Asignacion();
                match(";");
            }
        }
            //Asignacion -> Identificador = Expresion;
        private void Asignacion()
        {
            match(Tipos.Identificador);
            if(getContenido()== "=")
            {
                match("=");
                if(getContenido()== "Console")
                {
                    match("Console");
                    match(".");
                    if (getContenido()== "Read" )
                    {
                        match("Read");
                        match("(");                
                        match(")");
                        Console.Read();
                        
                
                    }
                    else if(getContenido()== "ReadLine")
                    {
                        match("ReadLine");
                        match("(");
                        match(")");
                        Console.ReadLine();
                       
               
                    }
                     //match(";");
                    
                }
                
                
                else
                {
                    Expresion();
                }
                
            
            }
            else if(getContenido()== "++")
            {
                match("++");
            }
            else if(getContenido()== "--")
            {
                match("--");
            }
            else if(getClasificacion() == Tipos.IncrementoTermino)
            {
                match(Tipos.IncrementoTermino);
                Expresion();
            }
            else if(getClasificacion() == Tipos.IncrementoFactor) 
            {
                match(Tipos.IncrementoFactor); 
                Expresion();
            }
            
        }
            //If -> if (Condicion) bloqueInstrucciones | instruccion
            //(else bloqueInstrucciones | instruccion)?
        private void If()
        {
            match("if");
            match("(");
            Condicion();
            match(")");
            if(getContenido()== "{")
            {   
                
                BloqueInstrucciones();
            }
            else
            {
                Instruccion();
            }


            if(getContenido() == "else")
            {
                match("else");
                if(getContenido() == "{")
                {   
                    
                    BloqueInstrucciones();
                }
                else
                {
                    Instruccion();
                }
            }

        }
            
        
            //Condicion -> Expresion operadorRelacional Expresion
        private void Condicion()
        {
            Expresion();
            match(Tipos.OperadorRelacional);
            Expresion();
        }

            //While -> while(Condicion) bloqueInstrucciones | instruccion
        private void While()
        {
            match("while");
            match("(");
            Condicion();
            match(")");
            if(getContenido()== "{")
            {   
                
                BloqueInstrucciones();
            }
            else
            {
                Instruccion();
            }

        }
            //Do -> do 
            // bloqueInstrucciones | intruccion
            // while(Condicion);
        private void Do()
        {
            match("do");
            if(getContenido()== "{")
            {   
                
                BloqueInstrucciones();
            }
            else
            {
                Instruccion();
            }
            match("while");
            match("(");
            Condicion();
            match(")");
            match(";");

        }
           
        //For -> for(Asignacion; Condicion; Asignacion)
        // BloqueInstrucciones | Intruccion 
        private void For()
        {
            match("for");
            match("(");
            Asignacion();
            match(";");
            Condicion();
            match(";");
            Asignacion();
            match(")");
            if(getContenido()== "{")
            {   
                
                BloqueInstrucciones();
            }
            else
            {
                Instruccion();
            }
        }

           // Console -> Console.(WriteLine|Write) (cadena concatenaciones?);
        private void console()
        {
            match("Console");
            match(".");
            
             if (getContenido() == "WriteLine")
            {
                match("WriteLine");
                match("(");
                if (getClasificacion() == Tipos.Cadena) 
                {
                    
                    Console.WriteLine(getContenido(). Trim('\"'));                    
                    match(Tipos.Cadena); 
               
                }
                else 
                {
                   // match(")");
                    Console.WriteLine();
                    //match(";");   
                }

                match(")"); 
                match(";");
            }
            else if (getContenido() == "Write")
            {
                match("Write");
                match("(");
                if (getClasificacion() == Tipos.Cadena) 
                {
                    
                    Console.Write(getContenido(). Trim('\"'));
                    match(Tipos.Cadena);
                }
                match(")"); 
                match(";");
            }
            

           // match("(");
           // Console.WriteLine(getContenido(). Trim('\"'));
           // match(Tipos.Cadena);
            //match(")");
           // match(";");


            /*else
            {
                throw new Error("Sintaxis se espera WriteLine o Write");
            }*/

        }
        
    
        
        // Main      -> static void Main(string[] args) BloqueInstrucciones 
        private void Main()
        {
            match("static");
            match("void");
            match("Main");
            match("(");
            match("string");
            match("[");
            match("]");
            match("args");
            match(")");
            BloqueInstrucciones();
            
        }

           // Expresion -> Termino MasTermino
        private void Expresion()
        {
            Termino();
            MasTermino();
        }
           // MasTermino -> (OperadorTermino Termino)?
        private void MasTermino()
        {
            if(getClasificacion() == Tipos.OperadorTermino)
            {
                match(Tipos.OperadorTermino);
                Termino();
            } 
        }
            //Termino -> Factor PorFactor
        private void Termino()
        {
            Factor();
            PorFactor();
        }
            //PorFactor -> (OperadorFactor Factor)?
        private void PorFactor()
        {
            if(getClasificacion() == Tipos.OperadorFactor)
            {
                match(Tipos.OperadorFactor);
                Factor();
            } 

        }
            //Factor -> numero | identificador | (Expresion)
        private void Factor()
        {
            if(getClasificacion() == Tipos.Numero) 
            {
                match(Tipos.Numero);
            }
            else if(getClasificacion() == Tipos.Identificador)
            {
                match(Tipos.Identificador);
            }
            else
            {
                match("(");
                Expresion();
                match(")");
            }
        }
    }
}