/*
 * Hypercube Broadcast
 * 
 * Alfred Li
 */
using System;
using System.Linq;
using System.Collections.Generic;

namespace Broadcast
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            var nodes = CreateNodes(int.Parse(args[0]));
            Broadcast(nodes, int.Parse(args[1]));
            PrintMsg(nodes);
        }
        
        /**
         * Create the amount of node in hypercube
         */
        private static List<Node> CreateNodes(int num)
        {
            var nodes = new List<Node>();
            for (var i = 0; i < num; i++)
            {
                nodes.Add(new Node(i));
            }
            return nodes;
        }

        /**
         * Broadcast message from the given node to every other node
         */
        private static void Broadcast(IReadOnlyList<Node> nodes, int startingNode)
        {
            nodes[startingNode].MsgRecv = true;
            for (var fold = 0; fold < Math.Log2(nodes.Count); fold++)
            {
                var send = nodes.Where(n => n.MsgRecv).ToList();
                foreach (var n in send)
                {
                    var bit = (int) Math.Pow(2, fold);
                    // Check the fold bit
                    var x = n.Name;
                    var y = x >> fold;
                    var z = (y & 1) == 1 ? x ^ bit : x | bit;
                    nodes[x].Msg += "Send " + z + " ";
                    nodes[z].Msg += "Recv " + x + " ";
                    nodes[z].MsgRecv = true;
                }
            }
        }

        /**
         * Output message in every node
         */
        private static void PrintMsg(List<Node> nodes)
        {
            foreach (var n in nodes)
            {
                Console.WriteLine("{0}: {1}", n.Name, n.Msg);
            }
        }
    }

    /**
     * Class represent a single node
     */
    public class Node
    {
        public readonly int Name;
        public string Msg;
        public bool MsgRecv;

        public Node(int name)
        {
            Name = name;
        }
    }
}