using JPStockMarket.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JPStockMarket.Data
{
    public class MemoryStockDAO
    {
        public Dictionary<String, Stock> stockMap = new Dictionary<String, Stock>();

        public void addStock(Stock stock)
        {
            stockMap.Add(stock.getStockSymbol(), stock);
        }

        public Stock getStock(String symbol)
        {
            return stockMap.FirstOrDefault(i => i.Value.getStockSymbol() == symbol).Value;
        }

        public Stock getStockById(double id)
        {
            return stockMap.FirstOrDefault(i => i.Value.getId() == Convert.ToInt32(id)).Value;
        }

        public List<Stock> getStocks()
        {
            return stockMap.Values.ToList(); ;
        }
    }
}
