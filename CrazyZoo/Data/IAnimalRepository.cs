using CrazyZoo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrazyZoo.Data;

public interface IAnimalRepository
{
    void Add(Animal animal);
    IEnumerable<Animal> GetAll();
    void Remove(Animal animal);
}
