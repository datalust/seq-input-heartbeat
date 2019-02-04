using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Seq.Apps;

namespace Seq.Input.Heartbeat
{
    /// <summary>
    /// The <see cref="HeartbeatInput"/> class is the plug-in type loaded by Seq at runtime.
    /// </summary>
    /// <remarks>
    /// Information from the <see cref="SeqAppAttribute"/> applied here is displayed
    /// when listing the app in the Seq user interface.
    /// </remarks>
    [SeqApp("Heartbeat", Description = "An example input that publishes an event at a specified interval")]
    public class HeartbeatInput : SeqApp, IPublishJson
    {
        Task _heartbeat;
        readonly CancellationTokenSource _cancel = new CancellationTokenSource();

        /// <summary>
        /// An example setting. Seq will request values for <see cref="SeqAppSettingAttribute"/>-annotated
        /// properties from the user when setting up an instance of the app.
        /// </summary>
        [SeqAppSetting(DisplayName = "Interval (ms)")]
        public int MillisecondsInterval { get; set; }

        /// <summary>
        /// Start publishing events from the input.
        /// </summary>
        /// <param name="inputWriter"></param>
        public void Start(TextWriter inputWriter)
        {
            _heartbeat = Task.Run(() => HeartbeatAsync(inputWriter), _cancel.Token);
        }

        async Task HeartbeatAsync(TextWriter inputWriter)
        {
            var counter = 1;
            while (!_cancel.IsCancellationRequested)
            {
                var evt = $"{{\"@t\":\"{DateTime.UtcNow:o}\",\"@mt\":\"Heartbeat {{Counter}}\",\"Counter\":{counter:g2} }}";
                await inputWriter.WriteLineAsync(evt);
                await Task.Delay(MillisecondsInterval);
                counter++;
            }
        }

        public void Stop()
        {
            _cancel.Cancel();
            _heartbeat.Wait();
        }
    }
}
