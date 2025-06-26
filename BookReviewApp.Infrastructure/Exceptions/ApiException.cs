using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookReviewApp.Infrastructure.Exceptions
{
    public class ApiException : Exception
    {
        public int StatusCode { get; }
        public string ResponseContent { get; }
        public IDictionary<string, IEnumerable<string>> Headers { get; }

        public ApiException(
            string message,
            int statusCode,
            string responseContent = null,
            IDictionary<string, IEnumerable<string>> headers = null,
            Exception innerException = null)
            : base(message, innerException)
        {
            StatusCode = statusCode;
            ResponseContent = responseContent;
            Headers = headers;
        }

        public override string ToString()
        {
            return $"{base.ToString()}\nStatus Code: {StatusCode}\nResponse Content: {ResponseContent}";
        }
    }

    public class ApiException<T> : ApiException
    {
        public T Details { get; }

        public ApiException(
            string message,
            int statusCode,
            string responseContent,
            IDictionary<string, IEnumerable<string>> headers,
            T result,
            Exception innerException = null)
            : base(message, statusCode, responseContent, headers, innerException)
        {
            Details = result;
        }

        public override string ToString()
        {
            return $"{base.ToString()}\nResult: {Details}";
        }
    }
}
