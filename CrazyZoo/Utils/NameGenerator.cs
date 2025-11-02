using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrazyZoo.Utils
{
    public static class NameGenerator
    {
        private static readonly Random _rnd = new();

        private static readonly List<string> CatNames = new()
    {
        "Miisu", "Muri", "Täpi", "Triibu", "Nurr", "Sädel", "Kassu"
    };

        private static readonly List<string> DogNames = new()
    {
        "Pontu", "Kusti", "Bruno", "Muki", "Rex", "Sultan", "Tupsu"
    };

        private static readonly List<string> BirdNames = new()
    {
        "Säuts", "Tiivuline", "Pipi", "Flaps", "Kirps", "Lendaja"
    };

        private static readonly List<string> RaccoonNames = new()
    {
        "Varas", "Sädel", "Riba", "Mask", "Pesu", "Kaval"
    };

        private static readonly List<string> MonkeyNames = new()
    {
        "Banaan", "Ahvi", "Kongo", "Mango", "Albert", "Pähkel", "Karamell"
    };

        public static string GetNameFor<T>() where T : class
        {
            if (typeof(T).Name == nameof(Models.Cat))
                return CatNames[_rnd.Next(CatNames.Count)];

            if (typeof(T).Name == nameof(Models.Dog))
                return DogNames[_rnd.Next(DogNames.Count)];

            if (typeof(T).Name == nameof(Models.Bird))
                return BirdNames[_rnd.Next(BirdNames.Count)];

            if (typeof(T).Name == nameof(Models.Raccoon))
                return RaccoonNames[_rnd.Next(RaccoonNames.Count)];

            if (typeof(T).Name == nameof(Models.Monkey))
                return MonkeyNames[_rnd.Next(MonkeyNames.Count)];

            return $"Loom{_rnd.Next(100)}";
        }
    }
}
