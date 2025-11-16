using CrazyZoo.Interfaces;
using CrazyZoo.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace CrazyZoo.Data
{
    public class EfAnimalRepository : IAnimalRepository
    {
        private readonly ZooContext _context;

        public EfAnimalRepository(ZooContext context)
        {
            _context = context;
        }

        public void Add(Animal animal)
        {
            _context.Animals.Add(animal);
            _context.SaveChanges();
        }

        public void Remove(Animal animal)
        {
            _context.Animals.Remove(animal);
            _context.SaveChanges();
        }

        public IEnumerable<Animal> GetAll()
        {
            return _context.Animals.AsNoTracking().ToList();
        }
    }
}
