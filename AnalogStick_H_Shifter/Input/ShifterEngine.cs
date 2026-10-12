using System;
using System.Collections.Concurrent;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace AnalogStick_H_Shifter
{
    /// <summary>
    /// Polls the stick on a dedicated thread, detects the engaged gear and sends the key presses.
    /// The UI only reads the published state and drains <see cref="Shifts"/>; it never blocks this loop.
    /// </summary>
    sealed class ShifterEngine : IDisposable
    {
        const int PollIntervalMs = 4;
        const int DisconnectedIntervalMs = 100;

        readonly Thread thread;
        readonly object stateLock = new object();
        volatile bool running = true;
        volatile bool outputEnabled;
        volatile IStickSource source;
        volatile GearZone[] zones = new GearZone[0];

        PointF position = new PointF(0.5f, 0.5f);
        bool connected;
        GearZone engaged;
        Keys heldKey = Keys.None;

        /// <summary>Gears engaged while the output was enabled (for the statistics).</summary>
        public ConcurrentQueue<GearZone> Shifts { get; } = new ConcurrentQueue<GearZone>();

        public ShifterEngine()
        {
            thread = new Thread(Run) { IsBackground = true, Name = "Shifter input", Priority = ThreadPriority.AboveNormal };
            thread.Start();
        }

        public bool OutputEnabled
        {
            get => outputEnabled;
            set
            {
                if (outputEnabled == value) return;
                outputEnabled = value;
                if (value) timeBeginPeriod(1); else timeEndPeriod(1);
            }
        }

        public void SetSource(IStickSource newSource)
        {
            var old = source;
            source = newSource;
            // The old source may still be in use by the poll loop for one iteration; it is disposed there.
            if (old != null) pendingDispose.Enqueue(old);
        }

        readonly ConcurrentQueue<IStickSource> pendingDispose = new ConcurrentQueue<IStickSource>();

        /// <summary>Hands a snapshot of the zones to the poll loop (call after adding/removing zones).</summary>
        public void SetZones(GearZone[] newZones) => zones = newZones ?? new GearZone[0];

        public void GetState(out PointF stickPosition, out bool isConnected, out GearZone engagedGear)
        {
            lock (stateLock)
            {
                stickPosition = position;
                isConnected = connected;
                engagedGear = engaged;
            }
        }

        void Run()
        {
            while (running)
            {
                while (pendingDispose.TryDequeue(out var old)) old.Dispose();

                var src = source;
                bool ok = false;
                PointF p = new PointF(0.5f, 0.5f);

                try { ok = src != null && src.TryRead(out p); }
                catch (Exception ex) { Console.WriteLine("Input error: " + ex.Message); }

                GearZone hit = null;
                if (ok)
                {
                    foreach (var zone in zones)
                    {
                        if (zone.Contains(p)) { hit = zone; break; }
                    }
                }

                Engage(hit);

                lock (stateLock)
                {
                    position = p;
                    connected = ok;
                    engaged = hit;
                }

                Thread.Sleep(ok ? PollIntervalMs : DisconnectedIntervalMs);
            }

            ReleaseHeldKey();
            source?.Dispose();
        }

        void Engage(GearZone hit)
        {
            Keys wanted = outputEnabled && hit != null ? hit.Key : Keys.None;

            if (hit != engaged && hit != null && outputEnabled)
                Shifts.Enqueue(hit);

            if (wanted == heldKey && hit == engaged) return;

            ReleaseHeldKey();
            if (wanted != Keys.None)
            {
                KeySender.TapAndHold(wanted);
                heldKey = wanted;
            }
        }

        void ReleaseHeldKey()
        {
            if (heldKey == Keys.None) return;
            KeySender.Up(heldKey);
            heldKey = Keys.None;
        }

        public void Dispose()
        {
            running = false;
            thread.Join(500);
            OutputEnabled = false;
        }

        [DllImport("winmm.dll")] static extern uint timeBeginPeriod(uint period);
        [DllImport("winmm.dll")] static extern uint timeEndPeriod(uint period);
    }
}
