
using MiniDeepThought.Configurations;
using MiniDeepThought.Interfaces;
using MiniDeepThought.Models.UI;
using MiniDeepThought.Services;


var jobService = new JobService();
IJobStore jobStore = new JobStore(AppConfig.FilePathJobs); //IO + persist
IJobRunner jobRunner = new JobRunner(jobStore);


var appService = new AppServices(jobService, jobRunner, jobStore);


//initialize algorithms menu
var algorithmsMenuOptions = new List<MenuOption>()
{
    new MenuOption() {Id = 1, Title = AlgorithmKey.Trivial },
    new MenuOption() {Id = 2, Title = AlgorithmKey.SlowCount },
    new MenuOption() {Id = 3, Title = AlgorithmKey.RandomGuess }
};

//initialize main menu options
var mainMenuOptions = new List<MenuOption>()
{
    new MenuOption() {Id = 1, Title = "Submit Question", ExecuteAction = () => appService.SubmitQuestion(algorithmsMenuOptions) }, //to avoid method execution because of parameter and it needs to be executed later, it needs to use lambda ExecuteAction = ()
    new MenuOption() {Id = 2, Title = "List Jobs", ExecuteAction = appService.ListJobs },
    new MenuOption() {Id = 3, Title = "View Result by JobId", ExecuteAction = appService.ViewResultByJobId },
    //new MenuOption() {Id = 4, Title = "Cancel Running Job (redundant, available on (1) when job is running )", ExecuteAction = appService.CancelRunningJob },
    new MenuOption() {Id = 4, Title = $"Test {AppConfig.NoJobs} jobs same time", ExecuteAction = () => appService.TestMultipleJobs(AppConfig.NoJobs) },
    new MenuOption() {Id = 5, Title = "Exit", ExecuteAction = appService.ExitApp }
};


//display menu and choose option
while (true)
{
    Console.WriteLine();
    Console.WriteLine("=== Choose option: ===");
    appService.DisplayMenuOptions(mainMenuOptions);

    //get entered option
    var option = appService.ReadOptionMainMenu(mainMenuOptions);

    //iterate main menu options
    foreach (var menuOption in mainMenuOptions)
    {
        if (menuOption.Id == option)
        {
            //execute method specific to entered option
            await menuOption.ExecuteAction();
            break;
        }
    }
}

