using Benday.Presidents.Common;
using Benday.Presidents.Api.DataAccess;
using Benday.Presidents.Api.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;

namespace Benday.Presidents.Api.Services
{
    public class Logger : ILogger
    {
        private IPresidentsDbContext _DatabaseContext;
        private IFeatureManager _FeatureManager;
        private IHttpContextAccessor _ContextAccessor;

        public Logger(IHttpContextAccessor contextAccessor, IPresidentsDbContext databaseContext, IFeatureManager featureManager)
        {
            if (contextAccessor == null)
                throw new ArgumentNullException(nameof(contextAccessor), $"{nameof(contextAccessor)} is null.");
            if (featureManager == null)
                throw new ArgumentNullException("featureManager", "featureManager is null.");
            if (databaseContext == null)
                throw new ArgumentNullException("databaseContext", "databaseContext is null.");

            _DatabaseContext = databaseContext;
            _FeatureManager = featureManager;
            _ContextAccessor = contextAccessor;
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

        private Microsoft.AspNetCore.Http.HttpContext GetHttpContext()
        {
            return _ContextAccessor.HttpContext;
        }

        private string SafeToString(IHeaderDictionary headers, string headerName)
        {
            if (headers.ContainsKey(headerName) == true)
            {
                return headers[headerName];
            }
            else
            {
                return string.Empty;
            }
        }

        private LogEntry GetPopulatedLogEntry()
        {
            var returnValue = new LogEntry();

            var context = GetHttpContext();

            string username = String.Empty;
            string referrer = String.Empty;
            string requestUrl = String.Empty;
            string userAgent = String.Empty;
            string ipAddress = String.Empty;

            if (context != null)
            {
                if (context.Request != null)
                {
                    referrer = SafeToString(context.Request.Headers, HeaderNames.Referer);
                    requestUrl = SafeToString(context.Request.Path.Value);
                    userAgent = SafeToString(context.Request.Headers, HeaderNames.UserAgent);
                    ipAddress = SafeToString(context.Connection.RemoteIpAddress);
                }

                if (context.User != null && context.User.Identity != null)
                {
                    username = context.User.Identity.Name;
                }
            }

            returnValue.LogDate = DateTime.UtcNow;
            returnValue.ReferrerUrl = referrer;
            returnValue.RequestUrl = requestUrl;
            returnValue.UserAgent = userAgent;
            returnValue.Username = username;
            returnValue.RequestIpAddress = ipAddress;
            returnValue.Message = String.Empty;

            return returnValue;
        }

        private string SafeToString(Uri value)
        {
            if (value == null)
            {
                return string.Empty;
            }
            else
            {
                return value.ToString();
            }
        }

        private string SafeToString(string value)
        {
            if (value == null)
            {
                return string.Empty;
            }
            else
            {
                return value;
            }
        }

        private string SafeToString(IPAddress address)
        {
            if (address == null)
            {
                return string.Empty;
            }
            else
            {
                return address.ToString();
            }
        }
    }
}
