using System;
using System.Threading;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;


namespace LR1
{
   internal class Program
   {
       public const int Answer = 109751326 ; //1225051;
                                    //63779


       private static bool multithreading = true;
       private static int NumberOfThreads = 4;
       public const string Alphabet = "1234567890qwertyuiopasdfghjklzxcvbnmQWERTYUIOPASDFGHJKLZXCVBNM";
       public static List<string> Passwords = new List<string>();
       public static bool Found = false;
       
      private static int GetCode(string St)
      {
          int N = 0;
          foreach (var t in St)
          {
              var c = (int)t;
              N = N * 10 + c * c;
          }
          return N ;
          }
      
      private static void GetCombinations(int length, int startPos = 0, int endPos = 62, string prefix = "")
      {
          if (length == 0)
          {
              var alphabetMatch = "";
              
              foreach (var i in prefix.Split('|'))
              {
                  alphabetMatch += Alphabet[int.Parse(i)];
              }
              
              //Console.WriteLine(alphabetMatch);
              
              if (GetCode(alphabetMatch) != Answer) return;
              Passwords.Add(alphabetMatch);
              Found = true;
              return;
          }
          for (var i = startPos; i < endPos; i++)
          {
              if (prefix == "")
              {
                  GetCombinations(length - 1, prefix: prefix + i);
              }
              else
              {
                  GetCombinations(length - 1, prefix: prefix + '|' + i);
              }

          }
      }

    
      public static void Main(string[] args)
      {        
          var myWatch = new Stopwatch();
          var timers = new List<int>();
          var myThreads = new List<Thread>();
          
          for (var i = 1; i <= 5; i++)
          {
              Console.WriteLine("===============================================================================");
              Console.WriteLine("Генерация паролей длины " + i + "....");

              if (multithreading == true && i >= 4)
              {
                  myWatch.Start();
                  var part = Alphabet.Length / NumberOfThreads;
                  var startPosition = Alphabet.Length - part;
                  var endPosition = Alphabet.Length;   
                  for (int j = 0; j < NumberOfThreads; j++)
                  {
                      if (j == NumberOfThreads - 1)
                      {
                          myThreads.Add(new Thread(new ThreadStart(delegate { GetCombinations(i, startPosition - 2, endPos:endPosition); })));
                      }
                      else
                      {
                          myThreads.Add(new Thread(new ThreadStart(delegate { GetCombinations(i, startPosition, endPos:endPosition); })));
                      }

                      myThreads[j].Start();
                      Thread.Sleep(10);
                      startPosition -= part;
                      endPosition -= part;
                  }

                  foreach (var VARIABLE in myThreads)
                  {
                      VARIABLE.Join();
                  }
                  myWatch.Stop();
                  timers.Add(myWatch.Elapsed.Minutes);
                  myThreads.Clear();
              }
              else
              {
                  myWatch.Start();
                  GetCombinations(i);
                  myWatch.Stop();
                  timers.Add(myWatch.Elapsed.Minutes);
              }
              
              /*
              if (Found)
              {
                  if (i >= 4)
                  {
                      Console.WriteLine("Генерация заняла " + myWatch.Elapsed.Minutes + " мин.");
                      Console.WriteLine("Генерация завершена. Подобраны следующие пароли:");
                  }
                  else
                  {
                      Console.WriteLine("Пароли данной длины не найдены. Переход к поиску паролей длины " + (i + 1));
                      Console.WriteLine("Генерация заняла " + myWatch.Elapsed.Minutes + " мин.");
                  }
                  
                  Console.WriteLine("Найден пароль для данного кода!");
                  Console.WriteLine("Генерация заняла " + myWatch.Elapsed.Minutes + " мин.");
              }
              else
              {
                  if (i == 5)
                  {
                      Console.WriteLine("Генерация заняла " + myWatch.Elapsed.Minutes + " мин.");
                      Console.WriteLine("Генерация завершена. Подобраны следующие пароли:");
                  }
                  else
                  {
                      Console.WriteLine("Пароли данной длины не найдены. Переход к поиску паролей длины " + (i + 1));
                      Console.WriteLine("Генерация заняла " + myWatch.Elapsed.Milliseconds + " мc.");
                  }
              }
              */
              if (i < 5)
              {
                  Console.WriteLine("Переход к генерации паролей длины " + (i + 1));
                  //Console.WriteLine("===============================================================================");
              }
             
              else
              {
                  Console.WriteLine("Подбор паролей окончен. Проверьте файл Passwords.txt....");
                  Console.WriteLine("===============================================================================");
              }
          }
         
          
          var path = @"D:\USATU\3 Курс\Информационные технологии\LR1\Passwords.txt";
          
          File.AppendAllText(path, "Найдены следующие пароли:" + "\n");
          foreach (var VARIABLE in Passwords.Distinct())
          {
              //Console.WriteLine(VARIABLE);  
              File.AppendAllText(path, VARIABLE + "\n");
          }
          
          File.AppendAllText(path, "Поиск составил:" + "\n");
          File.AppendAllText(path, timers.Last() + " min.");
          }
  }
}