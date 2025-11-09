using CrazyZoo.Domain;
using CrazyZoo.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace CrazyZoo.Data
{
    public class SqlAnimalRepository : IAnimalRepository
    {
        private readonly string _masterConn =
            @"Data Source=(LocalDB)\MSSQLLocalDB;Integrated Security=True;";

        private readonly string _dbConn =
            @"Data Source=(LocalDB)\MSSQLLocalDB;Initial Catalog=CrazyZooDB;Integrated Security=True;";

        public SqlAnimalRepository()
        {
            EnsureDatabaseAndTable();
        }

        private void EnsureDatabaseAndTable()
        {
            using (var conn = new SqlConnection(_masterConn))
            {
                conn.Open();
                using (var cmd = new SqlCommand(
                    "IF DB_ID('CrazyZooDB') IS NULL CREATE DATABASE CrazyZooDB;", conn))
                {
                    cmd.ExecuteNonQuery();
                }
            }

            using (var conn = new SqlConnection(_dbConn))
            {
                conn.Open();
                using (var cmd = new SqlCommand(@"
                    IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Animals' AND xtype='U')
                    CREATE TABLE Animals (
                        Id INT IDENTITY PRIMARY KEY,
                        Name NVARCHAR(100),
                        Age INT,
                        Type NVARCHAR(50)
                    );", conn))
                {
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void Add(Animal animal)
        {
            using var conn = new SqlConnection(_dbConn);
            conn.Open();
            using var cmd = new SqlCommand(
                "INSERT INTO Animals (Name, Age, Type) VALUES (@n, @a, @t);", conn);
            cmd.Parameters.AddWithValue("@n", animal.Name);
            cmd.Parameters.AddWithValue("@a", animal.Age);
            cmd.Parameters.AddWithValue("@t", animal.GetType().Name);
            cmd.ExecuteNonQuery();
        }

        public void Remove(Animal animal)
        {
            using var conn = new SqlConnection(_dbConn);
            conn.Open();
            using var cmd = new SqlCommand("DELETE FROM Animals WHERE Name=@n;", conn);
            cmd.Parameters.AddWithValue("@n", animal.Name);
            cmd.ExecuteNonQuery();
        }

        public IEnumerable<Animal> GetAll()
        {
            var list = new List<Animal>();
            using var conn = new SqlConnection(_dbConn);
            conn.Open();
            using var cmd = new SqlCommand("SELECT Name, Age, Type FROM Animals;", conn);
            using var r = cmd.ExecuteReader();
            while (r.Read())
            {
                string type = r.GetString(2);
                Animal a = type switch
                {
                    "Cat" => new Cat(),
                    "Dog" => new Dog(),
                    "Bird" => new Bird(),
                    "Raccoon" => new Raccoon(),
                    "Monkey" => new Monkey(),
                    _ => new Cat()
                };
                a.Name = r.GetString(0);
                a.Age = r.GetInt32(1);
                list.Add(a);
            }
            return list;
        }
    }
}
