using Newtonsoft.Json;
using StreamJsonRpc;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SJRClient
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            using (var stream = new NamedPipeClientStream(".", "StreamJsonRpcSamplePipe", PipeDirection.InOut, PipeOptions.Asynchronous))
            {
                await stream.ConnectAsync();
                using (var jsonRpc = JsonRpc.Attach(stream))
                {
                    while (true)
                    {
                        var cmd = Console.ReadLine();
                        switch (cmd)
                        {
                            case "1":
                                await ActAsRpcClientAsync(jsonRpc);
                                break;
                            case "2":
                                await ActAsRpcClientAsync2(jsonRpc);
                                break;
                            case "0":
                                goto outerLoop;
                                break;
                            default:
                                break;
                        }
                    }
                outerLoop:
                    Console.WriteLine("Terminating stream...");
                }
            }
        }

        private static async Task ActAsRpcClientAsync(JsonRpc jsonRpc)
        {
            Console.WriteLine("Connected. Sending request...");
            int sum = await jsonRpc.InvokeAsync<int>("Add", 3, 5);
            Console.WriteLine($"3 + 5 = {sum}");
        }

        private static async Task ActAsRpcClientAsync2(JsonRpc jsonRpc)
        {
            Console.WriteLine("Connected. Sending request...");
            Param sum = await jsonRpc.InvokeAsync<Param>("Get", new Param { Name = "안녕" });
            Console.WriteLine($"Param = {sum.Name}");
        }

        
        internal class Param
        {
            public string Name { get; set; }
        }
    }
}
