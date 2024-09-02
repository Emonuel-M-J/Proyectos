using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Léxico_1


{

    public class Token
    {
        public enum Tipos
    {
        Identificador,Numero,Caracter,FiSentencia,

        InicioBloque, FinBloque,OperadorTernario
        OperadorTermino
        //llave abierta, llave cerrada
    }
        int clasificacion;
        string contenido;
public Token()
{
    contenido= "";
    clasificacion =0;
}
    }

   public  void setContenido(string contenido)
    {
        this.contenido= contenido;
    }
    public void setClasificacion(Tipos clasificacion)
    {
        this.clasificacion = clasificacion;

    }

    public string getContenido()
    {
        return this.contenido;
    }
    public Tipos getClasificacion()
    {
        return this.clasificacion;
    }
}