using Application.Airports.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace Api.Modules.Errors;

public static class AirportErrorHandler
{
    public static ObjectResult ToObjectResult(this AirportException exception)
    {
        return new ObjectResult(exception.Message)
        {
            StatusCode = exception switch
            {
                AirportNotFoundException => StatusCodes.Status404NotFound,
                AirportAlreadyExistsException => StatusCodes.Status409Conflict,
                AirportUnknownException => StatusCodes.Status500InternalServerError,
                _ => throw new NotImplementedException("Airport error handler does not implement handling this error.")
            }
        };
    }
}
