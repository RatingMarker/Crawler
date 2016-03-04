namespace Crawler.Workflow.ViewModels
{
    public class CounterViewModel
    {
        public int Count { get; set; }
        public int Saved { get; set; }

        public override string ToString()
        {
            return $"{Saved} / {Count}";
        }
    }
}