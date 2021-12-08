using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace BackEnd.Core
{
    public class Validators
    {
        ExceptionHandeller EXH;
        CultureInfo provider = CultureInfo.InvariantCulture;

        public Validators(ExceptionHandeller exh)
        {
            EXH = exh;
        }
        public bool IsValidSessionPeriod(string period)
        {
            try
            {
                if(period==null||period == "")
                {
                    return false;
                }
                 int intPeriod = Convert.ToInt32(period);
                return true;
            }
            catch (Exception ex)
            {
                EXH.LogException(ex, MethodBase.GetCurrentMethod().ReflectedType.Name, MethodBase.GetCurrentMethod().Name);
                return false;
            }
        }
        public bool IsValidWaitingPeriod(string period)
        {
            try
            {
                if (period == null || period == "")
                {
                    return false;
                }
                int intPeriod = Convert.ToInt32(period);
                return true;
            }
            catch (Exception ex)
            {
                EXH.LogException(ex, MethodBase.GetCurrentMethod().ReflectedType.Name, MethodBase.GetCurrentMethod().Name);
                return false;
            }
        }

        public bool IsValidRestPeriod(string period)
        {
            try
            {
                if (period == null || period == "")
                {
                    return false;
                }
                int intPeriod = Convert.ToInt32(period);
                return true;
            }
            catch (Exception ex)
            {
                EXH.LogException(ex, MethodBase.GetCurrentMethod().ReflectedType.Name, MethodBase.GetCurrentMethod().Name);
                return false;
            }
        }

        public bool IsDateTime(string stringDate)
        {
            bool result = false;
            try
            {
                DateTime.ParseExact(stringDate, "d-M-yyyy", provider);
                result = true;
            }
            catch (Exception ex)
            {
                EXH.LogException(ex, MethodBase.GetCurrentMethod().ReflectedType.Name, MethodBase.GetCurrentMethod().Name);
                result = false;
            }
            return result;
        }
    }
}
