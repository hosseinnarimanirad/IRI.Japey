﻿//The MIT License(MIT)

//Copyright(c) 2015 EMO

//Permission is hereby granted, free of charge, to any person obtaining a copy
//of this software and associated documentation files (the "Software"), to deal
//in the Software without restriction, including without limitation the rights
//to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//copies of the Software, and to permit persons to whom the Software is
//furnished to do so, subject to the following conditions:

//The above copyright notice and this permission notice shall be included in all
//copies or substantial portions of the Software.

//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
//SOFTWARE.

//https://github.com/emoacht/ManagedNativeWifi

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using static IRI.Ket.Common.Devices.ManagedNativeWifi.Win32.NativeMethod;
using Base = IRI.Ket.Common.Devices.ManagedNativeWifi.Win32.BaseMethod;

namespace IRI.Ket.Common.Devices.ManagedNativeWifi
{
	/// <summary>
	/// An instantiatable implementation of Native Wifi API
	/// </summary>
	public class NativeWifiPlayer : IDisposable
	{
		private readonly Base.WlanNotificationClient _client;

		/// <summary>
		/// Constructor
		/// </summary>
		public NativeWifiPlayer()
		{
			_client = new Base.WlanNotificationClient();
			_client.NotificationReceived += OnNotificationReceived;
		}

		/// <summary>
		/// Occurs when wireless LAN list is refreshed.
		/// </summary>
		public event EventHandler NetworkRefreshed;

		/// <summary>
		/// Occurs when a wireless interface is added/removed/enabled/disabled
		/// </summary>
		public event EventHandler InterfaceChanged;

		/// <summary>
		/// Occurs when connection to wireless LAN is changed.
		/// </summary>
		public event EventHandler ConnectionChanged;

		/// <summary>
		/// Occurs when a wireless profile or wireless profile name is changed.
		/// </summary>
		public event EventHandler ProfileChanged;

		private void OnNotificationReceived(object sender, WLAN_NOTIFICATION_DATA e)
		{
			Debug.WriteLine($"NotificationReceived: {(WLAN_NOTIFICATION_ACM)e.NotificationCode}");

			switch (((WLAN_NOTIFICATION_ACM)e.NotificationCode))
			{
				case WLAN_NOTIFICATION_ACM.wlan_notification_acm_scan_list_refresh:
					NetworkRefreshed?.Invoke(this, EventArgs.Empty);
					break;
				case WLAN_NOTIFICATION_ACM.wlan_notification_acm_interface_arrival:
				case WLAN_NOTIFICATION_ACM.wlan_notification_acm_interface_removal:
					InterfaceChanged?.Invoke(this, EventArgs.Empty);
					break;
				case WLAN_NOTIFICATION_ACM.wlan_notification_acm_connection_complete:
				case WLAN_NOTIFICATION_ACM.wlan_notification_acm_disconnected:
					ConnectionChanged?.Invoke(this, EventArgs.Empty);
					break;
				case WLAN_NOTIFICATION_ACM.wlan_notification_acm_profile_change:
				case WLAN_NOTIFICATION_ACM.wlan_notification_acm_profile_name_change:
					ProfileChanged?.Invoke(this, EventArgs.Empty);
					break;
			}
		}

		#region Dispose

		private bool _disposed = false;

		/// <summary>
		/// Dispose
		/// </summary>
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		/// <summary>
		/// Dispose
		/// </summary>
		/// <param name="disposing">Whether to dispose managed resources</param>
		protected virtual void Dispose(bool disposing)
		{
			if (_disposed)
				return;

			if (disposing)
			{
				_client.Dispose();
			}

			_disposed = true;
		}

		#endregion

		/// <summary>
		/// Enumerates wireless interface information.
		/// </summary>
		public IEnumerable<InterfaceInfo> EnumerateInterfaces() =>
			NativeWifi.EnumerateInterfaces(_client);

		/// <summary>
		/// Asynchronously requests wireless interfaces to scan wireless LANs.
		/// </summary>
		public Task<IEnumerable<Guid>> ScanNetworksAsync(TimeSpan timeout, CancellationToken cancellationToken) =>
			NativeWifi.ScanNetworksAsync(_client, timeout, cancellationToken);

		/// <summary>
		/// Enumerates SSIDs of available wireless LANs.
		/// </summary>
		public IEnumerable<NetworkIdentifier> EnumerateAvailableNetworkSsids() =>
			NativeWifi.EnumerateAvailableNetworkSsids(_client);

		/// <summary>
		/// Enumerates SSIDs of connected wireless LANs.
		/// </summary>
		public IEnumerable<NetworkIdentifier> EnumerateConnectedNetworkSsids() =>
			NativeWifi.EnumerateConnectedNetworkSsids(_client);

		/// <summary>
		/// Enumerates wireless LAN information on available wireless LANs.
		/// </summary>
		public IEnumerable<AvailableNetworkPack> EnumerateAvailableNetworks() =>
			NativeWifi.EnumerateAvailableNetworks(_client);

		/// <summary>
		/// Enumerates wireless LAN information on BSS networks.
		/// </summary>
		public IEnumerable<BssNetworkPack> EnumerateBssNetworks() =>
			NativeWifi.EnumerateBssNetworks(_client);

		/// <summary>
		/// Enumerates wireless profile information in preference order.
		/// </summary>
		public IEnumerable<ProfilePack> EnumerateProfiles() =>
			NativeWifi.EnumerateProfiles(_client);

		/// <summary>
		/// Sets (adds or overwrites) the content of a specified wireless profile.
		/// </summary>
		public bool SetProfile(Guid interfaceId, ProfileType profileType, string profileXml, string profileSecurity, bool overwrite) =>
			NativeWifi.SetProfile(_client, interfaceId, profileType, profileXml, profileSecurity, overwrite);

		/// <summary>
		/// Sets the position of a specified wireless profile in preference order.
		/// </summary>
		public bool SetProfilePosition(Guid interfaceId, string profileName, int position) =>
			NativeWifi.SetProfilePosition(_client, interfaceId, profileName, position);

		/// <summary>
		/// Deletes a specified wireless profile.
		/// </summary>
		public bool DeleteProfile(Guid interfaceId, string profileName) =>
			NativeWifi.DeleteProfile(_client, interfaceId, profileName);

		/// <summary>
		/// Attempts to connect to the wireless LAN associated to a specified wireless profile.
		/// </summary>
		public bool ConnectNetwork(Guid interfaceId, string profileName, BssType bssType) =>
			NativeWifi.ConnectNetwork(_client, interfaceId, profileName, bssType);

		/// <summary>
		/// Asynchronously attempts to connect to the wireless LAN associated to a specified wireless profile.
		/// </summary>
		public Task<bool> ConnectNetworkAsync(Guid interfaceId, string profileName, BssType bssType, TimeSpan timeout, CancellationToken cancellationToken) =>
			NativeWifi.ConnectNetworkAsync(_client, interfaceId, profileName, bssType, timeout, cancellationToken);

		/// <summary>
		/// Disconnects from the wireless LAN associated to a specified wireless interface.
		/// </summary>
		public bool DisconnectNetwork(Guid interfaceId) =>
			NativeWifi.DisconnectNetwork(_client, interfaceId);

		/// <summary>
		/// Asynchronously disconnects from the wireless LAN associated to a specified wireless interface.
		/// </summary>
		public Task<bool> DisconnectNetworkAsync(Guid interfaceId, TimeSpan timeout, CancellationToken cancellationToken) =>
			NativeWifi.DisconnectNetworkAsync(_client, interfaceId, timeout, cancellationToken);
	}
}