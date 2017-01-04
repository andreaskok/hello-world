using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPDomain.Helpers
{
    public static class CommonUtility
    {
        public static string Null2Empty(object oVal)
        {
            string sResult = "";
            if (oVal != null && oVal != "")
            {
                sResult = (string)oVal;
            }
            return sResult;
        }

        public static decimal Null2Zero(object oVal)
        {
            decimal dResult = 0;
            if (oVal != null && oVal != "")
            {
                dResult = decimal.Parse(oVal.ToString());
            }
            return dResult;
        }

        public static long Null2LongZero(object oVal)
        {
            long dResult = 0;
            if (oVal != null && oVal != "")
            {
                dResult = (long)oVal;
            }
            return dResult;
        }

        public static int Null2Int(object oVal)
        {
            int dResult = 0;
            if (oVal != null && oVal != "")
            {
                dResult = (int)oVal;
            }
            return dResult;
        }

        public static string Empty2Zero(string oVal)
        {
            if (oVal == "")
            {
                return "0";
            }
            else
            {
                return oVal;
            }

        }

        
    }
}
