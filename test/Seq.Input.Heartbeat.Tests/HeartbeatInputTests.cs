using System;
using System.IO;
using System.Threading.Tasks;
using Seq.Input.Heartbeat.Tests.Support;
using Xunit;

namespace Seq.Input.Heartbeat.Tests
{
    public class HeartbeatInputTests
    {
        [Fact]
        public async Task HeartbeatInputPublishesEvents()
        {
            var input = new HeartbeatInput();
            const int millisecondsInterval = 100;
            input.MillisecondsInterval = millisecondsInterval;
            input.Attach(new TestAppHost());

            var events = new StringWriter();
            input.Start(events);

            await Task.Delay((int)(millisecondsInterval * 1.5));
            input.Stop();

            var lines = events.ToString().Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
            Assert.Equal(2, lines.Length);
        }
    }
}
