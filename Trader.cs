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
        public readonly double buyingPriceModifier = 0.95;

        //Selling parameters
        public readonly double minRevenue = 1.0030;
        public readonly double dropTolerance = 0.975;

        public int totalLooseSells = 0;
        public bool loosingMoney = false;

        public int consecutiLooseSells = 0;
        public int consecutiLooseSellsCounter = 0;



        public void UpdateTrades(ref CurrencyState state, Wallet myWallet)
        {
            double BTC_open = state.GetOpenPrice();
            double BTC_close = state.GetClosePrice();
            double BTC_low = state.GetLowPrice();
            double BTC_high = state.GetHighPrice();
            double BTC_average = state.GetAveragePrice();

            double BTC_buyPrice = myWallet.GetBTCbuyPrice();
            double BTC_sellPrice = myWallet.GetBTCsellPrice();
            


            Console.WriteLine("BTC Price = {0} || AVERAGE = {1}", BTC_close, BTC_average);

            //BUYING
            if (myWallet.GetBuyAuthorization())
            {
                if (BTC_close < BTC_open && !loosingMoney)
                {
                    myWallet.UpdateBTC(myWallet.GetUSDTfunds() / BTC_close);
                    myWallet.UpdateBuyPrice(BTC_close);

                    Console.WriteLine("\nTrade authorized - Purchase Case 1! {0} BTC bought at {1}.", myWallet.GetBTC(), BTC_close);
                    Console.WriteLine("Current Balance: BTC: {0} || USDT: {1}", myWallet.GetBTC(), myWallet.GetUSDT());
                    Console.WriteLine("Case 1: Current BTC price is lower than last 24hs AVERAGE.\n");// DEBUG INFO

                    return;
                }
                

                //Buy BTC if last price is lower than average              
                else if (BTC_close < BTC_average * 0.99 && loosingMoney)
                {
                    myWallet.UpdateBTC(myWallet.GetUSDTfunds() / BTC_close);
                    myWallet.UpdateBuyPrice(BTC_close);

                    Console.WriteLine("\nTrade authorized - Purchase Case 1! {0} BTC bought at {1}.", myWallet.GetBTC(), BTC_close);
                    Console.WriteLine("Current Balance: BTC: {0} || USDT: {1}", myWallet.GetBTC(), myWallet.GetUSDT());
                    Console.WriteLine("Case 1: Current BTC price is lower than last 24hs AVERAGE.\n");// DEBUG INFO

                    return;
                }


                

                /*
                //Buy BTC if last price is a n% lower than last selling price
                else if (BTC_Price < myWallet.GetBTCsellPrice() * buyingPriceModifier && consecutiveLooseSells <= consecutiLooseSellThreshold)
                {
                    myWallet.UpdateBTC(myWallet.GetUSDTfunds() / state.GetLastPrice());
                    myWallet.UpdateBuyPrice(state.GetLastPrice());
                    
                    Console.WriteLine("\nTrade authorized - Purchase Case 2! {0} BTC bought at {1}.", myWallet.GetBTC(), state.GetLastPrice());
                    Console.WriteLine("Current Balance: BTC: {0} || USDT: {1}", myWallet.GetBTC(), myWallet.GetUSDT());
                    Console.WriteLine("Case 2: Current BTC price is lower than last SELL PRICE.\n"); //DEBUG INFO

                    return;
                }
                */

                else if (BTC_open > BTC_average && loosingMoney)
                {
                    myWallet.UpdateBTC(myWallet.GetUSDTfunds() / BTC_close);
                    myWallet.UpdateBuyPrice(BTC_close);

                    Console.WriteLine("AFTER LOOSE BUY!");//TO BE REMOVED
                    Console.WriteLine("Trade authorized! {0} BTC bought at {1}.", myWallet.GetBTC(), BTC_close);
                    Console.WriteLine("Current Balance: BTC: {0} || USDT: {1}", myWallet.GetBTC(), myWallet.GetUSDT());
                    
                    return;
                }
            }


            //SELLING
            else if (myWallet.GetSellAuthorization())
            {
                //Sells BTC if price reaches or surppases expected revenue.
                if (BTC_close > BTC_buyPrice * minRevenue)
                {
                    Console.WriteLine("\nTrade authorized! - Sale Case 1! {0} BTC sold at {1}.", myWallet.GetBTC(), BTC_close);

                    myWallet.UpdateUSDT(myWallet.GetBTCfunds() * BTC_close + myWallet.GetUSDT());
                    myWallet.UpdateSellPrice(BTC_close);

                    Console.WriteLine("Current Balance: BTC: {0} || USDT: {1}", myWallet.GetBTC(), myWallet.GetUSDT());

                    loosingMoney = false;

                    consecutiLooseSells = 0;

                    return;
                }

                //Sells BTC if price drops lower than a n% to prevent losses.
                else if (BTC_close < BTC_buyPrice * dropTolerance)
                {
                    Console.WriteLine("\nTrade authorized! Sale Case 2! {0} BTC sold at {1}.", myWallet.GetBTC(), BTC_close);

                    myWallet.UpdateUSDT(myWallet.GetBTCfunds() * BTC_close + myWallet.GetUSDT());
                    myWallet.UpdateSellPrice(BTC_close);

                    Console.WriteLine("Current Balance: BTC: {0} || USDT: {1}", myWallet.GetBTC(), myWallet.GetUSDT());

                    //loosingMoney = true;

                    consecutiLooseSells++;

                    if (consecutiLooseSells == 3)
                    {
                        //Console.ReadLine();
                        consecutiLooseSellsCounter++;
                        consecutiLooseSells = 0;
                    }

                    totalLooseSells++;

                    return;
                }
            }
        }
    }
}
