using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JPStockMarket.Util
{
    public static class StockExtensions
    {
        public static V GetValueOrDefault<K, V>(this IDictionary<K, V> @this, K key, Func<V> @default)
        {
            return @this.ContainsKey(key) ? @this[key] : @default();
        }
    }
}
