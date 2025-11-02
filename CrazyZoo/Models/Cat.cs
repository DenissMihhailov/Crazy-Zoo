using CrazyZoo.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrazyZoo.Models
{
    public class Cat : Animal, ICrazyAction
    {
        public override string MakeSound() => $"{Name}: Meow!";
        public string ActCrazy() => $"{Name} varastas köögist juustu!";
    }
}
