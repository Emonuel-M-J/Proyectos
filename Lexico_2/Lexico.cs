using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.CompilerServices;
using System.Formats.Asn1;
using System.Runtime.InteropServices;
using System.ComponentModel;


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
        private int automata(char c,int estado)
        {
            int nuevoEstado=estado;
            switch(estado)
            {
                case 0: 
                    if(char.IsWhiteSpace(c))
                    {
                        nuevoEstado = 0;
                    }
                    else if(char.IsLetter(c))
                    {
                        nuevoEstado = 1;
                    }
                    else if(char.IsDigit(c))
                    {
                        nuevoEstado = 2;

                    }
                    else
                    {
                        nuevoEstado = 8;
                    }
                    break;
                    
                case 1:
                    setClasificacion (Tipos.Identificador);
                    if(!char.IsLetterOrDigit(c))
                    {
                        nuevoEstado = F;
                    }
                    

                    break;
                case 2:
                    setClasificacion (Tipos.Numero);
                    if(char.IsDigit(c))
                    {
                        nuevoEstado = 2;
                    }
                    else if (c == '.')
                    {
                        nuevoEstado = 3;
                    }
                    else if (char.ToLower(c) == 'e')
                    {
                        nuevoEstado = 5;
                    }
                    else
                    {
                        nuevoEstado = F;
                    }

                    break;
                
                case 3:
                    if(char.IsDigit(c))
                    {
                        nuevoEstado = 4;
                    }
                    else
                    {
                        nuevoEstado = E;
                    }
                    break;
                case 4:
                    if(char.IsDigit(c))
                    {
                        nuevoEstado = 4;
                    }
                     else if(char.ToLower(c)== 'e')
                    {
                        nuevoEstado = 5;
                    }
                    else
                    {
                        nuevoEstado = F;
                    }
                    break;
                case 5:
                    if (char.IsDigit(c))
                    {
                        nuevoEstado = 7;
                    }
                    else if (c == '+' || c == '-')
                    {
                        nuevoEstado = 6;
                    }
                    else
                    {
                        nuevoEstado = E;
                    }
                    break;

                case 6:
                     if(char.IsDigit(c))
                    {
                        nuevoEstado = 7;
                    }
                    else 
                    {
                        nuevoEstado = E;
                    }
                    break;
                case 7:
                    if(char.IsDigit(c))
                    {
                        nuevoEstado = 7;
                    }
                    else 
                    {
                        nuevoEstado = F;
                    }
                    break;
                case 8:
                    setClasificacion(Tipos.Caracter);
                    nuevoEstado = F;
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
                estado = automata(transicion,estado); 
                if(estado == E)
                {
                    if(getClasificacion() == Tipos.Numero)
                    {
                        throw new Error(" Lexico, se espera un digito", log, linea);
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