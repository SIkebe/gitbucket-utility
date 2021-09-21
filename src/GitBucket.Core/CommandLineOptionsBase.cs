namespace GitBucket.Core
{
    public abstract class CommandLineOptionsBase
    {
        public DateTime ExecutedDate { get; set; } = DateTime.Now;
    }
}
