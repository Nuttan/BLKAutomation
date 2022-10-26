namespace BLKAutoFramework.Base
{
    public class DriverContext
    {

        public readonly ParallelConfig ParallelConfig;

        public DriverContext(ParallelConfig parallelConfig)
        {
            ParallelConfig = parallelConfig;
        }


        public void GoToUrl(string url)
        {
            ParallelConfig.Driver!.Url = url;
        }


        public static Browser? Browser { get; set; }

    }
}
