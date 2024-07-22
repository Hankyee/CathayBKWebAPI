namespace CathayBkWebAPI.Model
{
    public class BTCPrice
    {
        public int Id { get; set; }
        public DateTime UpdateTime { get; set; }
        public string CurrencyCode { get; set; }
        public string CurrencyChineseName { get; set; }
        public double Rate { get; set; }
    }
}
