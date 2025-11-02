using CrazyZoo.Data;
using CrazyZoo.Domain;
using CrazyZoo.Interfaces;
using CrazyZoo.Models;
using CrazyZoo.MVVM;
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
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainViewModel(new InMemoryRepository<Animal>(),
                                            new Enclosure<Animal>());
        }
    }
}