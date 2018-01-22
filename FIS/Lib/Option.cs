namespace FIS.Lib
{
    /// <summary>
    /// Represents an Optional Value.
    /// </summary>
    class Option<T>
    {
        /// <summary>
        /// Represents some <T>T</T> value otherwise the default is returned.
        /// </summary>
        internal T Value { get; private set; }

        /// <summary>
        /// Indicates whether this Option has some value.
        /// </summary>
        internal bool IsSome { get; }

        /// <summary>
        /// Indicates whether this Option han no value.
        /// </summary>
        internal bool IsNone { get; }

        /// <summary>
        /// Creates an optional that has no value.
        /// </summary>
        internal static Option<T> None { get; } = new Option<T>();

        Option ()
        {
            Value = default(T);
            IsNone = true;
        }
        Option (T value)
        {
            Value = value;
            IsSome = true;
            IsNone = false;
        }

        /// <summary>
        /// Creates an optional with some value.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        internal static Option<T> Some (T value) => new Option<T>(value);

        /// <summary>
        /// Returns a formatted string of this value.
        /// </summary>
        /// <returns></returns>
        public override string ToString () => IsSome ? $"Optional of : { Value.ToString() }" : "Nothing";
    }
}
