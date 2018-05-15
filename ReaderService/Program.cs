using System;

namespace ReaderService
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Reader Service";
            var readerService = new ReaderService();
            readerService.Start();

            Console.CancelKeyPress += (sender, eventArgs) => { readerService.Stop(); };

            readerService.WhenTerminated.Wait();
        }
    }
}
