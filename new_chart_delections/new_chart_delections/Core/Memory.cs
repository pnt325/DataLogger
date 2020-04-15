using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.NetworkInformation;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace new_chart_delections.Core
{
    public static class Memory
    {
        const int DEFAULT_SIZE = 128;
        public static uint BitSize { get; set; } = DEFAULT_SIZE;
        public static uint Int8Size { get; set; } = DEFAULT_SIZE;
        public static uint Uint8Size { get; set; } = DEFAULT_SIZE;
        public static uint Int16Size { get; set; } = DEFAULT_SIZE;
        public static uint Uint16Size { get; set; } = DEFAULT_SIZE;
        public static uint Int32Size { get; set; } = DEFAULT_SIZE;
        public static uint Uint32Size { get; set; } = DEFAULT_SIZE;
        public static uint FloatSize { get; set; } = DEFAULT_SIZE;

        public static Dictionary<string, uint> Address { get; private set; }       // = new Dictionary<string, uint>();
        public static Dictionary<string, MemoryTypes> Types { get; private set; }  // = new Dictionary<string, MemoryTypes>();

        private static byte[] memBit;
        private static sbyte[] memInt8;
        private static byte[] memUint8;
        private static short[] memInt16;
        private static ushort[] memUint16;
        private static int[] memInt32;
        private static uint[] memUint32;
        private static float[] memFloat;

        private static uint bit_Index = 0;
        private static uint int8_Index = 0;
        private static uint uint8_Index = 0;
        private static uint int16_Index = 0;
        private static uint uint16_Index = 0;
        private static uint int32_Index = 0;
        private static uint uint32_Index = 0;
        private static uint float_Index = 0;

        public static void Init()
        {
            memBit = new byte[BitSize];
            memInt8 = new sbyte[Int8Size];
            memUint8 = new byte[Uint8Size];
            memInt16 = new short[Int16Size];
            memUint16 = new ushort[Uint16Size];
            memInt32 = new int[Int32Size];
            memUint32 = new uint[Uint32Size];
            memFloat = new float[FloatSize];

            bit_Index = 0;
            int8_Index = 0;
            uint8_Index = 0;
            int16_Index = 0;
            uint16_Index = 0;
            int32_Index = 0;
            uint32_Index = 0;
            float_Index = 0;

            Types = new Dictionary<string, MemoryTypes>();
            Address = new Dictionary<string, uint>();
        }

        /// <summary>
        /// Clear all asign variable name and adress
        /// </summary>
        public static void Clear()
        {
            // clear index
            bit_Index = 0;
            int8_Index = 0;
            uint8_Index = 0;
            int16_Index = 0;
            uint16_Index = 0;
            int32_Index = 0;
            uint32_Index = 0;
            float_Index = 0;

            // clear items
            Address.Clear();
            Types.Clear();
        }

        /// <summary>
        /// Add new variable to memory
        /// </summary>
        /// <param name="name"></param>
        /// <param name="type"></param>
        /// <param name="address"></param>
        /// <returns></returns>
        public static  bool Add(string name, MemoryTypes type, out uint address)
        {
            bool result = true;
            uint adr = 0;
            switch (type)
            {
                case MemoryTypes.None:
                    break;
                case MemoryTypes.Bit:
                    if (bit_Index + 1 == BitSize)
                    {
                        result = false;
                    }
                    else
                    {
                        Types.Add(name, type);
                        Address.Add(name, bit_Index);

                        adr = bit_Index;
                        bit_Index++;
                    }
                    break;
                case MemoryTypes.Int8:
                    if (int8_Index + 1 == Int8Size)
                    {
                        result = false;
                    }
                    else
                    {
                        Address.Add(name, int8_Index);
                        Types.Add(name, type);

                        adr = int8_Index;
                        int8_Index++;
                    }
                    break;
                case MemoryTypes.Uint8:
                    if (uint8_Index + 1 == Uint8Size)
                    {
                        result = false;
                    }
                    else
                    {
                        Address.Add(name, uint8_Index);
                        Types.Add(name, type);

                        adr = uint8_Index;
                        uint8_Index++;
                    }
                    break;
                case MemoryTypes.Int16:
                    if (int16_Index + 1 == Int16Size)
                    {
                        result = false;
                    }
                    else
                    {
                        Address.Add(name, int16_Index);
                        Types.Add(name, type);

                        adr = int16_Index;
                        int16_Index++;
                    }
                    break;
                case MemoryTypes.Uint16:
                    if (uint16_Index + 1 == Uint16Size)
                    {
                        result = false;
                    }
                    else
                    {
                        Address.Add(name, uint16_Index);
                        Types.Add(name, type);

                        adr = uint16_Index;
                        uint16_Index++;
                    }
                    break;
                case MemoryTypes.Int32:
                    if (int32_Index + 1 == Int32Size)
                    {
                        result = false;
                    }
                    else
                    {
                        Address.Add(name, int32_Index);
                        Types.Add(name, type);

                        adr = int32_Index;
                        int32_Index++;
                    }
                    break;
                case MemoryTypes.Uint32:
                    if (uint32_Index + 1 == Uint32Size)
                    {
                        result = false;
                    }
                    else
                    {
                        Address.Add(name, uint32_Index);
                        Types.Add(name, type);

                        adr = uint32_Index;
                        uint32_Index++;
                    }
                    break;
                case MemoryTypes.Float:
                    if (float_Index + 1 == FloatSize)
                    {
                        result = false;
                    }
                    else
                    {
                        Address.Add(name, float_Index);
                        Types.Add(name, type);

                        adr = float_Index;
                        float_Index++;
                    }
                    break;
                default:
                    result = false;
                    break;
            }
            address = adr;

            return result;
        }

        /// <summary>
        /// Read value store on memory
        /// </summary>
        /// <param name="address"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static object Read(uint address, MemoryTypes type)
        {
            object value = 0;
            switch (type)
            {
                case MemoryTypes.None:
                    break;
                case MemoryTypes.Bit:
                    if(address < BitSize)
                    {
                        value = memBit[address];
                    }
                    break;
                case MemoryTypes.Int8:
                    if(address < Int8Size)
                    {
                        value = memInt8[address];
                    }
                    break;
                case MemoryTypes.Uint8:
                    if(address < Uint8Size)
                    {
                        value = memUint8[address];
                    }
                    break;
                case MemoryTypes.Int16:
                    if(address < Int16Size)
                    {
                        value = memInt16[address];
                    }
                    break;
                case MemoryTypes.Uint16:
                    if(address < Uint16Size)
                    {
                        value = memUint16[address];
                    }
                    break;
                case MemoryTypes.Int32:
                    if(address < Int32Size)
                    {
                        value = memInt32[address];
                    }
                    break;
                case MemoryTypes.Uint32:
                    if(address < Uint32Size)
                    {
                        value = memUint32[address];
                    }
                    break;
                case MemoryTypes.Float:
                    if(address < FloatSize)
                    {
                        value = memFloat[address];
                    }
                    break;
                default:
                    value = 0;
                    break;
            }
            return value;
        }

        /// <summary>
        /// Write value memory store.
        /// </summary>
        /// <param name="address"></param>
        /// <param name="type"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool Write(uint address, MemoryTypes type, object value)
        {
            bool result = true;
            switch (type)
            {
                case MemoryTypes.None:
                    result = false;
                    break;
                case MemoryTypes.Bit:
                    if(address < BitSize)
                    {
                        memBit[address] = (byte)value;
                    }
                    break;
                case MemoryTypes.Int8:
                    if (address < Int8Size)
                    {
                        memInt8[address] = (sbyte)value;
                    }
                    break;
                case MemoryTypes.Uint8:
                    if (address < Uint8Size)
                    {
                        memUint8[address] = (byte)value;
                    }
                    break;
                case MemoryTypes.Int16:
                    if (address < Int16Size)
                    {
                        memInt16[address] = (short)value;
                    }
                    break;
                case MemoryTypes.Uint16:
                    if (address < Uint16Size)
                    {
                        memUint16[address] = (ushort)value;
                    }
                    break;
                case MemoryTypes.Int32:
                    if (address < Int32Size)
                    {
                        memInt32[address] = (int)value;
                    }
                    break;
                case MemoryTypes.Uint32:
                    if (address < Uint32Size)
                    {
                        memUint32[address] = (uint)value;
                    }
                    break;
                case MemoryTypes.Float:
                    if (address < FloatSize)
                    {
                        memFloat[address] = (float)value;
                    }
                    break;
                default:
                    result = false;
                    break;
            }
            return result;
        }
    }

    public class MemoryType
    {
        public const string STR_BIT = "bit";
        public const string STR_INT8 = "int8";
        public const string STR_UINT8 = "uint8";
        public const string STR_INT16 = "int16";
        public const string STR_UINT16 = "uint16";
        public const string STR_INT32 = "int32";
        public const string STR_UINT32 = "uint32";
        public const string STR_FLOAT = "float";

        /// <summary>
        /// Get list of type name
        /// </summary>
        /// <returns>string[]</returns>
        public static string[] Names()
        {
            return new string[] { STR_BIT, STR_INT8, STR_UINT8, 
                STR_INT16, STR_UINT16, STR_INT32, 
                STR_UINT32, STR_FLOAT };
        }

        public static string ToString(MemoryTypes type)
        {
            string strType = "";
            switch (type)
            {
                case MemoryTypes.None:
                    strType = "None";
                    break;
                case MemoryTypes.Bit:
                    strType = STR_BIT;
                    break;
                case MemoryTypes.Int8:
                    strType = STR_INT8;
                    break;
                case MemoryTypes.Uint8:
                    strType = STR_UINT8;
                    break;
                case MemoryTypes.Int16:
                    strType = STR_INT16;
                    break;
                case MemoryTypes.Uint16:
                    strType = STR_UINT16;
                    break;
                case MemoryTypes.Int32:
                    strType = STR_INT32;
                    break;
                case MemoryTypes.Uint32:
                    strType = STR_UINT32;
                    break;
                case MemoryTypes.Float:
                    strType = STR_FLOAT;
                    break;
                default:
                    break;
            }

            return strType;
        }

        public static MemoryTypes ToType(string strType)
        {
            MemoryTypes type = MemoryTypes.None;

            switch (strType){
                case STR_BIT:
                    type = MemoryTypes.Bit;
                    break;
                case STR_INT8:
                    type = MemoryTypes.Int8;
                    break;
                case STR_UINT8:
                    type = MemoryTypes.Uint8;
                    break;
                case STR_INT16:
                    type = MemoryTypes.Int16;
                    break;
                case STR_UINT16:
                    type = MemoryTypes.Uint16;
                    break;
                case STR_INT32:
                    type = MemoryTypes.Int32;
                    break;
                case STR_UINT32:
                    type = MemoryTypes.Uint32;
                    break;
                case STR_FLOAT:
                    type = MemoryTypes.Float;
                    break;
                default:
                    break;
            }
            return type;
        }
    }

    public enum MemoryTypes
    {
        None,
        Bit,
        Int8,
        Uint8,
        Int16,
        Uint16,
        Int32,
        Uint32,
        Float
    }
}
