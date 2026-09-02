using Model.AppConfiguration;

var config1 = AppConfiguration.Instance;
var config2 = AppConfiguration.Instance;

Console.WriteLine($"Singleton example : Config1 = Config2 : {ReferenceEquals(config1, config2)} ");
