using System;
using System.Collections.Generic;

namespace SharedKernel.Domain.Entities.Paged
{
    public class DomainPagedList<T>
    {
        public DomainPagedList(IEnumerable<T> items, int total, int totalFiltered)
        {
            Items = items ?? throw new ArgumentNullException(nameof(items));
            Total = total;
            TotalFiltered = totalFiltered;
        }

        public IEnumerable<T> Items { get; }

        public int Total { get; }

        public int TotalFiltered { get; }
    }
}
