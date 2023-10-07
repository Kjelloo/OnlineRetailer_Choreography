namespace SharedModels.Helpers
{
    public static class RestConnectionHelper
    {
        public static string GetProductUrl()
        {
            return "http://productapi:80/Products/";
        }

        public static string GetCustomerUrl()
        {
            return "http://customerapi:80/Customers/";
        }

        public static string GetOrderUrl()
        {
            return "http://orderapi:80/Orders/";
        }
    }
}