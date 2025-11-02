using CrazyZoo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrazyZoo.Domain
{
    public class Enclosure<T> where T : Animal
    {
        private readonly List<T> _animals = new();

        public event EventHandler<AnimalEventArgs>? AnimalJoinedInSameEnclosure;
        public event EventHandler<FoodDroppedEventArgs>? FoodDropped;
        public event EventHandler<NightEventArgs>? NightEvent;

        public IReadOnlyList<T> Animals => _animals;

        public void Add(T animal)
        {
            _animals.Add(animal);
            AnimalJoinedInSameEnclosure?.Invoke(this, new AnimalEventArgs(animal));
        }

        public void Remove(T animal) => _animals.Remove(animal);

        public async Task DropFoodSequentialAsync(string food, Action<string> log, Func<T, int> eatDelayMs)
        {
            FoodDropped?.Invoke(this, new FoodDroppedEventArgs(food));

            foreach (var a in _animals.ToList())
            {
                var delay = Math.Max(50, eatDelayMs(a));
                await Task.Delay(delay);
                log($"{a.Name} sõi {food} ({delay} ms).");
            }
        }

        public void RaiseNightEvent() => NightEvent?.Invoke(this, new NightEventArgs());

        public IEnumerable<T> GetAll() => _animals.ToList();
        public IEnumerable<IGrouping<Type, T>> GroupByType() => _animals.GroupBy(a => a.GetType());
        public double AverageAge() => _animals.Any() ? _animals.Average(a => a.Age) : 0.0;
    }
}
