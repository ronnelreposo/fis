namespace FIS.Lib
{
    /// <summary>
    /// Represents an Optional Value.
    /// </summary>
    class Option<T>
    {
        internal T Value { get; private set; }
        internal bool IsSome { get { return Value != null; } }
        internal bool IsNone { get { return Value == null; } }
        
        internal static readonly Option<T> None = new Option<T>();

        Option ()
        {
            Value = default(T);
        }
        Option (T value)
        {
            Value = value;
        }

        internal static Option<T> Some (T value) => new Option<T>(value);

        public override string ToString () => IsSome ? $"Optional of : { Value }" : "Nothing";
    }
}
