using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JPStockMarket.Model
{
    public class Trade
    {
        private DateTime timeStamp;
        private Stock stock;
        private TradeIndicator tradeIndicator = TradeIndicator.BUY;
        private int sharesQuantity;
        private double price;

        public Trade(Stock stock, DateTime timestamp, int quantity, TradeIndicator type, double price)
        {
            this.stock = stock;
            this.timeStamp = timestamp;
            this.sharesQuantity = quantity;
            this.tradeIndicator = type;
            this.price = price;
        }

        public DateTime getTimeStamp()
        {
            return timeStamp;
        }

        public void setTimeStamp(DateTime timeStamp)
        {
            this.timeStamp = timeStamp;
        }

        public int getSharesQuantity()
        {
            return sharesQuantity;
        }

        public void setSharesQuantity(int sharesQuantity)
        {
            this.sharesQuantity = sharesQuantity;
        }

        public TradeIndicator getTradeIndicator()
        {
            return tradeIndicator;
        }

        public void setTradeIndicator(TradeIndicator tradeIndicator)
        {
            this.tradeIndicator = tradeIndicator;
        }

        public double getPrice()
        {
            return price;
        }

        public void setPrice(double price)
        {
            this.price = price;
        }

        public Stock getStock()
        {
            return stock;
        }

        public void setStock(Stock stock)
        {
            this.stock = stock;
        }

        public override String ToString()
        {
            String pattern = "Trade Object [timeStamp: %tF %tT, stock: %s, indicator: %s, shares quantity: %7d, price: %8.2f]";
            return String.Format(pattern, timeStamp, timeStamp, stock, tradeIndicator, sharesQuantity, price);
        }
    }
}
