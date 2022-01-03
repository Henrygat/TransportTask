using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OptimizationMethodsLab3
{
    class TransportationTable
    {
        private object[,] table;
        public TransportationTable(Node[,] matrix, double[] customersCount, double[] storagesCount)
        {   
            table = new object[matrix.GetLength(0) + 1, matrix.GetLength(1) + 1];

            for(int i = 0; i < storagesCount.Length; i++)
            {
                table[i, table.GetLength(1) - 1] = storagesCount[i];
            }
            for (int i = 0; i < customersCount.Length; i++)
            {
                table[table.GetLength(0) - 1,i] = customersCount[i];
            }
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    table[i, j] = matrix[i, j];
                }
            }
        }
        public void Print()
        {
            for (int i = 0; i < table.GetLength(0); i++)
            {
                for (int k = 0; k < table.GetLength(1); k++)
                {
                    if (i == table.GetLength(0) - 1) Console.Write($"    {table[i, k]}\t");
                    else Console.Write($"{table[i, k]}");
                }
                Console.WriteLine();
            }
            Console.WriteLine("\n");
        }
    }
}
