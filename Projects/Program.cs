using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using HBD.PulumiNet.Share.Ad;
using HBD.PulumiNet.Share.Core;
using HBD.PulumiNet.Share.KeyVaults;
using Pulumi;

class Program
{
    static Task<int> Main()
    {
        bool.TryParse(Environment.GetEnvironmentVariable("PULUMI_DEBUG"), out var debug);
        if (debug)
        {
            Console.WriteLine("Awaiting debugger to attach...");
            while (!Debugger.IsAttached)
                Thread.Sleep(100);
            Console.WriteLine("Running with debug mode.");
        }

        return Deployment.RunAsync(Stack.RunAsync);
    }
}