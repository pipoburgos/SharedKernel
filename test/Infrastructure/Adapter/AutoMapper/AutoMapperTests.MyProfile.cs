using AutoMapper;

namespace SharedKernel.Integration.Tests.Adapter.AutoMapper
{
    public partial class AutoMapperTests
    {
        internal class MyProfile : Profile
        {
            public MyProfile()
            {
                CreateMap<DocumentSource, DocumentTarget>();
            }
        }
    }
}