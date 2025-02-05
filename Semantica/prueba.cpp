using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

static void Main(string[] args)
{
 
  int b = 1.5;
  int a = 1.5;
  char c = a + b;

  int n = 5;

 /*for(b = 100; a < n; a++) {
    b++;
    while(b != 5) {
      if(n == 5) {
        aux = 5;
        Console.WriteLine("" + aux + " es igual a " + b);
      } else {
        aux = 5;
        Console.WriteLine("" + aux + " es diferente a " + n);
      }
    }
  }*/
  if(b % 2 != 0) {
    Console.WriteLine("Es impar " + a);
    if(b == 2) {
      
      Console.WriteLine("b es igual a " + aux);
    } else if( b > 3) {
      
      Console.WriteLine("b es mayor a " + aux + " y vale " + b);
    }
    else {
      
      Console.WriteLine("b no es igual a " + aux + " y vale " + b);
    }
  } else {
    Console.WriteLine("Es impar");
  }
}