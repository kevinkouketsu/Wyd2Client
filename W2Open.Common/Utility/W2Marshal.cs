using System;
using System.Runtime.InteropServices;
using WYD2.Common.GameStructure;

namespace WYD2.Common.Utility
{
    /// <summary>
    /// A wrapper encapsulating some core methods used to marshals raw data buffers into structures.
    /// </summary>
    public static class W2Marshal
    {
        /// <summary>
        /// Marshals a raw buffer to a given marshalable struct.
        /// </summary>
        public static unsafe T GetStructure<T>(byte[] buffer, int offset = 0) where T : struct
        {
            fixed (byte* bufferPin = &buffer[offset])
            {
                return GetStructure<T>(bufferPin);
            }
        }

        public static unsafe T GetStructure<T>(CCompoundBuffer buffer) where T : struct
        {
            return GetStructure<T>(buffer.RawBuffer, buffer.Offset);
        }

        public static unsafe T GetStructure<T>(byte* buffer) where T : struct
        {
            return (T)Marshal.PtrToStructure(new IntPtr(buffer), typeof(T));
        }

        /// <summary>
        /// Marshals a given T instance into a raw buffer.
        /// </summary>
        public static unsafe byte[] GetBytes<T>(T obj) 
        {
            byte[] rawBuffer = new byte[Marshal.SizeOf(obj)];

            fixed (byte* rawBufferPin = rawBuffer)
            {
                Marshal.StructureToPtr<T>(obj, new IntPtr(rawBufferPin), true);
            }

            return rawBuffer;
        }

        /// <summary>
        /// Crates a read-to-use marshaled instance of T
        /// </summary>
        /// <typeparam name="T">Type to be marshaled as zero-initialized instance.</typeparam>
        /// <returns>A zero-initialized instance of T.</returns>
        public static unsafe T CreateEmpty<T>() 
        {
            int typeSize = Marshal.SizeOf(typeof(T));

            byte* rawBuffer = stackalloc byte[typeSize];

            for (int i = 0; i < typeSize; i++)
                rawBuffer[i] = 0;
            
            T zeroInited = (T)Marshal.PtrToStructure(new IntPtr(rawBuffer), typeof(T));

            return zeroInited;
        }

        /// <summary>
        /// Creates a empty ready-to-use copy of a given implementation of IGamePacket.
        /// </summary>
        public static T GetEmptyValid<T>(ushort opcode) where T : IGamePacket
        {
            MPacketHeader validHeader = new MPacketHeader();
            T packet = W2Marshal.CreateEmpty<T>();

            // Set the default values to the packet header.
            validHeader.Size = (ushort)Marshal.SizeOf(packet);
            validHeader.Opcode = opcode;
            validHeader.Key = PacketSecurity.GetHashByte();
            validHeader.TimeStamp = PacketSecurity.GetTimeStamp() ;

            packet.Header = validHeader;

            return packet;
        }

        public static void BuildPacketHeader<T>(ref MPacketHeader pHeader, ushort userId)
        {
            pHeader.Size = (ushort)(Marshal.SizeOf(typeof(T)));
            pHeader.ClientId = userId;
            pHeader.Key = PacketSecurity.GetHashByte();
            pHeader.TimeStamp = PacketSecurity.GetTimeStamp();
        }
    }
}