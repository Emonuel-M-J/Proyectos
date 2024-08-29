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
        Identificador,Numero,Caracter
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

    public string GetContenido()
    {
        return this.contenido;
    }
    public Tipos GetClasificacion()
    {
        return this.clasificacion;
    }
}