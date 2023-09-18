using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Diagnostics.PerformanceData;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace CoinChanger
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int moneyToChange; 
            int ways = 0;

            Console.Write("Enter the amount you want to change as cent : ");
            moneyToChange = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("==========================");

            List<string> resultList = new List<string>();
            List<string> list = Calculate(moneyToChange);

            ways = list.Count;
            resultList.Add($"There are {ways} ways to make change for {moneyToChange} cents:");

            foreach (string item in list)
            {
                string correctString = item.Replace("1 quarters", "1 quarter");
                correctString = correctString.Replace("1 nickels", "1 nickel");
                correctString = correctString.Replace("1 dimes", "1 dime");
                correctString = correctString.Replace("1 pennies", "1 penny");
                resultList.Add(correctString);
            }

            foreach (var item in resultList)
            {
                Console.WriteLine(item);
            }

            Console.ReadLine();

        }
        private static List<string> Calculate(int moneyToChange)
        {
            List<string> calculatedList = new List<string>();
            List<int> coins = new List<int>();
            coins.Add(25);
            coins.Add(10);
            coins.Add(5);
            coins.Add(1);

            int remain = 0;


            for (int i = 0; i < coins.Count; i++)
            {
                string s = "";
                string coinName = "";
                List<string> tempList = new List<string>();

                if (coins[i] == 25)
                {
                    coinName = "quarters";
                }
                if (coins[i] == 10)
                {
                    coinName = "dimes";
                }
                if (coins[i] == 5)
                {
                    coinName = "nickels";
                }
                if (coins[i] == 1)
                {
                    coinName = "pennies";
                }

                var temp = moneyToChange / coins[i];
                remain = moneyToChange - (temp * coins[i]);

                if (temp > 0)
                {
                    if (remain == 0)
                    {
                        s += $"{temp} {coinName};";
                        calculatedList.Add(s);
                    }

                    if (coins[i] != 1)
                    {
                        tempList = CalculateBeforeRemain(temp, coins[i], coinName, moneyToChange);
                        foreach (var item in tempList)
                        {
                            calculatedList.Add(item);
                        }
                    }
                }
                else
                {
                    continue;
                }
            }

            return calculatedList;
        }

        public static List<string> CalculateBeforeRemain(int temp,int coin,string coinName,int moneyToChange)
        {
            List<string> list = new List<string>();
            List<string> appendList = new List<string>();

                while (temp > 0)
                {
                    string s = $"{temp} {coinName}";
                    var remainder = (moneyToChange - (temp * coin)); 

                    if (remainder > 0)
                    {
                        appendList = GetRemain(remainder, coin);
                        if (appendList.Count > 0)
                        {
                            foreach (var item in appendList)
                            {
                                var b = s + $",{item}";
                                list.Add(b);
                            }

                        }
                    }
                    temp--;
                }

            return list;
            
        }

        public static List<string> GetRemain(int remainder, int coin)
        {
            var dimes = 0;
           
            List<string> append = new List<string>(); 

            if(coin == 25)  // quarters
              dimes = remainder / 10;

            while (dimes>0)
            {
                var temp = remainder - (dimes * 10); 
                var nickels = temp / 5;
                if (temp>0)
                {
                    if((remainder - ((dimes*10)+(nickels*5))) == 0)
                    {
                        append.Add($"{dimes} dimes and {nickels} nickels;");
                    }
                }
                else
                {
                    append.Add($"{dimes} dimes;");
                }

                dimes--;
            }

            if (dimes == 0)
            {
                var nickels = 0;

                if(coin != 5)
                 nickels = remainder/5;

                while (nickels > 0)
                {
                    var temp = remainder - (nickels * 5);
                    if (temp > 0)
                    {
                        var penny = temp;

                        if ((remainder - ((nickels * 5) + (penny))) == 0)
                        {
                            append.Add($"{nickels} nickels and {penny} pennies;");
                        }
                    }
                    else
                    {
                        append.Add($"{nickels} nickels;");
                    }

                    nickels--;
                }

                if (nickels == 0)
                {
                    var pennies = remainder;
                    append.Add($"{pennies} pennies;");
                }
            }

            return append;
        }
    }
}
