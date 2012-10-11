using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WormCShConsole;
using System.IO;

namespace ConsoleApplication1
{
    class DeleteControlBytes
    {
        static string fileName = @"H:\\data.bin";

        static void Main(string[] args)
        {
            using (Stream copyStream = new FileStream(fileName + "copy.bin", FileMode.Append, FileAccess.Write, FileShare.None))
            {
                using (Stream readStream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.None))
                {
                    using (BinaryReader br = new BinaryReader(readStream))
                    {
                        using (BinaryWriter bw = new BinaryWriter(copyStream))
                        {
                            byte[] chunk;
                            byte[] chunkCopy;
                            byte previousPacketNumber;
                            byte currentPacketNumber;
                            byte previousPacketNumber3;
                            byte currentPacketNumber3;

                            int number = 0;
                            int chunkSum = 0;

                            chunk = br.ReadBytes(6);
                            chunkCopy = chunk;
                            bw.Write(chunk, 0, 5);
                            currentPacketNumber = chunk[5];
                            currentPacketNumber3 = chunk[2];
                            while ((chunk = br.ReadBytes(6)).Length > 0)
                            {
                                
                                number++;
                                previousPacketNumber = currentPacketNumber;
                                currentPacketNumber = chunk[chunk.Length - 1];
                                if ((previousPacketNumber + 0x01) != currentPacketNumber)
                                {   
                                    if (currentPacketNumber != 0x00)
                                    {
                                        //Console.WriteLine("number=" + number + 
                                        //    "\r\npreviousPacketNumber=" + previousPacketNumber + 
                                        //    "\r\ncurrentPacketNumber=" + currentPacketNumber);
                                        Console.WriteLine("number={0}, \r\nprevious chunk: {1}, \r\ncurrent chunk: {2}",
                                            number, BitConverter.ToString(chunkCopy), BitConverter.ToString(chunk));

                                    }
                                    else if (currentPacketNumber == 0x00 && (previousPacketNumber != 0xFF))
                                    {
                                        //Console.WriteLine("number=" + number + "\r\npreviousPacketNumber=" + previousPacketNumber + "\r\ncurrentPacketNumber=" + currentPacketNumber);
                                        Console.WriteLine("number={0}, \r\nprevious chunk: {1}, \r\ncurrent chunk: {2}",
    number, BitConverter.ToString(chunkCopy), BitConverter.ToString(chunk));
                                    }
                                        /////////////////////////////////////
    //                                else
    //                                {
    //                                    previousPacketNumber3 = currentPacketNumber3;
    //                                    currentPacketNumber3 = chunk[chunk.Length - 3];
    //                                    if ((previousPacketNumber3 + 0x01) != currentPacketNumber3)
    //                                    {
    //                                        if (currentPacketNumber3 != 0x00)
    //                                        {
    //                                            //Console.WriteLine("number=" + number + "\r\npreviousPacketNumber3=" + previousPacketNumber3 + "\r\ncurrentPacketNumber3=" + currentPacketNumber3);
    //                                            Console.WriteLine("number={0}, \r\nprevious chunk: {1}, \r\ncurrent chunk: {2}",
    //number, BitConverter.ToString(chunkCopy), BitConverter.ToString(chunk));
    //                                        }
    //                                        else if (currentPacketNumber3 == 0x00 && (previousPacketNumber3 != 0xFF))
    //                                        {
    //                                            //Console.WriteLine("number=" + number + "\r\npreviousPacketNumber3=" + previousPacketNumber3 + "\r\ncurrentPacketNumber3=" + currentPacketNumber3);
    //                                            Console.WriteLine("number={0}, \r\nprevious chunk: {1}, \r\ncurrent chunk: {2}",
    //number, BitConverter.ToString(chunkCopy), BitConverter.ToString(chunk));
    //                                        }
    //                                    }
    //                                }
                                    ///////////////////////////////////////////
                                }
                                chunkCopy = chunk;
                                bw.Write(chunk, 0, (chunk.Length - 1));
                            }
                        }
                    }
                }
            }
            Console.WriteLine("Control bytes are deleted");
            while (Console.KeyAvailable == false) { };
        }
    }    
}
