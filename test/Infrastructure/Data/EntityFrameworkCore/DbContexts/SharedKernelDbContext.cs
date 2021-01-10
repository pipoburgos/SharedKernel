﻿using Microsoft.EntityFrameworkCore;
using SharedKernel.Domain.Events;
using SharedKernel.Infrastructure.Data.EntityFrameworkCore.DbContexts;

namespace SharedKernel.Infraestructure.Tests.Data.EntityFrameworkCore.DbContexts
{
    public class SharedKernelDbContext : DbContextBase
    {
        public SharedKernelDbContext(DbContextOptions<SharedKernelDbContext> options,
            IAuditableService auditable = null, IEventBus eventBus = null) : base(options, "skr",
            typeof(SharedKernelDbContext).Assembly, auditable, eventBus)
        {

        }

    }
}
