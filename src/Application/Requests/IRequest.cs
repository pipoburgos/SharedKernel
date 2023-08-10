using SharedKernel.Domain.Requests;
// ReSharper disable UnusedTypeParameter

namespace SharedKernel.Application.Requests;

/// <summary> Request made to a bus. </summary>
public interface IRequest<out TResponse> : IRequest
{
}

