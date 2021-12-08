using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackEnd.Data
{
    public class Constants
    {
        #region Messages
        public const string ExceptionOccoured = "Exception";
        public const string NotValidStartDate = "NotValidStartDate";
        public const string NotValidEndDate = "NotValidEndDate";
        public const string NotValidPeriod = "NotValidPeriod";
        public const string FialedToGetSetting = "FialedToGetSetting";
        public const string NoAvilabeAppointment = "NoAvilabeAppointment";

        #endregion

        #region Settings
        public const string SessionPeriodSetting = "SessionPeriod";
        public const string WaitingPeriodSetting = "WaitingPeriod";
        public const string RestPeriodSetting = "RestPeriod";

        #endregion
    }
}
