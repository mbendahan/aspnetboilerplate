using System.Text;
using Abp.Utils.Extensions;

namespace Abp.WebApi.Controllers.Dynamic.Scripting.Actions
{
    internal class HttpPostActionScriptProxyGenerator : ActionScriptProxyGenerator
    {
        private const string AjaxPatametersTemplate = 
@"            url: '{url}',
            type: '{type}',
            data: JSON.stringify({postData})";

        public HttpPostActionScriptProxyGenerator(DynamicApiControllerInfo controllerInfo, DynamicApiActionInfo methodInfo)
            : base(controllerInfo, methodInfo)
        {

        }

        protected override string GenerateAjaxCallParameters()
        {
            var ajaxPatameters = AjaxPatametersTemplate
                .Replace("{url}", PlainActionUrl)
                .Replace("{type}", ActionInfo.Verb.ToString().ToUpperInvariant())
                .Replace("{postData}", GeneratePostData());

            return ajaxPatameters;
        }

        private string GeneratePostData()
        {
            var parameters = ActionInfo.Method.GetParameters();
            if (parameters.Length <= 0)
            {
                return "{}";
            }

            if (parameters.Length == 1)
            {
                return parameters[0].Name.ToCamelCase();
            }

            var sb = new StringBuilder();
            sb.AppendLine("{");
            for (var i = 0; i < parameters.Length; i++)
            {
                var parameterInfo = parameters[i];
                sb.AppendLine("                " + parameterInfo.Name.ToCamelCase() + ": " + parameterInfo.Name.ToCamelCase() + (i < parameters.Length - 1 ? "," : ""));
            }
            sb.AppendLine("}");

            return sb.ToString();
        }
    }
}