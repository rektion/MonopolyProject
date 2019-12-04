﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monopoly
{
    public class Borough
    {
        public int length;
        public List<BuyableCase> cases;

        public Borough(int _length)
        {
            length = _length;
            cases = new List<BuyableCase>();
        }

        public bool Monopoly()
        {
            bool monopoly = true;
            Player p = this.cases[0].Owner;
            if(p != null)
            {
                foreach (BuyableCase bc in this.cases)
                {
                    if (!p.Equals(bc.Owner) || bc.IsMort)
                    {
                        monopoly = false;
                    }
                }
            }
            else
            {
                monopoly = false;
            }
            return monopoly;
            // TODO : Verify if the borough is owned by the same player on all the cases
        }
    }
}
