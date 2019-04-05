﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RouterV1
{
    class MyMain
    {
        static void Main(string[] args)
        {
            // host1 <--> sendingRouter <--> midRouter <--> receivingRouter <--> host2
            Router sendingRouter = new Router("sendingRouter");
            UDPSocket socket0 = new UDPSocket();
            socket0.Server(Utils.destinationIP, 26999, sendingRouter);
            RoutingLine line1 = new RoutingLine(27000, "host2");
            sendingRouter.AddRoutingLine(line1);
            sendingRouter.AddReceivingSocket(socket0);

            Router midRouter = new Router("midRouter");
            UDPSocket socket = new UDPSocket();
            socket.Server(Utils.destinationIP, 27000, midRouter);
            RoutingLine line2 = new RoutingLine(27001, "host2");
            midRouter.AddReceivingSocket(socket);
            midRouter.AddRoutingLine(line2);

            Router receivingRouter = new Router("receivingRouter");
            UDPSocket socket2 = new UDPSocket();
            socket2.Server(Utils.destinationIP, 27001, receivingRouter);
            RoutingLine line3 = new RoutingLine(27002, "host2");
            receivingRouter.AddRoutingLine(line3);
            receivingRouter.AddReceivingSocket(socket2);
            

            //for (int i = 0; i < 100; i++)
            //    socket1.Send(i.ToString());
            //sendingRouter.SendPacket("host2;tresc wiadomosci.......sasgg", 27000);
            
            
            Console.ReadLine();
            Console.WriteLine("Host: " + midRouter.GetDestinationHost());
            Console.WriteLine(midRouter.GetName());
            

            Console.ReadLine();
            Console.WriteLine("Host: " + receivingRouter.GetDestinationHost());
            Console.WriteLine(receivingRouter.GetName());
            

            Console.WriteLine("Odebrano wiadomosci socket: " + socket.counter);
            Console.ReadKey();

            Console.WriteLine("Odebrano wiadomosci socket2: " + socket2.counter);
            Console.ReadKey();


        }
    }
}

