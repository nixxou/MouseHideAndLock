using System.Collections.Generic;
using System;
using System.Runtime.InteropServices;
using System.Text;

namespace MouseHideAndLock
{

	public class IniFile
	{
		private string filePath;

		[DllImport("kernel32")]
		private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);

		[DllImport("kernel32")]
		private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);

		[DllImport("Kernel32.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		private static extern UInt32 GetPrivateProfileSection
				(
					[In][MarshalAs(UnmanagedType.LPStr)] string strSectionName,
					// Note that because the key/value pars are returned as null-terminated
					// strings with the last string followed by 2 null-characters, we cannot
					// use StringBuilder.
					[In] IntPtr pReturnedString,
					[In] UInt32 nSize,
					[In][MarshalAs(UnmanagedType.LPStr)] string strFileName
				);

		public IniFile(string filePath)
		{
			this.filePath = filePath;
		}

		public void Write(string section, string key, string value)
		{
			WritePrivateProfileString(section, key, value, filePath);
		}

		public string Read(string section, string key, string defaultValue = "")
		{
			StringBuilder sb = new StringBuilder(255);
			int i = GetPrivateProfileString(section, key, defaultValue, sb, 255, filePath);
			return sb.ToString();
		}

	}
}
