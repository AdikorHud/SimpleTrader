using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradeTest
{
    class Wallet
    {
        private double BTC = 0.0;
        private double USDT = 1000;
        private double BTC_buyPrice = 0.0;
        private double BTC_sellPrice = 0.0;
        private bool authorizeBuy = true;
        private bool authorizeSell = false;
        public int totalTrades = 0;

        public void UpdateBTC(double value)
        {
            BTC = value;
        }

        public double GetBTC()
        {
            return BTC;
        }

        public double GetBTCbuyPrice()
        {
            return BTC_buyPrice;
        }

        public void UpdateBuyPrice(double value)
        {
            BTC_buyPrice = value;
        }

        public double GetBTCsellPrice()
        {
            return BTC_sellPrice;
        }

        public void UpdateSellPrice(double value)
        {
            BTC_sellPrice = value;
        }

        public void UpdateUSDT(double value)
        {
            USDT = value;
        }

        public double GetUSDT()
        {
            return USDT;
        }

        public double GetBTCfunds()
        {
            double authorizedFunds = 0.0;
            double baseFeePercent = 0.00075;

            if (BTC > 0 && authorizeSell == true)
            {
                authorizedFunds = BTC - (BTC * baseFeePercent);
                UpdateBTC(0);

                authorizeBuy = true;
                authorizeSell = false;

                totalTrades++;

                return authorizedFunds;
            }

            else
            {
                return 0;
            }
        }

        //Returns 50% of USDT accuantancies to buy BTC (should be replaced by the Binance API code)
        public double GetUSDTfunds()
        {
            double autohorizedFunds = 0.0;
            double baseFeePercent = 0.00075;
            double transactionFee = 0.0;

            if (USDT > 1 && authorizeBuy == true)
            {
                autohorizedFunds = USDT * 0.5;
                transactionFee = autohorizedFunds * baseFeePercent; //Calculates transaction fee.
                autohorizedFunds = autohorizedFunds - transactionFee; //Discounts transaction fee.

                UpdateUSDT((USDT * 0.5));

                authorizeBuy = false;
                authorizeSell = true;

                return autohorizedFunds;
            }

            else
            {
                return 0;
            }
        }

        public bool GetBuyAuthorization()
        {
            return authorizeBuy;
        }

        public bool GetSellAuthorization()
        {
            return authorizeSell;
        }
    }
}
