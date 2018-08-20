using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradeTest
{
    ///To be completed. This class will define how to proceed when market conditions are unclear/uncertain. 
    public class Analyzer
    {
        private static bool validMarketConditions = true;


        private static void ValidateMarketConditions(double lastPrice, double sellPrice)
        {

            if (lastPrice > sellPrice * 1.25)
            {
                Console.WriteLine("BTC price has increased +25% since last SELL. Do you want to continue? Y/N");

                char userInput = Console.ReadKey().KeyChar;

                if (userInput == 'y' || userInput == 'Y')
                {
                    Console.WriteLine("Program will continue...");
                    validMarketConditions = true;
                }

                else if (userInput == 'n' || userInput == 'N')
                {
                    Console.WriteLine("Closing program...");
                    validMarketConditions = false;
                }

                else
                {
                    validMarketConditions = false;
                    Console.WriteLine("ERROR! Closing program...");
                    Console.ReadLine();
                }

            }

            else
            {
                validMarketConditions = true;
            }
        }


        public static bool GetApproval(double lastPrice, double lowPrice)
        {
            ValidateMarketConditions(lastPrice, lowPrice);
            return validMarketConditions;
        }

    }
}
