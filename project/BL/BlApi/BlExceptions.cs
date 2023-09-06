namespace BlApi;

/// <summary>
/// exception for item not found when dal throws bl exception
/// </summary>
public class BlEntityNotFoundException : Exception
{
    public BlEntityNotFoundException(Exception inner, string? m=null) : base(m, inner) { }
}

/// <summary>
/// regular exception for item not found
/// </summary>
public class BlEntityNotFoundEx : Exception
{
    public readonly string msg;
    public BlEntityNotFoundEx(string m) { msg = m; }
    public override string Message =>
                    $"{msg} not found";
}

/// <summary>
/// exception for existing id
/// </summary>
public class BlExistingIdException : Exception
{
    public readonly string msg;
    public BlExistingIdException(string m) { msg = m; }
    public override string Message =>
                    $"a {msg} already exists with this id";
}

/// <summary>
/// exception for an invalid choice
/// </summary>
public class BlInvalidChoiceException : Exception
{
    public override string Message =>
                    "invalid choice exception";
}

/// <summary>
/// exception for an invalid integer
/// </summary>
public class BlInvalidIntegerException : Exception
{
    public override string Message =>
                    "invalid integer exception";
}


/// <summary>
/// exception for an item out of stock
/// </summary>
public class BlOutOfStockException : Exception
{
    public override string Message =>
                    "not enough in stock";

}


/// <summary>
/// exception for a negative value in a case when only a positive number is valid 
/// </summary>
public class BlNegativeValueException : Exception
{
    public override string Message =>
                    "invalid negative number input";

}

/// <summary>
/// exception for a null value
/// </summary>
public class BlNullValueException : Exception
{
    public override string Message =>
                    "null value exception";

}

/// <summary>
/// exception for an invalid email format
/// </summary>
public class BlInvalidEmailException : Exception
{
    public override string Message =>
                    "invalid email exception";

}

/// <summary>
/// exception for trying to update something which doesn't need updating
/// </summary>
public class BlNoNeedToUpdateException : Exception
{
    public override string Message =>
                    "no need to update exception";

}

/// <summary>
/// exception for updating dates in wrong order
/// </summary>
public class BlDateSeqException : Exception
{
    public override string Message =>
                    "can't update dates in wrong sequence";

}

/// <summary>
/// exception for an unsuccessful attempt to upate something
/// </summary>
public class BlUnsuccessfulDeleteException : Exception
{
    public override string Message =>
                    "can't delete a product that is ordered";

}

public class BlinvalidRequest : Exception
{
    public override string Message =>
                    "can't understand the request";

}
