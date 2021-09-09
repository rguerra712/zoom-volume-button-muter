using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;
using NAudio.CoreAudioApi;
using NAudio.Wave;

namespace ZoomVolumeButtonMuter
{
    public partial class ZoomVolumeButtonMuter : Form
    {
        private static IEnumerable<MMDevice> _devices = GetDevices();

        public ZoomVolumeButtonMuter()
        {
            RegisterDevicesForVolumeChangeEvents();
            InitializeComponent();
        }

        private void ZoomVolumeButtonMuter_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
            MinimizeToTray();
        }

        private static void RegisterDevicesForVolumeChangeEvents()
        {
            foreach (var device in _devices)
            {
                device.AudioEndpointVolume.OnVolumeNotification +=
                    _ => CheckForVolumeChangeAndGiveMuteZoomIfItChanges();
            }
        }

        private static void CheckForVolumeChangeAndGiveMuteZoomIfItChanges()
        {
            SendKeys.SendWait("^+%"); // CTRL-ALT-SHIFT to focus zoom
            SendKeys.SendWait("%{a}"); // ALT-A to toggle mute
        }

        private static IEnumerable<MMDevice> GetDevices()
        {
            var enumerator = new MMDeviceEnumerator();
            for (var i = 0; i < WaveOut.DeviceCount; i++)
                yield return enumerator.EnumerateAudioEndPoints(DataFlow.Render, DeviceState.Active)[i];
            enumerator.Dispose();
        }

        private void MinimizeToTray()
        {
            Hide();
            notifyIcon.Visible = true;
            ShowInTaskbar = false;
        }

        // Thanks to <a href="https://iconscout.com/icons/mute" target="_blank">Mute Colored Outline Icon</a> by <a href="https://iconscout.com/contributors/baitisstudio" target="_blank">Baiti Studio</a>
        private void ZoomVolumeButtonMuter_Resize(object sender, EventArgs e)
        {
            if (this.WindowState != FormWindowState.Minimized) return;
            MinimizeToTray();
        }

        private void AboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Show();
            WindowState = FormWindowState.Normal;
        }

        private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void MinimizeToTrayToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MinimizeToTray();
        }
        
    }
}
