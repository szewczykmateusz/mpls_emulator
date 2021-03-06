﻿using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Globalization;


namespace Management_System
{
    public class UDPSocket
    {
        private Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        private const int bufSize = 8 * 1024;
        private State state = new State();
        private EndPoint epFrom = new IPEndPoint(IPAddress.Any, 0);
        private AsyncCallback recv = null;
        private int portNumber;
        private String timeStamp = time.GetTimestamp(DateTime.Now);
        private DateTime dt = DateTime.Now;
        private static Time time = new Time();

        public class State
        {
            public byte[] buffer = new byte[bufSize];
        }
        public UDPSocket(int p) { portNumber = p; }
        public int GetPort() { return portNumber; }

        public void RunServer(string serverAddress, int portNumber)
        {
            socket.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.ReuseAddress, true);
            socket.Bind(new IPEndPoint(IPAddress.Parse(serverAddress), portNumber));
            Console.WriteLine(time.GetTimestamp(DateTime.Now) + " Created UDPServer at: " + serverAddress + ":" + portNumber);
            Receive();
        }

        public void Connect(string address, int port)
        {
            socket.Connect(IPAddress.Parse(address), port);
            Receive();
        }
        public void Server(string address, int port)
        {
            socket.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.ReuseAddress, true);
            socket.Bind(new IPEndPoint(IPAddress.Parse(address), portNumber));
            Console.WriteLine(time.GetTimestamp(DateTime.Now) + " Created UDPServer at: " + address + ":" + portNumber);
            Receive();
        }
        public void Client(string address, int port)
        {
            Console.WriteLine(time.GetTimestamp(DateTime.Now) + " Created UDPClient at: " + address + ":" + portNumber);
            socket.Connect(address, portNumber);
            Receive();
        }


        public void Send(string text)
        {
            byte[] data = Encoding.ASCII.GetBytes(text);
            socket.BeginSend(data, 0, data.Length, SocketFlags.None, (ar) =>
            {
                State so = (State)ar.AsyncState;
                int bytes = socket.EndSend(ar);
                Console.WriteLine(time.GetTimestamp(DateTime.Now) + " SEND: bytes: [{0}], message: [{1}]", bytes, text);
            }, state);
        }

        private void Receive()
        {
            string message = "";
            socket.BeginReceiveFrom(state.buffer, 0, bufSize, SocketFlags.None, ref epFrom, recv = (ar) =>
            {
                try
                {
                    State so = (State)ar.AsyncState;
                    int bytes = socket.EndReceiveFrom(ar, ref epFrom);
                    socket.BeginReceiveFrom(so.buffer, 0, bufSize, SocketFlags.None, ref epFrom, recv, so);
                    Console.WriteLine(time.GetTimestamp(DateTime.Now) + " RECV: from: [{0}]: bytes: [{1}]", epFrom.ToString(), bytes);
                    message = Encoding.ASCII.GetString(so.buffer, 0, bytes);
                    Console.WriteLine(time.GetTimestamp(DateTime.Now) + " RCV MESSAGE: " + message);
                    ManagementSystem.ProcessRequest(message);
                }
                catch(Exception e)
                {
                    timeStamp = time.GetTimestamp(DateTime.Now);
                    Console.WriteLine(time.GetTimestamp(DateTime.Now) + " Nie mozna nawiazac polaczenia");
                }
            }, state);
        }

    }
}
