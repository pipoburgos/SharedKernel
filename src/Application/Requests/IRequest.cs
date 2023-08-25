namespace SharedKernel.Application.Requests;

/// <summary> Request made to a bus. </summary>
// ReSharper disable once UnusedTypeParameter
public interface IRequest<out TResponse> : IRequest
{
}

