using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Collections;

namespace DeleteZeros
{
    class DeleteZeros
    {
        static string fileName = "D:\\im1.bin";

        static void Main(string[] args)
        {
            using (Stream copyStream = new FileStream(fileName + "copy1.bin", FileMode.Append, FileAccess.Write, FileShare.None))
            {
                using (Stream readStream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.None))
                {
                    using (BinaryReader br = new BinaryReader(readStream))
                    {
                        using (BinaryWriter bw = new BinaryWriter(copyStream))
                        {
                            //seek for first non-zero position
                            long position;
                            position = FirstNonZeroBytePosition(br);
                            br.BaseStream.Seek(position, SeekOrigin.Begin);
  
                            long chunkCounter = 0;
                            int chunkLenght = 0x100000;
                            byte[] chunk;
                            byte[] chunkOut;
                            BitArray chunkBitsIn;
                            BitArray chunkBitsOut;
                            BitArray chunkBuffer = null;
                            BitArray rawData;

                            //delete zeros in first non-zero byte
                            chunkBuffer = DeleteZerosInFirstByte(br, chunkBuffer);

                            //begin loop
                            while ((chunk = br.ReadBytes(chunkLenght)).Length > 0)
                            {
                                //interators
                                int inIter = 0;
                                int outIter = 0;
                                int zeroCount = 0;

                                rawData = new BitArray(chunk);
                                chunkBitsIn = new BitArray(chunkBuffer);

                                chunkBitsIn.Length += rawData.Length;
                                for (int i = chunkBuffer.Length; i < chunkBitsIn.Length; i++)
                                {
                                    chunkBitsIn[i] = rawData[i - chunkBuffer.Length];
                                }
                                chunkBitsOut = new BitArray(chunkBitsIn.Length, false);

                                while (inIter < chunkBitsIn.Length)
                                {
                                    if (chunkBitsIn[inIter])
                                    {
                                        chunkBitsOut[outIter] = true;
                                        outIter++;
                                        inIter++;
                                    }
                                    else
                                    {
                                        zeroCount = 1;
                                        while ((inIter + zeroCount) < chunkBitsIn.Length && !chunkBitsIn[inIter + zeroCount])
                                            zeroCount++;

                                        if (zeroCount <= 12 && (inIter + zeroCount) < chunkBitsIn.Length)
                                            outIter += zeroCount;
                                        if (zeroCount > 0x100)
                                            Console.WriteLine("big zero, {0} mb", chunkCounter);
                                        inIter += zeroCount;
                                    }
                                }
                                chunkBitsOut.Length = outIter;

                                //add Chunk Buffer
                                int lenghtToCopy = chunkBitsOut.Length / 8 * 8;
                                chunkBuffer = new BitArray(chunkBitsOut.Length - lenghtToCopy);

                                for (int i = 0; i < chunkBuffer.Length; i++)
                                    chunkBuffer[i] = chunkBitsOut[i + lenghtToCopy];

                                chunkBitsOut.Length = lenghtToCopy;
                                chunkOut = new byte[lenghtToCopy / 8];
                                chunkBitsOut.CopyTo(chunkOut, 0);
                                bw.Write(chunkOut);

                                chunkCounter++;
                                Console.WriteLine("{0} Mb is read", chunkCounter);
                            }

                            chunkOut = new byte[1];
                            chunkBuffer.CopyTo(chunkOut, 0);
                            bw.Write(chunkOut);                           
                        }
                    }
                }
            }
            Console.WriteLine("End of file is riched!Zere aro no zeros any more!");
            while (Console.KeyAvailable == false) { };
        }



        private static BitArray DeleteZerosInFirstByte(BinaryReader br, BitArray chunkBuffer)
        {
            byte[] b = { br.ReadByte() };
            BitArray ba = new BitArray(b);
            //DisplayBits(ba);
            //while(Console.KeyAvailable == false){}
            int baZeroCount = 0;
            while (baZeroCount<ba.Length && !ba[baZeroCount])
                baZeroCount++;
            chunkBuffer = new BitArray(ba.Length - baZeroCount);
            for (int i = baZeroCount; i < ba.Length; i++)
                chunkBuffer[i - baZeroCount] = ba[i];
            return chunkBuffer;
        }

        private static void DisplayBits(BitArray chunkBitsOut)
        {
            foreach (bool bit in chunkBitsOut)
            {
                Console.Write(bit ? 1 : 0);
            }
            Console.WriteLine();
        }

        private static long FirstNonZeroBytePosition(BinaryReader br)
        {
            int chunkLenght = 0x10000;
            byte[] chunk = new byte[chunkLenght];

            while ((chunk = br.ReadBytes(chunkLenght)).Length > 0)
            {
                for (int i = 0; i < chunk.Length; i++)
                {
                    if (chunk[i] != 0)
                    {
                        byte b = chunk[i];
                        return br.BaseStream.Position - chunk.Length + i;
                    }
                }
            }

            return br.BaseStream.Position;
        }
    }
}
