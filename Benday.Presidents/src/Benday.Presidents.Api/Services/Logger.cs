using Benday.Presidents.Api.DataAccess;
using Benday.Presidents.Api.Interfaces;
using Benday.Presidents.Common;
using System;
using System.Linq;

namespace Benday.Presidents.Api.Services
{
    public class Logger : ILogger
    {
        private IPresidentsDbContext _DatabaseContext;
        private IFeatureManager _FeatureManager;
        private IUsernameProvider _UsernameProvider;


        public Logger(IUsernameProvider usernameProvider, IPresidentsDbContext databaseContext, IFeatureManager featureManager)
        {
            if (featureManager == null)
                throw new ArgumentNullException("featureManager", "featureManager is null.");
            if (databaseContext == null)
                throw new ArgumentNullException("databaseContext", "databaseContext is null.");

            _DatabaseContext = databaseContext;
            _FeatureManager = featureManager;
            _UsernameProvider = usernameProvider;
        }

        public void LogCustomerSatisfaction(string feedback)
        {
            if (_FeatureManager.CustomerSatisfaction == false)
            {
                return;
            }

            var entry = GetPopulatedLogEntry();

            entry.LogType = "CustomerSatisfaction";
            entry.FeatureName = String.Empty;
            entry.Message = feedback;

            _DatabaseContext.LogEntries.Add(entry);
            _DatabaseContext.SaveChanges();
        }

        public void LogFeatureUsage(string featureName)
        {
            if (_FeatureManager.FeatureUsageLogging == false)
            {
                return;
            }

            var entry = GetPopulatedLogEntry();

            entry.LogType = "FeatureUsage";
            entry.FeatureName = featureName;

            _DatabaseContext.LogEntries.Add(entry);
            _DatabaseContext.SaveChanges();
        }        

        private LogEntry GetPopulatedLogEntry()
        {
            var returnValue = new LogEntry();

            string username = String.Empty;
            string referrer = String.Empty;
            string requestUrl = String.Empty;
            string userAgent = String.Empty;
            string ipAddress = String.Empty;


            username = _UsernameProvider.GetUsername();
                        
            returnValue.LogDate = DateTime.UtcNow;
            returnValue.ReferrerUrl = referrer;
            returnValue.RequestUrl = requestUrl;
            returnValue.UserAgent = userAgent;
            returnValue.Username = username;
            returnValue.RequestIpAddress = ipAddress;
            returnValue.Message = String.Empty;

            return returnValue;
        }
    }
}
