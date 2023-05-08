using System;
using System.Collections.Generic;

namespace War
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Country south = new Country("Южная");
            Country north = new Country("Северная");
            bool isGame = true;
            Random random= new Random();

            while (isGame)
            {
                south.ShowSquadInfo();
                Console.WriteLine();
                north.ShowSquadInfo();
                south.Attack(north, random);
                north.RemoveDeathSoldier();
                south.ShowSquadInfo();
                Console.WriteLine();
                north.ShowSquadInfo();
                north.Attack(south, random);
                south.RemoveDeathSoldier();

                if (north.IsLuse())
                {
                    Console.WriteLine($"{south.Name} победила в войне");
                    isGame = false;
                }
                else if (south.IsLuse())
                {
                    Console.WriteLine($"{north.Name} победила в войне");
                    isGame = false;
                }

                Console.ReadKey();
                Console.Clear();
            }
        }
    }

    class Country
    {
        private List<Soldier> _squad = new List<Soldier>();

        public Country(string name)
        {
            CreateSquad();
            Name = name;
        }

        public string Name { get; private set; }

        public void Attack(Country enemyCountry, Random random)
        {
            int minSquadIndex = 0;
            Soldier soldier = _squad[random.Next(minSquadIndex, _squad.Count)];
            Soldier enemy = enemyCountry.GetSoldier(random.Next(minSquadIndex, enemyCountry.GetSquadLenght()));
            Console.WriteLine($"Солдат {soldier.Name} страны {Name} атакует противника - {enemy.Name} страны {enemyCountry.Name}.");
            soldier.Attack(enemy);
        }

        public void ShowSquadInfo()
        {
            Console.WriteLine($"{Name}");

            if (_squad.Count > 0)
            {
                for (int i = 0; i < _squad.Count; i++)
                {
                    Console.Write($"{i + 1}) ");
                    _squad[i].ShowStats();
                }
            }
            else
            {
                Console.WriteLine($"В взводе страны -{Name}- не осталось солдат. Нажмите любую клавишу для продолжения...");
            }
        }

        public void RemoveDeathSoldier()
        {
            for (int i = 0; i < _squad.Count; i++)
            {
                if (_squad[i].Health <= 0)
                {
                    _squad.RemoveAt(i);
                    i--;
                }
            }
        }

        public bool IsLuse()
        {
            return _squad.Count == 0;
        }

        private void CreateSquad()
        {
            int squadQuantity = 1;

            for (int i = 0; i < squadQuantity; i++)
            {
                _squad.Add(new Infantryman(30, 100));
                _squad.Add(new Shiper(50, 70));
                _squad.Add(new MachineGunner(40, 80));
            }
        }

        private Soldier GetSoldier(int index)
        {
            return _squad[index];
        }

        private int GetSquadLenght()
        {
            return _squad.Count;
        }
    }

    class Soldier
    {
        public Soldier(string name, int damage, int health)
        {
            Name = name;
            Damage = damage;
            Health = health;
        }

        public string Name { get; protected set; }
        public int Damage { get; protected set; }
        public int Health { get; protected set; }

        public virtual void Attack(Soldier soldier)
        {
            soldier.TakeDamage(Damage);
        }

        public void ShowStats()
        {
            Console.WriteLine($"{Name}. Здоровье - {Health}. Урон - {Damage}.");

        }

        private void TakeDamage(int damage)
        {
            Health-= damage;
        }
    }

    class Infantryman : Soldier
    {
        private int _medicaments = 10;

        public Infantryman(int damage, int health, string name = "Пехотинец") : base(name, damage, health) { }

        public override void Attack(Soldier soldier)
        {
            UseMedicaments();
            base.Attack(soldier);
        }

        private void UseMedicaments()
        {
            int lowHealth = 50;

            if (Health <= lowHealth)
            {
                Console.WriteLine($"{Name} использует медикаменты для исцеления.");
                Health += _medicaments;
            }
        }
    }

    class Shiper : Soldier
    {
        private int _criticalHitModificator = 3;

        public Shiper(int damage, int health, string name = "Снайпер") : base(name, damage, health) { }

        public override void Attack(Soldier soldier)
        {
            HitInHead(soldier);
            base.Attack(soldier);
        }

        private void HitInHead(Soldier soldier)
        {
            int lowHealth = 50;
            int badDamage = 20;

            if (soldier.Health <= lowHealth)
            {
                Console.WriteLine($"{Name} добивает врага выстрелом в голову.");
                Damage *= _criticalHitModificator;
            }
            else
            {
                Console.WriteLine($"{Name} плохо видит врага и наносит уменьшенный урон");
                Damage = badDamage;
            }
        }
    }

    class MachineGunner : Soldier
    {
        private int _rage = 3;

        public MachineGunner(int damage, int health, string name = "Пулемётчик") : base(name, damage, health) { }

        public override void Attack(Soldier soldier)
        {
            ActivateRage();
            base.Attack(soldier);
        }

        private void ActivateRage()
        {
            int lowHealth = 50;

            if (Health <= lowHealth)
            {
                Console.WriteLine($"{Name} впадает в ярость и увеличивает свой урон и здоровье");
                Damage += _rage;
                Health += _rage;
            }
        }
    }
}