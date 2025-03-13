namespace APICatalogo.Pagination
{
    public class ProductsPriceFilter : QueryStringParameters
    {
        public decimal? Price { get; set; }
        public string? PriceCriterion { get; set; } // maior, menor, igual
    }
}
