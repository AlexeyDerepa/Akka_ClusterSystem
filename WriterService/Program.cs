using System;

namespace WriterService
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Write Service";

            var service = new WriterService();
            service.Start();

            Console.CancelKeyPress += (sender, eventArgs) => { service.Stop(); };

            service.WhenTerminated.Wait();
        }
    }
}
