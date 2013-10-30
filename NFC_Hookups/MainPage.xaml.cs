using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Windows.Networking.Proximity;
using WP8_NFCSample_01.Resources;

namespace WP8_NFCSample_01
{
    public partial class MainPage : PhoneApplicationPage
    {
        Windows.Networking.Proximity.ProximityDevice proximityDevice;

        // Constructor
        public MainPage()
        {
            InitializeComponent();

            InitializeProximityDevice();
        }

        private void InitializeProximityDevice()
        {
            proximityDevice = ProximityDevice.GetDefault();

            if (proximityDevice != null)
            {
                txtInfo.Text = "Your phone has NFC, and NFC has been enabled.\n";

                proximityDevice.DeviceArrived += ProximityDeviceArrived;
                proximityDevice.DeviceDeparted += ProximityDeviceDeparted;

                txtInfo.Text = "Proximity device initialized.\n";
            }
            else
            {
                txtInfo.Text = "Your phone has no NFC or NFC is disabled\n";
                txtInfo.Text = "Failed to initialized proximity device.\n";
            }
        }

        // DOESN'T WORK BECAUSE THREAD ACCESS VIOLATION
        //private void ProximityDeviceArrived(ProximityDevice device)
        //{
        //    txtInfo.Text = "Proximate device arrived. id = " + device.DeviceId + "\n";
        //}

        //private void ProximityDeviceDeparted(ProximityDevice device)
        //{
        //    txtInfo.Text = "Proximate device departed. id = " + device.DeviceId + "\n";
        //}
        // DOESN'T WORK BECAUSE THREAD ACCESS VIOLATION

        private void ProximityDeviceArrived(object sender)
        {
            Deployment.Current.Dispatcher.BeginInvoke(() =>
            {
                txtInfo.Text += "Proximity device arrived.\n";
                txtInfo.Text += "Device ID: " + ((ProximityDevice)sender).DeviceId + "\n";
            });
        }

        private void ProximityDeviceDeparted(object sender)
        {
            Deployment.Current.Dispatcher.BeginInvoke(() =>
            {
                txtInfo.Text += "Proximity device departed.\n";
            });
        }  

        public void SendNFCMessage()
        {
            ProximityDevice device = ProximityDevice.GetDefault();

            // Make sure NFC is supported
            if (device != null)
            {
                long Id = device.PublishMessage("Windows.SampleMessageType", "Hello World!");
                Debug.WriteLine("Published Message. ID is {0}", Id);

                // Store the unique message Id so that it 
                // can be used to stop publishing this message
            }
        }

        private void OnSendMessage(object sender, System.Windows.Input.GestureEventArgs e)
        {
            SendNFCMessage();
        }
    }
}