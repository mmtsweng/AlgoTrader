using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgoTraderDAL
{
    public class ErrorLogger
    {
        private static readonly Lazy<ErrorLogger> lazy = new Lazy<ErrorLogger>(() => new ErrorLogger());
        public static ErrorLogger Instance { get { return lazy.Value; } }

        public void LogException(string source, Exception ex)
        {
            try
            {
                using (Entities entities = new Entities())
                {
                    ErrorLog log = new ErrorLog()
                    {
                        Source = source,
                        StackTrace = ex.StackTrace,
                        Message = ex.Message,
                        LogTime = DateTime.Now
                    };
                    entities.ErrorLogs.Add(log);
                    entities.SaveChanges();
                }
            }
            catch (Exception)
            {
                //Do nothing
            }
        }
    }
}
