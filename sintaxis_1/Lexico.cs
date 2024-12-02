using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.CompilerServices;
using System.Formats.Asn1;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Data;



/*  
    Requerimiento 1: Sobrecargar el constructor Lexico para que reciba como
                     argumento el nombre del archvo a compilar
    Requerimiento 2: Tener un contador de lineas 
    Requerimiento 3: Agregar operador relacional y Operador Lógico
                    ==, >,=>,<,<=,<>,!=,<=,<        &&,||,!
*/
namespace sintaxis_1
{
    public class Lexico : Token, IDisposable
    {
        StreamReader archivo;
        protected StreamWriter log;
        protected StreamWriter asm;
        protected int linea;
        protected int columna;
        const int F=-1;
        const int E= -2;
        int [,] TRAND ={
            {  0,  1,  2, 33,  1, 12, 14,  8,  9, 10, 11, 23, 16, 16, 18, 20, 21, 26, 25, 27, 29, 32, 34,  0,  F, 33  },
            {  F,  1,  1,  F,  1,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F  },
            {  F,  F,  2,  3,  5,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F  },
            {  E,  E,  4,  E,  E,  E,  E,  E,  E,  E,  E,  E,  E,  E,  E,  E,  E,  E,  E,  E,  E,  E,  E,  E,  E,  E  },
            {  F,  F,  4,  F,  5,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F  },
            {  E,  E,  7,  E,  E,  6,  6,  E,  E,  E,  E,  E,  E,  E,  E,  E,  E,  E,  E,  E,  E,  E,  E,  E,  E,  E  },
            {  E,  E,  7,  E,  E,  E,  E,  E,  E,  E,  E,  E,  E,  E,  E,  E,  E,  E,  E,  E,  E,  E,  E,  E,  E,  E  },
            {  F,  F,  7,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F  },
            {  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F  },
            {  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F  },
            {  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F  },
            {  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F  },
            {  F,  F,  F,  F,  F, 13,  F,  F,  F,  F,  F, 13,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F  },
            {  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F  },
            {  F,  F,  F,  F,  F,  F, 13,  F,  F,  F,  F, 13,  F,  F,  F,  F,  F,  F, 15,  F,  F,  F,  F,  F,  F,  F  },
            {  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F  },
            {  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F, 17,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F  },
            {  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F  },
            {  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F, 19,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F  },
            {  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F  },
            {  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F, 19,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F  },
            {  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F, 22,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F  },
            {  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F  },
            {  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F, 24,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F  },
            {  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F  },
            {  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F, 24,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F  },
            {  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F, 24,  F,  F,  F,  F,  F,  F, 24,  F,  F,  F,  F,  F,  F,  F  },
            { 27, 27, 27, 27, 27, 27, 27, 27, 27, 27, 27, 27, 27, 27, 27, 27, 27, 27, 27, 28, 27, 27, 27, 27,  E, 27  },
            {  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F  },
            { 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30  },
            {  E,  E,  E,  E,  E,  E,  E,  E,  E,  E,  E,  E,  E,  E,  E,  E,  E,  E,  E,  E, 31,  E,  E,  E,  E,  E  },
            {  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F  },
            {  F,  F, 32,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F  },
            {  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F  },
            {  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F, 17, 36,  F,  F,  F,  F,  F,  F,  F,  F,  F, 35,  F,  F,  F  },
            { 35, 35, 35, 35, 35, 35, 35, 35, 35, 35, 35, 35, 35, 35, 35, 35, 35, 35, 35, 35, 35, 35, 35,  0, 35, 35  },
            { 36, 36, 36, 36, 36, 36, 36, 36, 36, 36, 36, 36, 37, 36, 36, 36, 36, 36, 36, 36, 36, 36, 36, 36, 36, 36  },
            { 36, 36, 36, 36, 36, 36, 35, 36, 36, 36, 36, 36, 37, 36, 36, 36, 36, 36, 36, 36, 36, 36,  0, 36, 36, 36  }

            

        };
        
        public Lexico()
        {

            
            log     = new StreamWriter("prueba.log");
            asm     = new StreamWriter("prueba.asm");
            log.AutoFlush=true;
            asm.AutoFlush=true;
            if (File.Exists("prueba.cpp"))
            {
                linea =1;
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
            if(Path.GetExtension(nombre) == ".cpp" )
            {
                
                
                if(File.Exists(nombre))
                {
                    asm  = new StreamWriter(nombre + ".asm"); 
                    asm.AutoFlush=true;
                    archivo = new StreamReader(nombre);
                     
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

            DateTime fechaEntrada = DateTime.Now;
            log.WriteLine("Fecha de compilacion: " + fechaEntrada);
            log.WriteLine("Archivo: "+ nombre);
                
        }
        
        public void Dispose()
        {
            //Contador de lineas 
        
            int contadorlinea = File.ReadAllLines("prueba.cpp").Length;
            log.WriteLine("Numero de lineas: " + contadorlinea   );
            archivo.Close();
            log.Close();
            asm.Close();
            


            
        }
        /*
            WS	L	D	.	E|e	+	-	;	{	}	?	=	*	%	&	|	!	<	>	"	\'	#	/	\n	EOF	λ
        */



        private int Column(char c)
        {
            if (c == '\n')
            {
                return 23;
            }
            else if (finArchivo())
            {
                return 24;
            }
            else if (char.IsWhiteSpace(c))
            {
                return 0;
            }
            else if (char.ToLower(c) == 'e')
            {
                return 4;
            }
            else if (char.IsLetter(c))
            {
                return 1;
            }
            else if (char.IsDigit(c))
            {
                return 2;
            }
            else if (c == '.')
            {
                return 3;
            }
            else if(c== '+')
            {
                return 5;
            }
            else if(c=='-')
            {
                return 6;
            }
            else if(c == ';')
            {
                return 7;
            }
            else if(c== '{')
            {
                return 8;
            }
            else if(c== '}')
            {
                return 9;
            }
            else if(c== '?')
            {       
                return 10;
            }
            else if(c== '=')
            {
                return 11;
            }
            else if(c== '*')
            {
                return 12;
            }
            else if(c== '%')
            {
                return 13;
            }
            else if(c== '&')
            {
                return 14;
            }
            else if(c== '|')
            {
                return 15;
            }
            else if(c== '!')
            {
                return 16;
            }
            else if(c== '<')
            {
                return 17;
            }
            else if(c== '>')
            {
                return 18;
            }
            else if(c=='"')
            {
                return 19;
            }
            else if(c== '\'')
            {
                return 20;
            }
            else if(c== '#')
            {
                return 21;
            }
            else if(c== '/')
            {
                return 22;
            }

               
            return 25;
            

            
        }
        private void Clasifica(int estado)
        {
            switch(estado)
            {
                case 1: setClasificacion(Tipos.Identificador); break;
                case 2: setClasificacion(Tipos.Numero); break;
                case 8: setClasificacion(Tipos.FinSentencia); break;
                case 9: setClasificacion(Tipos.InicioBloque); break;
                case 10: setClasificacion(Tipos.FinBloque); break;
                case 11: setClasificacion(Tipos.OperadorTernario); break;
                case 12:
                case 14: setClasificacion(Tipos.OperadorTermino); break;
                case 13: setClasificacion(Tipos.IncrementoTermino); break;
                case 15: setClasificacion(Tipos.Puntero); break;
                case 16:
                case 34: setClasificacion(Tipos.OperadorFactor); break;
                case 17: setClasificacion(Tipos.IncrementoFactor); break;
                case 18:
                case 20:
                case 29:
                case 32:
                case 33: setClasificacion(Tipos.Caracter); break;
                case 19:
                case 21: setClasificacion(Tipos.OperadorLogico); break;
                case 22:
                case 24:
                case 25:
                case 26: setClasificacion(Tipos.OperadorRelacional); break;
                case 23: setClasificacion(Tipos.Asignacion); break;
                case 27: setClasificacion(Tipos.Cadena); break;

                
                

            }
        }
        public void nextToken()
        {
            char c;
            string buffer = "";
            int estado = 0;
            
            while(estado >= 0)
            {   
                
                c = (char)archivo.Peek();
                estado = TRAND[estado, Column(c)];
                Clasifica(estado);
                
               
                
                if(estado >= 0)
                {
                    archivo.Read();
                    if (char.IsWhiteSpace(c))
                    {
                        columna++;

                    }
                    else if (char.IsLetterOrDigit(c))
                    {
                        columna++;
                    }
                    if(c == '\n')
                    {
                        linea++;
                    }
                    if(estado > 0)
                    {
                        buffer += c;
                    }
                    else
                    {
                        buffer= "";
                    }
                }
            }
                
            if(estado == E)
            {
                if(getClasificacion() == Tipos.Numero)
                {
                    throw new Error(" Lexico, se espera un digito", log, linea);
                }
                else if(getClasificacion() == Tipos.Cadena)
                {
                    throw new Error(" Lexico, se espera un cierre de comillas", log, linea);
                }
                else if(getClasificacion() == Tipos.Caracter)
                {
                    throw new Error(" Lexico, se espera cierre de una comilla", log, linea);
                }
                else
                {
                    throw new Error("Lexico, se espera cierre de comentario", log, linea);
                }
                    
            }
            setContenido(buffer);
            if(getClasificacion() == Tipos.Identificador) 
            {
                switch(getContenido())
                {
                    case "char":
                    case "int":
                    case "float":
                        setClasificacion(Tipos.TipoDato);
                    break;
                    case "if":
                    case "else":
                    case "do":
                    case "while":
                    case "for":
                        setClasificacion(Tipos.PalabraReservada);
                    break;

                }

            }
            
           
            if(!finArchivo())
            {
                setContenido(buffer);
               // log.WriteLine(getContenido() + " = " + getClasificacion()); 
                  
                     
            }                   
        } 
        public bool finArchivo()
        {   
            return archivo.EndOfStream;
        }
        
    }
}
/*
    Expresion regular: Metodo Formal que a través de una  secuencia de 
    caracteres que define un patron  de busquda
    
    a) Reglas BNF
    b) Reglas BNF extendidas
    c) Operacioes aplicadas al lenguaje
    
    OAL
    1. Concatenacion simple (·)
    2. Concatenacion exponencial(Exponente)
    3. Cerradura de Kleene (*)
    4. Cerradura Positiva (+)
    5. Cerradura Epsilon (?)
    6. Operador OR (|)
    7. Parentesis ()
    
    L = {A,B,C,D,E,...Z, a,b,c,d,...,z}
    D = {0,1,2,3,4,5,6,7,8,9}

    1.  L·D
        LD
        >=

    2.  L^3= LLL
        L^3D^2 = LLLDD
        D^5= DDDDD
        =^2 = ==

    3.  L*= Cero o más letras
        D* = Cero o mas digitos
    
    4.  L+ = Uno o más letras
        D+ = Uno o mas digitos
    
    5.  L? = Cero o una letra (La letra es optativa-opcional)
    6.  L|D = Letra o digito
        + | - = + o menos
    7. (L D) L? = Agrupacion de letras (Letra seguido de un digito y al final letra opcional)

    Produccion Gramatical

    Clasificacion del token -> Expresion Regular
    Identificaor -> L (L|D) *
    Numero -> D+ (.D+)? (E(+|-)?D+)?
    Caracter-> 'c' | #D* | Lambda 
    FinSentencia-> ;
    InicioBloque-> {}
    FinBloque-> }
    OperadorTernario-> ?
    OperadorTermino-> + | -
    
    IncrementoTermino-> +(+ | =) | -(-| =)
    Puntero-> ->
    Termino  + -> +(+ | =)?
    Termino -p -> -(- | = | >)?
    
    OperadorFactor-> * | / | % 
    IncrementoTermino->  
    IncrementoFactor-> *= | /= | %= 
    Factor  -> * | / | % (=)?
    
    Asignacion-> =
    AsgOpRel -> = (=)?
    Moneda-> 
    OperadorRelacional-> > (=)? | < (> | =)? | == | !=
    OperadorLogico-> && | || | !
    NotOpRel-> ! (=)?
    Cadena-> "c*"
    

    Automata:Modelo matemático que representa una expreion regular 
    a travez de un GRAFO, para ua maquina de estado finito 
    que conciste en un conjunt de estados bien definidos:  
    - un estado inicial.s 
    - un alfabeto de entrada 
    - una funcion de transmicio 

*/