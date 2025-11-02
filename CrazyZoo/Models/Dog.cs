using CrazyZoo.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrazyZoo.Models
{
    public class Dog : Animal, ICrazyAction
    {
        public override string MakeSound() => $"{Name}: Woof!";

        public string ActCrazy() => $"{Name} teeb mini-esinemise: " +
                                    "Woof! Woof! Woof! Woof! Woof!";
    }
}
