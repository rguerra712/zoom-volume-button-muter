using System;
using System.Collections.Generic;
using System.Windows.Forms;
using NAudio.CoreAudioApi;
using NAudio.Wave;

namespace ZoomVolumeButtonMuter
{
    public partial class ZoomVolumeButtonMuter : Form
    {
        public ZoomVolumeButtonMuter()
        {
            InitializeComponent();
        }

        private void ZoomVolumeButtonMuter_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
            MinimizeToTray();
            RegisterDevicesForVolumeChangeEvents();
        }

        private void RegisterDevicesForVolumeChangeEvents()
        {
            var devices = GetDevices();
            foreach (var device in devices)
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

        private IEnumerable<MMDevice> GetDevices()
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
