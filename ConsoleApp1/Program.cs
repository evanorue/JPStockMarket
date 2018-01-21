using JPStockMarket.Model;
using JPStockMarket.Services;
using System;
using System.Collections.Generic;
using System.Threading;

namespace JPStockMarket
{
    class Program
    {
        private static StockMarketService stockService = new StockMarketService();
        private static TradeMarketService tradeService = new TradeMarketService();

        static void Main(string[] args)
        {
            initStocks();
            calculateDividendYield();
            calculatePERatio();
            calculateVolumeWeight_GBCE();
            
            Console.ReadLine();
        }

        private static void calculateVolumeWeight_GBCE()
        {
            bool isThreadAlive = true;

            ThreadStart thread1Start = new ThreadStart(new Program().recordBuyTrade);
            ThreadStart thread2Start = new ThreadStart(new Program().recordSellTrade);

            Thread[] threads = new Thread[2];
            threads[0] = new Thread(thread1Start);
            threads[1] = new Thread(thread2Start);

            threads[0].Start();
            Console.WriteLine("Generating Buy Trades...");

            threads[1].Start();
            Console.WriteLine("Generating Sell Trades...");
           
            if (!threads[0].Join(60000))
            {
                threads[0].Abort();
                threads[1].Abort();
                Console.WriteLine("Finish...");
                Console.WriteLine("--- --- --- ---" + System.Environment.NewLine);

                while (isThreadAlive)
                {
                    if (!threads[0].IsAlive && !threads[1].IsAlive)
                    {
                        isThreadAlive = false;
                        List<double> volumeStock = new List<double>();
                        foreach (Stock stock in stockService.getStocks())
                        {
                            //Calculate Volume Weighted Stock Price 
                            calculateVWSP(stock, volumeStock);                            
                        }
                        //Calculate GBCE All Share Index 
                        calculateGBCE(volumeStock);
                    }
                }
            }
        }

        public void recordBuyTrade()
        {
            while (true)
            {
                var stock = getStockFromUser();
                var quantity = getQuantityFromUser();
                var trade = TradeIndicator.BUY;
                var stockPrice = getStockPriceFromUser();

                recordBuyTrade(stock, quantity, trade, stockPrice);
                Thread.Sleep(1);
            }
        }

        public void recordSellTrade()
        {
            while (true)
            {
                var stock = getStockFromUser();
                var quantity = getQuantityFromUser();
                var trade = TradeIndicator.SELL;
                var stockPrice = getStockPriceFromUser();

                recordSellTrade(stock, quantity, trade, stockPrice);
                Thread.Sleep(1);
            }            
        }

        private static void initStocks()
        {
            Console.WriteLine("JPMorgan - Super simple stock market");
            Console.WriteLine("--- --- --- ---" + System.Environment.NewLine);
            Console.WriteLine("Stock Symbol | Type | Last Dividend | Fixed Dividend | Par Value");

            Console.WriteLine("TEA | COMMON    | 0  | 0 | 100");
            Console.WriteLine("POP | COMMON    | 8  | 0 | 100");
            Console.WriteLine("ALE | COMMON    | 23 | 0 | 60");
            Console.WriteLine("GIN | PREFERRED | 8  | 2 | 100");
            Console.WriteLine("JOE | PREFERRED | 13 | 0 | 250");
            Console.WriteLine("--- --- --- ---" + System.Environment.NewLine);
            stockService.addStock(new Stock(0, "TEA", StockType.COMMON, 0, 0, 100));
            stockService.addStock(new Stock(1, "POP", StockType.COMMON, 8, 0, 100));
            stockService.addStock(new Stock(2, "ALE", StockType.COMMON, 23, 0, 60));
            stockService.addStock(new Stock(3, "GIN", StockType.PREFERRED, 8, 2, 100));
            stockService.addStock(new Stock(4, "JOE", StockType.PREFERRED, 13, 0, 250));
        }
        
        private static Stock getStockFromUser()
        {
            double randomStock = new Random().NextDouble() * (4 - 0) + 0;
            Stock stock = stockService.getStockById(randomStock);
            if (stock == null)
            {
                throw new Exception("Stock not found");
            }
            return stock;
        }

        private static double getStockPriceFromUser()
        {
            try
            {
                int _min = 001;
                int _max = 999;
                Random _rdm = new Random();
                double result = _rdm.Next(_min, _max);
                if (result <= 0)
                {
                    throw new Exception("Invalid price: Must be greated than 0");
                }
                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine("error: " + e.Message);
                return 0;
            }
        }

        private static TradeIndicator getTradeType()
        {
            double randomStock = new Random().NextDouble();
            try
            {
                if (randomStock == 0)
                {
                    return TradeIndicator.BUY;
                }
                else
                {
                    return TradeIndicator.SELL;
                }     
            }
            catch (Exception e)
            {
                throw new Exception("Invalid trade type: Must be BUY or SELL");
            }
        }

        private static int getQuantityFromUser()
        {
            try
            {
                int _min = 01;
                int _max = 99;
                Random _rdm = new Random();
                double randomQuantity = _rdm.Next(_min, _max);
                int result = Convert.ToInt32(randomQuantity);
                if (result <= 0)
                {
                    throw new Exception("Invalid quantity: Must be greated than 0");
                }
                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine("error: " + e.Message);
                return 0;
            }
        }

        private static void calculateDividendYield()
        {
            Console.WriteLine("--- Calculate Dividend Yield ---" + System.Environment.NewLine);
            foreach (Stock stock in stockService.getStocks())
            {
                double result = stockService.calculateDividendYield(stock, getStockPriceFromUser());
                Console.WriteLine(stock.getStockSymbol() + " - Price: " + getStockPriceFromUser() + " - Dividend Yield: " + result);
            }
            
            Console.WriteLine("--- --- --- ---" + System.Environment.NewLine);
        }

        private static void calculatePERatio()
        {
            Console.WriteLine("--- Calculate PE Ratio ---" + System.Environment.NewLine);
            foreach (Stock stock in stockService.getStocks())
            {
                double result = stockService.calculatePERatio(getStockFromUser(), getStockPriceFromUser());
                Console.WriteLine(stock.getStockSymbol() + " - Price: " + getStockPriceFromUser() + " - PE Ratio: " + result);
            }               
            
            Console.WriteLine("--- --- --- ---" + System.Environment.NewLine);
        }

        private static void calculateVWSP(Stock stock, List<double> volumeStock)
        {
            Console.WriteLine("Calculate Volume Weighted Stock Price: " + stock.getStockSymbol());
            List<Trade> trades = tradeService.getTradesByStock(stock);
            
            if (trades == null || trades.Count == 0)
            {
                Console.WriteLine("Volume Weighted Stock Price " + stock.getStockSymbol() +": No trades");
                Console.WriteLine("--- --- --- ---" + System.Environment.NewLine);
            }
            else
            {
                double result = stockService.calculateVolumeWeightedStockPrice(trades);
                Console.WriteLine("Volume Weighted Stock Price: " + stock.getStockSymbol() + ": " + result);
                volumeStock.Add(result);
                Console.WriteLine("--- --- --- ---" + System.Environment.NewLine);
            }           
        }

        private static void recordBuyTrade(Stock stock, int quantity, TradeIndicator type, double price)
        {
            tradeService.recordBuyTrade(new Trade(stock, DateTime.Now, quantity, type, price));
        }

        private static void recordSellTrade(Stock stock, int quantity, TradeIndicator type, double price)
        {
            tradeService.recordSellTrade(new Trade(stock, DateTime.Now, quantity, type, price));
        }

        private static void calculateGBCE(List<double> tradeVolumeStocks)
        {
            Console.WriteLine("Calculate GBCE");
            List<Trade> allTrades = tradeService.getAllTrades();
            if (allTrades == null || allTrades.Count == 0)
            {
                Console.WriteLine("Unable to calculate GBCE: No trades");
            }
            else
            {
                Console.WriteLine("GBCE: " + stockService.calculateGBCEShare(tradeVolumeStocks));
            }
        }

        private static void printResult(String result)
        {
            Console.WriteLine("-------------------------------------");
            Console.WriteLine(result);
            Console.WriteLine("-------------------------------------");
        }
    }
}
