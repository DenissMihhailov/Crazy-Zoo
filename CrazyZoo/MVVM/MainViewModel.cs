using CrazyZoo.Data;
using CrazyZoo.Domain;
using CrazyZoo.Interfaces;
using CrazyZoo.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Timers;
using CrazyZoo.Utils;


namespace CrazyZoo.MVVM
{
    public class MainViewModel
    {
        public ObservableCollection<Animal> Animals { get; } = new();
        public ObservableCollection<string> Log { get; } = new();
        public ObservableCollection<string> StatsLines { get; } = new();

        private readonly IRepository<Animal> _repo;
        private readonly Enclosure<Animal> _enclosure;
        private readonly Random _rnd = new();
        private readonly System.Timers.Timer _nightTimer;

        private Animal? _selected;
        public Animal? SelectedAnimal
        {
            get => _selected;
            set { _selected = value; UpdateStats(); }
        }

        public string FoodInput { get; set; } = "";

        public RelayCommand MakeSoundCmd { get; }
        public RelayCommand FeedCmd { get; }
        public RelayCommand CrazyCmd { get; }
        public RelayCommand AddCmd { get; }
        public RelayCommand RemoveCmd { get; }
        public RelayCommand DropFoodCmd { get; }

        public MainViewModel(IRepository<Animal> repo, Enclosure<Animal> enclosure)
        {
            _repo = repo;
            _enclosure = enclosure;

            _enclosure.AnimalJoinedInSameEnclosure += (_, e) => LogAdd($"Uus loom voljeeris: {e.Animal.Describe()}");
            _enclosure.FoodDropped += (_, e) => LogAdd($"Toit kukkus: {e.Food}");
            _enclosure.NightEvent += (_, __) => OnNight();

            Seed();

            MakeSoundCmd = new RelayCommand(_ => { if (SelectedAnimal != null) LogAdd(SelectedAnimal.MakeSound()); });
            FeedCmd = new RelayCommand(_ => {
                if (SelectedAnimal != null)
                {
                    var food = string.IsNullOrWhiteSpace(FoodInput) ? "midagi maitsvat" : FoodInput.Trim();
                    LogAdd($"{SelectedAnimal.Name} sõi {food}.");
                    FoodInput = "";
                }
            });
            CrazyCmd = new RelayCommand(_ => DoCrazy());
            AddCmd = new RelayCommand(_ => AddRandomAnimal());
            RemoveCmd = new RelayCommand(_ => {
                if (SelectedAnimal != null)
                {
                    var removed = SelectedAnimal;
                    _repo.Remove(removed);
                    Animals.Remove(removed);
                    _enclosure.Remove(removed);
                    LogAdd($"Eemaldatud: {removed.Name}");
                    SelectedAnimal = Animals.FirstOrDefault();

                }
            }, _ => SelectedAnimal != null);

            DropFoodCmd = new RelayCommand(async _ => await DropFoodSequence());

            _nightTimer = new System.Timers.Timer(10_000);
            _nightTimer.Elapsed += (_, __) => _enclosure.RaiseNightEvent();
            _nightTimer.AutoReset = true;
            _nightTimer.Start();


            UpdateStats();
        }

        private void Seed()
        {
            var init = new Animal[] {
            new Cat { Name = "Marsik", Age = 2 },
            new Dog { Name = "Lucky",  Age = 4 },
            new Bird{ Name = "Gosha", Age = 1 },
            new Raccoon { Name = "Felix", Age = 3 },
            new Monkey  { Name = "Albert", Age = 5 }
        };

            foreach (var a in init)
            {
                _repo.Add(a);
                Animals.Add(a);
                _enclosure.Add(a);
            }
            SelectedAnimal = Animals.FirstOrDefault();
        }

        private void LogAdd(string line)
        {
            App.Current.Dispatcher.Invoke(() =>
            {
                Log.Add(line);
            });
        }

        private void DoCrazy()
        {
            if (SelectedAnimal is null) return;

            if (SelectedAnimal is ICrazyAction crazy)
                LogAdd(crazy.ActCrazy());

            if (SelectedAnimal is IFlyable fly)
            {
                fly.Fly();
                if (SelectedAnimal is Bird b)
                    LogAdd(b.IsFlying ? $"{b.Name} hakkas lendama!" : $"{b.Name} maandus!");
            }

            if (SelectedAnimal is Monkey m)
            {
                var others = Animals.Where(a => a != m).ToList();
                if (others.Count >= 2)
                {
                    var first = others[_rnd.Next(others.Count)];
                    Animal second;
                    do { second = others[_rnd.Next(others.Count)]; }
                    while (second == first);

                    (first.Name, second.Name) = (second.Name, first.Name);
                    LogAdd($"{m.Name} vahetas {first.GetType().Name} ja {second.GetType().Name} nimed!");
                }
            }

            UpdateStats();
        }

        private void AddRandomAnimal()
        {
            Animal a = _rnd.Next(5) switch
            {
                0 => new Cat { Name = NameGenerator.GetNameFor<Cat>(), Age = _rnd.Next(1, 15) },
                1 => new Dog { Name = NameGenerator.GetNameFor<Dog>(), Age = _rnd.Next(1, 12) },
                2 => new Bird { Name = NameGenerator.GetNameFor<Bird>(), Age = _rnd.Next(1, 10) },
                3 => new Raccoon { Name = NameGenerator.GetNameFor<Raccoon>(), Age = _rnd.Next(1, 8) },
                _ => new Monkey { Name = NameGenerator.GetNameFor<Monkey>(), Age = _rnd.Next(1, 18) }
            };

            _repo.Add(a);
            Animals.Add(a);
            _enclosure.Add(a);
            SelectedAnimal = a;
            LogAdd($"Lisatud: {a.Describe()}");
            UpdateStats();
        }

        private async Task DropFoodSequence()
        {
            var food = string.IsNullOrWhiteSpace(FoodInput) ? "toit" : FoodInput.Trim();
            await _enclosure.DropFoodSequentialAsync(
                food,
                log: LogAdd,
                eatDelayMs: a => a switch
                {
                    Cat => 300,
                    Dog => 450,
                    Bird => 200,
                    Raccoon => 500,
                    Monkey => 600,
                    _ => 350
                });
            UpdateStats();
        }

        private void OnNight()
        {
            foreach (var a in Animals)
            {
                switch (a)
                {
                    case Cat c: LogAdd($"{c.Name} magab aknalaual..."); break;
                    case Dog d: LogAdd($"{d.Name} valvab ja uriseb vaikselt."); break;
                    case Bird b: LogAdd($"{b.Name} peidab pea tiiva alla."); break;
                    case Raccoon r: LogAdd($"{r.Name} varastab öösel midagi sädelevat..."); break;
                    case Monkey m: LogAdd($"{m.Name} korjab banaane ja plaanitseb pahandust."); break;
                }
            }
        }

        private void UpdateStats()
        {
            StatsLines.Clear();

            var groups = Animals.GroupBy(a => a.GetType().Name)
                                .Select(g => $"{g.Key}: {g.Count()} tk (avg {g.Average(x => x.Age):0.0})");
            foreach (var line in groups) StatsLines.Add(line);

            if (Animals.Any())
            {
                var oldest = Animals.OrderByDescending(a => a.Age).First();
                StatsLines.Add($"Vanim: {oldest.Name} ({oldest.GetType().Name}), {oldest.Age} a.");
            }
            var flying = Animals.OfType<Bird>().Count(b => b.IsFlying);
            StatsLines.Add($"Lendavad linnud: {flying}");
        }
    }
}
