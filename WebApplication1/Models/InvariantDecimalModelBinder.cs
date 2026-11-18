using System;
using System.Globalization;
using System.Web.Mvc;

namespace WebApplication1.Models
{
    /// <summary>
    /// Interpreta decimales enviados por inputs type="number" (siempre con punto decimal),
    /// sin depender de la cultura del servidor (es-ES usa coma). Vacío = 0 (o null si es nullable).
    /// </summary>
    public class InvariantDecimalModelBinder : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var valor = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
            bool nullable = Nullable.GetUnderlyingType(bindingContext.ModelType) != null;

            if (valor == null || string.IsNullOrWhiteSpace(valor.AttemptedValue))
            {
                return nullable ? (object)null : 0m;
            }

            string texto = valor.AttemptedValue.Trim();
            decimal resultado;
            if (decimal.TryParse(texto, NumberStyles.Number, CultureInfo.InvariantCulture, out resultado) ||
                decimal.TryParse(texto, NumberStyles.Number, CultureInfo.CurrentCulture, out resultado))
            {
                return resultado;
            }

            bindingContext.ModelState.AddModelError(bindingContext.ModelName, "El valor '" + texto + "' no es un número válido.");
            return null;
        }
    }
}
