using HelloWorld.Grains;
using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.Clustering.Kubernetes;
using Orleans.Configuration;
using Orleans.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace KubeGatewayHost
{
    public class Program
    {
        private static readonly AutoResetEvent Closing = new AutoResetEvent(false);

        public static async Task<int> Main(string[] args)
        {
            try
            {
                var host = await StartSilo();
                Console.WriteLine("Gateway is ready!");

                Console.CancelKeyPress += OnExit;
                Closing.WaitOne();

                Console.WriteLine("Shutting down...");

                await host.StopAsync();

                return 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return 1;
            }
        }

        private static async Task<ISiloHost> StartSilo()
        {
            var builder = new SiloHostBuilder()
                .Configure<ClusterOptions>(options =>  { options.ClusterId = "testcluster"; options.ServiceId = "testservice"; })
                .ConfigureEndpoints(new Random(1).Next(30001, 30100), new Random(1).Next(20001, 20100), listenOnAnyHostAddress: true)
                .AddMemoryGrainStorageAsDefault()
                .UseKubeMembership(opt =>
                {
                    //opt.APIEndpoint = "http://localhost:8001";
                    opt.CanCreateResources = true;
                    //opt.DropResourcesOnInit = true;
                })
                .ConfigureApplicationParts(parts => parts.AddApplicationPart(typeof(HelloGrain).Assembly).WithReferences())
                .ConfigureLogging(logging => logging.AddConsole());

            var host = builder.Build();
            await host.StartAsync();
            return host;
        }

        private static void OnExit(object sender, ConsoleCancelEventArgs args)
        {
            Closing.Set();
        }
    }
}
