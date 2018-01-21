using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JPStockMarket.Model
{
    public class Stock
    {
        private string stockSymbol = null;
        private StockType stockType = StockType.COMMON;
        private double lastDividend = 0.0;
        private double fixedDividend = 0.0;
        private double parValue = 0.0;
        private double tickerPrice = 0.0;
        private int id;

        public Stock(int id, String symbol, StockType type, double lastDividend, double fixedDividend, double parValue)
        {
            this.id = id;
            this.stockSymbol = symbol;
            this.stockType = type;
            this.lastDividend = lastDividend;
            this.fixedDividend = fixedDividend;
            this.parValue = parValue;
        }

        public int getId()
        {
            return id;
        }

        public void setId(int id)
        {
            this.id = id;
        }

        public String getStockSymbol()
        {
            return stockSymbol;
        }

        public void setStockSymbol(String stockSymbol)
        {
            this.stockSymbol = stockSymbol;
        }

        public StockType getStockType()
        {
            return stockType;
        }

        public void setStockType(StockType stockType)
        {
            this.stockType = stockType;
        }

        public double getLastDividend()
        {
            return lastDividend;
        }

        public void setLastDividend(double lastDividend)
        {
            this.lastDividend = lastDividend;
        }

        public double getFixedDividend()
        {
            return fixedDividend;
        }

        public void setFixedDividend(double fixedDividend)
        {
            this.fixedDividend = fixedDividend;
        }

        public double getParValue()
        {
            return parValue;
        }

        public void setParValue(double parValue)
        {
            this.parValue = parValue;
        }

        public double getDividendYield()
        {
            double dividendYield = -1.0;
            if (tickerPrice > 0.0)
            {
                if (stockType == StockType.COMMON)
                {
                    dividendYield = lastDividend / tickerPrice;
                }
                else
                {
                    // PREFERRED
                    dividendYield = (fixedDividend * parValue) / tickerPrice;
                }
            }
            return dividendYield;
        }


        public double getPeRatio()
        {
            double peRatio = -1.0;

            if (tickerPrice > 0.0)
            {
                peRatio = tickerPrice / getDividendYield();
            }

            return peRatio;
        }

        public double getTickerPrice()
        {
            return tickerPrice;
        }

        public void setTickerPrice(double tickerPrice)
        {
            this.tickerPrice = tickerPrice;

        }

        public override String ToString()
        {
            String pattern = "Stock Object [stockSymbol: %s, stockType: %s, lastDividend: %7.0f, fixedDividend: %7.2f, parValue: %7.2f]";
            return String.Format(pattern, stockSymbol, stockType, lastDividend, fixedDividend, parValue);
        }
    }

}
