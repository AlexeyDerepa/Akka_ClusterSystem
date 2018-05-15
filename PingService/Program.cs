using System;



namespace PingService
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Ping Service";

            var pingService = new PingService();
            pingService.Start();

            Console.CancelKeyPress += (sender, eventArgs) => { pingService.Stop(); };

            pingService.WhenTerminated.Wait();
            //new Infostructure.ArpRequest().Run();


            //Infostructure.PingRequest ping = new Infostructure.PingRequest();
            //foreach (string address in new Infostructure.ArpRequest().GetIPAddresses())
            //{
            //    Console.WriteLine($"============={address}============");

            //    ping.Request(address);
            //    Console.WriteLine("\n\n");
            //}
        }
    }
}