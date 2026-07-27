
namespace MiniDeepThought.Configurations
{
    //application configurations
    public static class AppConfig
    {
        //constants:

        //number of jobs to test concurrency
        public const int NoJobs = 5;

        //folder containing files specific to the application; located at the base directory of the application: \src\MiniDeepThought\bin\Debug\net10.0\files
        public static readonly string FolderFiles = Path.Combine(AppContext.BaseDirectory, "files");

        //full path to the file where jobs are stored in JSON format
        public static readonly string FilePathJobs = Path.Combine(FolderFiles, "deepthought-jobs.json");
    }

    //constants for algorithms names; the displayed text can be changed
    public static class AlgorithmKey
    {
        public const string Trivial = "Trivial";
        public const string SlowCount = "SlowCount";
        public const string RandomGuess = "RandomGuess";
    }

    //constants for Job status; the displayed text can be changed
    public static class JobStatus
    {
        public const string Pending = "Pending";
        public const string Running = "Running";
        public const string Completed = "Completed";
        public const string Cancelled = "Cancelled";
        public const string Failed = "Failed";
    }

}
