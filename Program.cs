using System;
using System.Collections.Generic;

namespace Market
{
    class Program
    {
        static void Main(string[] args)
        {
            Market market = new Market();

            market.Work();
        }
    }

    class Item
    {
        private string _name;
        private int _price;

        public Item(string name, int price)
        {
            _name = name;
            _price = price;
        }

        public int Price => _price;

        public void ShowInfo()
        {
            Console.WriteLine($"{_name.PadRight(25)} {_price.ToString().PadLeft(5)}");
        }
    }

    abstract class Human
    {
        protected int Money;
        protected List<Item> Items;

        public Human(int money)
        {
            Money = money;
            Items = new List<Item>();
        }

        public virtual void ShowItems()
        {
            int index = 1;

            if (Items.Count == 0)
            {
                Console.WriteLine($"Товаров нет");
                return;
            }

            foreach (Item item in Items)
            {
                Console.Write($"{index,2} ");
                item.ShowInfo();
                index++;
            }
        }
    }

    class Seller: Human
    {
        public Seller(int money):base ( money)
        {
            Items = Fill();
        }

        public override void ShowItems()
        {
            Console.WriteLine($"Товары продавца:");
            base.ShowItems();
        }
        public Item GetItem(int index)
        {
            return Items[index];
        }

        public bool GetItemIndex(out int number)
        {
            Console.WriteLine($"Введите номер товара");

            if (int.TryParse(Console.ReadLine(), out number))
            {
                if (number > 0 && number <= Items.Count)
                {
                    number--;
                    return true;
                }
            }
            
            return false;
        }

        public void Sell(Item item)
        {
            Money += item.Price;
            Items.Remove(item);
        }

        private List<Item> Fill()
        {
            List<Item> items = new List<Item>()
            {
                new Item("проводная мышка", 250),
                new Item("беспроводная мышка", 500),
                new Item("клавиатура программиста", 3000),
                new Item("клавиатура с подсветкой", 1500),
                new Item("коврик для мыши", 100),
                new Item("беспроводная гарнитура", 1500)
            };

            return items;
        }
    }

    class Buyer: Human
    {
        public Buyer(int money):base(money)
        {
            Money = money;
            Items = new List<Item>();
        }

        public bool IsCanPay(int cost)
        {
            return Money >= cost;
        }

        public void Buy(Item item)
        {
            Money -= item.Price;
            Items.Add(item);
        }

        public override void ShowItems()
        {
            Console.WriteLine($"Покупательская способность: {Money}");
            Console.WriteLine($"Товары покупателя:");

            base.ShowItems();
        }
    }

    class Market
    {
        const string SellerItemsCommand = "1";
        const string BuyerItemsCommand = "2";
        const string BuyItem = "3";
        const string ExitCommand = "4";
        
        private Seller _seller = new Seller(0);
        private Buyer _buyer = new Buyer(1000);
        private bool _isOpen = true;

        public void Work()
        {
            while (_isOpen)
            {
                Console.Clear();
                ShowMenu();

                Console.WriteLine($"Введите номер команды:");
                string userCommand = Console.ReadLine();

                ExecuteUserCommand(userCommand);                

                Console.ReadKey();
            }
        }

        private void Trade()
        {
            _seller.ShowItems();

            if (_seller.GetItemIndex(out int index)==false)
            {
                Console.WriteLine($"Ошибка ввода данных");
                return;
            }

            Item item = _seller.GetItem(index);
            item.ShowInfo();

            if (_buyer.IsCanPay(item.Price))
            {
                _seller.Sell(item);
                _buyer.Buy(item);

                Console.WriteLine($"товар успешно куплен");
            }
            else
            {
                Console.WriteLine($"Недостаточно денег");
            }
        }

        private void ShowMenu()
        {            
            Console.WriteLine($"Лавка компьютерных товаров");
            Console.WriteLine($"{new string('-', 35)}");
            Console.WriteLine($"{SellerItemsCommand}. Товары продавца");
            Console.WriteLine($"{BuyerItemsCommand}. Сумка покупателя");
            Console.WriteLine($"{BuyItem}. Купить товар");
            Console.WriteLine($"{ExitCommand}. Выйти");
            Console.WriteLine($"{new string('-', 35)}");
        }

        private void ExecuteUserCommand(string command)
        {
            switch (command)
            {
                case SellerItemsCommand:
                    _seller.ShowItems();
                    break;

                case BuyerItemsCommand:
                    _buyer.ShowItems();
                    break;

                case BuyItem:
                    Trade();
                    break;

                case ExitCommand:
                    _isOpen = false;
                    break;

                default:
                    Console.WriteLine($"Ошибка ввода команды");
                    break;
            }
        }
    }
}
