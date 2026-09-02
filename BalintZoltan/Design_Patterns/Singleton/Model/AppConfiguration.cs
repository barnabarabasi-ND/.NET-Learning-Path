namespace Model.AppConfiguration;
public sealed class AppConfiguration                                    // sealed -> No inheritance
{
    private static readonly AppConfiguration _instance = new();         // static   -> Belongs to the class, not to an object instance
                                                                        // readonly -> Cannot be reassigned after initialization
                                                                        // Together with the private constructor, this holds the single instance                                                        

    private AppConfiguration()                                          // private -> Prevents external instance creation
    {
    }

    public static AppConfiguration Instance => _instance;               // Provides global access to the single instance

    public string ApplicationName { get; set; } = "My App";

}