using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradeTest
{
    class Program
    {
        static void Main(string[] args)
        {
            CurrencyState state = new CurrencyState();
            Trader myTrader = new Trader();
            Wallet myWallet = new Wallet();         
                        
            double InitialUSDT = myWallet.GetUSDT();
            
            Console.WriteLine("Starting balance: {0} BTC || {1} USDT.", myWallet.GetBTC(), myWallet.GetUSDT());
            Console.WriteLine("Press any key to continue..");
            Console.ReadLine();
            
            
            while(Analyzer.GetApproval(state.GetClosePrice(), state.GetLowPrice()) && state.UpdatePrice())
            {
                state.UpdatePrice();
                myTrader.UpdateTrades(ref state, myWallet);
            }


            Console.WriteLine("\nEnding balance: {0} BTC || {1} USDT.", myWallet.GetBTC(), myWallet.GetUSDT());
            Console.WriteLine("Net revenue USDT: {0}", ((myWallet.GetBTC() * state.GetClosePrice()) + myWallet.GetUSDT()) - InitialUSDT);
            Console.WriteLine("Total trades: {0} and Loose Sells: {1} Consecutive: {2}", myWallet.totalTrades, myTrader.totalLooseSells, myTrader.consecutiLooseSellsCounter);

            Console.WriteLine("Average Modifier: " + myTrader.buyingPriceModifier);
            Console.WriteLine("Min. revenue: " + myTrader.minRevenue);
            Console.WriteLine("Loose tolerance: " + myTrader.dropTolerance);
                        
            Console.WriteLine("Press any key to exit..");
            Console.ReadLine();
        }
    }
}
