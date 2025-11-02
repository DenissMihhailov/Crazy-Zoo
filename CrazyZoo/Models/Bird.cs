using CrazyZoo.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrazyZoo.Models
{
    public class Bird : Animal, IFlyable, ICrazyAction
    {
        public bool IsFlying { get; private set; }
        public override string MakeSound() => $"{Name}: Chirp!";
        public void Fly() => IsFlying = !IsFlying;

        public override string Describe()
        {
            var state = IsFlying ? " (lendab)" : " (ei lenda)";
            return base.Describe() + state;
        }

        public string ActCrazy() => $"{Name} karjub: CHIRP!!!";
    }
}
