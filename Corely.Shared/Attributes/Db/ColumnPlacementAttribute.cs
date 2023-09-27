namespace Corely.Shared.Attributes.Db
{
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class ColumnPlacementAttribute : Attribute
    {
        private string _beforeColumn;
        private string _afterColumn;

        public string AfterColumn
        {
            get => _afterColumn;
            init
            {
                _afterColumn = value;
                ThrowIfColumnsAreSame();
            }
        }

        public string BeforeColumn
        {
            get => _beforeColumn;
            init
            {
                _beforeColumn = value;
                ThrowIfColumnsAreSame();
            }
        }

        public ColumnPlacementAttribute() { }

        public ColumnPlacementAttribute(string afterColumn)
        {
            AfterColumn = afterColumn;
        }

        public ColumnPlacementAttribute(string afterColumn, string beforeColumn)
            : this(afterColumn)
        {
            BeforeColumn = beforeColumn;
        }

        private void ThrowIfColumnsAreSame()
        {
            if (AfterColumn == BeforeColumn)
            {
                throw new ArgumentException("AfterColumn cannot be the same as BeforeColumn");
            }
        }
    }
}
