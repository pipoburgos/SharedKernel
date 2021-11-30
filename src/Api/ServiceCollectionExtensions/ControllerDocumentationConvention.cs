using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace SharedKernel.Api.ServiceCollectionExtensions
{
    /// <summary>
    /// Para agrupar en swagger por la propiedad Name de la ruta del controlador
    /// </summary>
    public class ControllerDocumentationConvention : IControllerModelConvention
    {
        void IControllerModelConvention.Apply(ControllerModel controller)
        {
            if (controller == null)
                return;

            foreach (var attribute in controller.Attributes)
            {
                if (attribute.GetType() != typeof(RouteAttribute)) continue;

                var routeAttribute = (RouteAttribute)attribute;
                if (!string.IsNullOrWhiteSpace(routeAttribute.Name))
                    controller.ControllerName = routeAttribute.Name;
            }

        }
    }
}
