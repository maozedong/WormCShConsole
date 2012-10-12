using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

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
                            long position = 0;
                            position = FirstNonZeroBytePosition(br);

                            br.BaseStream.Seek(position, SeekOrigin.Begin);

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



                            //List<byte> bList = new List<byte>();
                            //for (int i = 0; i < chunk.Length; i++)
                            //{
                            //    if (chunk[i] != 0x00)
                            //        bList.Add(chunk[i]);
                            //}
                            //var bChunk = bList.ToArray();
                            //bw.Write(bChunk, 0, bChunk.Length);

                            //while (br.BaseStream.Position < br.BaseStream.Length)
                            //{
                            //    sum = 0;
                            //    //chunkCopy = chunk;
                            //    chunk = br.ReadBytes(chunkLenght);
                            //    foreach (byte b in chunk)
                            //    {
                            //        sum += b;
                            //    }
                            //    if (sum != 0x00)
                            //    {
                            //        bw.Write(chunk, 0, chunk.Length);
                            //    }
                            //    else
                            //    {
                            //        List<byte[]> bArrayList = new List<byte[]>();

                            //        while ((sum == 0x00) && (br.BaseStream.Position < br.BaseStream.Length))
                            //        {
                            //            bArrayList.Add(chunk);
                            //            chunk = br.ReadBytes(chunkLenght);
                            //            foreach (byte b in chunk)
                            //            {
                            //                sum += b;
                            //            }
                            //        }
                            //        if (br.BaseStream.Position < br.BaseStream.Length)
                            //        {
                            //            bArrayList.Add(chunk);
                            //            foreach (var b in bArrayList)
                            //            {
                            //                bw.Write(b, 0, b.Length);
                            //            }
                            //        }
                            //        //else
                            //        //{
                            //        //    List<byte> bList2 = new List<byte>();
                            //        //    for (int i = 0; i < chunkCopy.Length; i++)
                            //        //    {
                            //        //        if (chunkCopy[i] != 0x00)
                            //        //            bList.Add(chunkCopy[i]);
                            //        //    }
                            //        //    var bChunk2 = bList2.ToArray();
                            //        //    bw.Write(bChunk2, 0, bChunk2.Length);
                            //        //}                
                            //    }
                            //}
                            Console.WriteLine("End of file is riched!Zere aro no zeros any more!");
                            while (Console.KeyAvailable == false) { };
                        }
                    }
                }
            }
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
