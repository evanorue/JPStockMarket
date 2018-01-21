using JPStockMarket.Model;
using JPStockMarket.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JPStockMarket.Data
{
    public class MemoryTradeDAO
    {
        private Dictionary<String, List<Trade>> tradeBuyMap = new Dictionary<String, List<Trade>>(); // Performance
        private Dictionary<String, List<Trade>> tradeSellMap = new Dictionary<String, List<Trade>>(); // Performance

        public MemoryTradeDAO()
        {
        }

        public void addBuyTrade(Trade trade)
        {
            List<Trade> trades = new List<Trade>();
            var exisingTrade = StockExtensions.GetValueOrDefault(tradeBuyMap, trade.getStock().getStockSymbol(), () => null);
            if (exisingTrade != null)
            {
                tradeBuyMap[trade.getStock().getStockSymbol()].Add(trade);
            }
            else
            {
                trades.Add(trade);
                tradeBuyMap.Add(trade.getStock().getStockSymbol(), trades);
            }
            
        }

        public void addSellTrade(Trade trade)
        {
            List<Trade> trades = new List<Trade>();
            var exisingTrade = StockExtensions.GetValueOrDefault(tradeSellMap, trade.getStock().getStockSymbol(), () => null);
            if (exisingTrade != null)
            {
                tradeSellMap[trade.getStock().getStockSymbol()].Add(trade);
            }
            else
            {
                trades.Add(trade);
                tradeSellMap.Add(trade.getStock().getStockSymbol(), trades);
            }

        }

        public List<Trade> getTrades(Stock stock, int minutes)
        {
            List<Trade> result = new List<Trade>();
            DateTime afterDate = getDateXMinutesEarlier(minutes);
            List<Trade> buyTrades = tradeBuyMap.FirstOrDefault(i => i.Key == stock.getStockSymbol()).Value;
            List<Trade> sellTrades = tradeSellMap.FirstOrDefault(i => i.Key == stock.getStockSymbol()).Value;

            foreach (Trade trade in buyTrades)
            {
                if (trade.getTimeStamp() < afterDate)
                {
                    result.Add(trade);
                }
            }

            foreach (Trade trade in sellTrades)
            {
                if (trade.getTimeStamp() < afterDate)
                {
                    result.Add(trade);
                }
            }

            return result;
        }

        public List<Trade> getTradesByStock(Stock stock)
        {
            List<Trade> result = new List<Trade>();            
            List<Trade> buyTrades = tradeBuyMap.FirstOrDefault(i => i.Key == stock.getStockSymbol()).Value;
            List<Trade> sellTrades = tradeSellMap.FirstOrDefault(i => i.Key == stock.getStockSymbol()).Value;

            if (buyTrades != null)
            {
                foreach (Trade trade in buyTrades)
                {
                    result.Add(trade);
                }
            }

            if (sellTrades != null)
            {
                foreach (Trade trade in sellTrades)
                {
                    result.Add(trade);
                }
            }

            return result;
        }

        public List<Trade> getAllTrades()
        {
            List<Trade> result = new List<Trade>();

            foreach (string stockSymbol in tradeBuyMap.Keys)
            {
                result.AddRange(tradeBuyMap.FirstOrDefault(i => i.Key == stockSymbol).Value);
            }

            foreach (string stockSymbol in tradeSellMap.Keys)
            {
                result.AddRange(tradeSellMap.FirstOrDefault(i => i.Key == stockSymbol).Value);
            }

            return result;
        }

        public DateTime getDateXMinutesEarlier(int minutes)
        {
            return DateTime.Now.AddMinutes(-minutes);
        }
    }
}
