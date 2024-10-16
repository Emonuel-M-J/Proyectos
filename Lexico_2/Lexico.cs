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
namespace Lexico_2
{
    public class Lexico : Token, IDisposable
    {
        StreamReader archivo;
        StreamWriter log;
        StreamWriter asm;
        int linea=1;
        const int F=-1;
        const int E= -2;
        
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
        private int automata(int estado, char transicion)
        {
            int nuevoEstado=estado;
            switch(estado)
            {
                case 0: 
                    if(char.IsWhiteSpace(transicion))
                    {
                        nuevoEstado = 0;
                    }
                    else if(char.IsLetter(transicion))
                    {
                        nuevoEstado = 1;
                    }
                    else if(char.IsDigit(transicion))
                    {
                        nuevoEstado = 2;

                    }
                    else if( transicion == ';' )
                    {
                        nuevoEstado = 8;
                    }
                    else if( transicion == '{' )
                    {
                        nuevoEstado = 9;
                    }
                    else if( transicion == '}' )
                    {
                        nuevoEstado = 10;
                    }
                    else if( transicion == '?' )
                    {
                        nuevoEstado = 11;
                    }
                    else if( transicion == '+' )
                    {
                        nuevoEstado = 12;
                    }
                    else if (transicion == '-' )
                    {
                        nuevoEstado = 14;
                    }
                    else if(transicion == '*' || transicion == '%')
                    {
                        nuevoEstado = 16;
                    }
                    else if(transicion == '&')
                    {
                        nuevoEstado = 18;
                    }
                    else if(transicion == '|')
                    {
                        nuevoEstado = 20;
                    }
                    else if(transicion == '!')
                    {
                        nuevoEstado = 21;
                    }
                    else if(transicion == '=')
                    {
                        nuevoEstado = 23;
                    }
                    else if(transicion == '>')
                    {
                        nuevoEstado = 25;
                    }
                    else if(transicion == '<')
                    {
                        nuevoEstado= 26;
                    }
                    else if(transicion == '"')
                    {
                        nuevoEstado = 27;
                    }
                    else if (transicion == '\'')
                    {
                        nuevoEstado = 29;
                    }
                    else if(transicion == '#')
                    {
                        nuevoEstado = 32;
                    }
                    else if(transicion == '|')
                    {
                        nuevoEstado = 34;
                    }
                    else if(transicion == '/')
                    {
                        nuevoEstado = 34;
                    }
                    break;
                    
                    
                case 1:
                    setClasificacion (Tipos.Identificador);
                    if(!char.IsLetterOrDigit(transicion))
                    {
                        nuevoEstado = F;
                    }
                    

                    break;
                case 2:
                    setClasificacion (Tipos.Numero);
                    if(char.IsDigit(transicion))
                    {
                        nuevoEstado = 2;
                    }
                    else if (transicion == '.')
                    {
                        nuevoEstado = 3;
                    }
                    else if (char.ToLower(transicion) == 'e')
                    {
                        nuevoEstado = 5;
                    }
                    else
                    {
                        nuevoEstado = F;
                    }

                    break;
                
                case 3:
                    if(char.IsDigit(transicion))
                    {
                        nuevoEstado = 4;
                    }
                    else
                    {
                        nuevoEstado = E;
                    }
                    break;
                case 4:
                    if(char.IsDigit(transicion))
                    {
                        nuevoEstado = 4;
                    }
                     else if(char.ToLower(transicion)== 'e')
                    {
                        nuevoEstado = 5;
                    }
                    else
                    {
                        nuevoEstado = F;
                    }
                    break;
                case 5:
                    if (char.IsDigit(transicion))
                    {
                        nuevoEstado = 7;
                    }
                    else if (transicion == '+' || transicion == '-')
                    {
                        nuevoEstado = 6;
                    }
                    else
                    {
                        nuevoEstado = E;
                    }
                    break;

                case 6:
                     if(char.IsDigit(transicion))
                    {
                        nuevoEstado = 7;
                    }
                    else 
                    {
                        nuevoEstado = E;
                    }
                    break;
                case 7:
                    if(char.IsDigit(transicion))
                    {
                        nuevoEstado = 7;
                    }
                    else 
                    {
                        nuevoEstado = F;
                    }
                    break;
                    
                case 8:
                    setClasificacion(Tipos.FinSentencia);
                    nuevoEstado = F;
                break;
                case 9:
                    setClasificacion(Tipos.InicioBloque);
                    nuevoEstado = F;
                break;
                case 10:
                    setClasificacion(Tipos.FinBloque);
                    nuevoEstado = F;    
                break;
                case 11:
                    setClasificacion(Tipos.OperadorTernario);
                    nuevoEstado = F;
                break;
                case 12:
                    setClasificacion(Tipos.OperadorTermino);
                    nuevoEstado = F;
                    if(transicion == '+' || transicion =='=')
                    {
                         nuevoEstado = 13;

                    }

                break;
                case 13:
                    setClasificacion(Tipos.IncrementoFactor);
                    nuevoEstado = F;
                break;
                case 14:
                    setClasificacion(Tipos.OperadorTermino);
                    nuevoEstado = F;
                    if(transicion == '-'|| transicion =='=')
                    {
                        nuevoEstado=13;
                    }
                    else if(transicion == '>')
                    {
                        nuevoEstado = 15;
                    }
                break;
                case 15:
                    setClasificacion(Tipos.Puntero);
                    nuevoEstado = F;
                break;
                case 16:
                    setClasificacion(Tipos.OperadorFactor);
                    nuevoEstado = F;
                    if(transicion== '=')
                    {
                     nuevoEstado = 17;
                    }
                break;
                case 17:
                    setClasificacion(Tipos.IncrementoFactor);
                    nuevoEstado = F;
                break;
                case 18:
                    setClasificacion(Tipos.Caracter);
                    nuevoEstado = F;
                    if(transicion == '&')
                    {
                        nuevoEstado = 19;
                    }
                break;
                case 19:
                    setClasificacion(Tipos.OperadorLogico);
                    nuevoEstado = F;
                break;
                case 20:
                    setClasificacion(Tipos.Caracter);
                    nuevoEstado = F;
                    if(transicion == '|')
                    {
                        nuevoEstado = 19;
                    }
                break;
                case 21:
                    setClasificacion(Tipos.OperadorLogico);
                    nuevoEstado = F;
                    if(transicion == '=')
                    {
                        nuevoEstado = 22;
                    }
                break;
                case 22:
                    setClasificacion(Tipos.OperadorRelacional);
                    nuevoEstado = F;
                break;
                case 23:
                    setClasificacion(Tipos.Asignacion);
                    nuevoEstado = F;
                    if(transicion == '=')
                    {
                        nuevoEstado = 24;
                    }
                break;
                case 24:
                    setClasificacion(Tipos.OperadorRelacional);
                    nuevoEstado = F;
                break;
                case 25:
                    setClasificacion(Tipos.OperadorRelacional);
                    nuevoEstado = F;
                    if(transicion == '=')
                    {
                        nuevoEstado = 24;
                    }
                break;
                case 26:
                    setClasificacion(Tipos.OperadorRelacional); 
                    nuevoEstado = F;
                    if(transicion == '>' || transicion == '=')
                    {
                        nuevoEstado = 24;
                    }
                break;
                case 27:
                    setClasificacion(Tipos.Cadena);
                    nuevoEstado = 27;
                    if(transicion == '"')
                    {
                        nuevoEstado = 28;
                    }
                    else if(finArchivo())
                    {
                        nuevoEstado = E;
                    }
                break;
                case 28:
                    nuevoEstado = F;
                break;
                case 29:
                    setClasificacion(Tipos.Caracter);
                    nuevoEstado = 30;
                break;
                case 30:
                    if (transicion == '\'')
                    {
                        nuevoEstado = 31;
                    }
                    else
                    {
                        nuevoEstado = E;
                    }
                break;
                case 31:
                    nuevoEstado = F;
                
                break;
                case 32:
                    setClasificacion(Tipos.Caracter);
                    nuevoEstado = F;
                    if(char.IsDigit(transicion))
                    {
                        nuevoEstado = 32;
                    }
                break;
                case 33:
                    setClasificacion(Tipos.Caracter);
                    nuevoEstado = F;
                break;
                case 34:
                    setClasificacion(Tipos.OperadorFactor);
                    nuevoEstado = F;
                    if(transicion == '=')
                    {
                        nuevoEstado = 17;
                    }
                    else if(transicion == '/')
                    {
                        nuevoEstado = 35;
                    }
                    else if(transicion == '*')
                    {
                        nuevoEstado = 36;
                    }
                break;
                case 35:
                    if(transicion == '\n')
                    {
                        nuevoEstado=0;
                    }
                break;
                case 36:
                    nuevoEstado=36;
                    if(transicion == '*')
                    {
                    nuevoEstado=37;
                    }
                    else if(finArchivo())
                    {
                        throw new Exception("Se esperaba cierre de comentario");
                    }
                    
                break;
                case 37:
                    nuevoEstado=36;
                    if(transicion == '*')
                    {
                        nuevoEstado=37;
                    } 
                    else if(transicion == '/')
                    {
                        nuevoEstado=0;
                    }
                    else if(finArchivo())
                    {
                        throw new Exception("Se esperaba cierre de comentario");
                    }
                break;

            }
            return nuevoEstado;
        }
        public void nextToken()
        {
            char transicion;
            string buffer = "";
            int estado = 0;

            while(estado >= 0)
            {
                transicion = (char)archivo.Peek();
                estado = automata(estado,transicion); 
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
                    
                }
                if(estado >= 0)
                {
                    archivo.Read();
                    if(transicion == '\n')
                    {
                        linea++;
                    }
                    if(estado > 0)
                    {
                        buffer += transicion;
                    }
                }
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