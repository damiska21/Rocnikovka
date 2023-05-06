using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace shopík
{
    internal class Program
    {
        static void Main(string[] args)
        {
            /*  Všechny komentáře jsou čistě pro mě, na týhle práci budu dělat tak dlouho, že budu pravdědpodobně zapomínat co jak funguje :D
             *  Kódem se klidně inspirujte, na vlastní nebezpečí ;)
             * */
            Console.Title = "Damiánův nádherný e-shop";
            Console.ForegroundColor = ConsoleColor.White;
            Console.WindowHeight = 40;

            bool loggedin = false;

            //dočasný import z filu
            string import = File.ReadAllText("save.txt", Encoding.UTF8).Replace("\r\n", ""); //replace aby kód nenačítal entery v .txt souboru

            //ZPŮSOB ZAPISOVÁNÍ PRODUKTŮ
            //jmeno_popisek_cena_pocet-na-sklade_artík
            //[0]     [1]    [2]       [3]        [4]
            string[] database = import.Split(';');
            Item[] items = new Item[database.Length];
            string name = ""; string desc = ""; int price = 0; int stock = 0; string art = "";
            for (int i = 0; i < items.Length; i++) //vezme save soubor a rozdělí ho na dvojrozměrné pole: 0 (i) jsou itemy, 1 (j ) jsou jejich properties
            {
                try
                {
                        string[] mrda = database[i].Split('_');
                        for (int j = 0; j < mrda.Length; j++)
                        {
                        if (j == 0)
                        {
                            name = mrda[j];
                        }
                        else if (j == 1)
                        {
                            desc = mrda[j];
                        }
                        else if (j ==2)
                        {
                            price = Convert.ToInt32(mrda[j]);
                        }
                        else if (j == 3)
                        {
                            stock = Convert.ToInt32(mrda[j]);
                        }
                        else if (j ==4)
                        {
                            art = mrda[j];
                        }
                        }
                    items[i] = new Item(name, desc, price, stock, art);

                }
                catch { } //poslední index je mimo z nějakého důvodu
            }
            Menu:
            //MENU
            string[] menuItems = { "Nakupovat", "Účet", "Vypnout obchod" };
            int menuOutput = DecisionMaker(3, menuItems, "0", 0, ConsoleColor.White, ConsoleColor.Blue, items, 1);
            switch (menuOutput)
            {
                case 0:
                    Shop(items);
                    goto Menu;
                case 1:
                    Ucet(loggedin, items);
                    goto Menu;
                case 2:
                    Environment.Exit(0);
                    break;
                default:
                    break;
            }

        }
        static public int Shop(Item[] items)
        {
            int currentItem = 0;
            int lastItem = items.GetLength(0) - 1;
            ConsoleKeyInfo keydown;
            int radekOffset = 0;

            int itemNum = (items.GetLength(0) - 1);//getlength 0 je počet itemů, 1 definice itemu
            int konecRadku = (itemNum % 3);//kolik itemů je na posledním řádku
            // % vrací zbytek dělení třemi např 8 % 3 = 2

            int pocetRadku = ((items.GetLength(0) - 1) / 3); //počítá řádky

            /*int posledniPlnyRadek = pocetRadku; tohle se k ničemu nepoužívá, ale jsem moc paranoidní to smazat
            if (konecRadku > 0) { pocetRadku++; posledniPlnyRadek = pocetRadku - 1; } //přidá se jeden řádek, pokud je nějaký přebytek*/


            while (true)
            {
                Console.Clear(); Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("     Nakupovat     Nastavení     Vypnout Obchod     [stisknutím L se vrátíte do lišty]");
                radekOffset = Vypis(items, radekOffset, currentItem);
                keydown = Console.ReadKey();
                switch (keydown.Key)
                {
                    case ConsoleKey.LeftArrow:
                        if (currentItem > 0)
                        {
                            currentItem--;
                            if (currentItem < (radekOffset * 3))//posouvá itemy nahoru / dolu, pokud je currentItem offscreen
                            {
                                radekOffset--;
                            }
                        }
                        break;
                    case ConsoleKey.UpArrow:
                        if (currentItem > 2)
                        {
                            currentItem -= 3;
                            if (currentItem < (radekOffset * 3))
                            {
                                radekOffset--;
                            }
                        }
                        break;
                    case ConsoleKey.RightArrow:
                        if (currentItem < lastItem-1)
                        { 
                            currentItem++;
                            if (currentItem > (radekOffset * 3) + 5)
                            {
                                radekOffset++;
                            }
                        }
                        break;
                    case ConsoleKey.DownArrow:
                        if (currentItem < lastItem -3) //(radekOffset*3)+5 kalkulace posledního zobrazovaného ítemu
                        {
                            currentItem += 3;
                            if (currentItem > (radekOffset * 3) + 5)
                            {
                                radekOffset++;
                            }
                        }
                        break;
                    case ConsoleKey.Enter:
                        break;
                    case ConsoleKey.L:
                        goto Exit;
                    default:
                        break;
                }
            }
        Exit:
            return 1;
        }
        //radekOffset je kolikátý řádek se zobrazuje první currentItem je to co se bude highlitovat
        static public int Vypis(Item[] items, int radekOffset, int currentItem)
        {
            int itemNum = (items.GetLength(0) - 1);//getlength 0 je počet itemů, 1 definice itemu
            int konecRadku = (itemNum % 3);//kolik itemů je na posledním řádku
            // % vrací zbytek dělení třemi např 8 % 3 = 2

            int pocetRadku = ((items.GetLength(0)-1) / 3); //počítá řádky

            int posledniPlnyRadek = pocetRadku;
            if (konecRadku > 0) { pocetRadku++; posledniPlnyRadek = pocetRadku - 1; } //přidá se jeden řádek, pokud je nějaký přebytek

            for (int i = 0; i < 2; i++)
            {
                int m = i * 3;
                if (radekOffset > 0)
                {
                    if (i == 0)
                    {
                        m = radekOffset * 3;
                    }
                    else
                    {
                        m = (radekOffset * 3) + 3;
                    }
                }
                if (m/3 < posledniPlnyRadek || posledniPlnyRadek == 0) //výpis plných řádků
                {
                    Console.WriteLine("");
                    Console.WriteLine("");
                    Console.Write("       ");
                    if (m == currentItem) {Console.ForegroundColor = ConsoleColor.Blue;}
                    VypisovacJmen(items[m].Name); if (m+1 == currentItem) { Console.ForegroundColor = ConsoleColor.Blue; }
                    VypisovacJmen(items[m + 1].Name); if (m+2 == currentItem) { Console.ForegroundColor = ConsoleColor.Blue; }
                    VypisovacJmen(items[m + 2].Name); Console.ForegroundColor = ConsoleColor.White;
                    
                    Console.WriteLine("");
                    Obrazky(items[m].Art, items[m+1].Art, items[m+2].Art, 3);

                    Console.Write("\r\n       ");
                    VypisovacJmen(items[m].Desc);
                    VypisovacJmen(items[m + 1].Desc);
                    VypisovacJmen(items[m + 2].Desc);
                    Console.Write("\r\n       ");
                    VypisovacJmen(items[m].Price + " Kč");
                    VypisovacJmen(items[m + 1].Price + " Kč");
                    VypisovacJmen(items[m + 2].Price + " Kč");
                    Console.WriteLine("");
                    }
                else //výpis 1/2 produktů
                {
                    Console.WriteLine("");

                    Console.Write("       ");
                    if (konecRadku == 2)
                    {
                        if (m == currentItem) { Console.ForegroundColor = ConsoleColor.Blue; }
                        VypisovacJmen(items[m].Name); if (m + 1 == currentItem) { Console.ForegroundColor = ConsoleColor.Blue; }
                        VypisovacJmen(items[m+1].Name);
                        Console.WriteLine("");
                        Obrazky(items[m].Art, items[m + 1].Art, "0", 2); //pokud jsou potřeba vypsat pouze dva, třetí je prázdný

                        Console.Write("\r\n       ");
                        VypisovacJmen(items[m].Desc); 
                        VypisovacJmen(items[m + 1].Desc);
                        Console.Write("\r\n       ");
                        VypisovacJmen(items[m].Price + " Kč");
                        VypisovacJmen(items[m + 1].Price + " Kč");
                        Console.WriteLine("");
                    }
                    else
                    {
                        try
                        {
                            if (m == currentItem) { Console.ForegroundColor = ConsoleColor.Blue; }
                            VypisovacJmen(items[m].Name);
                            Console.WriteLine("");
                            Obrazky(items[m].Art, "0", "0", 1); //pokud je potřeba vypsat jenom jeden

                            Console.Write("\r\n       ");
                            VypisovacJmen(items[m].Desc);
                            Console.Write("\r\n       ");
                            VypisovacJmen(items[m].Price + " Kč");
                            Console.WriteLine("");
                        }
                        catch { Console.WriteLine("Byl vypsán jenom jeden item."); }
                    }
                }
            }
            return radekOffset;
        }
        static public int SingleItemDisplay(Item[] items, int currentItem)
        {
            //TADY JSEM SKONČIL

            return 0;
        }
        static public void VypisovacJmen(string name)
        {
            int mezera = (20 - (name.Length));
            if (mezera % 2 != 0)
            {
                Console.Write(" ");
            }
            for (int q = 0; q < mezera / 2; q++)
            {
                Console.Write(" ");
            }
            Console.Write(name);
            for (int q = 0; q < mezera / 2; q++)
            {
                Console.Write(" ");
            }
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("   |   ");
        }
        static public int[] Ucet(bool loggedin, Item[] items) //nedodělaný
        {
            int[] output = new int[4];
            if (!loggedin)
            {
                Console.WriteLine("Přihlášení");
                string[] menuItems = { "Přihlášení (mám účet)", "Registrace (nemám účet)", "Zpět do obchodu" };
                int menuOutput = DecisionMaker(3, menuItems, "ÚČET", 1, ConsoleColor.White, ConsoleColor.Blue, items, 0); Console.Clear();
                switch (menuOutput)
                {
                    case 0:
                        Console.WriteLine("PŘIHLÁŠENÍ");
                        Console.Write("Přihlašovací jméno: ");
                        string user = Console.ReadLine();
                        break;
                    case 1:
                        Console.WriteLine("REGISTRACE");
                        Console.WriteLine("Uživatelské jméno budete používat pro přihlášení");
                        Console.Write("Vaše uživatelské jméno: ");
                        string newuser = Console.ReadLine();
                        break;
                    case 2:
                        return output;
                    default:
                        break;
                }
            }
            
            return output;
        }
        //0 je černá, 1 bílá, 2 modrá, 3 žlutá, 4 zelená, 5 tyrkisová, jakýkoli písmenko je červená
        static public void Obrazky(string obrazekNumbers1, string obrazekNumbers2, string obrazekNumbers3, int num)
        {
            int[] obrazekNums1 = obrazekNumbers1.Split(' ').Select(int.Parse).ToArray();//nevim co to dělá, ale funguje to
            int[] obrazekNums2 = new int[25];
            int[] obrazekNums3 = new int[25];
            if (num > 1)
            {
                obrazekNums2 = obrazekNumbers2.Split(' ').Select(int.Parse).ToArray();//ze stackoverflowu btw
                if (num == 3)
                {
                    obrazekNums3 = obrazekNumbers3.Split(' ').Select(int.Parse).ToArray();//dělá ze stringu array čísel
                }
            }

            Console.WriteLine("");
                for (int i = 0; i < 5; i++)
                {
                    Console.Write("       ");
                    for (int j = 0; j < 5; j++)
                    {
                        switch (Convert.ToInt32(obrazekNums1[j + (i * 5)]))
                        {
                            case 0:
                                Console.ForegroundColor = ConsoleColor.Black;
                                break;
                            case 1:
                                Console.ForegroundColor = ConsoleColor.White;
                                break;
                            case 2:
                                Console.ForegroundColor = ConsoleColor.Blue;
                                break;
                            case 3:
                                Console.ForegroundColor = ConsoleColor.Yellow;
                                break;
                            case 4:
                                Console.ForegroundColor = ConsoleColor.Green;
                                break;
                            case 5:
                                Console.ForegroundColor = ConsoleColor.Cyan;
                                break;
                            default:
                                Console.ForegroundColor = ConsoleColor.Red;
                                break;
                        }
                        Console.Write("████");
                    }
                    if (num > 1)
                    {
                    Console.ForegroundColor = ConsoleColor.White;
                        Console.Write("   |   ");

                        for (int j = 0; j < 5; j++)
                        {
                        switch (Convert.ToInt32(obrazekNums2[j + (i * 5)]))
                        {
                            case 0:
                                Console.ForegroundColor = ConsoleColor.Black;
                                break;
                            case 1:
                                Console.ForegroundColor = ConsoleColor.White;
                                break;
                            case 2:
                                Console.ForegroundColor = ConsoleColor.Blue;
                                break;
                            case 3:
                                Console.ForegroundColor = ConsoleColor.Yellow;
                                break;
                            case 4:
                                Console.ForegroundColor = ConsoleColor.Green;
                                break;
                            case 5:
                                Console.ForegroundColor = ConsoleColor.Cyan;
                                break;
                            default:
                                Console.ForegroundColor = ConsoleColor.Red;
                                break;
                        }
                        Console.Write("████");
                        }
                    }
                    
                    if (num ==3)
                    {
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write("   |   ");

                        for (int j = 0; j < 5; j++)
                        {
                        switch (Convert.ToInt32(obrazekNums3[j + (i * 5)]))
                        {
                            case 0:
                                Console.ForegroundColor = ConsoleColor.Black;
                                break;
                            case 1:
                                Console.ForegroundColor = ConsoleColor.White;
                                break;
                            case 2:
                                Console.ForegroundColor = ConsoleColor.Blue;
                                break;
                            case 3:
                                Console.ForegroundColor = ConsoleColor.Yellow;
                                break;
                            case 4:
                                Console.ForegroundColor = ConsoleColor.Green;
                                break;
                            case 5:
                                Console.ForegroundColor = ConsoleColor.Cyan;
                                break;
                            default:
                                Console.ForegroundColor = ConsoleColor.Red;
                                break;
                        }
                        Console.Write("████");
                        }
                    
                    }

                    Console.WriteLine("");
                    Console.Write("       ");
                    for (int k = 0; k < 5; k++)
                    {
                    switch (Convert.ToInt32(obrazekNums1[k + (i * 5)]))
                    {
                        case 0:
                            Console.ForegroundColor = ConsoleColor.Black;
                            break;
                        case 1:
                            Console.ForegroundColor = ConsoleColor.White;
                            break;
                        case 2:
                            Console.ForegroundColor = ConsoleColor.Blue;
                            break;
                        case 3:
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            break;
                        case 4:
                            Console.ForegroundColor = ConsoleColor.Green;
                            break;
                        case 5:
                            Console.ForegroundColor = ConsoleColor.Cyan;
                            break;
                        default:
                            Console.ForegroundColor = ConsoleColor.Red;
                            break;
                    }
                    Console.Write("████");
                    }
                    if (num > 1)
                    {
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write("   |   ");
                        for (int j = 0; j < 5; j++)
                        {
                        switch (Convert.ToInt32(obrazekNums2[j + (i * 5)]))
                        {
                            case 0:
                                Console.ForegroundColor = ConsoleColor.Black;
                                break;
                            case 1:
                                Console.ForegroundColor = ConsoleColor.White;
                                break;
                            case 2:
                                Console.ForegroundColor = ConsoleColor.Blue;
                                break;
                            case 3:
                                Console.ForegroundColor = ConsoleColor.Yellow;
                                break;
                            case 4:
                                Console.ForegroundColor = ConsoleColor.Green;
                                break;
                            case 5:
                                Console.ForegroundColor = ConsoleColor.Cyan;
                                break;
                            case 6:
                                Console.ForegroundColor = ConsoleColor.Red;
                                break;
                            default:
                                Console.ForegroundColor = ConsoleColor.Red;
                                break;
                        }
                        Console.Write("████");
                        }
                    }

                    if (num ==3)
                    {
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write("   |   ");
                        for (int j = 0; j < 5; j++)
                        {
                        switch (Convert.ToInt32(obrazekNums3[j + (i * 5)]))
                        {
                            case 0:
                                Console.ForegroundColor = ConsoleColor.Black;
                                break;
                            case 1:
                                Console.ForegroundColor = ConsoleColor.White;
                                break;
                            case 2:
                                Console.ForegroundColor = ConsoleColor.Blue;
                                break;
                            case 3:
                                Console.ForegroundColor = ConsoleColor.Yellow;
                                break;
                            case 4:
                                Console.ForegroundColor = ConsoleColor.Green;
                                break;
                            case 5:
                                Console.ForegroundColor = ConsoleColor.Cyan;
                                break;
                            default:
                                Console.ForegroundColor = ConsoleColor.Red;
                                break;
                        }
                        Console.Write("████");
                        }
                    }
                    Console.WriteLine("     ");
                }
        }

        //ukradenej z mýho jinýho projektu. Jdou s ním dělat menu či dialogy, je super
        public static int DecisionMaker(int decisionNum, string[] decisions, string otazka, int vertical, ConsoleColor mainColor, ConsoleColor highlightColor, Item[] items, int vypis)
        {
            int currentDecision = 0;
            if (vertical == 1)//vertical dělá lištu, 1 dělá menu, 0 lištu
            {
                while (true)
                {
                    Console.Clear();
                    Console.WriteLine(otazka);
                    Console.WriteLine("");
                    for (int i = 0; i < decisionNum; i++)
                    {
                        if (currentDecision == i)
                        {
                            Console.ForegroundColor = highlightColor;
                            Console.Write("> ");
                        }
                        else { Console.Write("  "); }
                        Console.WriteLine(decisions[i]);
                        Console.WriteLine("");
                        Console.ForegroundColor = mainColor;
                    }
                    var input = Console.ReadKey();
                    if (input.Key == ConsoleKey.DownArrow && currentDecision < decisionNum - 1)
                    {
                        currentDecision++;
                    }
                    else if (input.Key == ConsoleKey.UpArrow && currentDecision > 0)
                    {
                        currentDecision--;
                    }
                    else if (input.Key == ConsoleKey.Enter)
                    {
                        return currentDecision;
                    }
                }
            }
            else
            {
                while(true)
                {
                    Console.Clear();
                    for (int i = 0; i < decisionNum; i++)
                    {
                        Console.Write("   ");
                        if (currentDecision == i)
                        {
                            Console.ForegroundColor = highlightColor;
                            Console.Write("> " + decisions[i]);
                            Console.ForegroundColor = mainColor;
                        }
                        else
                        {
                            Console.ForegroundColor = mainColor;

                            Console.Write("  " + decisions[i]);
                        }
                    }
                    if (vypis == 1)
                    {
                        Vypis(items, 0, -1);
                    }
                    var input = Console.ReadKey();
                    if (input.Key == ConsoleKey.RightArrow && currentDecision < decisionNum - 1)
                    {
                        currentDecision++;
                    }
                    else if (input.Key == ConsoleKey.LeftArrow && currentDecision > 0)
                    {
                        currentDecision--;
                    }
                    else if (input.Key == ConsoleKey.Enter)
                    {
                        return currentDecision;
                    }
                    
                }
            }
            
        }
    }
}
