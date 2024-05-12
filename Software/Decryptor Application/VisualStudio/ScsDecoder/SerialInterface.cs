using System.Diagnostics.SymbolStore;
using System.IO.Ports;

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

        public static void ConsoleWrite(ConsoleColor color, string msg)
        {
            ConsoleColor old = Console.ForegroundColor = color;
            Console.Write(msg);
            Console.ForegroundColor = old;
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
