namespace SharedKernel.Infrastructure.Dapper.Data.Queries
{
    /// <summary> </summary>
    public class QueryTable
    {
        /// <summary> </summary>
        public QueryTable(string name, string key, string schema = "dbo")
        {
            Schema = schema;
            Name = name;
            Key = key;
        }

        /// <summary> </summary>
        public QueryTable(string name, string alias, string key, string schema = "dbo")
        {
            Schema = schema;
            Name = name;
            Key = key;
            Alias = alias;
        }

        /// <summary> </summary>
        protected string Schema { get; }

        /// <summary> </summary>
        protected string Name { get; }

        /// <summary> </summary>
        private string Key { get; }

        /// <summary> </summary>
        private string Alias { get; } = null!;

        /// <summary> </summary>
        public override string ToString()
        {
            return !string.IsNullOrEmpty(Alias) ? $"{Schema}.{Name} AS {Alias}" : $"{Schema}.{Name}";
        }

        /// <summary> </summary>
        public string Join => !string.IsNullOrEmpty(Alias) ? $"{Alias}.{Key}" : $"{Schema}.{Name}.{Key}";
    }
}