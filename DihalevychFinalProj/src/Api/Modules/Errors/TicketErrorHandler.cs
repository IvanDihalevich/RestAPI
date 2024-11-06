using Application.Tickets.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace Api.Modules.Errors;

public static class TicketErrorHandler
{
    public static ObjectResult ToObjectResult(this TicketException exception)
    {
        return new ObjectResult(exception.Message)
        {
            StatusCode = exception switch
            {
                TicketNotFoundException => StatusCodes.Status404NotFound,
                TicketUnknownException => StatusCodes.Status500InternalServerError,
                _ => throw new NotImplementedException("Ticket error handler does not implement handling this error.")
            }
        };
    }
}