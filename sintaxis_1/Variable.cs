using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sintaxis_1
{
    public class Variable
    {
        public enum TipoDato
        {
            Char,
            Int,
            Float,
        
        }
     TipoDato Tipo;
     string nombre;
     float valor;
     public Variable(TipoDato tipo, string nombre, float valor = 0)
     {
        this.Tipo = tipo;
        this.nombre = nombre;
        this.valor = valor;
     }
     public void setValor(float valor)
     {
        this.valor = valor;

     }
     public float getValor()
     {
        return valor;
     }
     public string getNombre()
     {
        return nombre;
     }
     public TipoDato getTipoDato()
     {
        return Tipo;
     }

    }
}