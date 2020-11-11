namespace SharedKernel.Api.Grids.DataTables
{
    /// <summary>
    /// 
    /// </summary>
    public class Column
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="name"></param>
        /// <param name="search"></param>
        /// <param name="searchable"></param>
        /// <param name="orderable"></param>
        public Column(string data, string name = null, Search search = null, bool searchable = true,
            bool orderable = true)
        {
            Data = data;
            Name = name;
            Search = search;
            Searchable = searchable;
            Orderable = orderable;
        }

        /// <summary>
        /// 
        /// </summary>
        public string Data { get; }

        /// <summary>
        /// 
        /// </summary>
        public string Name { get;  }

        /// <summary>
        /// 
        /// </summary>
        public Search Search { get;  }

        /// <summary>
        /// 
        /// </summary>
        public bool Searchable { get;  }

        /// <summary>
        /// 
        /// </summary>
        public bool Orderable { get; }
    }
}