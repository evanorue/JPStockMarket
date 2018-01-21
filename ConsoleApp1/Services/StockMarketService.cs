using JPStockMarket.Data;
using JPStockMarket.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JPStockMarket.Services
{
    public class StockMarketService
    {
        private MemoryStockDAO stockDao = new MemoryStockDAO();

        public void addStock(Stock stock)
        {
            stockDao.addStock(stock);
        }

        public Stock getStock(String symbol)
        {
            return stockDao.getStock(symbol);
        }

        public Stock getStockById(double id)
        {
            return stockDao.getStockById(id);
        }

        public List<Stock> getStocks()
        {
            return stockDao.getStocks();
        }

        public double calculateDividendYield(Stock stock, double price)
        {
            if (StockType.PREFERRED.Equals(stock.getStockType()))
            {
                return (stock.getFixedDividend() * stock.getParValue()) / price;
            }
            double result = stock.getLastDividend() / price;
            return Convert.ToDouble(Math.Round(Convert.ToDecimal(result), 2));
        }

        public double calculatePERatio(Stock stock, double price)
        {
            double result = price / stock.getLastDividend();
            return Convert.ToDouble(Math.Round(Convert.ToDecimal(result), 2));
        }

        public double calculateVolumeWeightedStockPrice(List<Trade> trades)
        {
            double sumOfPriceQuantity = 0;
            int sumOfQuantity = 0;

            foreach (Trade trade in trades)
            {
                sumOfPriceQuantity = sumOfPriceQuantity + (trade.getPrice() * trade.getSharesQuantity());
                sumOfQuantity = sumOfQuantity + trade.getSharesQuantity();
            }
            if (sumOfQuantity > 0)
            {
                double result = sumOfPriceQuantity / sumOfQuantity;
                return Convert.ToDouble(Math.Round(Convert.ToDecimal(result), 2));
            }
            return 0;
        }        

        public double calculateGBCEShare(List<double> volumeStock)
        {
            double total = 1;
            foreach (double volume in volumeStock)
            {
                if (total != 0)
                    total *= volume;
            }
            double result = Math.Pow(total, (1.0 / volumeStock.Count));

            return Convert.ToDouble(Math.Round(Convert.ToDecimal(result), 2));
        }
    }
}
