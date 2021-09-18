// ***********************************************************************
// Assembly         : maxx.testcommon
// Author           : Muhammed Ali Khan
// Created          : 09-18-2021
//
// Last Modified By : Muhammed Ali Khan
// Last Modified On : 09-18-2021
// ***********************************************************************
// <copyright file="AspNetCoreIntegrationTestHelper.cs" company="maxx.testcommon">
//     Copyright (c) Maxx Technologies. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace maxx.testcommon.Helpers
{
	using System;
	using System.Collections.Generic;
	using System.IO;
	using System.Linq;
	using System.Net;
	using System.Net.Http;
	using System.Net.Mime;
	using System.Runtime.Serialization.Formatters.Binary;
	using System.Text;
	using System.Threading.Tasks;
	using JsonDiffPatchDotNet;
	using maxx.testcommon.Models;
	using Microsoft.AspNetCore.Hosting;
	using Microsoft.AspNetCore.Mvc;
	using Microsoft.AspNetCore.TestHost;
	using Microsoft.Extensions.DependencyInjection;
	using Newtonsoft.Json;
	using Newtonsoft.Json.Linq;
	using Newtonsoft.Json.Serialization;

	/// <summary>
	/// Class AspNetCoreIntegrationTestHelper.
	/// Implements the <see cref="System.IDisposable" />
	/// </summary>
	/// <seealso cref="System.IDisposable" />
	public class AspNetCoreIntegrationTestHelper : IDisposable
	{
		/// <summary>
		/// The application json content type
		/// </summary>
		private const string ApplicationJsonContentType = MediaTypeNames.Application.Json;

		/// <summary>
		/// The application octet stream content type
		/// </summary>
		private const string ApplicationOctetStreamContentType = MediaTypeNames.Application.Octet;

		/// <summary>
		/// The application text content type
		/// </summary>
		private const string ApplicationTextContentType = MediaTypeNames.Text.Plain;

		/// <summary>
		/// The authorization
		/// </summary>
		private const string Authorization = "Authorization";

		/// <summary>
		/// The multipart form content type
		/// </summary>
		private const string MultipartFormContentType = "multipart/form-data";

		/// <summary>
		/// The SVG content type
		/// </summary>
		private const string SvgContentType = "image/svg+xml";

		/// <summary>
		/// The web host builder
		/// </summary>
		private readonly WebHostBuilder _webHostBuilder;

		/// <summary>
		/// The ASP net core integration test data
		/// </summary>
		private AspNetCoreIntegrationTestData _aspNetCoreIntegrationTestData;

		/// <summary>
		/// The client
		/// </summary>
		private HttpClient _client;

		/// <summary>
		/// The test server
		/// </summary>
		private TestServer _testServer;

		/// <summary>
		/// Initializes a new instance of the <see cref="AspNetCoreIntegrationTestHelper" /> class.
		/// </summary>
		public AspNetCoreIntegrationTestHelper()
		{
			_webHostBuilder = new WebHostBuilder();
			_aspNetCoreIntegrationTestData = new AspNetCoreIntegrationTestData();
		}

		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		public void Dispose()
		{
			_client?.Dispose();
			_testServer?.Dispose();
		}

		/// <summary>
		/// Adds the query parameters.
		/// </summary>
		/// <param name="path">The path.</param>
		/// <param name="parameters">The parameters.</param>
		/// <returns>System.String.</returns>
		public static string AddQueryParameters(string path, params string[] parameters)
		{
			var newPath = path; // we don't want to change the original path

			if (parameters == null || !parameters.Any())
			{
				return newPath; // no change is needed
			}

			// there is at least one parameter
			var isFirst = !path.Contains(value: "?"); // indicates the first parameter

			foreach (var parameter in parameters)
			{
				if (string.IsNullOrWhiteSpace(value: parameter))
				{
					continue; // no need to consider empty strings
				}

				var delimiter = isFirst ? "?" : "&";
				newPath = $"{newPath}{delimiter}{parameter}";

				isFirst = false;
			}

			return newPath;
		}

		/// <summary>
		/// Verifies this instance.
		/// </summary>
		/// <exception cref="System.ArgumentNullException"></exception>
		/// <exception cref="ArgumentNullException"></exception>
		public async Task Verify()
		{
			try
			{
				// Getting pact file
				var pactTest = await GetPactTest();


				// Validate http method
				ValidateMethod(pactTest: pactTest);

				// Validate route
				ValidateRoute(pactTest: pactTest);

				// Validate response status code
				ValidateResponseStatusCode(pactTest: pactTest);

				// Set query parameters
				SetQueryParameters(pactTest: pactTest);

				// Initialize
				Initialize();

				// Building request method and body
				var request = GetHttpRequestMessage(pactTest: pactTest);

				if (request == null)
				{
					throw new ArgumentNullException(paramName: nameof(request));
				}


				// Act
				var rawResponse = await _client.SendAsync(request: request);

				// Assert response status code
				AssertResponseStatusCode(pactTest: pactTest, rawResponse: rawResponse);

				// Assert response headers
				AssertResponseHeaders(pactTest: pactTest, rawResponse: rawResponse);

				// Assert response body
				await AssertResponseBody(pactTest: pactTest, rawResponse: rawResponse);
			}
			finally
			{
				Cleanup();
			}
		}

		/// <summary>
		/// Withes the authorization header.
		/// </summary>
		/// <param name="authorizationHeader">The authorization header.</param>
		/// <returns>AspNetCoreIntegrationTestHelper.</returns>
		public AspNetCoreIntegrationTestHelper WithAuthorizationHeader(string authorizationHeader)
		{
			_aspNetCoreIntegrationTestData.AuthorizationHeader = authorizationHeader;

			return this;
		}

		/// <summary>
		/// Withes the custom response comparer.
		/// </summary>
		/// <param name="customResponseComparer">The custom response comparer. Passing expected, actual responses to the comparer</param>
		/// <returns>AspNetCoreIntegrationTestHelper.</returns>
		public AspNetCoreIntegrationTestHelper WithCustomResponseComparer(Func<string, string, string> customResponseComparer)
		{
			_aspNetCoreIntegrationTestData.CustomResponseComparer = customResponseComparer;

			return this;
		}

		/// <summary>
		/// Withes the custom services.
		/// </summary>
		/// <param name="customServices">The custom services.</param>
		/// <returns>AspNetCoreIntegrationTestHelper.</returns>
		public AspNetCoreIntegrationTestHelper WithCustomServices(Action<IServiceCollection> customServices)
		{
			_webHostBuilder.ConfigureServices(customServices);

			return this;
		}

		/// <summary>
		/// Withes the HTTP method.
		/// </summary>
		/// <param name="httpMethod">The HTTP method.</param>
		/// <returns>AspNetCoreIntegrationTestHelper.</returns>
		public AspNetCoreIntegrationTestHelper WithHttpMethod(HttpMethod httpMethod)
		{
			_aspNetCoreIntegrationTestData.HttpMethod = httpMethod;

			return this;
		}

		/// <summary>
		/// Withes the pact.
		/// </summary>
		/// <param name="pactFilePath">The pact file path.</param>
		/// <param name="providerState">State of the provider.</param>
		/// <returns>AspNetCoreIntegrationTestHelper.</returns>
		public AspNetCoreIntegrationTestHelper WithPact(string pactFilePath, string providerState)
		{
			_aspNetCoreIntegrationTestData.PactFilePath = pactFilePath;
			_aspNetCoreIntegrationTestData.PactProviderState = providerState;

			return this;
		}

		/// <summary>
		/// Withes the query parameters.
		/// </summary>
		/// <param name="queryParameters">The query parameters.</param>
		/// <returns>AspNetCoreIntegrationTestHelper.</returns>
		public AspNetCoreIntegrationTestHelper WithQueryParameters(string queryParameters)
		{
			_aspNetCoreIntegrationTestData.QueryParameters = queryParameters;

			return this;
		}

		/// <summary>
		/// Withes the query parameters.
		/// </summary>
		/// <param name="queryParameters">The query parameters.</param>
		/// <returns>AspNetCoreIntegrationTestHelper.</returns>
		public AspNetCoreIntegrationTestHelper WithQueryParameters(Dictionary<string, string> queryParameters)
		{
			_aspNetCoreIntegrationTestData.QueryParametersTable = queryParameters;

			return this;
		}

		/// <summary>
		/// Withes the query parameters.
		/// </summary>
		/// <param name="queryParameters">The query parameters.</param>
		/// <returns>AspNetCoreIntegrationTestHelper.</returns>
		public AspNetCoreIntegrationTestHelper WithQueryParameters(List<string> queryParameters)
		{
			_aspNetCoreIntegrationTestData.QueryParametersList = queryParameters;

			return this;
		}

		/// <summary>
		/// Withes the request as form data.
		/// </summary>
		/// <param name="multiPartFormData">The multi part form data.</param>
		/// <returns>AspNetCoreIntegrationTestHelper.</returns>
		public AspNetCoreIntegrationTestHelper WithRequestAsFormData(FormData multiPartFormData)
		{
			_aspNetCoreIntegrationTestData.MultiPartFormData = multiPartFormData;

			return this;
		}

		/// <summary>
		/// Withes the request body.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="requestBody">The request body.</param>
		/// <param name="serializeToJson">if set to <c>true</c> [serialize to json].</param>
		/// <returns>AspNetCoreIntegrationTestHelper.</returns>
		public AspNetCoreIntegrationTestHelper WithRequestBody<T>(T requestBody, bool serializeToJson)
		{
			if (!string.IsNullOrWhiteSpace(value: _aspNetCoreIntegrationTestData.RequestContentType)
					&& _aspNetCoreIntegrationTestData.RequestContentType.StartsWith(
																																					value: MultipartFormContentType,
																																					comparisonType: StringComparison.Ordinal))
			{
				return this;
			}

			if (serializeToJson || _aspNetCoreIntegrationTestData.RequestContentType == ApplicationJsonContentType)
			{
				_aspNetCoreIntegrationTestData.RequestContentType = ApplicationJsonContentType;
				_aspNetCoreIntegrationTestData.RequestBody = JsonConvert.SerializeObject(value: requestBody);
			}
			else if (_aspNetCoreIntegrationTestData.RequestContentType != ApplicationTextContentType)
			{
				_aspNetCoreIntegrationTestData.RequestContentType = ApplicationOctetStreamContentType;
				var binaryFormatter = new BinaryFormatter();
				using var ms = new MemoryStream();

				binaryFormatter.Serialize(serializationStream: ms, graph: requestBody);
				_aspNetCoreIntegrationTestData.RequestBodyBytes = ms.ToArray();
			}

			return this;
		}

		/// <summary>
		/// Withes the request body.
		/// </summary>
		/// <param name="stringContent">Content of the string.</param>
		/// <returns>AspNetCoreIntegrationTestHelper.</returns>
		public AspNetCoreIntegrationTestHelper WithRequestBody(string stringContent)
		{
			_aspNetCoreIntegrationTestData.RequestBody = stringContent;

			return this;
		}

		/// <summary>
		/// Withes the type of the request content.
		/// </summary>
		/// <param name="mimeType">Type of the MIME.</param>
		/// <returns>AspNetCoreIntegrationTestHelper.</returns>
		/// <exception cref="System.NotImplementedException"></exception>
		/// <exception cref="NotImplementedException"></exception>
		public AspNetCoreIntegrationTestHelper WithRequestContentType(string mimeType)
		{
			var supportedMimeTypes = new List<string>
			{
				ApplicationJsonContentType, ApplicationTextContentType, ApplicationOctetStreamContentType, MultipartFormContentType
			};

			if (!supportedMimeTypes.Contains(item: mimeType))
			{
				throw new NotImplementedException();
			}

			_aspNetCoreIntegrationTestData.RequestContentType = mimeType;

			return this;
		}

		/// <summary>
		/// Withes the request headers.
		/// </summary>
		/// <param name="requestHeaders">The request headers.</param>
		/// <returns>AspNetCoreIntegrationTestHelper.</returns>
		public AspNetCoreIntegrationTestHelper WithRequestHeaders(Dictionary<string, string> requestHeaders)
		{
			_aspNetCoreIntegrationTestData.RequestHeaders = requestHeaders;

			return this;
		}

		/// <summary>
		/// Withes the content of the response as file.
		/// </summary>
		/// <param name="contentResult">The content result.</param>
		/// <returns>AspNetCoreIntegrationTestHelper.</returns>
		public AspNetCoreIntegrationTestHelper WithResponseAsFileContent(FileContentResult contentResult)
		{
			_aspNetCoreIntegrationTestData.FileContentResult = contentResult;

			return this;
		}

		/// <summary>
		/// Withes the response body.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="responseBody">The response body.</param>
		/// <param name="serializeToJson">if set to <c>true</c> [serialize to json].</param>
		/// <returns>AspNetCoreIntegrationTestHelper.</returns>
		public AspNetCoreIntegrationTestHelper WithResponseBody<T>(T responseBody, bool serializeToJson)
		{
			if (!string.IsNullOrWhiteSpace(value: _aspNetCoreIntegrationTestData.ResponseContentType)
					&& _aspNetCoreIntegrationTestData.ResponseContentType.StartsWith(
																																					value: SvgContentType,
																																					comparisonType: StringComparison.Ordinal))
			{
				return this;
			}

			if (serializeToJson || _aspNetCoreIntegrationTestData.ResponseContentType == ApplicationJsonContentType)
			{
				_aspNetCoreIntegrationTestData.ResponseContentType = ApplicationJsonContentType;

				_aspNetCoreIntegrationTestData.ResponseBody = JsonConvert.SerializeObject(value: responseBody, new JsonSerializerSettings
				{
					ContractResolver = new CamelCasePropertyNamesContractResolver()
				});
			}
			else if (_aspNetCoreIntegrationTestData.ResponseContentType != ApplicationTextContentType)
			{
				_aspNetCoreIntegrationTestData.ResponseContentType = ApplicationOctetStreamContentType;
				var binaryFormatter = new BinaryFormatter();

				using (var ms = new MemoryStream())
				{
					binaryFormatter.Serialize(serializationStream: ms, graph: responseBody);
					_aspNetCoreIntegrationTestData.ResponseBodyBytes = ms.ToArray();
				}
			}

			return this;
		}

		/// <summary>
		/// Withes the response body.
		/// </summary>
		/// <param name="stringContent">Content of the string.</param>
		/// <returns>AspNetCoreIntegrationTestHelper.</returns>
		public AspNetCoreIntegrationTestHelper WithResponseBody(string stringContent)
		{
			_aspNetCoreIntegrationTestData.ResponseContentType = ApplicationTextContentType;
			_aspNetCoreIntegrationTestData.ResponseBody = stringContent;

			return this;
		}

		/// <summary>
		/// Withes the type of the response content.
		/// </summary>
		/// <param name="mimeType">Type of the MIME.</param>
		/// <returns>AspNetCoreIntegrationTestHelper.</returns>
		/// <exception cref="System.NotImplementedException"></exception>
		public AspNetCoreIntegrationTestHelper WithResponseContentType(string mimeType)
		{
			var supportedMimeTypes = new List<string>
			{
				ApplicationJsonContentType, ApplicationTextContentType, ApplicationOctetStreamContentType, SvgContentType
			};

			if (!supportedMimeTypes.Contains(item: mimeType))
			{
				throw new NotImplementedException();
			}

			_aspNetCoreIntegrationTestData.ResponseContentType = mimeType;

			return this;
		}

		/// <summary>
		/// Withes the response headers.
		/// </summary>
		/// <param name="responseHeaders">The response headers.</param>
		/// <returns>AspNetCoreIntegrationTestHelper.</returns>
		public AspNetCoreIntegrationTestHelper WithResponseHeaders(Dictionary<string, string> responseHeaders)
		{
			_aspNetCoreIntegrationTestData.ResponseHeaders = responseHeaders;

			return this;
		}

		/// <summary>
		/// Withes the response status code.
		/// </summary>
		/// <param name="httpStatusCode">The HTTP status code.</param>
		/// <returns>AspNetCoreIntegrationTestHelper.</returns>
		public AspNetCoreIntegrationTestHelper WithResponseStatusCode(HttpStatusCode httpStatusCode)
		{
			_aspNetCoreIntegrationTestData.ResponseHttpStatusCode = httpStatusCode;

			return this;
		}

		/// <summary>
		/// Withes the route.
		/// </summary>
		/// <param name="route">The route.</param>
		/// <returns>AspNetCoreIntegrationTestHelper.</returns>
		public AspNetCoreIntegrationTestHelper WithRoute(string route)
		{
			_aspNetCoreIntegrationTestData.Route = route;

			return this;
		}

		/// <summary>
		/// Withes the startup.
		/// </summary>
		/// <typeparam name="TStartup">The type of the t startup.</typeparam>
		/// <returns>AspNetCoreIntegrationTestHelper.</returns>
		public AspNetCoreIntegrationTestHelper WithStartup<TStartup>()
			where TStartup : class
		{
			_webHostBuilder.UseStartup<TStartup>();

			return this;
		}

		/// <summary>
		/// Asserts the response body.
		/// </summary>
		/// <param name="pactTest">The pact test.</param>
		/// <param name="rawResponse">The raw response.</param>
		/// <exception cref="System.Exception"></exception>
		/// <exception cref="System.Exception"></exception>
		/// <exception cref="System.Exception"></exception>
		/// <exception cref="System.Exception"></exception>
		/// <exception cref="Exception"></exception>
		/// <exception cref="Exception"></exception>
		/// <exception cref="Exception"></exception>
		private async Task AssertResponseBody(PactTest pactTest, HttpResponseMessage rawResponse)
		{
			if (pactTest?.Response?.Body != null
					|| !string.IsNullOrWhiteSpace(value: _aspNetCoreIntegrationTestData.ResponseBody))
			{
				if (pactTest?.Response?.Body != null)
				{
					var responseAsJson = await rawResponse.Content.ReadAsStringAsync();
					var actualResponse = JToken.Parse(json: responseAsJson);

					if (!JToken.DeepEquals(t1: actualResponse, t2: pactTest.Response.Body))
					{
						throw new Exception(message: "Expected response body does not match actual response body");
					}
				}
				else
				{
					if (!string.IsNullOrWhiteSpace(value: _aspNetCoreIntegrationTestData.ResponseBody))
					{
						if (_aspNetCoreIntegrationTestData.ResponseContentType == ApplicationJsonContentType
								|| _aspNetCoreIntegrationTestData.ResponseContentType == ApplicationTextContentType)
						{
							var expectedResponse = _aspNetCoreIntegrationTestData.ResponseBody;
							var actualResponse = await rawResponse.Content.ReadAsStringAsync();

							if (_aspNetCoreIntegrationTestData.CustomResponseComparer != null)
							{
								var comparisonError = _aspNetCoreIntegrationTestData.CustomResponseComparer(expectedResponse, actualResponse);

								if (!string.IsNullOrWhiteSpace(comparisonError))
								{
									throw new Exception(comparisonError);
								}
							}
							else
							{
								var jdp = new JsonDiffPatch();

								var deltaResponseBody = jdp.Diff(expectedResponse, actualResponse);

								if (!string.IsNullOrWhiteSpace(deltaResponseBody))
								{
									throw new Exception(deltaResponseBody);
								}
							}
						}
					}
					else if (_aspNetCoreIntegrationTestData.ResponseBodyBytes != null)
					{
						if (_aspNetCoreIntegrationTestData.ResponseContentType == ApplicationOctetStreamContentType)
						{
							var responseBody = await rawResponse.Content.ReadAsByteArrayAsync();

							if (!responseBody.SequenceEqual(second: _aspNetCoreIntegrationTestData.ResponseBodyBytes))
							{
								throw new Exception(message: "Expected response body does not match actual response body");
							}
						}
					}
				}
			}
		}

		/// <summary>
		/// Asserts the response headers.
		/// </summary>
		/// <param name="pactTest">The pact test.</param>
		/// <param name="rawResponse">The raw response.</param>
		/// <exception cref="System.Exception"></exception>
		/// <exception cref="System.Exception"></exception>
		/// <exception cref="System.Exception"></exception>
		/// <exception cref="System.Exception"></exception>
		/// <exception cref="System.Exception"></exception>
		/// <exception cref="Exception"></exception>
		/// <exception cref="Exception"></exception>
		/// <exception cref="Exception"></exception>
		/// <exception cref="Exception"></exception>
		private void AssertResponseHeaders(PactTest pactTest, HttpResponseMessage rawResponse)
		{
			if (pactTest != null)
			{
				if (pactTest.Response.Headers != null && rawResponse.Headers != null)
				{
					foreach (var responseHeader in pactTest.Response.Headers)
					{
						if (rawResponse.Content.Headers.TryGetValues(name: responseHeader.Key, values: out var headerValues))
						{
							var actualHeader = headerValues.First();

							if (actualHeader != responseHeader.Value)
							{
								throw new
									Exception(message:
														$"Expected response header {responseHeader.Key} with value {responseHeader.Value} does not match the actual {actualHeader}");
							}
						}
						else
						{
							throw new
								Exception(message:
													$"Expected response header {responseHeader.Key} with value {responseHeader.Value} does not exist in the response");
						}
					}
				}
			}
			else if (_aspNetCoreIntegrationTestData.ResponseHeaders != null)
			{
				if (rawResponse.Headers != null)
				{
					foreach (var responseHeader in _aspNetCoreIntegrationTestData.ResponseHeaders)
					{
						if (rawResponse.Content.Headers.TryGetValues(name: responseHeader.Key, values: out var contentHeaderValues))
						{
							var actualHeader = contentHeaderValues.First();

							if (actualHeader != responseHeader.Value)
							{
								throw new
									Exception(message:
														$"Expected response header {responseHeader.Key} with value {responseHeader.Value} does not match the actual {actualHeader}");
							}
						}
						else if (rawResponse.Headers.TryGetValues(name: responseHeader.Key, values: out var headerValues))
						{
							var actualHeader = headerValues.First();

							if (actualHeader != responseHeader.Value)
							{
								throw new
									Exception(message:
														$"Expected response header {responseHeader.Key} with value {responseHeader.Value} does not match the actual {actualHeader}");
							}
						}
						else
						{
							throw new
								Exception(message:
													$"Expected response header {responseHeader.Key} with value {responseHeader.Value} does not exist in the response");
						}
					}
				}
			}
		}

		/// <summary>
		/// Asserts the response status code.
		/// </summary>
		/// <param name="pactTest">The pact test.</param>
		/// <param name="rawResponse">The raw response.</param>
		/// <exception cref="System.Exception"></exception>
		/// <exception cref="System.Exception"></exception>
		/// <exception cref="Exception"></exception>
		/// <exception cref="Exception"></exception>
		private void AssertResponseStatusCode(PactTest pactTest, HttpResponseMessage rawResponse)
		{
			if (pactTest != null)
			{
				if ((int)rawResponse.StatusCode != pactTest.Response.Status)
				{
					throw new
						Exception(message:
											$"Expected response status code {pactTest.Response.Status} does not match the actual {rawResponse.StatusCode}");
				}
			}
			else if (rawResponse.StatusCode != _aspNetCoreIntegrationTestData.ResponseHttpStatusCode)
			{
				throw new
					Exception(message:
										$"Expected response status code {_aspNetCoreIntegrationTestData.ResponseHttpStatusCode} does not match the actual {rawResponse.StatusCode}");
			}
		}

		/// <summary>
		/// Cleanups this instance.
		/// </summary>
		private void Cleanup()
		{
			_aspNetCoreIntegrationTestData = new AspNetCoreIntegrationTestData();
		}

		/// <summary>
		/// Gets the HTTP request message.
		/// </summary>
		/// <param name="pactTest">The pact test.</param>
		/// <returns>HttpRequestMessage.</returns>
		private HttpRequestMessage GetHttpRequestMessage(PactTest pactTest)
		{
			HttpRequestMessage request;

			if (pactTest != null)
			{
				var pactMethod = ParsePactHttpMethod(method: pactTest.Request.Method);
				request = new HttpRequestMessage(method: pactMethod, requestUri: pactTest.Request.Path);

				if (pactTest.Request.Body != null)
				{
					request.Content = new StringContent(content: pactTest.Request.Body.ToString(), encoding: Encoding.UTF8,
																							mediaType: ApplicationJsonContentType);
				}

				if (pactTest.Request.Headers != null)
				{
					foreach (var requestHeader in pactTest.Request.Headers)
					{
						if (requestHeader.Key != "Content-Type")
						{
							request.Headers.Add(name: requestHeader.Key, value: requestHeader.Value);
						}
					}
				}
			}
			else
			{
				request = new HttpRequestMessage(method: _aspNetCoreIntegrationTestData.HttpMethod,
																				requestUri: _aspNetCoreIntegrationTestData.Route);

				if (_aspNetCoreIntegrationTestData.RequestContentType == ApplicationJsonContentType)
				{
					if (!string.IsNullOrWhiteSpace(value: _aspNetCoreIntegrationTestData.RequestBody))
					{
						request.Content = new StringContent(content: _aspNetCoreIntegrationTestData.RequestBody,
																								encoding: Encoding.UTF8, mediaType: ApplicationJsonContentType);
					}
				}
				else if (_aspNetCoreIntegrationTestData.RequestContentType == ApplicationTextContentType)
				{
					if (!string.IsNullOrWhiteSpace(value: _aspNetCoreIntegrationTestData.RequestBody))
					{
						request.Content = new StringContent(content: _aspNetCoreIntegrationTestData.RequestBody,
																								encoding: Encoding.UTF8, mediaType: ApplicationTextContentType);
					}
				}
				else if (_aspNetCoreIntegrationTestData.RequestBodyBytes != null
								&& _aspNetCoreIntegrationTestData.RequestContentType == ApplicationOctetStreamContentType)
				{
					request.Content = new ByteArrayContent(content: _aspNetCoreIntegrationTestData.RequestBodyBytes);
				}
				else if (_aspNetCoreIntegrationTestData.MultiPartFormData != null
								&& !string.IsNullOrWhiteSpace(value: _aspNetCoreIntegrationTestData.RequestContentType)
								&& _aspNetCoreIntegrationTestData.RequestContentType.StartsWith(
																																								value: MultipartFormContentType,
																																								comparisonType: StringComparison.Ordinal))
				{
					var multipartFormDataContent = new MultipartFormDataContent(boundary: _aspNetCoreIntegrationTestData.MultiPartFormData.Boundary)
					{
						{
							_aspNetCoreIntegrationTestData.MultiPartFormData.FormContent,
							_aspNetCoreIntegrationTestData.MultiPartFormData.Name,
							_aspNetCoreIntegrationTestData.MultiPartFormData.FileName
						}
					};

					request.Content = multipartFormDataContent;
				}

				if (!string.IsNullOrWhiteSpace(value: _aspNetCoreIntegrationTestData.AuthorizationHeader))
				{
					request.Headers.Add(name: Authorization, value: _aspNetCoreIntegrationTestData.AuthorizationHeader);
				}

				if (_aspNetCoreIntegrationTestData.RequestHeaders != null)
				{
					foreach (var requestHeader in _aspNetCoreIntegrationTestData.RequestHeaders)
					{
						if (requestHeader.Key != "Content-Type")
						{
							if (string.IsNullOrWhiteSpace(value: _aspNetCoreIntegrationTestData.AuthorizationHeader)
									|| requestHeader.Key != Authorization)
							{
								request.Headers.Add(name: requestHeader.Key, value: requestHeader.Value);
							}
						}
					}
				}
			}

			return request;
		}

		/// <summary>
		/// Gets the pact test.
		/// </summary>
		/// <returns>Task&lt;PactTest&gt;.</returns>
		/// <exception cref="System.IO.FileNotFoundException"></exception>
		/// <exception cref="System.Exception"></exception>
		/// <exception cref="FileNotFoundException"></exception>
		/// <exception cref="Exception"></exception>
		private async Task<PactTest> GetPactTest()
		{
			PactTest pactTest = null;

			if (!string.IsNullOrWhiteSpace(value: _aspNetCoreIntegrationTestData.PactFilePath)
					&& !string.IsNullOrWhiteSpace(value: _aspNetCoreIntegrationTestData.PactProviderState))
			{
				if (!File.Exists(path: _aspNetCoreIntegrationTestData.PactFilePath))
				{
					throw new FileNotFoundException(message: "Pact file is not found");
				}

				var pactFileContent = await File.ReadAllTextAsync(path: _aspNetCoreIntegrationTestData.PactFilePath);
				var pactFile = JsonConvert.DeserializeObject<PactFile>(value: pactFileContent);

				pactTest =
					pactFile.Interactions?.FirstOrDefault(predicate: c => c.ProviderState
																																== _aspNetCoreIntegrationTestData.PactProviderState);

				if (pactTest == null)
				{
					throw new
						Exception(message:
											$"Pact test with provider state {_aspNetCoreIntegrationTestData.PactProviderState} is not found");
				}
			}

			return pactTest;
		}

		/// <summary>
		/// Initializes this instance.
		/// </summary>
		private void Initialize()
		{
			if (_testServer == null)
			{
				_testServer = new TestServer(builder: _webHostBuilder);
			}

			if (_client == null)
			{
				_client = _testServer.CreateClient();
			}
		}

		/// <summary>
		/// Parses the pact HTTP method.
		/// </summary>
		/// <param name="method">The method.</param>
		/// <returns>System.Nullable&lt;HttpMethod&gt;.</returns>
		private HttpMethod? ParsePactHttpMethod(string method)
		{
			HttpMethod? httpMethod = null;

			switch (method.ToLower())
			{
				case "notset":
					break;
				case "get":
					httpMethod = HttpMethod.Get;

					break;
				case "post":
					httpMethod = HttpMethod.Post;

					break;
				case "put":
					httpMethod = HttpMethod.Put;

					break;
				case "delete":
					httpMethod = HttpMethod.Delete;

					break;
				case "head":
					httpMethod = HttpMethod.Head;

					break;
				case "patch":
					httpMethod = HttpMethod.Patch;

					break;
			}

			return httpMethod;
		}

		/// <summary>
		/// Sets the query parameters.
		/// </summary>
		/// <param name="pactTest">The pact test.</param>
		private void SetQueryParameters(PactTest pactTest)
		{
			if (pactTest != null)
			{
				if (!string.IsNullOrWhiteSpace(value: pactTest.Request.Query))
				{
					pactTest.Request.Path = AddQueryParameters(path: pactTest.Request.Path, pactTest.Request.Query);
				}
			}
			else if (!string.IsNullOrWhiteSpace(value: _aspNetCoreIntegrationTestData.QueryParameters))
			{
				_aspNetCoreIntegrationTestData.Route =
					AddQueryParameters(path: _aspNetCoreIntegrationTestData.Route,
														_aspNetCoreIntegrationTestData.QueryParameters);
			}
			else if (_aspNetCoreIntegrationTestData.QueryParametersTable != null)
			{
				foreach (var query in _aspNetCoreIntegrationTestData.QueryParametersTable)
				{
					_aspNetCoreIntegrationTestData.Route =
						AddQueryParameters(path: _aspNetCoreIntegrationTestData.Route, $"{query.Key}={query.Value}");
				}
			}
			else if (_aspNetCoreIntegrationTestData.QueryParametersList != null)
			{
				foreach (var query in _aspNetCoreIntegrationTestData.QueryParametersList)
				{
					_aspNetCoreIntegrationTestData.Route = AddQueryParameters(path: _aspNetCoreIntegrationTestData.Route, query);
				}
			}
		}

		/// <summary>
		/// Validates the method.
		/// </summary>
		/// <param name="pactTest">The pact test.</param>
		/// <exception cref="System.ArgumentNullException"></exception>
		/// <exception cref="System.ArgumentNullException"></exception>
		/// <exception cref="ArgumentNullException"></exception>
		/// <exception cref="ArgumentNullException"></exception>
		private void ValidateMethod(PactTest pactTest)
		{
			if (pactTest != null)
			{
				var pactMethod = ParsePactHttpMethod(method: pactTest.Request.Method);

				if (pactMethod == null)
				{
					throw new ArgumentNullException(paramName: "HttpMethod");
				}
			}
			else if (_aspNetCoreIntegrationTestData.HttpMethod == null)
			{
				throw new ArgumentNullException(paramName: "HttpMethod");
			}
		}

		/// <summary>
		/// Validates the response status code.
		/// </summary>
		/// <param name="pactTest">The pact test.</param>
		/// <exception cref="System.ArgumentNullException"></exception>
		/// <exception cref="ArgumentNullException"></exception>
		private void ValidateResponseStatusCode(PactTest pactTest)
		{
			// Validate response http status code
			if (pactTest == null && _aspNetCoreIntegrationTestData.ResponseHttpStatusCode == null)
			{
				throw new ArgumentNullException(paramName: "ResponseHttpStatusCode");
			}
		}

		/// <summary>
		/// Validates the route.
		/// </summary>
		/// <param name="pactTest">The pact test.</param>
		/// <exception cref="System.ArgumentNullException"></exception>
		/// <exception cref="System.ArgumentNullException"></exception>
		/// <exception cref="ArgumentNullException"></exception>
		/// <exception cref="ArgumentNullException"></exception>
		private void ValidateRoute(PactTest pactTest)
		{
			if (pactTest != null)
			{
				if (string.IsNullOrWhiteSpace(value: pactTest.Request.Path))
				{
					throw new ArgumentNullException(paramName: "Route");
				}
			}
			else if (string.IsNullOrWhiteSpace(value: _aspNetCoreIntegrationTestData.Route))
			{
				throw new ArgumentNullException(paramName: "Route");
			}
		}
	}

	/// <summary>
	/// Class AspNetCoreIntegrationTestData.
	/// </summary>
	internal class AspNetCoreIntegrationTestData
	{
		/// <summary>
		/// Gets or sets the authorization header.
		/// </summary>
		/// <value>The authorization header.</value>
		public string AuthorizationHeader { get; set; }

		/// <summary>
		/// Gets or sets the custom response comparer.
		/// </summary>
		/// <value>The custom response comparer.</value>
		public Func<string, string, string> CustomResponseComparer { get; set; }

		/// <summary>
		/// Gets or sets the file content result.
		/// </summary>
		/// <value>The file content result.</value>
		public FileContentResult FileContentResult { get; set; }

		/// <summary>
		/// Gets or sets the HTTP method.
		/// </summary>
		/// <value>The HTTP method.</value>
		public HttpMethod? HttpMethod { get; set; }

		/// <summary>
		/// Gets or sets the multi part form data.
		/// </summary>
		/// <value>The multi part form data.</value>
		public FormData MultiPartFormData { get; set; }

		/// <summary>
		/// Gets or sets the pact file path.
		/// </summary>
		/// <value>The pact file path.</value>
		public string PactFilePath { get; set; }

		/// <summary>
		/// Gets or sets the state of the pact provider.
		/// </summary>
		/// <value>The state of the pact provider.</value>
		public string PactProviderState { get; set; }

		/// <summary>
		/// Gets or sets the query parameters.
		/// </summary>
		/// <value>The query parameters.</value>
		public string QueryParameters { get; set; }

		/// <summary>
		/// Gets or sets the query parameters list.
		/// </summary>
		/// <value>The query parameters list.</value>
		public List<string> QueryParametersList { get; set; }

		/// <summary>
		/// Gets or sets the query parameters table.
		/// </summary>
		/// <value>The query parameters table.</value>
		public Dictionary<string, string> QueryParametersTable { get; set; }

		/// <summary>
		/// Gets or sets the request body.
		/// </summary>
		/// <value>The request body.</value>
		public string RequestBody { get; set; }

		/// <summary>
		/// Gets or sets the request body bytes.
		/// </summary>
		/// <value>The request body bytes.</value>
		public byte[] RequestBodyBytes { get; set; }

		/// <summary>
		/// Gets or sets the type of the request content.
		/// </summary>
		/// <value>The type of the request content.</value>
		public string RequestContentType { get; set; }

		/// <summary>
		/// Gets or sets the request form body.
		/// </summary>
		/// <value>The request form body.</value>
		public string RequestFormBody { get; set; }

		/// <summary>
		/// Gets or sets the request headers.
		/// </summary>
		/// <value>The request headers.</value>
		public Dictionary<string, string> RequestHeaders { get; set; }

		/// <summary>
		/// Gets or sets the response body.
		/// </summary>
		/// <value>The response body.</value>
		public string ResponseBody { get; set; }

		/// <summary>
		/// Gets or sets the response body bytes.
		/// </summary>
		/// <value>The response body bytes.</value>
		public byte[] ResponseBodyBytes { get; set; }

		/// <summary>
		/// Gets or sets the type of the response content.
		/// </summary>
		/// <value>The type of the response content.</value>
		public string ResponseContentType { get; set; }

		/// <summary>
		/// Gets or sets the response headers.
		/// </summary>
		/// <value>The response headers.</value>
		public Dictionary<string, string> ResponseHeaders { get; set; }

		/// <summary>
		/// Gets or sets the response HTTP status code.
		/// </summary>
		/// <value>The response HTTP status code.</value>
		public HttpStatusCode? ResponseHttpStatusCode { get; set; }

		/// <summary>
		/// Gets or sets the route.
		/// </summary>
		/// <value>The route.</value>
		public string Route { get; set; }
	}

	/// <summary>
	/// Class PactFile.
	/// </summary>
	internal class PactFile
	{
		/// <summary>
		/// Gets or sets the consumer.
		/// </summary>
		/// <value>The consumer.</value>
		[JsonProperty(PropertyName = "consumer", NullValueHandling = NullValueHandling.Ignore)]
		public PactProviderOrConsumer Consumer { get; set; }

		/// <summary>
		/// Gets or sets the interactions.
		/// </summary>
		/// <value>The interactions.</value>
		[JsonProperty(PropertyName = "interactions", NullValueHandling = NullValueHandling.Ignore)]
		public List<PactTest> Interactions { get; set; }

		/// <summary>
		/// Gets or sets the provider.
		/// </summary>
		/// <value>The provider.</value>
		[JsonProperty(PropertyName = "provider", NullValueHandling = NullValueHandling.Ignore)]
		public PactProviderOrConsumer Provider { get; set; }
	}

	/// <summary>
	/// Class PactProviderOrConsumer.
	/// </summary>
	internal class PactProviderOrConsumer
	{
		/// <summary>
		/// Gets or sets the name.
		/// </summary>
		/// <value>The name.</value>
		[JsonProperty(PropertyName = "name", NullValueHandling = NullValueHandling.Ignore)]
		public string Name { get; set; }
	}

	/// <summary>
	/// Class PactTest.
	/// </summary>
	internal class PactTest
	{
		/// <summary>
		/// Gets or sets the description.
		/// </summary>
		/// <value>The description.</value>
		[JsonProperty(PropertyName = "description", NullValueHandling = NullValueHandling.Ignore)]
		public string Description { get; set; }

		/// <summary>
		/// Gets or sets the state of the provider.
		/// </summary>
		/// <value>The state of the provider.</value>
		[JsonProperty(PropertyName = "provider_state", NullValueHandling = NullValueHandling.Ignore)]
		public string ProviderState { get; set; }

		/// <summary>
		/// Gets or sets the request.
		/// </summary>
		/// <value>The request.</value>
		[JsonProperty(PropertyName = "request", NullValueHandling = NullValueHandling.Ignore)]
		public PactTestRequest Request { get; set; }

		/// <summary>
		/// Gets or sets the response.
		/// </summary>
		/// <value>The response.</value>
		[JsonProperty(PropertyName = "response", NullValueHandling = NullValueHandling.Ignore)]
		public PactTestResponse Response { get; set; }
	}

	/// <summary>
	/// Class PactTestRequest.
	/// </summary>
	internal class PactTestRequest
	{
		/// <summary>
		/// Gets or sets the body.
		/// </summary>
		/// <value>The body.</value>
		[JsonProperty(PropertyName = "body", NullValueHandling = NullValueHandling.Ignore)]
		public dynamic Body { get; set; }

		/// <summary>
		/// Gets or sets the headers.
		/// </summary>
		/// <value>The headers.</value>
		[JsonProperty(PropertyName = "headers", NullValueHandling = NullValueHandling.Ignore)]
		public Dictionary<string, string> Headers { get; set; }

		/// <summary>
		/// Gets or sets the method.
		/// </summary>
		/// <value>The method.</value>
		[JsonProperty(PropertyName = "method", NullValueHandling = NullValueHandling.Ignore)]
		public string Method { get; set; }

		/// <summary>
		/// Gets or sets the path.
		/// </summary>
		/// <value>The path.</value>
		[JsonProperty(PropertyName = "path", NullValueHandling = NullValueHandling.Ignore)]
		public string Path { get; set; }

		/// <summary>
		/// Gets or sets the query.
		/// </summary>
		/// <value>The query.</value>
		[JsonProperty(PropertyName = "query", NullValueHandling = NullValueHandling.Ignore)]
		public string Query { get; set; }
	}

	/// <summary>
	/// Class PactTestResponse.
	/// </summary>
	internal class PactTestResponse
	{
		/// <summary>
		/// Gets or sets the body.
		/// </summary>
		/// <value>The body.</value>
		[JsonProperty(PropertyName = "body", NullValueHandling = NullValueHandling.Ignore)]
		public dynamic Body { get; set; }

		/// <summary>
		/// Gets or sets the headers.
		/// </summary>
		/// <value>The headers.</value>
		[JsonProperty(PropertyName = "headers", NullValueHandling = NullValueHandling.Ignore)]
		public Dictionary<string, string> Headers { get; set; }

		/// <summary>
		/// Gets or sets the status.
		/// </summary>
		/// <value>The status.</value>
		[JsonProperty(PropertyName = "status", NullValueHandling = NullValueHandling.Ignore)]
		public int Status { get; set; }
	}
}
