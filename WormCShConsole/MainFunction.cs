using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FTD2XX_NET;
using System.Threading;
using System.IO;

namespace WormCShConsole
{
    class MainFunction
    {
        static string fileName = "H:\\data.bin";

        static void Main(string[] args)
        {
            string deviceSerialNumber = "33VRWQARA";
            FTDI.FT_STATUS status = new FTDI.FT_STATUS();
            FTDI device = new FTDI();
            UInt32 numberOfDevices = 0;
            int sleepTime = 100;

            status = device.GetNumberOfDevices(ref numberOfDevices);
            FTDI.FT_DEVICE_INFO_NODE[] devicelist = new FTDI.FT_DEVICE_INFO_NODE[numberOfDevices];
            status = device.GetDeviceList(devicelist);
            Thread.Sleep(sleepTime);
            if (status == FTDI.FT_STATUS.FT_OK)
                Console.WriteLine("We have {0} devices", numberOfDevices);
            else
                Console.WriteLine("Failed to get number of devices");


            status = device.OpenBySerialNumber(deviceSerialNumber);
            Thread.Sleep(sleepTime);
            if(status == FTDI.FT_STATUS.FT_OK)
                Console.WriteLine("Device {0} is opened", deviceSerialNumber);
            else
                Console.WriteLine("Failed to open {0} device", deviceSerialNumber);

            status = device.SetBitMode(0xff, FTDI.FT_BIT_MODES.FT_BIT_MODE_RESET);
            Thread.Sleep(sleepTime);
            if (status == FTDI.FT_STATUS.FT_OK)
                Console.WriteLine("BitMode is resetted");
            else
                Console.WriteLine("Failed to reset BitMode");

            status = device.SetBitMode(0xff, FTDI.FT_BIT_MODES.FT_BIT_MODE_SYNC_FIFO);
            Thread.Sleep(sleepTime);
            if (status == FTDI.FT_STATUS.FT_OK)
                Console.WriteLine("BitMode is {0}", FTDI.FT_BIT_MODES.FT_BIT_MODE_SYNC_FIFO);
            else
                Console.WriteLine("Failed to set BitMode");

            byte latency = 2;
            device.SetLatency(latency);
            Thread.Sleep(sleepTime);
            if (status == FTDI.FT_STATUS.FT_OK)
                Console.WriteLine("Latency timer value is {0}", latency);
            else
                Console.WriteLine("Failed to set latency");

            uint inTransferSize = 0x10000;
            device.InTransferSize(inTransferSize);
            Thread.Sleep(sleepTime);
            if (status == FTDI.FT_STATUS.FT_OK)
                Console.WriteLine("inTransferSize value is {0}", inTransferSize);
            else
                Console.WriteLine("Failed to set inTransferSize");

            device.SetFlowControl(FTDI.FT_FLOW_CONTROL.FT_FLOW_RTS_CTS, 0x00, 0x00);
            Thread.Sleep(sleepTime);
            if (status == FTDI.FT_STATUS.FT_OK)
                Console.WriteLine("FlowControl is {0}", FTDI.FT_FLOW_CONTROL.FT_FLOW_RTS_CTS);
            else
                Console.WriteLine("Failed to set FlowControl");

            device.Purge(FTDI.FT_PURGE.FT_PURGE_RX);

            uint numBytes = 0;
            //int cycles = 0;
            using(FileStream fs = new FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                using (BinaryWriter bw = new BinaryWriter(fs))
                {
                    while (Console.KeyAvailable == false)
                    {                       
                        device.GetRxBytesAvailable(ref numBytes);
                        if (numBytes >= 1)
                        {
                            //cycles++;
                            byte[] bBuf = new byte[numBytes];
                            device.Read(bBuf, numBytes, ref numBytes);
                            bw.Write(bBuf);

                            //if (cycles == 1)
                            //{
                            //    cycles = 0;
                            //    Console.WriteLine("{0}", bBuf.Length);
                            //}
                        }
                        if (numBytes >= 0x10000)
                        {
                            Console.WriteLine("Buffer overload!");
                        }                     
                    }
                    
                    //Console.WriteLine("Press 'p' to erase control bytes, 'q' to quite");
                    //ConsoleKeyInfo cki = Console.ReadKey(false);
                    //if (cki.Key == ConsoleKey.Q)
                    //    Environment.Exit(-1);
                    //if (cki.Key == ConsoleKey.P)
                    //{
                        
                    //}
                }
            }
            Console.WriteLine("Key is pressed, end of file writting");
        }
    }
}
