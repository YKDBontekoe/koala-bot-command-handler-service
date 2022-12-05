namespace Koala.CommandHandlerService.DTOs;

public struct OptionDto<T>
{
    private readonly T _value;
    private readonly Exception _error;
    private readonly bool _hasValue;

    public OptionDto(T value)
    {
        _value = value;
        _hasValue = true;
    }
    
    public OptionDto(Exception error)
    {
        _error = error;
        _hasValue = false;
    }

    public bool HasValue => _hasValue;

    public T Value
    {
        get
        {
            if (!_hasValue)
                throw new InvalidOperationException("Option does not have a value.");
            return _value;
        }
    }
    
    public Exception Error
    {
        get
        {
            if (_hasValue)
                throw new InvalidOperationException("Option has a value.");
            return _error;
        }
    }

    public static implicit operator OptionDto<T>(T value)
    {
        return new OptionDto<T>(value);
    }

    public static implicit operator OptionDto<T>(Exception error)
    {
        return new OptionDto<T>(error);
    }
}