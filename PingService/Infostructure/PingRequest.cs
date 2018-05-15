using System;
using System.Collections.Generic;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;

namespace PingService.Infostructure
{
    class PingRequest
    {
        Ping pingSender = new Ping();
        PingOptions options = new PingOptions();
        PingReply reply = null;
        //public PingRequest()
        //{
        //// Use the default Ttl value which is 128,
        //// but change the fragmentation behavior.
        //options.DontFragment = true;



        //}

        public string Request(string addr = "192.168.201.81", string data = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa", int timeout = 120)
        {
            byte[] buffer = Encoding.ASCII.GetBytes(data);
            IPAddress address = IPAddress.Parse(addr);
            StringBuilder response = new StringBuilder();
            try
            {
                reply = pingSender.Send(address/*, timeout, buffer, options*/);
                if (reply.Status == IPStatus.Success)
                {
                    response.Append($"Address: {reply.Address.ToString()} \n");
                    response.Append($"RoundTrip time: { reply.RoundtripTime} \n");
                    response.Append($"Time to live: {reply.Options.Ttl} \n");
                    response.Append($"Don't fragment: {reply.Options.DontFragment} \n");
                    response.Append($"Buffer size: {reply.Buffer.Length} \n");
                }
                else
                {
                    response.Append($"Address: {reply.Address.ToString()} \n");
                    response.Append($"Status: {reply.Status} \n");
                }
            }
            catch (Exception ex)
            {

                response.Append(addr + "\n\r" + ex.Message);
            }

            return response.ToString();;
        }
    }
}
