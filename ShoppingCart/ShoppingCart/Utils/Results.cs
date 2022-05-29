using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingCart.Utils
{
    public enum State
    {
        ok,
        badRequest,
        notFound,
        unProcessableEntity,
        noContent
    }
    public class Response
    {
        public State Status { get; private set; }
        public bool IsSuccessful { get; private set; }
        public dynamic ResponseMessage { get; private set; }

        public Response(State state, bool isSuccessful, dynamic response)
        {
            Status = state;
            IsSuccessful = isSuccessful;
            ResponseMessage = response;
        }
    }
}
