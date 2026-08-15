public interface IStateValidator<T> 
{
    public bool Validate(T toValidate);
}
