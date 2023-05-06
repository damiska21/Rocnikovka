using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace shopík
{
    internal class Item
    {
        //ZPŮSOB ZAPISOVÁNÍ PRODUKTŮ
        //jmeno_popisek_cena_pocet-na-sklade_artík
        //[0]     [1]    [2]       [3]        [4]
        public string name;
        public string desc;
        public int price;
        public int stock;
        public string art;
        public Item(string Name, string Desc, int Price, int Stock, string Art)
        {
            name = Name;
            desc = Desc;
            price = Price;
            stock = Stock;
            art = Art;
        }
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        public string Desc
        {
            get { return desc; }
            set { desc = value; }
        }
        public int Price
        {
            get { return price; }
            set { price = value; }
        }
        public int Stock
        {
            get { return stock; }
            set { stock = value; }
        }
        public string Art
        {
            get { return art; }
            set { art = value; }
        }
    }
}
