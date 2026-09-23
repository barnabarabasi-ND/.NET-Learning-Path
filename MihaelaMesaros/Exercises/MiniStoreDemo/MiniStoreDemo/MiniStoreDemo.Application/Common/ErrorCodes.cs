namespace MiniStoreDemo.Application.Common;

/// <summary>
/// Contains constant error codes used throughout the application.
/// </summary>
public static class ErrorCodes
{
    public static class Product
    {
        public const string AlreadyExists = "Product.AlreadyExists";
        public const string NotFound = "Product.NotFound";
    }

    public static class Category
    {
        public const string NotFound = "Category.NotFound";
    }
}
