using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PayGate.Web.Net.Models
{
    public class PayGateReturnModelBinder : IModelBinder
    {

        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            try
            {
                if (bindingContext == null)
                {
                    throw new ArgumentNullException(nameof(bindingContext));
                }

                var formCollection = controllerContext.HttpContext.Request.Form;

                if (formCollection == null || formCollection.Count < 1)
                {
                    return null;
                }

                var properties = new Dictionary<string, string>();

                foreach (string key in formCollection.Keys)
                {
                    var value = formCollection.Get(key);

                    properties.Add(key: key, value: value);
                }

                var model = new PayGateReturn();
                model.FromFormCollection(properties);

                return model;
            }
            catch (Exception ex)
            {
                // TODO: Handle exception

                return null;
            }
        }
    }
}