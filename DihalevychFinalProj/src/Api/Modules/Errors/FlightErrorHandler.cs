using Application.Flights.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace Api.Modules.Errors;

public static class FlightErrorHandler
{
    public static ObjectResult ToObjectResult(this FlightException exception)
    {
        return new ObjectResult(exception.Message)
        {
            StatusCode = exception switch
            {
                
                FlightNotFoundException => StatusCodes.Status404NotFound,
                FlightAlreadyExistsException => StatusCodes.Status409Conflict,
                FlightUnknownException => StatusCodes.Status500InternalServerError,
                _ => throw new NotImplementedException("Flight error handler does not implement handling this error.")
            }
        };
    }
}