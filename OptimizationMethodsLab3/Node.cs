using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OptimizationMethodsLab3
{
    class Node : ICloneable
    {
        private double defaultItem;
        private object transportationAmount;
        private object potentialValue;
        private bool state;
        public double DefaultItem 
        {
            get => defaultItem;
            set { defaultItem = value; }
        }
        public object TransportationAmount
        {
            get => transportationAmount;
            set { transportationAmount = value; }
        }
        public object PotentialValue
        {
            get => potentialValue;
            set { potentialValue = value; }
        }
        public bool State
        {
            get => state;
            set { state = value; }
        }
        public object Clone()
        {
            Node node = new Node();
            node.DefaultItem = DefaultItem;
            node.PotentialValue = PotentialValue;
            node.TransportationAmount = TransportationAmount;
            return node;
        }
        public override string ToString() 
        {
            string result = (PotentialValue == null) ? $"{DefaultItem}  {TransportationAmount}\t|" : $"  {DefaultItem}   {TransportationAmount}    {PotentialValue}\t|";
            return result;
        }
    }
}
