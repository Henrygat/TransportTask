using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OptimizationMethodsLab3
{
    class Program
     {

        // Решение сбалансированной транспортной задачи
         static void Main(string[] args)
         {
            // 18 вариант
            double[,] matrix18 =
            {
                { 17, 16, 15, 29, 9},
                { 6, 27, 20, 25, 20 },
                { 6, 15, 12, 8, 14 },
                { 10, 24, 23, 5, 22 }
            };
            double[] b18 = { 16, 16, 16, 16, 16 };
            double[] a18 = { 25, 25, 15, 15 };

            // 9 вариант
            double[,] matrix9 =
            {
                { 10, 15, 14, 28, 1},
                { 16, 7, 30, 8, 29 },
                { 1, 21, 22, 19, 12 },
                { 8, 25, 28, 5, 19 }
            };
            double[] b9 = { 11, 11, 11, 8, 15 };
            double[] a9 = { 14, 14, 12, 16 };

            // 30 вариант
            double[,] matrix30 =
            {
                { 19, 9, 14, 17, 9},
                { 4, 21, 27, 8, 29 },
                { 22, 30, 4, 1, 24 },
                { 10, 22, 8, 5, 27 }
            };
            double[] b30 = { 9, 9, 9, 9, 24 };
            double[] a30 = { 17, 17, 16, 10 };

            // 16 вариант
            double[,] matrix16 =
            {
                { 33, 22,14, 34, 19},
                { 26, 16, 7, 29, 16 },
                { 28, 18, 17, 23, 30 },
                { 35, 25, 11, 22, 9 }
            };
            double[] b16 = { 14, 14, 14, 18, 10 };
            double[] a16 = { 16, 17, 21, 16 };

            // 21 вариант
            double[,] matrix21 =
            {
                { 3, 25, 11, 22, 12},
                { 9, 15, 4, 26, 12 },
                { 13, 22, 15, 12, 27 },
                { 6, 19, 8, 11, 8 }
            };
            double[] b21 = { 18, 18, 18, 18, 18 };
            double[] a21 = { 23, 25, 12, 30 };

            ProblemModel problemModel = new ProblemModel(matrix16, b16, a16);
            double result = problemModel.Calculate();
            problemModel.PrintResults();
            Console.WriteLine($"Общая стоимость перевозок составила: {result}\n\n");
         }
    }
}