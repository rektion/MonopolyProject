﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monopoly
{
    class Station : BuyableCase
    {
        public Station(string _name, Borough _borough)
        {
            this.Name = _name;
            this.BuyPrice = 20000;
            this.Owner = null;
            this.MortgagePrice = (uint)10000;
            this.Borough = _borough;
        }

        override public void Effect(Player p)
        {
            if (this.Owner == null)
            {
                Board.PurchaseProposal(p, this);
            }
            else
            {
                if (this.Owner.Equals(p))
                {
                    return;
                }
                else
                {
                    Console.WriteLine("Le montant du loyer est : {0}", this.Rent());
                    this.Owner.Money = (int)(this.Owner.Money + this.Rent());
                    p.Taxe((int)this.Rent());
                }
            }
            Console.ReadKey();
        }

        override public uint Rent()
        {
            uint rent = 0;
            foreach(Station station in this.Borough.cases)
            {
                if (this.Owner.Equals(station.Owner))
                {
                    if(rent == 0)
                    {
                        rent = 2500;
                    }
                    rent = rent * 2;
                }
            }
            return rent;
        }
    }
}
