using System;
using System.Collections.Generic;
using System.Text;
using Seq.Apps;
using Serilog;

namespace Seq.Input.Heartbeat.Tests.Support
{
    class TestAppHost : IAppHost
    {
        public App App { get; } = new App("test", "Test", new Dictionary<string, string>(), ".");
        public Host Host { get; } = new Host("https://seq.example.com", null);
        public ILogger Logger { get; } = new LoggerConfiguration().CreateLogger();
        public string StoragePath { get; } = ".";
    }
}
