using HotChocolate;
using CRM.Shared.ValidationModel;

namespace CRM.GraphQL.Errors
{
    public class ValidationErrorFilter : IErrorFilter
    {
        public IError OnError(IError error)
        {
            if(error.Exception is ValidationException exception) 
            {
                return error
                    .AddExtension("ValidationError", exception.ValidationResultModel.ToString());
            }

            return error;
        }
    }
}