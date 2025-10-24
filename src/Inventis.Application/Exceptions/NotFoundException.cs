namespace Inventis.Application.Exceptions;

/// <summary>
/// Should be thrown when resourceName cannot be found.
/// </summary>
/// <message>{resourceName} not found</message>
public class NotFoundException(string resourceName) : Exception(resourceName + " not found");
