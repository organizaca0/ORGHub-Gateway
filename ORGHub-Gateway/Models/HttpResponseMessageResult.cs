using Microsoft.AspNetCore.Mvc;

namespace ORGHub_Gateway.Models
{
    public class HttpResponseMessageResult : IActionResult
    {
        private readonly HttpResponseMessage _responseMessage;

        public HttpResponseMessageResult(HttpResponseMessage responseMessage)
        {
            _responseMessage = responseMessage;
        }

        public async Task ExecuteResultAsync(ActionContext context)
        {
            var response = context.HttpContext.Response;

            response.StatusCode = (int)_responseMessage.StatusCode;

            foreach (var header in _responseMessage.Headers)
            {
                response.Headers.TryAdd(header.Key, header.Value.ToArray());
            }

            if (_responseMessage.Content != null)
            {
                foreach (var header in _responseMessage.Content.Headers)
                {
                    response.Headers.TryAdd(header.Key, header.Value.ToArray());
                }

                await _responseMessage.Content.CopyToAsync(response.Body);
            }
        }
    }
}
