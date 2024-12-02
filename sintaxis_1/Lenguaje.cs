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
        Stack <float> s;
        List <Variable>l;

        public Lenguaje():base()
        {
            s = new Stack<float>();
            l = new List<Variable>();

        }
        
        public Lenguaje (String name) : base (name)
        {
            log.WriteLine("Constructor Lenguaje");
            s = new Stack<float>();
            l = new List<Variable>();

        }
        private void displayStack()
        {
            Console.WriteLine("Contenido del Stack");
            foreach(float elemento in s)
            {
                Console.WriteLine(elemento);

            }
        }
        private void displayList()
        {
            log.WriteLine("Lista de Variables: ");
            foreach(Variable elemento in l)
            {
                log.WriteLine($"{elemento.getNombre()} {elemento.getTipoDato()}{ elemento.getValor()}");

            }
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
            displayList();
           
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
        //Variables -> tipodato Lista_identificadores; Variables?
        private void Variables()
        {
            Variable.TipoDato t= Variable.TipoDato.Char;
            switch(getContenido())
            {
                case "int": t = Variable.TipoDato.Int; break;
                case "float": t = Variable.TipoDato.Float; break; 
            }
            match(Tipos.TipoDato);
            ListaIdentificadores(t);
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
        private void ListaIdentificadores(Variable.TipoDato t)
        {
            if(l.Find(variable => variable.getNombre() == getContenido()) != null)
            {
                throw new Error("Sintaxis: La variable "+ getContenido() +" ya existe", log, linea,columna);
            }
            l.Add(new Variable(t, getContenido()));
            match(Tipos.Identificador);
            if(getContenido()== "=")
            {
                match("=");
                Expresion();
                float r= s.Pop();
            }
            if(getContenido() == ",")
            {
                match(",");
                ListaIdentificadores(t);
            }
        }

        //BloqueInstrucciones -> { listaIntrucciones? }
        private void BloqueInstrucciones(bool ejecuta)
        {
            match("{");
            if(getContenido() != "}") 
            {
                ListaInstrucciones(ejecuta);
            } 
            else 
            {
                match("}");
            }


        }          
            //ListaInstrucciones -> Instruccion ListaInstrucciones?
        private void ListaInstrucciones(bool ejecuta)
        {
            Instruccion(ejecuta);
            if(getContenido() != "}")
            {
                ListaInstrucciones(ejecuta);
            }
        
        }
            //Instruccion -> Console | If | While | do | For | Variables | Asignación
        private void Instruccion(bool ejecuta)
        {
            if(getContenido()== "Console")
            {
                console(ejecuta);
            }
            else if(getContenido() == "if")
            {
                If(ejecuta);
            }
            else if(getContenido() == "while")
            {
                While(ejecuta);
            }
            else if(getContenido() == "do")
            {
                Do(ejecuta);
            }
            else if(getContenido() == "for")
            {
                For(ejecuta);
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
            Variable? v = l.Find(variable => variable.getNombre() == getContenido());
            if(v == null)
            {
                throw new Error("Sintaxis: La variable  "+ getContenido() +" no está definida", log, linea,columna);
            }
            s.Push(v.getValor());
            //Console.Write(getContenido() + " = ");
            
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
                        int value = Console.Read();
                        v.setValor(value);
                    
                    }
                    else if(getContenido()== "ReadLine")
                    {
                        match("ReadLine");
                        match("(");
                        match(")");
                        string? value = Console.ReadLine();
                        if (int.TryParse(value, out int parsedValue))
                        {
                            v.setValor(parsedValue); 
                        }
                        else
                        {
                            throw new Error("Entrada no válida", log, linea, columna);
                        }
                       
               
                    }
                    //match(";");
                }
                else
                {
                    Expresion();
                }
               // Console.WriteLine(" = " + s.Pop());
                //displayStack();
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
            float r = s.Pop();
            v.setValor(r);
            
        }
            //If -> if (Condicion) bloqueInstrucciones | instruccion
            //(else bloqueInstrucciones | instruccion)?
        private void If(bool ejecuta2)
        {
            match("if");
            match("(");
            bool ejecuta= Condicion() && ejecuta2;

            match(")");
            if(getContenido()== "{")
            {   
                
                BloqueInstrucciones(ejecuta);
            }
            else
            {
                Instruccion(ejecuta);
            }


            if(getContenido() == "else")
            {
                match("else");
                if(getContenido() == "{")
                {   
                    
                    BloqueInstrucciones(false);
                }
                else
                {
                    Instruccion(false);
                }
            }

        }
            
        
            //Condicion -> Expresion operadorRelacional Expresion
        private bool Condicion()
        {
            Expresion();
            float valor1= s.Pop();
            String operador = getContenido();
            match(Tipos.OperadorRelacional);
            Expresion();
            float valor2=s.Pop();

            switch(operador){
                case ">":  return valor1 > valor2;
                case ">=":  return valor1 >= valor2;
                case "<":  return valor1 < valor2;
                case "<=":  return valor1 <= valor2;
                case "==":  return valor1 == valor2;
                default: return valor1 != valor2;
                


            }

        }

            //While -> while(Condicion) bloqueInstrucciones | instruccion
        private void While(bool ejecuta)
        {
            match("while");
            match("(");
            Condicion();
            match(")");
            if(getContenido()== "{")
            {   
                
                BloqueInstrucciones(true);
            }
            else
            {
                Instruccion(true);
            }

        }
            //Do -> do 
            // bloqueInstrucciones | intruccion
            // while(Condicion);
        private void Do(bool ejecuta)
        {
            match("do");
            if(getContenido()== "{")
            {   
                
                BloqueInstrucciones(true);
            }
            else
            {
                Instruccion(true);
            }
            match("while");
            match("(");
            Condicion();
            match(")");
            match(";");

        }
           
        //For -> for(Asignacion; Condicion; Asignacion)
        // BloqueInstrucciones | Intruccion 
        private void For(bool ejecuta)
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
                
                BloqueInstrucciones(true);
            }
            else
            {
                Instruccion(true);
            }
        }

           // Console -> Console.(WriteLine|Write) (cadena concatenaciones?);
        private void console(bool ejecuta)
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
                   
                    Console.WriteLine();
                       
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
            BloqueInstrucciones(true);
            
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
                string operador = getContenido();
                match(Tipos.OperadorTermino);
                Termino();


                //Console.Write(operador + "");

                float n1 = s.Pop();
                float n2 = s.Pop();

                switch(operador){

                    case "+": s.Push(n2 + n1); break;
                    case "-": s.Push(n2 - n1); break;
                    

                }

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
                string operador = getContenido();
                match(Tipos.OperadorFactor);
                Factor();
               // Console.Write(operador + " ");
                float n1 = s.Pop();
                float n2 = s.Pop();

                switch(operador){

                    case "*": s.Push(n2 * n1); break;
                    case "/": s.Push(n2 / n1); break;
                    case "%": s.Push(n2 % n1); break;

                }
            } 

        }
            //Factor -> numero | identificador | (Expresion)
        private void Factor()
        {
            if(getClasificacion() == Tipos.Numero) 
            {
                s.Push(float.Parse(getContenido()));
                //Console.Write(getContenido() + " ");
                match(Tipos.Numero);
            }
            else if(getClasificacion() == Tipos.Identificador)
            {
                Variable? v= l.Find(variable => variable.getNombre() == getContenido());
                if(v== null)
                {
                    throw new Error("Sintaxis: La variable "+ getContenido() +" No está definida ", log, linea, columna);
                }
                s.Push(v.getValor());
                //Console.Write(getContenido() + " ");
                match(Tipos.Identificador);
            }
            else
            {
                match("(");
                Expresion();
                match(")");
            }
        }
        private void Concatenaciones()
        {
            if ( getClasificacion() == Tipos.Identificador) 
            {
                match(Tipos.Identificador);
                
            }
            else 
            {                   
                match(Tipos.Cadena);
            }
            
            if(getContenido() == "+")
            {
                match("+");
                Concatenaciones();
            }
            
        }
    }
}