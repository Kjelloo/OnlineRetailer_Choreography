namespace SharedModels.Helpers
{
    public static class RestConnectionHelper
    {
        public static string GetProductUrl() => "http://productapi:80/Products/";
        public static string GetCustomerUrl() => "http://customerapi:80/Customers/";
        public static string GetOrderUrl() => "http://orderapi:80/Orders/";
    }
}