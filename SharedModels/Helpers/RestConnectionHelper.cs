namespace SharedModels.Helpers
{
    public static class RestConnectionHelper
    {
        public static string GetProductUrl() => "http://localhost:5080/api/Customer/";
        public static string GetCustomerUrl() => "http://localhost:6080/Products/";
    }
}