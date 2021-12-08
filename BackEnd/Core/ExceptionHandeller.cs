using System;
using System.Collections.Generic;
using BackEnd.Data;
using System.Threading.Tasks;

namespace BackEnd.Core
{
    public class ExceptionHandeller
    {
        ApplicationDBContext _db;
        public ExceptionHandeller(ApplicationDBContext db)
        {
            _db = db;
        }
        public void LogException(Exception exception, string className = "", string methodName = "")
        {
            try
            {
                Data.ExceptionLog exceptionLog = new ExceptionLog
                {
                    ClassName = className,
                    MethodName = methodName,
                    Message = exception?.Message,
                    DateTime = DateTime.Now,
                    InnerException = exception.InnerException==null?"": exception.InnerException.Message,
                    StackTrace = exception?.StackTrace
                };
                //_unitOfWork.ExceptionLogs.Add(new Data.Entities.ExceptionLog() { });
                //_unitOfWork.Complete();
                _db.Add<ExceptionLog>(exceptionLog);
                _db.SaveChanges();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
