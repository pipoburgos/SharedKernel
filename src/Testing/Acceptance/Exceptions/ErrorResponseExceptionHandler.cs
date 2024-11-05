using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using SharedKernel.Application.Extensions;
using System.Net;

namespace SharedKernel.Testing.Acceptance.Exceptions;

public class ErrorResponseExceptionHandler
{
    #region Atributes

    private const string ErrorsStartString = "{\"errors\":{";
    private readonly HttpResponseMessage _responseMessage;
    private string? _error;
    private Dictionary<string, string[]> _errors = new();

    #endregion

    #region Constructors

    public ErrorResponseExceptionHandler(HttpResponseMessage responseMessage)
    {
        _responseMessage = responseMessage;
    }

    #endregion

    #region Public Methods

    public async Task<ErrorResponseExceptionHandler> Build()
    {
        _responseMessage.StatusCode.Should().BeOneOf(new List<HttpStatusCode>
            {HttpStatusCode.BadRequest, HttpStatusCode.NotAcceptable});

        var message = await _responseMessage.Content.ReadAsStringAsync();

        _errors = new Dictionary<string, string[]>();

        if (!message.ToLower().Contains(ErrorsStartString))
        {
            _error = JsonConvert.DeserializeObject<string>(message)!;
            return this;
        }

        var jsonResponse = JsonConvert.DeserializeObject<Root>(message, new JsonSerializerSettings
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
        });

        if (jsonResponse == default)
            return this;

        foreach (var property in jsonResponse.Errors.Properties())
        {
            var valor = property.Value.ToObject<string[]>();
            if (string.IsNullOrWhiteSpace(property.Name))
                _error = valor?.First();
            else
                _errors.Add(property.Name.CapitalizeFirstLetter(), valor!);
        }

        return this;
    }

    public void Should(string message)
    {
        _error.Should().Contain(message);
    }

    public void Should(string propertyName, string message)
    {
        _errors.Should().ContainKey(propertyName);
        _errors[propertyName].Should().Contain(message);
    }

    #endregion

    #region Private

    // ReSharper disable UnusedMember.Local
    private class Root
    {
        public string? Type { get; set; }
        public string? Title { get; set; }
        public int Status { get; set; }
        public string? TraceId { get; set; }
        public JObject Errors { get; set; } = new();
    }

    #endregion
}