using System.Collections.Generic;

namespace SharedKernel.Application.Cqrs.Queries.Entities
{
    public class Results<T>
    {
        public int Draw { get; set; }
        public int RecordsTotal { get; set; }
        public int RecordsFiltered { get; set; }
        public List<T> Data { get; set; }
    }
}
