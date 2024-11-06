using Application.Passengers.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace Api.Modules.Errors;

public static class PassengerErrorHandler
{
    public static ObjectResult ToObjectResult(this PassengerException exception)
    {
        return new ObjectResult(exception.Message)
        {
            StatusCode = exception switch
            {
                PassengerNotFoundException => StatusCodes.Status404NotFound,
                PassengerAlreadyExistsException => StatusCodes.Status409Conflict,
                PassengerUnknownException => StatusCodes.Status500InternalServerError,
                _ => throw new NotImplementedException("Passenger error handler does not implement handling this error.")
            }
        };
    }
}