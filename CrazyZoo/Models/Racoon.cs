using CrazyZoo.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrazyZoo.Models
{
    public class Raccoon : Animal, ICrazyAction
    {
        static readonly string[] Loot =
        {
        "sädelev münt", "võti", "lusikas", "kilekott", "kelluke", "klaasikild"
    };

        public override string MakeSound() => $"{Name}: Hiss...";

        public string ActCrazy()
        {
            var rnd = new Random();
            var item = Loot[rnd.Next(Loot.Length)];
            return $"{Name} leidis sädeleva vidina: {item}.";
        }
    }
}
