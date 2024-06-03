using System.Diagnostics.SymbolStore;
using System.IO.Ports;
using static ScsDecoder.Utils;

namespace ScsDecoder
{
    public class SerialInterface
    {
        public static SerialPort port;

        public static byte[] data = new byte[64];
        public static int offset = 0;
        //public static object mutex_lock_read = new object();

        private static Thread threadReadSerial = new Thread(new ThreadStart(ThreadReadSerial));
        private static void ThreadReadSerial()
        {
            while (true)
            {
                /*lock (mutex_lock_read)
                {*/
                    try
                    {
                        int size = port.Read(data, offset, data.Length - offset);
                        if (size > 0)
                            offset += size;
                    }
                    catch
                    {
                        if (offset > 0)
                        {
                            try
                            {
                                int index = 0;
                                while (true)
                                {
                                    //byte packetId = data[index++];
                                    int tmpSize = data[index];

                                    if ((tmpSize & 0x80) > 0)
                                    {
                                        ReciveCommand((byte)(tmpSize & 0x7F));
                                        ++index;
                                    }
                                    else
                                    {
                                        byte[] tmpBuffer = new byte[data[index]];
                                        ++index;

                                        for (int i = 0; i < tmpSize; i++)
                                            tmpBuffer[i] = data[i + index];

                                        if (tmpBuffer.Length > 0)
                                            RecivePacket(tmpBuffer);

                                        index += tmpSize;
                                    }
                                    if (index > offset)
                                        break;
                                }
                            }
                            catch { }

                            Array.Fill(data, (byte)0, 0, offset);
                            offset = 0;
                        }
                    }
                //}
            }
        }

        public static bool isSameBuffer(byte[] a, byte[] b)
        {
            if(a == null || b == null || b.Length != a.Length)
                return false;

            for(int i = 0; i < a.Length; i++)
            {
                if (a[i] != b[i])
                    return false;
            }

            return true;
        }

        private static byte[] bufferCopy = null;
        private static int counts = 1;
        private static void RecivePacket(byte[] buffer)
        {
            if(isSameBuffer(bufferCopy, buffer))
            {
                if (counts > 1)
                    Console.CursorTop--;

                ++counts;
                ConsoleWrite(ConsoleColor.DarkYellow, "^ X");
                ConsoleWrite(ConsoleColor.Gray, $"{counts}\n");
                return;
            }
            counts = 1;
            bufferCopy = new byte[buffer.Length];
            Array.Copy(buffer, bufferCopy, buffer.Length);

            if(buffer.Length == 1 && buffer[0] == 0xA5)
            {
                ConsoleWrite(ConsoleColor.Green, "ACK\n");
            }
            else
            {
                if (buffer[0] == 0xA8 && buffer[buffer.Length-1] == 0xA3) // SCS Packet
                {
                    byte parity = 0;
                    for(int i = 1; i < buffer.Length-2; i++)
                        parity ^= buffer[i];

                    bool goodparity = parity == buffer[buffer.Length - 2];

                    if(buffer.Length == 7)
                    {
                        byte[] subBuffer = new byte[buffer.Length - 3];
                        Array.Copy(buffer, 1, subBuffer, 0, subBuffer.Length);
                        PacketDispatcher.reciveA8A3_SIZE7(subBuffer, goodparity);
                    }
                    else if (buffer.Length == 11 && (buffer[1] & 0xF0) == 0xD0)
                    {
                        byte[] subBuffer = new byte[buffer.Length - 3];
                        Array.Copy(buffer, 1, subBuffer, 0, subBuffer.Length);
                        PacketDispatcher.reciveA8D0A3_SIZE11(subBuffer, goodparity);
                    }
                    else if (buffer.Length == 11 && buffer[1] == 0xEC)
                    {
                        byte[] subBuffer = new byte[buffer.Length - 3];
                        Array.Copy(buffer, 1, subBuffer, 0, subBuffer.Length);
                        PacketDispatcher.reciveA8ECA3_SIZE11(subBuffer, goodparity);
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        foreach (byte b in buffer)
                            Console.Write($"ERR1 0x{b.ToString("X2")}");
                        Console.WriteLine();
                    }
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    foreach (byte b in buffer)
                        Console.Write($"ERR0 0x{b.ToString("X2")}");
                    Console.WriteLine();
                }
            }
        }

        private static void ReciveCommand(byte cmd)
        {
            if(cmd == 0x01)
            {
                ConsoleWrite(ConsoleColor.DarkMagenta, "SEND AVALEBLES !\n");
            }
        }

        public static void SendData(byte[] _buffer, int rep)
        {
            byte[] sendBuffer = new byte[_buffer.Length+2];
            
            sendBuffer[0] = (byte)_buffer.Length;
            sendBuffer[1] = (byte)rep;

            for (int i = 0; i < _buffer.Length; i++)
            {
                sendBuffer[i + 2] = _buffer[i];
            }
            port.Write(sendBuffer, 0, sendBuffer.Length);
        }

        public static byte[] WrapPacketA8A3(byte[] _buffer)
        {
            byte[] sendBuffer = new byte[_buffer.Length + 3];

            sendBuffer[sendBuffer.Length - 2] = 0;
            sendBuffer[0] = 0xA8;
            sendBuffer[sendBuffer.Length - 1] = 0xA3;

            for (int i = 0; i < _buffer.Length; i++)
            {
                sendBuffer[i + 1] = _buffer[i];
                sendBuffer[sendBuffer.Length - 2] ^= _buffer[i];
            }

            return sendBuffer;
        }

        public static void Run()
        {
            port = new SerialPort("COM4")
            {
                ReadTimeout = 2
                //ReadBufferSize = 256
            };
            port.Open();
            port.DiscardInBuffer();

            threadReadSerial.Start();

        }
    }
}
