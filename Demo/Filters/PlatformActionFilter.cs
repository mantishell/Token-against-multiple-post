using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Demo.Filters
{
    public class PlatformActionFilterAttribute:FilterAttribute, IActionFilter
    {
        private static log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public PlatformActionFilterAttribute()
        { 
        }

        string hiddenToken = "hiddenToken";

        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            string httpMethod = filterContext.RequestContext.HttpContext.Server.HtmlEncode(filterContext.RequestContext.HttpContext.Request.HttpMethod);

            if (httpMethod.ToUpper() == "POST")
            {
                string cacheToken = filterContext.HttpContext.Request[hiddenToken];

                if (cacheToken != null)
                {
                    if (System.Web.HttpContext.Current.Cache[cacheToken] == null)
                    {
                        System.Web.HttpContext.Current.Cache.Insert(cacheToken, cacheToken, null, DateTime.MaxValue, TimeSpan.FromSeconds(1));
                        //ok
                        log.Info("提交成功");
                    }
                    else
                    {
                        for (int i = 0; i < 50; i++)
                        {
                            log.Info("重复提交了"+i.ToString());
                        }
                        
                    }
                }
            }
        }

        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
        }
    }
}