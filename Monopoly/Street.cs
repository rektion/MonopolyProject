﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monopoly
{
    class Street : BuyableCase
    {

        public Street(string _name, uint _buyPrice, uint _mortgagePrice, Borough _borough)
        {
            this.Name = _name;
            this.BuyPrice = _buyPrice;
            this.Owner = null;
            this.MortgagePrice = _mortgagePrice;
            this.Borough = _borough;
            this.HousePrice = _borough.housePrice;
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

        public override uint Rent()
        {
            uint rent = this.BuyPrice / 20;
            if (this.Borough.Monopoly() && (this.Houses + this.Hotel) == 0)
            {
                rent = rent * 2;
            }
            if(this.Houses >= 1)
            {
                rent = rent * 5;
            }
            if(this.Houses >= 2)
            {
                rent = rent * 3;
            }
            if(this.Houses >= 3)
            {
                rent = rent * 3;
            }
            if(this.Houses == 4)
            {
                rent = rent * 2;
            }
            if(this.Hotel == 1)
            {
                rent = rent * 135;
            }
            return rent;
        }
    }
}
