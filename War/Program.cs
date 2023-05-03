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
            south.CreateSquad();
            north.CreateSquad();
            bool IsGame = true;

            while (IsGame)
            {
                south.ShowSquadInfo();
                Console.WriteLine();
                north.ShowSquadInfo();
                south.Attack(north);
                north.KillSoldier();
                south.ShowSquadInfo();
                Console.WriteLine();
                north.ShowSquadInfo();
                north.Attack(south);
                south.KillSoldier();

                if (north.IfLuse())
                {
                    Console.WriteLine($"{south.Name} победила в войне");
                    IsGame = false;
                }
                else if (south.IfLuse())
                {
                    Console.WriteLine($"{north.Name} победила в войне");
                    IsGame = false;
                }

                Console.ReadKey();
                Console.Clear();
            }
        }

        class Country
        {
            private List<Soldier> _squad { get; } = new List<Soldier>();

            public Country(string name)
            {
                Name = name;
            }

            public string Name { get; private set; }

            public void CreateSquad()
            {
                int squadQuantity = 1;

                for (int i = 0; i < squadQuantity; i++)
                {
                    _squad.Add(new Infantryman(30,100));
                    _squad.Add(new Shiper(50, 70));
                    _squad.Add(new MachineGunner(40, 80));
                }
            }

            public void Attack(Country enemyCountry)
            {
                Console.WriteLine($"Ваша страна - {Name}. Выберите солдата, которым будете атаковать!");
                string friendlySoldierIndex = Console.ReadLine();

                if (int.TryParse(friendlySoldierIndex, out int userSoldierIndex))
                {
                    if(userSoldierIndex <= _squad.Count && userSoldierIndex > 0)
                    {
                        Console.WriteLine("Выберите кого атаковать!");
                        string enemySoldierIndex = Console.ReadLine();

                        if (int.TryParse(enemySoldierIndex, out int enemyIndex))
                        {
                            if(enemyIndex <= enemyCountry.GetSquadLenght() && userSoldierIndex > 0)
                            {
                                _squad[userSoldierIndex - 1].Attack(enemyCountry.GetSoldier(enemyIndex - 1));
                            }
                            else
                            {
                                Console.WriteLine("Вы промазали по противнику! Вы пропускаете свой ход.");                              
                            }
                        }
                        else
                        {
                            Console.WriteLine("Неккоректный ввод...Вы пропускаете свой ход.");           
                        }
                    }
                    else
                    {
                        Console.WriteLine("Такого солдата нет в вашем взводе...Вы пропускаете свой ход.");
                    }
                }
                else
                {
                    Console.WriteLine("Неккоректный ввод...Вы пропускаете свой ход.");
                }
            }

            public void ShowSquadInfo()
            {
                Console.WriteLine($"{Name}");

                if(_squad.Count > 0)
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

            public void KillSoldier()
            {
                for (int i = 0; i < _squad.Count; i++)
                {
                    if (_squad[i].Health <= 0)
                    {
                        _squad.RemoveAt(i);
                    }
                }
            }

            public bool IfLuse()
            {
                return _squad.Count == 0;
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
                soldier.Health -= Damage;
            }

            public void ShowStats()
            {
                Console.WriteLine($"{Name}. Здоровье - {Health}. Урон - {Damage}.");

            }
        }
        class Infantryman : Soldier
        {
            private int _medicaments = 10;

            public Infantryman(int damage, int health, string name = "Пехотинец") : base(name, damage, health) { }

            public override void Attack(Soldier soldier)
            {
                UseKnife();
                base.Attack(soldier);
            }

            private void UseKnife()
            {
                int lowHealth = 50;

                if(Health <= lowHealth)
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

                if(soldier.Health <= lowHealth)
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

                if(Health<= lowHealth)
                {
                    Console.WriteLine($"{Name} впадает в ярость и увеличивает свой урон и здоровье");
                    Damage += _rage;
                    Health += _rage;
                }
            }
        }
    }
}

