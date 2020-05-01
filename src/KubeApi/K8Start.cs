using System;
using System.Net;
using System.Threading.Tasks;
using HelloWorld.Interfaces;
using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.Clustering.Kubernetes;
using Orleans.Configuration;
using Orleans.Runtime;

namespace KubeApi
{
    public class K8Start
    {
        public static async Task<IClusterClient> StartClientWithRetries(bool local,
            int initializeAttemptsBeforeFailing = 5)
        {
            int attempt = 0;
            while (true)
            {
                try
                {
                    return await StartClusterClient(local);
                }
                catch (SiloUnavailableException)
                {
                    attempt++;
                    Console.WriteLine(
                        $"Attempt {attempt} of {initializeAttemptsBeforeFailing} failed to initialize the Orleans client.");
                    if (attempt > initializeAttemptsBeforeFailing)
                    {
                        throw;
                    }

                    await Task.Delay(TimeSpan.FromSeconds(4));
                }
            }
        }

        private static async Task<IClusterClient> StartClusterClient(bool local)
        {
            var builder =
                local ? new ClientBuilder().UseLocalhostClustering() : new ClientBuilder().UseKubeGatewayListProvider();
            builder
                .Configure<ClusterOptions>(options =>
                {
                    options.ClusterId = "greetingCluster";
                    options.ServiceId = "greetingService";
                })
                .Configure<EndpointOptions>(options => options.AdvertisedIPAddress = IPAddress.Loopback)
                .ConfigureApplicationParts(parts => parts.AddApplicationPart(typeof(IHello).Assembly).WithReferences())
                .ConfigureLogging(logging => logging.AddConsole());

            var client = builder.Build();
            await client.Connect();
            Console.WriteLine("Client successfully connect to silo host");
            return client;
        }
    }
}