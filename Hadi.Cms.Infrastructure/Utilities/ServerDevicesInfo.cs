using System;
using System.Collections.Generic;
using System.Management;

namespace Hadi.Cms.Infrastructure.Utilities
{
    public class ServerDevicesInfo
    {
        private static List<Win32Bios> _devicesInfo = null;

        public static List<Win32Bios> DevicesInfo
        {
            get
            {
                if (_devicesInfo == null)
                {
                    var searcher = new ManagementObjectSearcher("SELECT * FROM Win32_BIOS");
                    var objectSearcherCollection = searcher.Get();

                    _devicesInfo = new List<Win32Bios>();
                    foreach (var obj in objectSearcherCollection)
                    {
                        var devicesInfoItem = new Win32Bios();
                        devicesInfoItem.BiosCharacteristics = (ushort[])obj["BiosCharacteristics"];
                        devicesInfoItem.BiosVersion = (string[])obj["BIOSVersion"];
                        devicesInfoItem.BuildNumber = (string)obj["BuildNumber"];
                        devicesInfoItem.Caption = (string)obj["Caption"];
                        devicesInfoItem.CodeSet = (string)obj["CodeSet"];
                        devicesInfoItem.CurrentLanguage = (string)obj["CurrentLanguage"];
                        devicesInfoItem.Description = (string)obj["Description"];
                        devicesInfoItem.IdentificationCode = (string)obj["IdentificationCode"];
                        devicesInfoItem.InstallableLanguages = (ushort?)obj["InstallableLanguages"];
                        devicesInfoItem.InstallDate = (DateTime?)obj["InstallDate"];
                        devicesInfoItem.LanguageEdition = (string)obj["LanguageEdition"];
                        devicesInfoItem.ListOfLanguages = (string[])obj["ListOfLanguages"];
                        devicesInfoItem.Manufacturer = (string)obj["Manufacturer"];
                        devicesInfoItem.Name = (string)obj["Name"];
                        devicesInfoItem.OtherTargetOs = (string)obj["OtherTargetOS"];
                        devicesInfoItem.PrimaryBios = (bool?)obj["PrimaryBIOS"];
                        devicesInfoItem.ReleaseDate = (string)obj["ReleaseDate"];
                        devicesInfoItem.SerialNumber = (string)obj["SerialNumber"];
                        devicesInfoItem.SMBIOSBIOSVersion = (string)obj["SMBIOSBIOSVersion"];
                        devicesInfoItem.SMBIOSMajorVersion = (ushort?)obj["SMBIOSMajorVersion"];
                        devicesInfoItem.SMBIOSMinorVersion = (ushort?)obj["SMBIOSMinorVersion"];
                        devicesInfoItem.SMBIOSPresent = (bool?)obj["SMBIOSPresent"];
                        devicesInfoItem.SoftwareElementId = (string)obj["SoftwareElementID"];
                        devicesInfoItem.SoftwareElementState = (ushort?)obj["SoftwareElementState"];
                        devicesInfoItem.Status = (string)obj["Status"];
                        devicesInfoItem.TargetOperatingSystem = (ushort?)obj["TargetOperatingSystem"];
                        devicesInfoItem.Version = (string)obj["Version"];

                        _devicesInfo.Add(devicesInfoItem);
                    }
                }

                return _devicesInfo;
            }
        }
    }

    public class Win32Bios
    {
        public ushort[] BiosCharacteristics;
        public string[] BiosVersion;
        public string BuildNumber;
        public string Caption;
        public string CodeSet;
        public string CurrentLanguage;
        public string Description;
        public string IdentificationCode;
        public ushort? InstallableLanguages;
        public DateTime? InstallDate;
        public string LanguageEdition;
        public string[] ListOfLanguages;
        public string Manufacturer;
        public string Name;
        public string OtherTargetOs;
        public bool? PrimaryBios;
        public string ReleaseDate;
        public string SerialNumber;
        public string SMBIOSBIOSVersion;
        public ushort? SMBIOSMajorVersion;
        public ushort? SMBIOSMinorVersion;
        public bool? SMBIOSPresent;
        public string SoftwareElementId;
        public ushort? SoftwareElementState;
        public string Status;
        public ushort? TargetOperatingSystem;
        public string Version;
    }
}