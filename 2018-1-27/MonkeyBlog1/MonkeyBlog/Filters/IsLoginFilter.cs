using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MonkeyBlog.Filters
{
    public class IsLoginFilter:ActionFilterAttribute
    {
        // 摘要:
        //     在执行操作方法后由 ASP.NET MVC 框架调用。
        //
        // 参数:
        //   filterContext:
        //     筛选器上下文。
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {

        }
        //
        // 摘要:
        //     在执行操作方法之前由 ASP.NET MVC 框架调用。
        //
        // 参数:
        //   filterContext:
        //     筛选器上下文。
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            //得到操作方法的名称
            string action = filterContext.ActionDescriptor.ActionName;

            //得到控制器名称
            string controllername = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;

            //得到网完整路径
            string url = filterContext.HttpContext.Request.RawUrl;

            //判断是否在登陆页面
            if (action.ToLower() == "home" && controllername.ToLower() == "index")
            {
                return;
            }
            else
            {
                //判断是否存在session（是否登陆成功）
                if (filterContext.RequestContext.HttpContext.Session["ULogin"]==null)
                {
                    filterContext.Result = new RedirectResult("/home/index");
                }
            }
        }
    }
}