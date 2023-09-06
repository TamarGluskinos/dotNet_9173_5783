using System;
namespace PL;

/// <summary>
/// exception for null value
/// </summary>
public class PlNullValueException : Exception
{
    /// <summary>
    /// Default constructor.
    /// </summary>
    public string nullField { get; set; }
    public PlNullValueException(string field)
    {
        nullField = field;
    }
    public override string Message => $@"{nullField} can not be null";

}

/// <summary>
/// exception for invalid value assigned to an integer variable
/// </summary>
public class PlInvalidIntegerException : Exception
{
    public override string Message =>
                    "invalid integer value exception";
}

/// <summary>
/// exception for invalid value assigned to a double variable
/// </summary>
public class PlInvalidDoubleException : Exception
{
    public override string Message =>
                    "invalid double value exception";
}


/// <summary>
/// exception for no orders to be updated
/// </summary>
public class PlNoOrdersToUpdateException : Exception
{
    public override string Message =>
                    "all orders updated";
}
