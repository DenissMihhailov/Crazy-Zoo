using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrazyZoo.Models
{
    public class AnimalEventArgs : EventArgs
    {
        public Animal Animal { get; }
        public AnimalEventArgs(Animal animal) => Animal = animal;
    }

    public class FoodDroppedEventArgs : EventArgs
    {
        public string Food { get; }
        public FoodDroppedEventArgs(string food) => Food = food;
    }

    public class NightEventArgs : EventArgs { }
}
