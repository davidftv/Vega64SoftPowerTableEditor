﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

namespace Vega64SoftPowerTableEditor
{
    //
    // "typedefs" for making it easier to copy paste the ATOM C Structs
    //
    using USHORT = UInt16;
    using UCHAR = Byte;
    using ULONG = UInt32;

    public class SoftPowerTable
    {   
        //
        // ATOM Structs
        //
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public class ATOM_COMMON_TABLE_HEADER
        {
            Int16 usStructureSize;
            Byte ucTableFormatRevision;
            Byte ucTableContentRevision;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public unsafe class ATOM_POWERPLAY_TABLE
        {
            public ATOM_POWERPLAY_TABLE() {
            }
            public ATOM_COMMON_TABLE_HEADER sHeader;
            UCHAR ucTableRevision;
            USHORT usTableSize;                        /* the size of header structure */
            ULONG ulGoldenPPID;                       /* PPGen use only */
            ULONG ulGoldenRevision;                   /* PPGen use only */
            USHORT usFormatID;                         /* PPGen use only */
            ULONG ulPlatformCaps;                     /* See ATOM_Vega10_CAPS_* */
            ULONG ulMaxODEngineClock;                 /* For Overdrive. */
            ULONG ulMaxODMemoryClock;                 /* For Overdrive. */
            USHORT usPowerControlLimit;
            USHORT usUlvVoltageOffset;                 /* in mv units */
            USHORT usUlvSmnclkDid;
            USHORT usUlvMp1clkDid;
            USHORT usUlvGfxclkBypass;
            USHORT usGfxclkSlewRate;
            UCHAR ucGfxVoltageMode;
            UCHAR ucSocVoltageMode;
            UCHAR ucUclkVoltageMode;
            UCHAR ucUvdVoltageMode;
            UCHAR ucVceVoltageMode;
            UCHAR ucMp0VoltageMode;
            UCHAR ucDcefVoltageMode;
            public USHORT usStateArrayOffset;                 /* points to ATOM_Vega10_State_Array */
            public USHORT usFanTableOffset;                   /* points to ATOM_Vega10_Fan_Table */
            USHORT usThermalControllerOffset;          /* points to ATOM_Vega10_Thermal_Controller */
            USHORT usSocclkDependencyTableOffset;      /* points to ATOM_Vega10_SOCCLK_Dependency_Table */
            public USHORT usMclkDependencyTableOffset;        /* points to ATOM_Vega10_MCLK_Dependency_Table */
            public USHORT usGfxclkDependencyTableOffset;      /* points to ATOM_Vega10_GFXCLK_Dependency_Table */
            USHORT usDcefclkDependencyTableOffset;     /* points to ATOM_Vega10_DCEFCLK_Dependency_Table */
            public USHORT usVddcLookupTableOffset;            /* points to ATOM_Vega10_Voltage_Lookup_Table */
            public USHORT usVddmemLookupTableOffset;          /* points to ATOM_Vega10_Voltage_Lookup_Table */
            USHORT usMMDependencyTableOffset;          /* points to ATOM_Vega10_MM_Dependency_Table */
            USHORT usVCEStateTableOffset;              /* points to ATOM_Vega10_VCE_State_Table */
            USHORT usReserve;                          /* No PPM Support for Vega10 */
            public USHORT usPowerTuneTableOffset;             /* points to ATOM_Vega10_PowerTune_Table */
            USHORT usHardLimitTableOffset;             /* points to ATOM_Vega10_Hard_Limit_Table */
            USHORT usVddciLookupTableOffset;           /* points to ATOM_Vega10_Voltage_Lookup_Table */
            USHORT usPCIETableOffset;                  /* points to ATOM_Vega10_PCIE_Table */
            USHORT usPixclkDependencyTableOffset;      /* points to ATOM_Vega10_PIXCLK_Dependency_Table */
            USHORT usDispClkDependencyTableOffset;     /* points to ATOM_Vega10_DISPCLK_Dependency_Table */
            USHORT usPhyClkDependencyTableOffset;      /* points to ATOM_Vega10_PHYCLK_Dependency_Table */
        };

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public unsafe class ATOM_Vega10_State
        {
            UCHAR ucSocClockIndexHigh;
            UCHAR ucSocClockIndexLow;
            UCHAR ucGfxClockIndexHigh;
            UCHAR ucGfxClockIndexLow;
            UCHAR ucMemClockIndexHigh;
            UCHAR ucMemClockIndexLow;
            USHORT usClassification;
            ULONG ulCapsAndSettings;
            USHORT usClassification2;
        };

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public unsafe class ATOM_Vega10_State_Array
        {
            public UCHAR ucRevId;
            public UCHAR ucNumEntries;                                         /* Number of entries. */
        };

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public unsafe class ATOM_Vega10_GFXCLK_Dependency_Table
        {
            public UCHAR ucRevId;
            public UCHAR ucNumEntries;                                         /* Number of entries. */
        };

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public unsafe class ATOM_Vega10_GFXCLK_Dependency_Record
        {
            public ULONG ulClk;                                               /* Clock Frequency */
            public UCHAR ucVddInd;                                            /* SOC_VDD index */
            public USHORT usCKSVOffsetandDisable;                              /* Bits 0~30: Voltage offset for CKS, Bit 31: Disable/enable for the GFXCLK level. */
            public USHORT usAVFSOffset;                                        /* AVFS Voltage offset */
            public UCHAR ucACGEnable;
            public UCHAR ucReserved1;
            public UCHAR ucReserved2;
            public UCHAR ucReserved3;
        };

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public unsafe class ATOM_Vega10_MCLK_Dependency_Table
        {
            public UCHAR ucRevId;
            public UCHAR ucNumEntries;                                         /* Number of entries. */
        };

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public unsafe class ATOM_Vega10_MCLK_Dependency_Record
        {
            public ULONG ulMemClk;                                            /* Clock Frequency */
            public UCHAR ucVddInd;                                            /* SOC_VDD index */
            public UCHAR ucVddMemInd;                                         /* MEM_VDD - only non zero for MCLK record */
            public UCHAR ucVddciInd;                                          /* VDDCI   = only non zero for MCLK record */
        };

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public unsafe class ATOM_Vega10_Voltage_Lookup_Record
        {
            public USHORT usVdd;                                               /* Base voltage */
        };

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public unsafe class ATOM_Vega10_Voltage_Lookup_Table
        {
            public UCHAR ucRevId;
            public UCHAR ucNumEntries;                                          /* Number of entries */
        };

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public unsafe class ATOM_Vega10_Fan_Table
        {
            public UCHAR ucRevId;
            public USHORT usFanOutputSensitivity;
            public USHORT usFanAcousticLimitRpm;
            public USHORT usThrottlingRPM;
            public USHORT usTargetTemperature;
            public USHORT usMinimumPWMLimit;
            public USHORT usTargetGfxClk;
            public USHORT usFanGainEdge;
            public USHORT usFanGainHotspot;
            public USHORT usFanGainLiquid;
            public USHORT usFanGainVrVddc;
            public USHORT usFanGainVrMvdd;
            public USHORT usFanGainPlx;
            public USHORT usFanGainHbm;
            public UCHAR ucEnableZeroRPM;
            public USHORT usFanStopTemperature;
            public USHORT usFanStartTemperature;
            public UCHAR ucFanParameters;
            public UCHAR ucFanMinRPM;
            public UCHAR ucFanMaxRPM;
        };

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public unsafe class ATOM_Vega10_PowerTune_Table_V3
        {
            public UCHAR ucRevId;
            public USHORT usSocketPowerLimit;
            public USHORT usBatteryPowerLimit;
            public USHORT usSmallPowerLimit;
            public USHORT usTdcLimit;
            public USHORT usEdcLimit;
            public USHORT usSoftwareShutdownTemp;
            public USHORT usTemperatureLimitHotSpot;
            public USHORT usTemperatureLimitLiquid1;
            public USHORT usTemperatureLimitLiquid2;
            public USHORT usTemperatureLimitHBM;
            public USHORT usTemperatureLimitVrSoc;
            public USHORT usTemperatureLimitVrMem;
            public USHORT usTemperatureLimitPlx;
            public USHORT usLoadLineResistance;
            public UCHAR ucLiquid1_I2C_address;
            public UCHAR ucLiquid2_I2C_address;
            public UCHAR ucLiquid_I2C_Line;
            public UCHAR ucVr_I2C_address;
            public UCHAR ucVr_I2C_Line;
            public UCHAR ucPlx_I2C_address;
            public UCHAR ucPlx_I2C_Line;
            public USHORT usTemperatureLimitTedge;
            public USHORT usBoostStartTemperature;
            public USHORT usBoostStopTemperature;
            public ULONG ulBoostClock;
            public ULONG Reserve0;
            public ULONG Reserve1;
        };

        //
        // PowerTable Members
        //
        public ATOM_POWERPLAY_TABLE atom_powerplay_table;

        public ATOM_Vega10_State_Array atom_vega10_state_array;

        public ATOM_Vega10_GFXCLK_Dependency_Table atom_vega10_gfxclk_table;
        public List<ATOM_Vega10_GFXCLK_Dependency_Record> atom_vega10_gfxclk_entries = new List<ATOM_Vega10_GFXCLK_Dependency_Record>();

        public ATOM_Vega10_MCLK_Dependency_Table atom_vega10_memclk_table;
        public List<ATOM_Vega10_MCLK_Dependency_Record> atom_vega10_memclk_entries = new List<ATOM_Vega10_MCLK_Dependency_Record>();

        public ATOM_Vega10_Voltage_Lookup_Table atom_vega10_gfxvdd_table;
        public List<ATOM_Vega10_Voltage_Lookup_Record> atom_vega10_gfxvdd_record = new List<ATOM_Vega10_Voltage_Lookup_Record>();

        public ATOM_Vega10_Voltage_Lookup_Table atom_vega10_memvdd_table;
        public List<ATOM_Vega10_Voltage_Lookup_Record> atom_vega10_memvdd_record = new List<ATOM_Vega10_Voltage_Lookup_Record>();

        public ATOM_Vega10_Fan_Table atom_vega10_fan_table;

        public ATOM_Vega10_PowerTune_Table_V3 atom_vega10_powertune_table;

        //
        // Private Members
        //
        private static String STR_HEX_START = "=hex:";

        private byte[] _originalData = null;
        private string _originalText = null;

        private List<int> _hexBlobNewLineIndices = new List<int>();
        private int _hexStartIndex = 0;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Vega64SoftPowerTableEditor.SoftPowerTable"/> class.
        /// </summary>
        public SoftPowerTable()
        {
        }

        /// <summary>
        /// Opens the reg file.
        /// </summary>
        /// <returns>SoftPowerTable</returns>
        public static SoftPowerTable OpenRegFile(string filename)
        {
            SoftPowerTable spt = new SoftPowerTable();

            // Parse out the hex data
            spt._originalText = File.ReadAllText(filename);
            spt._hexStartIndex = spt._originalText.IndexOf(STR_HEX_START, StringComparison.Ordinal) + STR_HEX_START.Length;
            String hexData = spt._originalText.Substring(spt._hexStartIndex, spt._originalText.Length - spt._hexStartIndex);

            // store newlines for later
            for (int i = spt._hexStartIndex; i < spt._originalText.Length; i++)
            {
                if (spt._originalText[i] == '\n') {
                    spt._hexBlobNewLineIndices.Add(i);
                }
            }

            // clean up the hex data
            hexData = Regex.Replace(hexData, @"\t|\n|\r|\s+|\\|,", "");

            // convert to byte array
            byte[] byteArray = StringToByteArray(hexData);
            spt._originalData = byteArray;

            // parse main table
            spt.atom_powerplay_table = FromBytes<ATOM_POWERPLAY_TABLE>(byteArray);

            // parse state array
            int offset = spt.atom_powerplay_table.usStateArrayOffset;
            spt.atom_vega10_state_array = FromBytes<ATOM_Vega10_State_Array>(byteArray.Skip(offset).ToArray());

            // parse gfx clk states (hurray!)
            offset = spt.atom_powerplay_table.usGfxclkDependencyTableOffset;
            spt.atom_vega10_gfxclk_table = FromBytes<ATOM_Vega10_GFXCLK_Dependency_Table>(byteArray.Skip(offset).ToArray());
            offset += Marshal.SizeOf(spt.atom_vega10_gfxclk_table);
            spt.atom_vega10_gfxclk_entries = ArrFromBytes<ATOM_Vega10_GFXCLK_Dependency_Record>(byteArray, offset, spt.atom_vega10_gfxclk_table.ucNumEntries);

            // parse mem clk states
            offset = spt.atom_powerplay_table.usMclkDependencyTableOffset;
            spt.atom_vega10_memclk_table = FromBytes<ATOM_Vega10_MCLK_Dependency_Table>(byteArray.Skip(offset).ToArray());
            offset += Marshal.SizeOf(spt.atom_vega10_memclk_table);
            spt.atom_vega10_memclk_entries = ArrFromBytes<ATOM_Vega10_MCLK_Dependency_Record>(byteArray, offset, spt.atom_vega10_memclk_table.ucNumEntries);

            // parse gfx voltages
            offset = spt.atom_powerplay_table.usVddcLookupTableOffset;
            spt.atom_vega10_gfxvdd_table = FromBytes<ATOM_Vega10_Voltage_Lookup_Table>(byteArray.Skip(offset).ToArray());
            offset += Marshal.SizeOf(spt.atom_vega10_gfxvdd_table);
            spt.atom_vega10_gfxvdd_record = ArrFromBytes<ATOM_Vega10_Voltage_Lookup_Record>(byteArray, offset, spt.atom_vega10_gfxvdd_table.ucNumEntries);

            // parse mem voltages
            offset = spt.atom_powerplay_table.usVddmemLookupTableOffset;
            spt.atom_vega10_memvdd_table = FromBytes<ATOM_Vega10_Voltage_Lookup_Table>(byteArray.Skip(offset).ToArray());
            offset += Marshal.SizeOf(spt.atom_vega10_gfxvdd_table);
            spt.atom_vega10_memvdd_record = ArrFromBytes<ATOM_Vega10_Voltage_Lookup_Record>(byteArray, offset, spt.atom_vega10_memvdd_table.ucNumEntries);

            // parse fan table
            spt.atom_vega10_fan_table = FromBytes<ATOM_Vega10_Fan_Table>(byteArray.Skip(spt.atom_powerplay_table.usFanTableOffset).ToArray());

            // parse powertune table
            spt.atom_vega10_powertune_table = FromBytes<ATOM_Vega10_PowerTune_Table_V3>(byteArray.Skip(spt.atom_powerplay_table.usPowerTuneTableOffset).ToArray());

            // debug
            Console.WriteLine(spt._originalText);

            return spt;
        }

        /// <summary>
        /// Saves the reg file.
        /// </summary>
        public void SaveRegFile(string filename)
        {
            // clone the original data
            byte[] data = this._originalData.ToArray();

            // start replacing bytes appropriately
            // powerplay table
            byte[] tmpBytes = GetBytes<ATOM_POWERPLAY_TABLE>(this.atom_powerplay_table);
            Array.Copy(tmpBytes, 0, data, 0, tmpBytes.Length);

            // gfx clock
            tmpBytes = GetBytes<ATOM_Vega10_GFXCLK_Dependency_Table>(this.atom_vega10_gfxclk_table);
            Array.Copy(tmpBytes, 0, data, this.atom_powerplay_table.usGfxclkDependencyTableOffset, tmpBytes.Length);

            int offset = this.atom_powerplay_table.usGfxclkDependencyTableOffset + Marshal.SizeOf(this.atom_vega10_gfxclk_table);
            tmpBytes = ArrGetBytes<ATOM_Vega10_GFXCLK_Dependency_Record>(this.atom_vega10_gfxclk_entries);
            Array.Copy(tmpBytes, 0, data, offset, tmpBytes.Length);

            // mem clock
            offset = this.atom_powerplay_table.usMclkDependencyTableOffset + Marshal.SizeOf(this.atom_vega10_memclk_table);
            tmpBytes = ArrGetBytes<ATOM_Vega10_MCLK_Dependency_Record>(this.atom_vega10_memclk_entries);
            Array.Copy(tmpBytes, 0, data, offset, tmpBytes.Length);

            // gfx vdd 
            offset = this.atom_powerplay_table.usVddcLookupTableOffset + Marshal.SizeOf(this.atom_vega10_gfxvdd_table);
            tmpBytes = ArrGetBytes<ATOM_Vega10_Voltage_Lookup_Record>(this.atom_vega10_gfxvdd_record);
            Array.Copy(tmpBytes, 0, data, offset, tmpBytes.Length);

            // mem vdd
            offset = this.atom_powerplay_table.usVddmemLookupTableOffset + Marshal.SizeOf(this.atom_vega10_memvdd_table);
            tmpBytes = ArrGetBytes<ATOM_Vega10_Voltage_Lookup_Record>(this.atom_vega10_memvdd_record);
            Array.Copy(tmpBytes, 0, data, offset, tmpBytes.Length);

            // powertune table
            offset = this.atom_powerplay_table.usPowerTuneTableOffset;
            tmpBytes = GetBytes<ATOM_Vega10_PowerTune_Table_V3>(this.atom_vega10_powertune_table);
            Array.Copy(tmpBytes, 0, data, offset, tmpBytes.Length);

            // fan table
            offset = this.atom_powerplay_table.usFanTableOffset;
            tmpBytes = GetBytes<ATOM_Vega10_Fan_Table>(this.atom_vega10_fan_table);
            Array.Copy(tmpBytes, 0, data, offset, tmpBytes.Length);

            // dump out hex string
            string hex = BitConverter.ToString(data).Replace("-", String.Empty);

            Console.WriteLine("Old:" + ByteArrayToString(this._originalData));
            Console.WriteLine("New:" + hex);

            // add commas back in
            hex = Regex.Replace(hex, ".{2}", "$0,");

            // add new hex string to original file
            string s = this._originalText;
            s = s.Remove(this._hexStartIndex, s.Length - this._hexStartIndex);
            s = s.Insert(this._hexStartIndex, hex);

            // add new lines back in
            foreach (int newlineIndex in this._hexBlobNewLineIndices) {
                if (newlineIndex < s.Length) {
                    s = s.Insert(newlineIndex-2, "\\\r\n  ");
                }
            }

            // remove extra comma at end
            s = s.Remove(s.Length - 1);

            // save to file
            File.WriteAllText(filename, s);
        }

        /// <summary>
        /// Marshal data to byte array
        /// </summary>
        /// <returns>The bytes.</returns>
        /// <param name="obj">Object.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        static byte[] GetBytes<T>(T obj)
        {
            int size = Marshal.SizeOf(obj);
            byte[] arr = new byte[size];

            IntPtr ptr = Marshal.AllocHGlobal(size);
            Marshal.StructureToPtr(obj, ptr, true);
            Marshal.Copy(ptr, arr, 0, size);
            Marshal.FreeHGlobal(ptr);
            return arr;
        }

        /// <summary>
        /// Given a list of objects, convert to continous array of bytes representing the objects.
        /// </summary>
        /// <returns>The get bytes.</returns>
        /// <param name="obj">Object.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        static byte[] ArrGetBytes<T>(List<T> obj)
        {
            byte[] arr = new byte[obj.Count * Marshal.SizeOf(typeof(T))];
            byte[] tmpBytes = null;

            int internal_offset = 0;

            foreach (T record in obj) {
                tmpBytes = GetBytes<T>(record);
                Array.Copy(tmpBytes, 0, arr, internal_offset, tmpBytes.Length);
                internal_offset += Marshal.SizeOf(record);
            }

            return arr;
        }

        /// <summary>
        /// Construct template type
        /// </summary>
        /// <returns>The object.</returns>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public static T GetObject<T>()
        {
            return (T)Activator.CreateInstance(typeof(T));
        }

        /// <summary>
        /// Marshal the data from byte array
        /// </summary>
        /// <returns>The bytes.</returns>
        /// <param name="arr">Arr.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        static T FromBytes<T>(byte[] arr)
        {
            T obj = GetObject<T>();
            int size = Marshal.SizeOf(obj);
            IntPtr ptr = Marshal.AllocHGlobal(size);
            Marshal.Copy(arr, 0, ptr, size);
            obj = (T)Marshal.PtrToStructure(ptr, obj.GetType());
            Marshal.FreeHGlobal(ptr);

            return obj;
        }

        /// <summary>
        /// Convert a continuous set of bytes into an array of objects.
        /// </summary>
        /// <returns>The from bytes.</returns>
        /// <param name="arr">Arr.</param>
        /// <param name="offset">Offset.</param>
        /// <param name="entries">Entries.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        static List<T> ArrFromBytes<T>(byte[] arr, int offset, int entries)
        {
            List<T> retVal = new List<T>();
            for (int i = 0; i< entries; i++)
            {               
                T record = FromBytes<T>(arr.Skip(offset).ToArray());
                retVal.Add(record);
                offset += Marshal.SizeOf(record);
            }

            return retVal;
        }

        /// <summary>
        /// Convert a string blob into bytes
        /// </summary>
        /// <returns>The to byte array.</returns>
        /// <param name="hex">Hex.</param>
        public static byte[] StringToByteArray(string hex)
        {
            return Enumerable.Range(0, hex.Length)
                             .Where(x => x % 2 == 0)
                             .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                             .ToArray();
        }

        /// <summary>
        /// Bytes the array to string.
        /// </summary>
        /// <returns>The array to string.</returns>
        /// <param name="ba">Ba.</param>
        public static string ByteArrayToString(byte [] ba)
        {
            string hex = BitConverter.ToString(ba);
            return hex.Replace("-", "");
        }
    }
}
