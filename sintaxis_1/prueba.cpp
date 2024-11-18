using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

static void Main(string[] args)
{
  int a;
  int b;

  a=0;

  if (1 == 2)
  {
    do
    {
      Console.WriteLine("Hola");
    } while (a < 10);
    a = 10;
    if (1 == 2)
      a = 20;
    else
      a = 30;
  }
  else
  {
    do
    {
      Console.Write(".");
    } while (a < 5);
    a = 40;
    for (a = 0; a < 10; a+=1)
    {
      //int while;
    }
    
  }
}
