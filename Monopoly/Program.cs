﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monopoly
{
    class Program
    {
        static void Main(string[] args)
        {
            Board board = Board.Instance;

            Board.players[0].Forward(5);
            Console.ReadKey();
        }
    }
}
