using System;
using System.Collections.Generic;
using System.IO;

namespace new_chart_delections.gui_new
{

    public static class Type
    {
        public const string BIT = "bit";
        public const string INT8 = "int8";
        public const string UINT8 = "uint8";
        public const string INT16 = "int16";
        public const string UINT16 = "uint16";
        public const string INT32 = "int32";
        public const string UINT32 = "uint32";
        public const string FLOAT = "float";

        public static string[] List()
        {
            string[] strList = new string[8];
            strList[0] = BIT;
            strList[1] = INT8;
            strList[2] = UINT8;
            strList[3] = INT16;
            strList[4] = UINT16;
            strList[5] = INT32;
            strList[6] = UINT32;
            strList[7] = FLOAT;
            return strList;
        }
    }

    public class MemoryManage
    {
        const int DefaultRegisterSize = 128;

        public int BitSize { get; set; } = DefaultRegisterSize;  // default 128 bit
        public int Int8Size { get; set; } = DefaultRegisterSize;    // 128 byte
        public int UInt8Size { get; set; } = DefaultRegisterSize;
        public int Int16Size { get; set; } = DefaultRegisterSize;
        public int UInt16Size { get; set; } = DefaultRegisterSize;
        public int Int32Size { get; set; } = DefaultRegisterSize;
        public int UInt32Size { get; set; } = DefaultRegisterSize;
        public int FloatSize { get; set; } = DefaultRegisterSize;

        byte[] memBit;
        sbyte[] memInt8;
        byte[] memUInt8;
        short[] memInt16;
        ushort[] memUInt16;
        int[] memInt32;
        uint[] memUInt32;
        float[] memFloat;

        public int LevelBit { get; set; }
        public int LevelInt8 { get; set; }
        public int LevelUInt8 { get; set; }
        public int LevelInt16 { get; set; }
        public int LevelUInt16 { get; set; }
        public int LevelInt32 { get; set; }
        public int LevelUInt32 { get; set; }
        public int LevelFloat { get; set; }

        public Dictionary<string, int> Address { get; set; }    // key:address
        public Dictionary<string, string> ValueType { get; set; }    // key:type

        public MemoryManage()
        {
            memBit = new byte[BitSize];
            memInt8 = new sbyte[Int8Size];
            memUInt8 = new byte[UInt8Size];
            memInt16 = new short[Int16Size];
            memUInt16 = new ushort[UInt16Size];
            memInt32 = new int[Int32Size];
            memUInt32 = new uint[UInt32Size];
            memFloat = new float[FloatSize];

            LevelBit = BitSize;
            LevelInt8 = BitSize + Int8Size;
            LevelUInt8 = LevelInt8 + UInt8Size;
            LevelInt16 = LevelUInt8 + Int16Size;
            LevelUInt16 = LevelInt16 + UInt16Size;
            LevelInt32 = LevelUInt16 + Int32Size;
            LevelUInt32 = LevelInt32 + UInt32Size;
            LevelFloat = LevelUInt32 + FloatSize;

            Address = new Dictionary<string, int>();
            ValueType = new Dictionary<string, string>();

            address = InitRegister();
        }

        public void Write(int address, object value)
        {
            if (address < LevelBit)
            {
                memBit[address] = (byte)value;
            }
            else if (address >= LevelBit && address < LevelInt8)
            {
                memInt8[address - LevelBit] = (sbyte)value;
            }
            else if (address >= LevelInt8 && address < LevelUInt8)
            {
                memUInt8[address - LevelBit] = (byte)value;
            }
            else if (address >= LevelUInt8 && address < LevelInt16)
            {
                memInt16[address - LevelUInt8] = (short)value;
            }
            else if (address >= LevelInt16 && address < LevelUInt16)
            {
                memUInt16[address - LevelInt16] = (ushort)value;
            }
            else if (address >= LevelUInt16 && address < LevelInt32)
            {
                memInt32[address - LevelUInt16] = (int)value;
            }
            else if (address >= LevelInt32 && address < LevelUInt32)
            {
                memUInt32[address - LevelInt32] = (uint)value;
            }
            else if (address >= LevelUInt32 && address < LevelFloat)
            {
                memFloat[address - LevelUInt32] = (float)value;
            }
            return;
        }

        public object Read(int address)
        {
            if (address < LevelBit)
            {
                return memBit[address];
            }
            else if (address >= LevelBit && address < LevelInt8)
            {
                return memInt8[address - LevelBit];
            }
            else if (address >= LevelInt8 && address < LevelUInt8)
            {
                return memUInt8[address - LevelInt8];
            }
            else if (address >= LevelUInt8 && address < LevelInt16)
            {
                return memInt16[address - LevelUInt8];
            }
            else if (address >= LevelInt16 && address < LevelUInt16)
            {
                return memUInt16[address - LevelInt16];
            }
            else if (address >= LevelUInt16 && address < LevelInt32)
            {
                return memInt32[address - LevelUInt16];
            }
            else if (address >= LevelInt32 && address < LevelUInt32)
            {
                return memUInt32[address - LevelInt32];
            }
            else if (address >= LevelUInt32 && address < LevelFloat)
            {
                return memFloat[address - LevelUInt32];
            }
            return 0;
        }

        public RegisterAddress InitRegister()
        {
            return new RegisterAddress()
            {
                Bit = 0,
                Int8 = LevelBit,
                UInt8 = LevelInt8,
                Int16 = LevelUInt8,
                Uint16 = LevelInt16,
                Int32 = LevelUInt16,
                UInt32 = LevelInt32,
                Float = LevelUInt32
            };
        }

        public void Clear()
        {
            Address.Clear();
            ValueType.Clear();
            address = InitRegister();
        }

        RegisterAddress address;
        public bool Add(string key, string type)
        {
            switch (type)
            {
                case Type.BIT:
                    Address.Add(key, address.Bit);
                    ValueType.Add(key, type);
                    address.Bit++;
                    break;
                case Type.INT8:
                    Address.Add(key, address.Int8);
                    ValueType.Add(key, type);
                    address.Int8++;
                    break;
                case Type.UINT8:
                    Address.Add(key, address.UInt8);
                    ValueType.Add(key, type);
                    address.UInt8++;
                    break;
                case Type.INT16:
                    Address.Add(key, address.Int16);
                    ValueType.Add(key, type);
                    address.Int16++;
                    break;
                case Type.UINT16:
                    Address.Add(key, address.Uint16);
                    ValueType.Add(key, type);
                    address.Uint16++;
                    break;
                case Type.INT32:
                    Address.Add(key, address.Int32);
                    ValueType.Add(key, type);
                    address.Int32++;
                    break;
                case Type.UINT32:
                    Address.Add(key, address.UInt32);
                    ValueType.Add(key, type);
                    address.UInt32++;
                    break;
                case Type.FLOAT:
                    Address.Add(key, address.Float);
                    ValueType.Add(key, type);
                    address.Float++;
                    break;
                default:
                    break;
            }
            return true;
        }


        string fileName = "config.cfg";
        public void Save()
        {
            string data = "";
            foreach(KeyValuePair<string, string> type in ValueType)
            {
                data += type.Key + ":" + type.Value + Environment.NewLine;
            }

            using (StreamWriter sw = new StreamWriter(fileName))
            {
                sw.Write(data);
            }
        }

        public bool Load()
        {
            if (!File.Exists(fileName))
                return false;

            using (StreamReader sr = new StreamReader(fileName))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    if (line == "")
                        continue;
                    string[] dic = line.Split(':');

                    if (dic.Length != 2)
                        continue;

                    try
                    {
                        Add(dic[0], dic[1]);
                    }
                    catch
                    {
                        continue;
                    }
                }

                return true;
            }
        }
    }

    public class RegisterAddress
    {
        public int Bit { get; set; }
        public int Int8 { get; set; }
        public int UInt8 { get; set; }
        public int Int16 { get; set; }
        public int Uint16 { get; set; }
        public int Int32 { get; set; }
        public int UInt32 { get; set; }
        public int Float { get; set; }
    }
}