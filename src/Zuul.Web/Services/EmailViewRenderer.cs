using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Zuul.Web.Services
{
    public class EmailViewRenderer : IEmailViewRenderer
    {
        public string EmailViewDirectoryName { get; set; }
        private IRazorViewEngine ViewEngine { get; }
        private ITempDataProvider TempDataProvider { get; }
        private IHttpContextAccessor HttpContextAccessor { get; }
        public EmailViewRenderer(IRazorViewEngine viewEngine, ITempDataProvider tempDataProvider, IHttpContextAccessor httpContextAccessor)
        {
            TempDataProvider = tempDataProvider;
            ViewEngine = viewEngine;
            HttpContextAccessor = httpContextAccessor;
            EmailViewDirectoryName = "Emails";
        }

        public async Task<string> RenderAsync(string viewName, ViewDataDictionary viewData)
        {
            var actionContext = CreateActionContext();
            var view = CreateView(viewName, actionContext);

            return await RenderView(view, viewData, actionContext);
        }

        ActionContext CreateActionContext()
        {
            var routeData = new Microsoft.AspNetCore.Routing.RouteData();
            var actionDescriptor = new Microsoft.AspNetCore.Mvc.Abstractions.ActionDescriptor();

            var actionContext = new ActionContext(HttpContextAccessor.HttpContext, routeData, actionDescriptor);
            actionContext.RouteData.Values.Add("controller", EmailViewDirectoryName);
            actionContext.ActionDescriptor.RouteValues.Add("controller", EmailViewDirectoryName);
            actionContext.ActionDescriptor.AttributeRouteInfo = new Microsoft.AspNetCore.Mvc.Routing.AttributeRouteInfo();

            return actionContext;
        }

        IView CreateView(string viewName, ActionContext actionContext)
        {
            var viewResult = ViewEngine.FindView(actionContext, viewName, true);
            if (!viewResult.Success)
            {
                throw new Exception($"Email not found for {viewName}. Locations searched: {Environment.NewLine} {string.Join(Environment.NewLine, viewResult.SearchedLocations)}");
            }

            return viewResult.View;
        }

        public async Task<string> RenderView(IView view, ViewDataDictionary viewData, ActionContext actionContext)
        {
            using (var sw = new StringWriter())
            {
                var viewContext = new ViewContext(actionContext, view, viewData, new TempDataDictionary(HttpContextAccessor.HttpContext, TempDataProvider), sw, new HtmlHelperOptions());
                await view.RenderAsync(viewContext);

                sw.Flush();

                return sw.ToString();
            }
        }
    }
}
