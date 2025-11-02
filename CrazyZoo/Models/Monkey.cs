using CrazyZoo.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrazyZoo.Models
{
    public class Monkey : Animal, ICrazyAction
    {
        public override string MakeSound() => $"{Name}: Ooh-ooh Aah-aah!";

        public string ActCrazy(ObservableCollection<Animal> enclosure)
        {
            var others = enclosure.Where(a => a != this).ToList();
            if (others.Count < 2)
                return $"{Name} üritas segadust külvata, aga teisi loomi on vähe.";

            var rnd = new Random();

            var first = others[rnd.Next(others.Count)];
            Animal second;
            do
            {
                second = others[rnd.Next(others.Count)];
            } while (second == first);

            (first.Name, second.Name) = (second.Name, first.Name);

            return $"{Name} vahetas {first.GetType().Name} ja {second.GetType().Name} nimed!";
        }


        public string ActCrazy() => $"{Name} loopis banaani ja tegi kaost!";
    }
}
