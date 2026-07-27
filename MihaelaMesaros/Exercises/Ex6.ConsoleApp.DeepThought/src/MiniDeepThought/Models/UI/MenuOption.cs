namespace MiniDeepThought.Models.UI
{
    public class MenuOption
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;

        //became Task because the actions are async and must wait for the completion of the task
        public Func<Task> ExecuteAction { get; set; } = () => Task.CompletedTask;

        //public Action ExecuteAction { get; set; } = () => { }; //ex: option.ExecuteAction() same with ExitApp()
        /*
         Action is a delegate type that represents a method, is a reference type that can hold a reference to a method.
         () => { } is a lambda expression that defines an anonymous method with no parameters and an empty body.
         */

        //equivalent:
        //public Action ExecuteAction { get; set; } = Empty;
        //private static void Empty()
        //{
        //}
    }
}
