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
        private double BTC_open = 0.0f;
        private double BTC_close = 0.0f;
        private double BTC_low = 0.0f;
        private double BTC_high = 0.0f;
        private double BTC_average = 0.0f;
        private long Time = 0;

        private static int counter = 0;

        public CurrencyState()
        {
            UpdatePrice();
        }

        public double GetOpenPrice()
        {
            return BTC_open;
        }

        public double GetClosePrice()
        {
            return BTC_close;
        }

        public double GetLowPrice()
        {
            return BTC_low;
        }

        public double GetHighPrice()
        {
            return BTC_high;
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
                //parser.ReadFields();
                
                // Skip processed and header
                for (int i = 0; i <= counter; i++)
                {
                    parser.ReadFields();
                }


                if (!parser.EndOfData)
                {
                    //Process row
                    string[] fields = parser.ReadFields();

                    Time = long.Parse(fields[0]);
                    BTC_high = double.Parse(fields[1]);
                    BTC_low = double.Parse(fields[2]);
                    BTC_open = double.Parse(fields[3]);
                    BTC_close = double.Parse(fields[4]);

                    BTC_average = (BTC_high + BTC_low) / 2;

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
