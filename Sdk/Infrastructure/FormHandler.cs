using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Atomia.OrderPage.Sdk.Infrastructure
{
    public interface IInputHandler<TInput, TDataObject>
    {
        TDataObject Translate(TInput input, TDataObject dataObject);
    }

    public sealed class FormHandler<TInput, TDataObject> where TDataObject : new()
    {
        private List<IInputHandler<TInput, TDataObject>> inputHandlers = new List<IInputHandler<TInput, TDataObject>>();

        public TDataObject Handle(TInput input)
        {
            var dataObject = new TDataObject();

            foreach (var handler in inputHandlers)
            {
                dataObject = handler.Translate(input, dataObject);
            }

            return dataObject;
        }

        public void RegisterInputHandler(IInputHandler<TInput, TDataObject> inputHandler)
        {
            inputHandlers.Add(inputHandler);
        }
    }
}
