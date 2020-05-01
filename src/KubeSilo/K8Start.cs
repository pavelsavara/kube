using System;
using System.Net;
using System.Threading.Tasks;
using HelloWorld.Grains;
using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.Clustering.Kubernetes;
using Orleans.Configuration;
using Orleans.Hosting;

public class K8Start
{
    public static async Task<ISiloHost> StartKubeSilo(bool local)
    {
        var builder = new SiloHostBuilder()
            .Configure<ClusterOptions>(options =>
            {
                options.ClusterId = "greeting-cluster";
                options.ServiceId = "greeting-service";
            })
            .AddMemoryGrainStorageAsDefault();

        
        builder = local
            ? builder
                .Configure<EndpointOptions>(options => options.AdvertisedIPAddress = IPAddress.Loopback)
                .UseLocalhostClustering()
            : builder
                .ConfigureEndpoints(new Random(1).Next(30001, 30100), new Random(1).Next(20001, 20100), listenOnAnyHostAddress: true)
                .UseKubeMembership(opt =>
                {
                    opt.CanCreateResources = true;
                    opt.DropResourcesOnInit = true;
                })
            ;

        builder = builder
            .ConfigureApplicationParts(parts => parts.AddApplicationPart(typeof(HelloGrain).Assembly).WithReferences())
            .ConfigureLogging(logging => logging.AddConsole());

        var host = builder.Build();
        await host.StartAsync();
        return host;
    }
}