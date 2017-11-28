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
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRI.Ket.Common.Devices.ManagedNativeWifi
{
	/// <summary>
	/// Data encryption type to be used to connect to wireless LAN
	/// </summary>
	/// <remarks>
	/// https://msdn.microsoft.com/en-us/library/windows/desktop/ms706969.aspx
	/// </remarks>
	public enum EncryptionType
	{
		/// <summary>
		/// None (valid value)
		/// </summary>
		None = 0,

		/// <summary>
		/// WEP encryption for WEP
		/// </summary>
		WEP,

		/// <summary>
		/// TKIP encryption for WPA/WPA2
		/// </summary>
		TKIP,

		/// <summary>
		/// AES (CCMP) encryption for WPA/WPA2
		/// </summary>
		AES
	}

	internal static class EncryptionTypeConverter
	{
		public static bool TryParse(string source, out EncryptionType encryption)
		{
			switch (source)
			{
				case "WEP":
					encryption = EncryptionType.WEP;
					return true;
				case "TKIP":
					encryption = EncryptionType.TKIP;
					return true;
				case "AES":
					encryption = EncryptionType.AES;
					return true;
				case "none":
					encryption = EncryptionType.None;
					return true;
			}
			encryption = default(EncryptionType);
			return true;
		}
	}
}