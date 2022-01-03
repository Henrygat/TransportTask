using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OptimizationMethodsLab3
{
    class ProblemModel 
    {
        private List<TransportationTable> tableList;
        private Node[,] matrix;
        private double[] customersCount, storagesCount;
        private object[] columnPotential, rowPotential;
        private List<int> markedRows, markedColumns;

        public ProblemModel(double[,] matrix, double[] customersCount, double[] storagesCount)
        {
            this.matrix = new Node[matrix.GetLength(0), matrix.GetLength(1)];
            this.customersCount = customersCount;
            this.storagesCount = storagesCount;
         
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    this.matrix[i, j] = new Node();
                    this.matrix[i, j].DefaultItem = matrix[i, j];
                }
            }

            markedColumns = new List<int>();
            markedRows = new List<int>();
            tableList = new List<TransportationTable>();

            columnPotential = new object[matrix.GetLength(1)];
            rowPotential = new object[matrix.GetLength(0)];
            rowPotential[0] = 0;
        }
        public double Calculate()
        {
            GetInitialPlan();
            return Recalculate();
        }
        private KeyValuePair<(int, int), Node>[] GetNode(int rowIndex, int columnIndex)
        {         
            Dictionary<(int, int), Node> list = new Dictionary<(int, int), Node>();
            List<(int, int)> currentColumnIndexes = new List<(int, int)>();
            List<(int, int)> currentRowndexes = new List<(int, int)>();

            void addItemToDictionary((int, int) indexes, bool state)
            {
                Node node = (Node)matrix[indexes.Item1, indexes.Item2].Clone();
                node.State = state;
                list.Add((indexes.Item1, indexes.Item2), node);
            }

            addItemToDictionary((rowIndex, columnIndex), true);

            bool CheckColumn(int currentColumnIndex,int generalRow)
            {
                bool result = default;
                for (int i = 0; i < matrix.GetLength(0); i++)
                {
                    if (matrix[i, currentColumnIndex].TransportationAmount != null)
                    {
                        if (i == generalRow)
                        {
                            result = true;
                            break;
                        }
                    }
                }
                return result;
            }
            bool CheckRow((int,int) columnIndexes,int generalRow)
            {
                bool result = default;
                for (int i = 0; i < matrix.GetLength(1); i++)
                {
                    if (matrix[columnIndexes.Item1, i].TransportationAmount != null) 
                    {
                        if (CheckColumn(i, generalRow) == true)
                        {
                            result = true;
                            break;
                        }
                    }
                }
                return result;
            }
            bool CheckCurrentRow((int, int) rowIndexes, int generalRow)
            {
                bool result = default;
                for (int i = 0; i < matrix.GetLength(1); i++)
                {
                    if (matrix[generalRow, i].TransportationAmount != null)
                    {
                        if (i == rowIndexes.Item2)
                        {
                            result = true;
                            break;
                        }
                    }
                }
                return result;
            }

            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                if (matrix[i, columnIndex].TransportationAmount != null && i != rowIndex) currentColumnIndexes.Add((i, columnIndex));
            }
            for (int i = 0; i < matrix.GetLength(1); i++)
            {
                if (matrix[rowIndex,i].TransportationAmount != null && i != columnIndex) currentRowndexes.Add((rowIndex, i));
            }

            int resultRowIndex = default;
            for (int i = 0; i < currentColumnIndexes.Count; i++)
            {
                if (CheckRow(currentColumnIndexes[i],rowIndex))
                {
                    addItemToDictionary((currentColumnIndexes[i].Item1, currentColumnIndexes[i].Item2), false);
                    resultRowIndex = currentColumnIndexes[i].Item1;
                    break;
                }
            }

            int resultColumnIndex = default;
            for (int i = 0; i < currentRowndexes.Count; i++)
            {
                if (CheckCurrentRow(currentRowndexes[i], resultRowIndex) == true)
                {
                    addItemToDictionary((currentRowndexes[i].Item1, currentRowndexes[i].Item2), false);
                    resultColumnIndex = currentRowndexes[i].Item2;
                    break;
                }
            }

            addItemToDictionary((resultRowIndex, resultColumnIndex), true);
            return list.ToArray(); 
        }
        private double Recalculate()
        {
            GetRowColumnPotentials();
            GetMatrixPotential();

            Node[,] tempMatrix = CloneMatrix();
            tableList.Add(new TransportationTable(tempMatrix, customersCount, storagesCount));

            if (IsBadPlan())
            {
                Node temp = new Node();
                temp.PotentialValue = int.MinValue;
                int rowIndex = 0;
                int columnIndex = 0;
                double max = int.MinValue;

                for (int i = 0; i < matrix.GetLength(0); i++)
                {
                    for (int j = 0; j < matrix.GetLength(1); j++)
                    {
                        if (double.Parse(matrix[i,j].PotentialValue.ToString()) > 0)
                        {
                            if (max < double.Parse(matrix[i, j].PotentialValue.ToString()))
                            {
                                max = double.Parse(matrix[i, j].PotentialValue.ToString());
                                rowIndex = i;
                                columnIndex = j;
                            }         
                        }
                    }
                }

                KeyValuePair<(int, int), Node>[] array = GetNode(rowIndex, columnIndex).ToArray();
 
                double min = double.MaxValue;
                for (int i = 1; i < array.Length; i++)
                {
                    if (min > double.Parse(array[i].Value.TransportationAmount.ToString()) && array[i].Value.State == false)
                    {
                        min = double.Parse(array[i].Value.TransportationAmount.ToString());
                    }
                }

                for (int i = 0; i < array.Length; i++)
                {
                    if (array[i].Value.State == true)
                    {
                        if (matrix[array[i].Key.Item1, array[i].Key.Item2].TransportationAmount == null)
                        {
                            matrix[array[i].Key.Item1, array[i].Key.Item2].TransportationAmount = 0;
                        }
                        matrix[array[i].Key.Item1, array[i].Key.Item2].TransportationAmount = double.Parse(matrix[array[i].Key.Item1, array[i].Key.Item2].TransportationAmount.ToString()) + min;
                    }
                    else
                    {
                        matrix[array[i].Key.Item1, array[i].Key.Item2].TransportationAmount = double.Parse(matrix[array[i].Key.Item1, array[i].Key.Item2].TransportationAmount.ToString()) - min;
                        if (double.Parse(matrix[array[i].Key.Item1, array[i].Key.Item2].TransportationAmount.ToString()) == 0)
                        {
                            matrix[array[i].Key.Item1, array[i].Key.Item2].TransportationAmount = null;
                        }
                    }
                }

                columnPotential = new object[matrix.GetLength(1)];
                rowPotential = new object[matrix.GetLength(0)];
                rowPotential[0] = 0;
                return Recalculate();
            }
           else return GetCurrentFunctionValue();
        }
        public void PrintResults()
        {
            for (int i = 0; i < tableList.Count; i++)
            {
                tableList[i].Print();
            }
        }
        private void GetInitialPlan()
        {
            Node[,] temp = CloneMatrix();
            tableList.Add(new TransportationTable(temp, customersCount, storagesCount));

            if (IsInitiaPlanReady() != false)
            {
                (int rowIndex, int columnIndex) = GetMaxtrixMinItemIndexes();
                matrix[rowIndex, columnIndex].TransportationAmount = Math.Min(customersCount[columnIndex], storagesCount[rowIndex]);
                customersCount[columnIndex] -= (double)matrix[rowIndex, columnIndex].TransportationAmount;
                storagesCount[rowIndex] -= (double)matrix[rowIndex, columnIndex].TransportationAmount;

                if (customersCount[columnIndex] == 0 && storagesCount[rowIndex] == 0) AddRestrictions(rowIndex, markedRows);
                else
                {
                    if (customersCount[columnIndex] == 0) AddRestrictions(columnIndex, markedColumns);
                    else AddRestrictions(rowIndex, markedRows);
                }
                GetInitialPlan();
            }
        }
        private (int, int) GetMaxtrixMinItemIndexes()
        {
            Node currentNode = new Node();
            currentNode.DefaultItem = double.MaxValue;
            int rowIndex = default;
            int columnIndex = default;

            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                if (markedRows.Contains(i)) continue;
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    if (markedColumns.Contains(j)) continue;
                    if (currentNode.DefaultItem > matrix[i, j].DefaultItem)
                    {
                        currentNode.DefaultItem = matrix[i, j].DefaultItem;
                        rowIndex = i;
                        columnIndex = j;
                    }
                }
            }
            return (rowIndex, columnIndex);
        }
        private bool IsBadPlan()
        {
            bool result = false;
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    if (0 < double.Parse(matrix[i,j].PotentialValue.ToString()))
                    {
                        result = true;
                        break;
                    }
                }
            }
            return result;
        } 
        private void GetRowColumnPotentials()
        {
            if (!(IsFull(columnPotential) && IsFull(rowPotential)))
            {
                for (int i = 0; i < matrix.GetLength(0); i++)
                {
                    for (int j = 0; j < matrix.GetLength(1); j++)
                    {
                        if (matrix[i, j].TransportationAmount != null)
                        {
                            if (rowPotential[i] != null && columnPotential[j] == null)
                                columnPotential[j] = matrix[i, j].DefaultItem + double.Parse(rowPotential[i].ToString());
                            else if (rowPotential[i] == null && columnPotential[j] != null)
                                rowPotential[i] = double.Parse(columnPotential[j].ToString()) - matrix[i, j].DefaultItem;
                        }
                    }
                }
                GetRowColumnPotentials();
            }
        }
        private bool IsFull(object[] array)
        {
            bool result = true;
            for (int i = 0; i < array.Length; i++)
            {
                if (array[i] == null)
                {
                    result = false;
                    break;
                }
            }
            return result;
        }
        private Node[,] CloneMatrix()
        {
            Node[,] temp = new Node[matrix.GetLength(0), matrix.GetLength(1)];
            for (int i = 0; i < temp.GetLength(0); i++)
            {
                for (int j = 0; j < temp.GetLength(1); j++)
                {
                    temp[i, j] = (Node)matrix[i, j].Clone();
                }
            }
            return temp;
        }
        private void GetMatrixPotential()
        {       
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                   matrix[i, j].PotentialValue = double.Parse(columnPotential[j].ToString()) - double.Parse(rowPotential[i].ToString()) - matrix[i, j].DefaultItem;
                }
            }
        }
        private double GetCurrentFunctionValue()
        {
            double result = default;
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    if (matrix[i,j].TransportationAmount != null) result += matrix[i, j].DefaultItem * (double)matrix[i, j].TransportationAmount;
                }
            }
            return result;
        }
        private void AddRestrictions(int index, List<int> list)
        {
            list.Add(index);
        }
        private bool IsInitiaPlanReady()
        {
            bool GetResult(double[] array)
            {
                bool result = default;
                for (int i = 0; i < array.Length; i++)
                {
                    if (array[i] != 0)
                    {
                        result = true;
                        break;
                    }
                }
                return result;
            }
            return GetResult(customersCount) & GetResult(storagesCount);
        }
    }
}
