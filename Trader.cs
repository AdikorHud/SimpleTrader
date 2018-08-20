using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradeTest
{
    class Trader
    {                
        //Buying parameters
        public readonly double averageModifier = 0.99;

        //Selling parameters
        public readonly double minRevenue = 1.003;
        public readonly double dropTolerance = 0.975;

        public int totalLooseSells = 0;
        public int consecutiveLooseSells = 0;

        public int consecutiLooseSellThreshold = 3;
        

        public void UpdateTrades(ref CurrencyState state, Wallet myWallet)
        {            

            double BTC_Price = state.GetLastPrice(); //To be replaced for a randomized number.             
            Console.WriteLine("BTC Price = {0}", BTC_Price);

            //BUYING
            if (myWallet.GetBuyAuthorization())
            {
                //Buy BTC if last price is lower than last 24hs average              
                if (BTC_Price < state.GetAveragePrice() && consecutiveLooseSells <= consecutiLooseSellThreshold)
                {
                    myWallet.UpdateBTC(myWallet.GetUSDTfunds() / state.GetLastPrice());
                    myWallet.UpdateBuyPrice(state.GetLastPrice());

                    Console.WriteLine("Trade authorized - Purchase Case 1! {0} BTC bought at {1}.", myWallet.GetBTC(), state.GetLastPrice());
                    Console.WriteLine("Current Balance: BTC: {0} || USDT: {1}", myWallet.GetBTC(), myWallet.GetUSDT());
                    Console.WriteLine("Case 1: Current BTC price is lower than last 24hs AVERAGE.");// DEBUG INFO

                    return;
                }

                //Buy BTC if last price is a n% lower than last selling price
                else if (BTC_Price < myWallet.GetBTCsellPrice() * averageModifier && consecutiveLooseSells <= consecutiLooseSellThreshold)
                {
                    myWallet.UpdateBTC(myWallet.GetUSDTfunds() / state.GetLastPrice());
                    myWallet.UpdateBuyPrice(state.GetLastPrice());
                    
                    Console.WriteLine("Trade authorized - Purchase Case 2! {0} BTC bought at {1}.", myWallet.GetBTC(), state.GetLastPrice());
                    Console.WriteLine("Current Balance: BTC: {0} || USDT: {1}", myWallet.GetBTC(), myWallet.GetUSDT());
                    Console.WriteLine("Case 2: Current BTC price is lower than last SELL PRICE."); //DEBUG INFO

                    return;
                }
                /*
                else if (BTC_Price < state.GetAveragePrice() * 1.01 && consecutiveLooseSells <= consecutiLooseSellThreshold)
                {
                    myWallet.UpdateBTC(myWallet.GetUSDTfunds() / state.GetLastPrice());
                    myWallet.UpdateBuyPrice(state.GetLastPrice());
                                        
                    Console.WriteLine("Trade authorized - Case 3! {0} BTC bought at {1}.", myWallet.GetBTC(), state.GetLastPrice());
                    Console.WriteLine("Current Balance: BTC: {0} || USDT: {1}", myWallet.GetBTC(), myWallet.GetUSDT());

                    //Console.ReadLine();
                }


                else if (BTC_Price > state.GetAveragePrice() * 1.015 && consecutiveLooseSells > consecutiLooseSellThreshold)
                {
                    myWallet.UpdateBTC(myWallet.GetUSDTfunds() / state.GetLastPrice());
                    myWallet.UpdateBuyPrice(state.GetLastPrice());

                    Console.WriteLine("AFTER LOOSE BUY!");//TO BE REMOVED
                    Console.WriteLine("Trade authorized! {0} BTC bought at {1}.", myWallet.GetBTC(), state.GetLastPrice());
                    Console.WriteLine("Current Balance: BTC: {0} || USDT: {1}", myWallet.GetBTC(), myWallet.GetUSDT());

                    //Console.ReadLine();
                }
                */

            }


            //SELLING
            else if (myWallet.GetSellAuthorization())
            {
                //Sells BTC if price reaches or surppases expected revenue.
                if (state.GetLastPrice() > myWallet.GetBTCbuyPrice() * minRevenue)
                {
                    Console.WriteLine("Trade authorized! - Sale Case 1! {0} BTC sold at {1}.", myWallet.GetBTC(), state.GetLastPrice());

                    myWallet.UpdateUSDT(myWallet.GetBTCfunds() * state.GetLastPrice() + myWallet.GetUSDT());
                    myWallet.UpdateSellPrice(state.GetLastPrice());

                    Console.WriteLine("Current Balance: BTC: {0} || USDT: {1}", myWallet.GetBTC(), myWallet.GetUSDT());

                    consecutiveLooseSells = 0;

                    return;
                }

                //Sells BTC if price drops lower than a n% to prevent losses.
                else if (state.GetLastPrice() < myWallet.GetBTCbuyPrice() * dropTolerance)
                {
                    ddConsole.WriteLine("Trade authorized! Sale Case 2! {0} BTC sold at {1}.", myWallet.GetBTC(), state.GetLastPrice());

                    myWallet.UpdateUSDT(myWallet.GetBTCfunds() * state.GetLastPrice() + myWallet.GetUSDT());
                    myWallet.UpdateSellPrice(state.GetLastPrice());

                    Console.WriteLine("Current Balance: BTC: {0} || USDT: {1}", myWallet.GetBTC(), myWallet.GetUSDT());
                                        
                    //consecutiveLooseSells++;
                    totalLooseSells++;

                    return;
                }
            }
        }
    }
}
