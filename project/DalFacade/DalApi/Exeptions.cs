namespace DalApi;

/// <summary>
/// exception for item not found
/// </summary>
public class EntityNotFoundException : Exception
{
    public readonly string msg;
    public EntityNotFoundException(string m) { msg = m; }
    public override string Message =>
                    $"{msg} Not Found";
}

/// <summary>
/// exception for existing id
/// </summary>
public class ExistingIdException : Exception
{
    public override string Message =>
                    "an item already exists with this id";
}

/// <summary>
/// exception for an invalid choice
/// </summary>
public class InvalidChoiceException : Exception
{
    public override string Message =>
                    "invalid choice";
}

/// <summary>
/// exception for an invalid integer
/// </summary>
public class InvalidIntegerException : Exception
{
    public override string Message =>
                    "invalid integer";
}

/// <summary>
/// exception for an invalid date-time variable
/// </summary>
public class InvalidDateTimeException : Exception
{
    public override string Message =>
                    "invalid date-time";
}


