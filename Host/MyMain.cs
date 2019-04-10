﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Host
{
    class MyMain
    {
        public static void Main(string[] args)
        {
            string userInput;
            UDPSocket clientSocket = new UDPSocket();
            Host host1 = new Host("host1", 26999, 29002);
            //Host host2 = new Host("host2", 1000, 10001);
            MPLSLine mpls1 = new MPLSLine("host3", 1);
            host1.AddRoutingLineMPLS(mpls1);
            NHLFELine nhlfe1 = new NHLFELine(1, 17, 0);
            ILMLine ilm1 = new ILMLine("31", "host1");
            host1.AddNHLFELine(nhlfe1);
            host1.AddILMLIne(ilm1);
            clientSocket.Client("127.0.0.1", 27001, host1);
            host1.SendPacket("host3", "wiadomosc testowa");
            Console.ReadLine();
            //while (true)
            //{
            //    string command;
            //    String[] proccessedCommand;
            //    Console.Write(host1.getName() + "> ");
            //    command = Console.ReadLine();
            //    if(command == "?")
            //    {
            //        BasicTerminal.GetHelp();
            //    }
            //    else
            //    {
            //        proccessedCommand = command.Split(' ');
            //        if(proccessedCommand.Length == 3)
            //        {
            //            if(proccessedCommand[0] == "send")
            //            clientSocket.Send(command);
                        
            //        }
            //    }
            //    //clientSocket.Send(msg);
            //}
        }
    }
}
