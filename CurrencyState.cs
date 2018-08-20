using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradeTest
{
    class CurrencyState
    {
        private double BTC_lastPrice = 0.0f;
        private double BTC_lowPrice = 0.0f;
        private double BTC_highPrice = 0.0f;
        private double BTC_average = 0.0f;
        private long Time = 0;

        private static int counter = 0;

        public CurrencyState()
        {
            UpdatePrice();
        }


        public double GetLastPrice()
        {
            return BTC_lastPrice;
        }

        public double GetLowPrice()
        {
            return BTC_lowPrice;
        }

        public double GetHighPrice()
        {
            return BTC_highPrice;
        }

        public double GetAveragePrice()
        {
            return BTC_average;
        }


        //Updates the currency price from the CSV file (should be replaced for a HTML Parser)
        public bool UpdatePrice()
        {
            using (TextFieldParser parser = new TextFieldParser(@"btcusdt.csv"))
            {
                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters(";");

                //Skip header
                parser.ReadFields();
                
                // Skip processed
                for (int i = 0; i < counter; i++)
                {
                    parser.ReadFields();
                }

                if (!parser.EndOfData && counter < 2000)
                {
                    //Process row
                    string[] fields = parser.ReadFields();

                    Time = long.Parse(fields[0]);
                    BTC_highPrice = double.Parse(fields[1]);
                    BTC_lowPrice = double.Parse(fields[2]);
                    BTC_lastPrice = double.Parse(fields[4]);

                    BTC_average = (BTC_highPrice + BTC_lowPrice) / 2;

                    counter++;

                    return true;
                }

                else
                {
                    return false;
                }
            }
        }
    }
}
