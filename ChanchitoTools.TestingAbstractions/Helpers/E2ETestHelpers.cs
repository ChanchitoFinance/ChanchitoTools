using System.Net;
using System.Text;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Xunit;

namespace ChanchitoBackend.System.Abstractions.Helpers
{
    /// <summary>
    /// Helper methods for E2E testing
    /// </summary>
    public static class E2ETestHelpers
    {
        /// <summary>
        /// Creates JSON content for HTTP requests
        /// </summary>
        public static StringContent CreateJsonContent<T>(T data)
        {
            var json = JsonSerializer.Serialize(data, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });
            return new StringContent(json, Encoding.UTF8, "application/json");
        }

        /// <summary>
        /// Creates form URL encoded content for HTTP requests
        /// </summary>
        public static FormUrlEncodedContent CreateFormContent(Dictionary<string, string> data)
        {
            return new FormUrlEncodedContent(data);
        }

        /// <summary>
        /// Asserts that a response contains the expected error message
        /// </summary>
        public static async Task AssertErrorResponseAsync(HttpResponseMessage response, string expectedError, HttpStatusCode expectedStatusCode = HttpStatusCode.BadRequest)
        {
            Assert.Equal(expectedStatusCode, response.StatusCode);

            var content = await response.Content.ReadAsStringAsync();
            Assert.Contains(expectedError, content, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Asserts that a response contains validation errors
        /// </summary>
        public static async Task AssertValidationErrorsAsync(HttpResponseMessage response, params string[] expectedFieldNames)
        {
            Assert.Equal(HttpStatusCode.UnprocessableEntity, response.StatusCode);

            var content = await response.Content.ReadAsStringAsync();
            var errorResponse = JsonSerializer.Deserialize<ValidationErrorResponse>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            Assert.NotNull(errorResponse);
            Assert.NotNull(errorResponse.Errors);

            foreach (var fieldName in expectedFieldNames)
            {
                Assert.True(errorResponse.Errors.Any(e => e.Field.Equals(fieldName, StringComparison.OrdinalIgnoreCase)),
                    $"Expected validation error for field '{fieldName}' but not found");
            }
        }

        /// <summary>
        /// Asserts that a response contains the expected data
        /// </summary>
        public static async Task<T> AssertSuccessResponseAsync<T>(HttpResponseMessage response, HttpStatusCode expectedStatusCode = HttpStatusCode.OK)
        {
            Assert.Equal(expectedStatusCode, response.StatusCode);

            var content = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<T>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            Assert.NotNull(result);
            return result;
        }

        /// <summary>
        /// Asserts that a response contains paginated data
        /// </summary>
        public static async Task<PagedResponse<T>> AssertPagedResponseAsync<T>(HttpResponseMessage response)
        {
            var result = await AssertSuccessResponseAsync<PagedResponse<T>>(response);
            Assert.NotNull(result.Data);
            Assert.NotNull(result.Meta);
            return result;
        }

        /// <summary>
        /// Asserts that a database entity exists
        /// </summary>
        public static async Task<T> AssertEntityExistsAsync<T>(DbContext context, Expression<Func<T, bool>> predicate) where T : class
        {
            var entity = await context.Set<T>().FirstOrDefaultAsync(predicate);
            Assert.NotNull(entity);
            return entity;
        }

        /// <summary>
        /// Asserts that a database entity does not exist
        /// </summary>
        public static async Task AssertEntityDoesNotExistAsync<T>(DbContext context, Expression<Func<T, bool>> predicate) where T : class
        {
            var entity = await context.Set<T>().FirstOrDefaultAsync(predicate);
            Assert.Null(entity);
        }

        /// <summary>
        /// Asserts that a database entity count matches the expected value
        /// </summary>
        public static async Task AssertEntityCountAsync<T>(DbContext context, int expectedCount) where T : class
        {
            var actualCount = await context.Set<T>().CountAsync();
            Assert.Equal(expectedCount, actualCount);
        }

        /// <summary>
        /// Asserts that a database entity count matches the expected value based on a predicate
        /// </summary>
        public static async Task AssertEntityCountAsync<T>(DbContext context, Expression<Func<T, bool>> predicate, int expectedCount) where T : class
        {
            var actualCount = await context.Set<T>().CountAsync(predicate);
            Assert.Equal(expectedCount, actualCount);
        }

        /// <summary>
        /// Asserts that a database entity has the expected property values
        /// </summary>
        public static async Task<T> AssertEntityPropertiesAsync<T>(DbContext context, Expression<Func<T, bool>> predicate, Action<T> propertyAssertions) where T : class
        {
            var entity = await AssertEntityExistsAsync(context, predicate);
            propertyAssertions(entity);
            return entity;
        }

        /// <summary>
        /// Asserts that a response header contains the expected value
        /// </summary>
        public static void AssertHeader(HttpResponseMessage response, string headerName, string expectedValue)
        {
            Assert.True(response.Headers.Contains(headerName), $"Header '{headerName}' not found");
            var headerValue = response.Headers.GetValues(headerName).FirstOrDefault();
            Assert.Equal(expectedValue, headerValue);
        }

        /// <summary>
        /// Asserts that a response header contains the expected value (case-insensitive)
        /// </summary>
        public static void AssertHeaderContains(HttpResponseMessage response, string headerName, string expectedValue)
        {
            Assert.True(response.Headers.Contains(headerName), $"Header '{headerName}' not found");
            var headerValue = response.Headers.GetValues(headerName).FirstOrDefault();
            Assert.Contains(expectedValue, headerValue, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Asserts that a response contains the expected content type
        /// </summary>
        public static void AssertContentType(HttpResponseMessage response, string expectedContentType)
        {
            var contentType = response.Content.Headers.ContentType?.ToString();
            Assert.NotNull(contentType);
            Assert.Contains(expectedContentType, contentType, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Asserts that a response contains JSON content
        /// </summary>
        public static void AssertJsonContent(HttpResponseMessage response)
        {
            AssertContentType(response, "application/json");
        }

        /// <summary>
        /// Asserts that a response contains the expected status code
        /// </summary>
        public static void AssertStatusCode(HttpResponseMessage response, HttpStatusCode expectedStatusCode)
        {
            Assert.Equal(expectedStatusCode, response.StatusCode);
        }

        /// <summary>
        /// Asserts that a response is successful (2xx status code)
        /// </summary>
        public static void AssertSuccess(HttpResponseMessage response)
        {
            Assert.True(response.IsSuccessStatusCode,
                $"Expected success status code, but got {response.StatusCode}. Content: {response.Content.ReadAsStringAsync().Result}");
        }

        /// <summary>
        /// Asserts that a response is not successful (non-2xx status code)
        /// </summary>
        public static void AssertNotSuccess(HttpResponseMessage response)
        {
            Assert.False(response.IsSuccessStatusCode,
                $"Expected non-success status code, but got {response.StatusCode}");
        }

        /// <summary>
        /// Asserts that a response contains the expected error code
        /// </summary>
        public static async Task AssertErrorCodeAsync(HttpResponseMessage response, string expectedErrorCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            var errorResponse = JsonSerializer.Deserialize<ErrorResponse>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            Assert.NotNull(errorResponse);
            Assert.Equal(expectedErrorCode, errorResponse.Code);
        }

        /// <summary>
        /// Asserts that a response contains the expected error message
        /// </summary>
        public static async Task AssertErrorMessageAsync(HttpResponseMessage response, string expectedErrorMessage)
        {
            var content = await response.Content.ReadAsStringAsync();
            var errorResponse = JsonSerializer.Deserialize<ErrorResponse>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            Assert.NotNull(errorResponse);
            Assert.Contains(expectedErrorMessage, errorResponse.Message, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Asserts that a response contains the expected location header
        /// </summary>
        public static void AssertLocationHeader(HttpResponseMessage response, string expectedLocation)
        {
            Assert.True(response.Headers.Contains("Location"), "Location header not found");
            var location = response.Headers.Location?.ToString();
            Assert.Equal(expectedLocation, location);
        }

        /// <summary>
        /// Asserts that a response contains the expected ETag header
        /// </summary>
        public static void AssertETagHeader(HttpResponseMessage response, string expectedETag)
        {
            Assert.True(response.Headers.Contains("ETag"), "ETag header not found");
            var etag = response.Headers.ETag?.ToString();
            Assert.Equal(expectedETag, etag);
        }

        /// <summary>
        /// Asserts that a response contains the expected cache control header
        /// </summary>
        public static void AssertCacheControlHeader(HttpResponseMessage response, string expectedCacheControl)
        {
            Assert.True(response.Headers.Contains("Cache-Control"), "Cache-Control header not found");
            var cacheControl = response.Headers.GetValues("Cache-Control").FirstOrDefault();
            Assert.Equal(expectedCacheControl, cacheControl);
        }

        /// <summary>
        /// Asserts that a response contains the expected last modified header
        /// </summary>
        public static void AssertLastModifiedHeader(HttpResponseMessage response, DateTime expectedLastModified)
        {
            Assert.True(response.Headers.Contains("Last-Modified"), "Last-Modified header not found");
            var lastModified = response.Headers.GetValues("Last-Modified").FirstOrDefault();
            Assert.NotNull(lastModified);

            var parsedLastModified = DateTime.Parse(lastModified);
            Assert.Equal(expectedLastModified.Date, parsedLastModified.Date);
        }

        /// <summary>
        /// Asserts that a response contains the expected expires header
        /// </summary>
        public static void AssertExpiresHeader(HttpResponseMessage response, DateTime expectedExpires)
        {
            Assert.True(response.Headers.Contains("Expires"), "Expires header not found");
            var expires = response.Headers.GetValues("Expires").FirstOrDefault();
            Assert.NotNull(expires);

            var parsedExpires = DateTime.Parse(expires);
            Assert.Equal(expectedExpires.Date, parsedExpires.Date);
        }

        /// <summary>
        /// Asserts that a response contains the expected vary header
        /// </summary>
        public static void AssertVaryHeader(HttpResponseMessage response, string expectedVary)
        {
            Assert.True(response.Headers.Contains("Vary"), "Vary header not found");
            var vary = response.Headers.GetValues("Vary").FirstOrDefault();
            Assert.Equal(expectedVary, vary);
        }

        /// <summary>
        /// Asserts that a response contains the expected allow header
        /// </summary>
        public static void AssertAllowHeader(HttpResponseMessage response, string expectedAllow)
        {
            Assert.True(response.Headers.Contains("Allow"), "Allow header not found");
            var allow = response.Headers.GetValues("Allow").FirstOrDefault();
            Assert.Equal(expectedAllow, allow);
        }

        /// <summary>
        /// Asserts that a response contains the expected content length header
        /// </summary>
        public static void AssertContentLengthHeader(HttpResponseMessage response, long expectedContentLength)
        {
            Assert.NotNull(response.Content.Headers.ContentLength);
            Assert.Equal(expectedContentLength, response.Content.Headers.ContentLength);
        }

        /// <summary>
        /// Asserts that a response contains the expected content encoding header
        /// </summary>
        public static void AssertContentEncodingHeader(HttpResponseMessage response, string expectedContentEncoding)
        {
            Assert.NotNull(response.Content.Headers.ContentEncoding);
            var contentEncoding = response.Content.Headers.ContentEncoding.FirstOrDefault();
            Assert.Equal(expectedContentEncoding, contentEncoding);
        }

        /// <summary>
        /// Asserts that a response contains the expected content language header
        /// </summary>
        public static void AssertContentLanguageHeader(HttpResponseMessage response, string expectedContentLanguage)
        {
            Assert.NotNull(response.Content.Headers.ContentLanguage);
            var contentLanguage = response.Content.Headers.ContentLanguage.FirstOrDefault();
            Assert.Equal(expectedContentLanguage, contentLanguage);
        }

        /// <summary>
        /// Asserts that a response contains the expected content disposition header
        /// </summary>
        public static void AssertContentDispositionHeader(HttpResponseMessage response, string expectedContentDisposition)
        {
            Assert.NotNull(response.Content.Headers.ContentDisposition);
            var contentDisposition = response.Content.Headers.ContentDisposition.ToString();
            Assert.Contains(expectedContentDisposition, contentDisposition, StringComparison.OrdinalIgnoreCase);
        }
    }

    /// <summary>
    /// Validation error response model
    /// </summary>
    public class ValidationErrorResponse
    {
        public string Code { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public List<ValidationError> Errors { get; set; } = new();
    }

    /// <summary>
    /// Validation error model
    /// </summary>
    public class ValidationError
    {
        public string Field { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
    }

    /// <summary>
    /// Error response model
    /// </summary>
    public class ErrorResponse
    {
        public string Code { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
    }

    /// <summary>
    /// Paged response model
    /// </summary>
    public class PagedResponse<T>
    {
        public List<T> Data { get; set; } = new();
        public PagedMeta Meta { get; set; } = new();
    }

    /// <summary>
    /// Paged meta model
    /// </summary>
    public class PagedMeta
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalItems { get; set; }
        public int TotalPages { get; set; }
    }
}
