using Application.Airplanes.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace Api.Modules.Errors;

public static class AirplaneErrorHandler
{
    public static ObjectResult ToObjectResult(this AirplaneException exception)
    {
        return new ObjectResult(exception.Message)
        {
            StatusCode = exception switch
            {
                AirplaneNotFoundException or AirportNotFoundException => StatusCodes.Status404NotFound,
                AirplaneAlreadyExistsException => StatusCodes.Status409Conflict,
                AirplaneUnknownException => StatusCodes.Status500InternalServerError,
                _ => throw new NotImplementedException("Airplane error handler does not implement handling this error.")
            }
        };
    }
}