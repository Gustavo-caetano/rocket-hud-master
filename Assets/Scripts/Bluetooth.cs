using UnityEngine;
using InTheHand.Net.Sockets;
using System.Collections.Generic;


class Bluetooth {
    private BluetoothDeviceInfo _device = null;

    public void ListDevices()
    {
        var cli = new BluetoothClient();
        IReadOnlyCollection<BluetoothDeviceInfo> peers = cli.DiscoverDevices();

        foreach (BluetoothDeviceInfo peer in peers)
        {
            Debug.Log(peer.DeviceName + "\n");
        }
    }
}