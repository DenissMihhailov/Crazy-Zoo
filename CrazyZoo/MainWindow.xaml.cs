using CrazyZoo.Interfaces;
using CrazyZoo.Models;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CrazyZoo
{
    public partial class MainWindow : Window
    {
        private readonly ObservableCollection<Animal> Animals = new();
        private int _counter = 1;
        private readonly Random _rnd = new();

        public MainWindow()
        {
            InitializeComponent();
            AnimalList.ItemsSource = Animals;

            Animals.Add(new Cat { Name = "Marsik", Age = 2 });
            Animals.Add(new Dog { Name = "Lucky", Age = 4 });
            Animals.Add(new Bird { Name = "Gosha", Age = 1 });
            Animals.Add(new Raccoon { Name = "Kris", Age = 3 });
            Animals.Add(new Monkey { Name = "Albert", Age = 5 });

            AnimalList.SelectedIndex = 0;
            UpdateDetails();
        }

        private void UpdateDetails()
        {
            if (AnimalList.SelectedItem is Animal a)
                DetailsText.Text = a.Describe();
            else
                DetailsText.Text = "";
        }

        private void Log(string text)
        {
            LogList.Items.Add(text);
            LogList.ScrollIntoView(LogList.Items[^1]);
        }

        private void AnimalList_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            UpdateDetails();
        }

        private void MakeSound_Click(object sender, RoutedEventArgs e)
        {
            if (AnimalList.SelectedItem is Animal a)
            {
                Log(a.MakeSound());
                UpdateDetails();
            }
        }

        private void Feed_Click(object sender, RoutedEventArgs e)
        {
            if (AnimalList.SelectedItem is Animal a)
            {
                var food = string.IsNullOrWhiteSpace(FoodBox.Text) ? "midagi maitsvat" : FoodBox.Text.Trim();
                Log($"{a.Name} sõi {food}.");
                FoodBox.Clear();
            }
        }

        private void Crazy_Click(object sender, RoutedEventArgs e)
        {
            if (AnimalList.SelectedItem is not Animal a) return;

            if (a is ICrazyAction crazy)
                Log(crazy.ActCrazy());

            if (a is IFlyable flyable)
            {
                flyable.Fly();
                if (a is Bird b)
                    Log(b.IsFlying ? $"{b.Name} hakkas lendama!" : $"{b.Name} maandus!");
            }

            if (a is Monkey m)
            {
                var msg = m.ActCrazy(Animals);
                Log(msg);
            }

            UpdateDetails();
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            Animal newAnimal = _rnd.Next(5) switch
            {
                0 => new Cat { Name = $"Kass {_counter}", Age = _rnd.Next(1, 10) },
                1 => new Dog { Name = $"Koer {_counter}", Age = _rnd.Next(1, 10) },
                2 => new Bird { Name = $"Lind {_counter}", Age = _rnd.Next(1, 10) },
                3 => new Raccoon { Name = $"Pesukaru {_counter}", Age = _rnd.Next(1, 10) },
                _ => new Monkey { Name = $"Ahv {_counter}", Age = _rnd.Next(1, 10) },
            };
            _counter++;

            Animals.Add(newAnimal);
            AnimalList.SelectedItem = newAnimal;
            Log($"Lisatud: {newAnimal.Describe()}");
        }

        private void Remove_Click(object sender, RoutedEventArgs e)
        {
            if (AnimalList.SelectedItem is Animal a)
            {
                Animals.Remove(a);
                Log($"Eemaldatud: {a.Name}");
            }
        }
    }
}