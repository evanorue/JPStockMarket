using JPStockMarket.Data;
using JPStockMarket.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JPStockMarket.Services
{
    public class TradeMarketService
    {
        private MemoryTradeDAO tradeDao = new MemoryTradeDAO();

        public void recordBuyTrade(Trade trade)
        {
            if (trade != null && trade.getStock() != null)
            {
                tradeDao.addBuyTrade(trade);
            }
        }

        public void recordSellTrade(Trade trade)
        {
            if (trade != null && trade.getStock() != null)
            {
                tradeDao.addSellTrade(trade);
            }
        }
     
        public List<Trade> getTrades(Stock stock, int minutes)
        {
            return tradeDao.getTrades(stock, minutes);
        }

        public List<Trade> getTradesByStock(Stock stock)
        {
            return tradeDao.getTradesByStock(stock);
        }
        
        public List<Trade> getAllTrades()
        {
            return tradeDao.getAllTrades();
        }
    }
}
