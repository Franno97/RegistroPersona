
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Mre.Sb.PersonRegistration.HttpApiClient
{
    [System.CodeDom.Compiler.GeneratedCode("NSwag", "13.14.4.0 (NJsonSchema v10.5.2.0 (Newtonsoft.Json v11.0.0.0))")]
    public partial class IdentidadClient : IIdentidadClient
    {
        private string _baseUrl = "";
        private System.Net.Http.HttpClient _httpClient;
        private System.Lazy<System.Text.Json.JsonSerializerOptions> _settings;

        public IdentidadClient(System.Net.Http.HttpClient httpClient)
        {
            //BaseUrl = baseUrl;
            _httpClient = httpClient;
            _settings = new System.Lazy<System.Text.Json.JsonSerializerOptions>(CreateSerializerSettings);
        }

        public void SetAccessToken(string accessToken)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        }

        private System.Text.Json.JsonSerializerOptions CreateSerializerSettings()
        {
            var settings = new System.Text.Json.JsonSerializerOptions();
            UpdateJsonSerializerSettings(settings);
            return settings;
        }

        public string BaseUrl
        {
            get { return _baseUrl; }
            set { _baseUrl = value; }
        }

        protected System.Text.Json.JsonSerializerOptions JsonSerializerSettings { get { return _settings.Value; } }

        partial void UpdateJsonSerializerSettings(System.Text.Json.JsonSerializerOptions settings);


        partial void PrepareRequest(System.Net.Http.HttpClient client, System.Net.Http.HttpRequestMessage request, string url);
        partial void PrepareRequest(System.Net.Http.HttpClient client, System.Net.Http.HttpRequestMessage request, System.Text.StringBuilder urlBuilder);
        partial void ProcessResponse(System.Net.Http.HttpClient client, System.Net.Http.HttpResponseMessage response);
        /// <returns>Success</returns>
        /// <exception cref="ApiException">A server side error occurred.</exception>
        public System.Threading.Tasks.Task IdentidadConfiguracionPostAsync(ActualizarIdentidadConfiguracionDtoDto body)
        {
            return IdentidadConfiguracionPostAsync(body, System.Threading.CancellationToken.None);
        }

        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>Success</returns>
        /// <exception cref="ApiException">A server side error occurred.</exception>
        public async System.Threading.Tasks.Task IdentidadConfiguracionPostAsync(ActualizarIdentidadConfiguracionDtoDto body, System.Threading.CancellationToken cancellationToken)
        {
            var urlBuilder_ = new System.Text.StringBuilder();
            urlBuilder_.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "").Append("/api/identidad/identidad-configuracion");

            var client_ = _httpClient;
            var disposeClient_ = false;
            try
            {
                using (var request_ = new System.Net.Http.HttpRequestMessage())
                {
                    var content_ = new System.Net.Http.StringContent(System.Text.Json.JsonSerializer.Serialize(body, _settings.Value));
                    content_.Headers.ContentType = System.Net.Http.Headers.MediaTypeHeaderValue.Parse("application/json");
                    request_.Content = content_;
                    request_.Method = new System.Net.Http.HttpMethod("POST");

                    PrepareRequest(client_, request_, urlBuilder_);

                    var url_ = urlBuilder_.ToString();
                    request_.RequestUri = new System.Uri(url_, System.UriKind.RelativeOrAbsolute);

                    PrepareRequest(client_, request_, url_);

                    var response_ = await client_.SendAsync(request_, System.Net.Http.HttpCompletionOption.ResponseHeadersRead, cancellationToken).ConfigureAwait(false);
                    var disposeResponse_ = true;
                    try
                    {
                        var headers_ = System.Linq.Enumerable.ToDictionary(response_.Headers, h_ => h_.Key, h_ => h_.Value);
                        if (response_.Content != null && response_.Content.Headers != null)
                        {
                            foreach (var item_ in response_.Content.Headers)
                                headers_[item_.Key] = item_.Value;
                        }

                        ProcessResponse(client_, response_);

                        var status_ = (int)response_.StatusCode;
                        if (status_ == 200)
                        {
                            return;
                        }
                        else
                        if (status_ == 403)
                        {
                            var objectResponse_ = await ReadObjectResponseAsync<RemoteServiceErrorResponse>(response_, headers_, cancellationToken).ConfigureAwait(false);

                            //if (objectResponse_.Object == null)
                            //{
                            //    throw new ApiException("Respuesta was null which was not expected.", status_, objectResponse_.Text, headers_, null);
                            //}

                            throw new ApiException<RemoteServiceErrorResponse>("Forbidden", status_, objectResponse_.Text, headers_, objectResponse_.Object, null);
                        }
                        else
                        if (status_ == 401)
                        {
                            var objectResponse_ = await ReadObjectResponseAsync<RemoteServiceErrorResponse>(response_, headers_, cancellationToken).ConfigureAwait(false);

                            //if (objectResponse_.Object == null)
                            //{
                            //    throw new ApiException("Respuesta was null which was not expected.", status_, objectResponse_.Text, headers_, null);
                            //}

                            throw new ApiException<RemoteServiceErrorResponse>("Unauthorized", status_, objectResponse_.Text, headers_, objectResponse_.Object, null);
                        }
                        else
                        if (status_ == 400)
                        {
                            var objectResponse_ = await ReadObjectResponseAsync<RemoteServiceErrorResponse>(response_, headers_, cancellationToken).ConfigureAwait(false);

                            //if (objectResponse_.Object == null)
                            //{
                            //    throw new ApiException("Respuesta was null which was not expected.", status_, objectResponse_.Text, headers_, null);
                            //}

                            throw new ApiException<RemoteServiceErrorResponse>("Bad Request", status_, objectResponse_.Text, headers_, objectResponse_.Object, null);
                        }
                        else
                        if (status_ == 404)
                        {
                            var objectResponse_ = await ReadObjectResponseAsync<RemoteServiceErrorResponse>(response_, headers_, cancellationToken).ConfigureAwait(false);

                            //if (objectResponse_.Object == null)
                            //{
                            //    throw new ApiException("Respuesta was null which was not expected.", status_, objectResponse_.Text, headers_, null);
                            //}

                            throw new ApiException<RemoteServiceErrorResponse>("Not Found", status_, objectResponse_.Text, headers_, objectResponse_.Object, null);
                        }
                        else
                        if (status_ == 501)
                        {
                            var objectResponse_ = await ReadObjectResponseAsync<RemoteServiceErrorResponse>(response_, headers_, cancellationToken).ConfigureAwait(false);

                            //if (objectResponse_.Object == null)
                            //{
                            //    throw new ApiException("Respuesta was null which was not expected.", status_, objectResponse_.Text, headers_, null);
                            //}

                            throw new ApiException<RemoteServiceErrorResponse>("Server Error", status_, objectResponse_.Text, headers_, objectResponse_.Object, null);
                        }
                        else
                        if (status_ == 500)
                        {
                            var objectResponse_ = await ReadObjectResponseAsync<RemoteServiceErrorResponse>(response_, headers_, cancellationToken).ConfigureAwait(false);

                            //if (objectResponse_.Object == null)
                            //{
                            //    throw new ApiException("Respuesta was null which was not expected.", status_, objectResponse_.Text, headers_, null);
                            //}

                            throw new ApiException<RemoteServiceErrorResponse>("Server Error", status_, objectResponse_.Text, headers_, objectResponse_.Object, null);
                        }
                        else
                        {
                            var responseData_ = response_.Content == null ? null : await response_.Content.ReadAsStringAsync().ConfigureAwait(false);
                            throw new ApiException("The HTTP status code of the response was not expected (" + status_ + ").", status_, responseData_, headers_, null);
                        }
                    }
                    finally
                    {
                        if (disposeResponse_)
                            response_.Dispose();
                    }
                }
            }
            finally
            {
                if (disposeClient_)
                    client_.Dispose();
            }
        }

        /// <returns>Success</returns>
        /// <exception cref="ApiException">A server side error occurred.</exception>
        public System.Threading.Tasks.Task<IdentidadConfiguracionDto> IdentidadConfiguracionGetAsync()
        {
            return IdentidadConfiguracionGetAsync(System.Threading.CancellationToken.None);
        }

        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>Success</returns>
        /// <exception cref="ApiException">A server side error occurred.</exception>
        public async System.Threading.Tasks.Task<IdentidadConfiguracionDto> IdentidadConfiguracionGetAsync(System.Threading.CancellationToken cancellationToken)
        {
            var urlBuilder_ = new System.Text.StringBuilder();
            urlBuilder_.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "").Append("/api/identidad/identidad-configuracion");

            var client_ = _httpClient;
            var disposeClient_ = false;
            try
            {
                using (var request_ = new System.Net.Http.HttpRequestMessage())
                {
                    request_.Method = new System.Net.Http.HttpMethod("GET");
                    request_.Headers.Accept.Add(System.Net.Http.Headers.MediaTypeWithQualityHeaderValue.Parse("text/plain"));

                    PrepareRequest(client_, request_, urlBuilder_);

                    var url_ = urlBuilder_.ToString();
                    request_.RequestUri = new System.Uri(url_, System.UriKind.RelativeOrAbsolute);

                    PrepareRequest(client_, request_, url_);

                    var response_ = await client_.SendAsync(request_, System.Net.Http.HttpCompletionOption.ResponseHeadersRead, cancellationToken).ConfigureAwait(false);
                    var disposeResponse_ = true;
                    try
                    {
                        var headers_ = System.Linq.Enumerable.ToDictionary(response_.Headers, h_ => h_.Key, h_ => h_.Value);
                        if (response_.Content != null && response_.Content.Headers != null)
                        {
                            foreach (var item_ in response_.Content.Headers)
                                headers_[item_.Key] = item_.Value;
                        }

                        ProcessResponse(client_, response_);

                        var status_ = (int)response_.StatusCode;
                        if (status_ == 200)
                        {
                            var objectResponse_ = await ReadObjectResponseAsync<IdentidadConfiguracionDto>(response_, headers_, cancellationToken).ConfigureAwait(false);

                            //if (objectResponse_.Object == null)
                            //{
                            //    throw new ApiException("Respuesta was null which was not expected.", status_, objectResponse_.Text, headers_, null);
                            //}

                            return objectResponse_.Object;
                        }
                        else
                        if (status_ == 403)
                        {
                            var objectResponse_ = await ReadObjectResponseAsync<RemoteServiceErrorResponse>(response_, headers_, cancellationToken).ConfigureAwait(false);

                            //if (objectResponse_.Object == null)
                            //{
                            //    throw new ApiException("Respuesta was null which was not expected.", status_, objectResponse_.Text, headers_, null);
                            //}

                            throw new ApiException<RemoteServiceErrorResponse>("Forbidden", status_, objectResponse_.Text, headers_, objectResponse_.Object, null);
                        }
                        else
                        if (status_ == 401)
                        {
                            var objectResponse_ = await ReadObjectResponseAsync<RemoteServiceErrorResponse>(response_, headers_, cancellationToken).ConfigureAwait(false);

                            //if (objectResponse_.Object == null)
                            //{
                            //    throw new ApiException("Respuesta was null which was not expected.", status_, objectResponse_.Text, headers_, null);
                            //}

                            throw new ApiException<RemoteServiceErrorResponse>("Unauthorized", status_, objectResponse_.Text, headers_, objectResponse_.Object, null);
                        }
                        else
                        if (status_ == 400)
                        {
                            var objectResponse_ = await ReadObjectResponseAsync<RemoteServiceErrorResponse>(response_, headers_, cancellationToken).ConfigureAwait(false);

                            //if (objectResponse_.Object == null)
                            //{
                            //    throw new ApiException("Respuesta was null which was not expected.", status_, objectResponse_.Text, headers_, null);
                            //}

                            throw new ApiException<RemoteServiceErrorResponse>("Bad Request", status_, objectResponse_.Text, headers_, objectResponse_.Object, null);
                        }
                        else
                        if (status_ == 404)
                        {
                            var objectResponse_ = await ReadObjectResponseAsync<RemoteServiceErrorResponse>(response_, headers_, cancellationToken).ConfigureAwait(false);

                            //if (objectResponse_.Object == null)
                            //{
                            //    throw new ApiException("Respuesta was null which was not expected.", status_, objectResponse_.Text, headers_, null);
                            //}

                            throw new ApiException<RemoteServiceErrorResponse>("Not Found", status_, objectResponse_.Text, headers_, objectResponse_.Object, null);
                        }
                        else
                        if (status_ == 501)
                        {
                            var objectResponse_ = await ReadObjectResponseAsync<RemoteServiceErrorResponse>(response_, headers_, cancellationToken).ConfigureAwait(false);

                            //if (objectResponse_.Object == null)
                            //{
                            //    throw new ApiException("Respuesta was null which was not expected.", status_, objectResponse_.Text, headers_, null);
                            //}

                            throw new ApiException<RemoteServiceErrorResponse>("Server Error", status_, objectResponse_.Text, headers_, objectResponse_.Object, null);
                        }
                        else
                        if (status_ == 500)
                        {
                            var objectResponse_ = await ReadObjectResponseAsync<RemoteServiceErrorResponse>(response_, headers_, cancellationToken).ConfigureAwait(false);

                            //if (objectResponse_.Object == null)
                            //{
                            //    throw new ApiException("Respuesta was null which was not expected.", status_, objectResponse_.Text, headers_, null);
                            //}

                            throw new ApiException<RemoteServiceErrorResponse>("Server Error", status_, objectResponse_.Text, headers_, objectResponse_.Object, null);
                        }
                        else
                        {
                            var responseData_ = response_.Content == null ? null : await response_.Content.ReadAsStringAsync().ConfigureAwait(false);
                            throw new ApiException("The HTTP status code of the response was not expected (" + status_ + ").", status_, responseData_, headers_, null);
                        }
                    }
                    finally
                    {
                        if (disposeResponse_)
                            response_.Dispose();
                    }
                }
            }
            finally
            {
                if (disposeClient_)
                    client_.Dispose();
            }
        }

        /// <returns>Success</returns>
        /// <exception cref="ApiException">A server side error occurred.</exception>
        public System.Threading.Tasks.Task<IdentityUserDto> UsuarioPostAsync(UsuarioCrearDto body)
        {
            return UsuarioPostAsync(body, System.Threading.CancellationToken.None);
        }

        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>Success</returns>
        /// <exception cref="ApiException">A server side error occurred.</exception>
        public async System.Threading.Tasks.Task<IdentityUserDto> UsuarioPostAsync(UsuarioCrearDto body, System.Threading.CancellationToken cancellationToken)
        {
            var urlBuilder_ = new System.Text.StringBuilder();
            urlBuilder_.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "").Append("/api/identidad/usuario");

            var client_ = _httpClient;
            var disposeClient_ = false;
            try
            {
                using (var request_ = new System.Net.Http.HttpRequestMessage())
                {
                    var content_ = new System.Net.Http.StringContent(System.Text.Json.JsonSerializer.Serialize(body, _settings.Value));
                    content_.Headers.ContentType = System.Net.Http.Headers.MediaTypeHeaderValue.Parse("application/json");
                    request_.Content = content_;
                    request_.Method = new System.Net.Http.HttpMethod("POST");
                    request_.Headers.Accept.Add(System.Net.Http.Headers.MediaTypeWithQualityHeaderValue.Parse("text/plain"));

                    PrepareRequest(client_, request_, urlBuilder_);

                    var url_ = urlBuilder_.ToString();
                    request_.RequestUri = new System.Uri(url_, System.UriKind.RelativeOrAbsolute);

                    PrepareRequest(client_, request_, url_);

                    var response_ = await client_.SendAsync(request_, System.Net.Http.HttpCompletionOption.ResponseHeadersRead, cancellationToken).ConfigureAwait(false);
                    var disposeResponse_ = true;
                    try
                    {
                        var headers_ = System.Linq.Enumerable.ToDictionary(response_.Headers, h_ => h_.Key, h_ => h_.Value);
                        if (response_.Content != null && response_.Content.Headers != null)
                        {
                            foreach (var item_ in response_.Content.Headers)
                                headers_[item_.Key] = item_.Value;
                        }

                        ProcessResponse(client_, response_);

                        var status_ = (int)response_.StatusCode;
                        if (status_ == 200)
                        {
                            var objectResponse_ = await ReadObjectResponseAsync<IdentityUserDto>(response_, headers_, cancellationToken).ConfigureAwait(false);

                            //if (objectResponse_.Object == null)
                            //{
                            //    throw new ApiException("Respuesta was null which was not expected.", status_, objectResponse_.Text, headers_, null);
                            //}

                            return objectResponse_.Object;
                        }
                        else
                        if (status_ == 403)
                        {
                            var objectResponse_ = await ReadObjectResponseAsync<RemoteServiceErrorResponse>(response_, headers_, cancellationToken).ConfigureAwait(false);

                            //if (objectResponse_.Object == null)
                            //{
                            //    throw new ApiException("Respuesta was null which was not expected.", status_, objectResponse_.Text, headers_, null);
                            //}

                            throw new ApiException<RemoteServiceErrorResponse>("Forbidden", status_, objectResponse_.Text, headers_, objectResponse_.Object, null);
                        }
                        else
                        if (status_ == 401)
                        {
                            var objectResponse_ = await ReadObjectResponseAsync<RemoteServiceErrorResponse>(response_, headers_, cancellationToken).ConfigureAwait(false);

                            //if (objectResponse_.Object == null)
                            //{
                            //    throw new ApiException("Respuesta was null which was not expected.", status_, objectResponse_.Text, headers_, null);
                            //}

                            throw new ApiException<RemoteServiceErrorResponse>("Unauthorized", status_, objectResponse_.Text, headers_, objectResponse_.Object, null);
                        }
                        else
                        if (status_ == 400)
                        {
                            var objectResponse_ = await ReadObjectResponseAsync<RemoteServiceErrorResponse>(response_, headers_, cancellationToken).ConfigureAwait(false);

                            //if (objectResponse_.Object == null)
                            //{
                            //    throw new ApiException("Respuesta was null which was not expected.", status_, objectResponse_.Text, headers_, null);
                            //}

                            throw new ApiException<RemoteServiceErrorResponse>("Bad Request", status_, objectResponse_.Text, headers_, objectResponse_.Object, null);
                        }
                        else
                        if (status_ == 404)
                        {
                            var objectResponse_ = await ReadObjectResponseAsync<RemoteServiceErrorResponse>(response_, headers_, cancellationToken).ConfigureAwait(false);

                            //if (objectResponse_.Object == null)
                            //{
                            //    throw new ApiException("Respuesta was null which was not expected.", status_, objectResponse_.Text, headers_, null);
                            //}

                            throw new ApiException<RemoteServiceErrorResponse>("Not Found", status_, objectResponse_.Text, headers_, objectResponse_.Object, null);
                        }
                        else
                        if (status_ == 501)
                        {
                            var objectResponse_ = await ReadObjectResponseAsync<RemoteServiceErrorResponse>(response_, headers_, cancellationToken).ConfigureAwait(false);

                            //if (objectResponse_.Object == null)
                            //{
                            //    throw new ApiException("Respuesta was null which was not expected.", status_, objectResponse_.Text, headers_, null);
                            //}

                            throw new ApiException<RemoteServiceErrorResponse>("Server Error", status_, objectResponse_.Text, headers_, objectResponse_.Object, null);
                        }
                        else
                        if (status_ == 500)
                        {
                            var objectResponse_ = await ReadObjectResponseAsync<RemoteServiceErrorResponse>(response_, headers_, cancellationToken).ConfigureAwait(false);

                            //if (objectResponse_.Object == null)
                            //{
                            //    throw new ApiException("Respuesta was null which was not expected.", status_, objectResponse_.Text, headers_, null);
                            //}

                            throw new ApiException<RemoteServiceErrorResponse>("Server Error", status_, objectResponse_.Text, headers_, objectResponse_.Object, null);
                        }
                        else
                        {
                            var responseData_ = response_.Content == null ? null : await response_.Content.ReadAsStringAsync().ConfigureAwait(false);
                            throw new ApiException("The HTTP status code of the response was not expected (" + status_ + ").", status_, responseData_, headers_, null);
                        }
                    }
                    finally
                    {
                        if (disposeResponse_)
                            response_.Dispose();
                    }
                }
            }
            finally
            {
                if (disposeClient_)
                    client_.Dispose();
            }
        }

        /// <returns>Success</returns>
        /// <exception cref="ApiException">A server side error occurred.</exception>
        public System.Threading.Tasks.Task<System.Collections.Generic.ICollection<IdentityUserDto>> UsuarioGetAsync(System.Collections.Generic.IEnumerable<System.Guid> input)
        {
            return UsuarioGetAsync(input, System.Threading.CancellationToken.None);
        }

        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>Success</returns>
        /// <exception cref="ApiException">A server side error occurred.</exception>
        public async System.Threading.Tasks.Task<System.Collections.Generic.ICollection<IdentityUserDto>> UsuarioGetAsync(System.Collections.Generic.IEnumerable<System.Guid> input, System.Threading.CancellationToken cancellationToken)
        {
            var urlBuilder_ = new System.Text.StringBuilder();
            urlBuilder_.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "").Append("/api/identidad/usuario?");
            if (input != null)
            {
                foreach (var item_ in input) { urlBuilder_.Append(System.Uri.EscapeDataString("input") + "=").Append(System.Uri.EscapeDataString(ConvertToString(item_, System.Globalization.CultureInfo.InvariantCulture))).Append("&"); }
            }
            urlBuilder_.Length--;

            var client_ = _httpClient;
            var disposeClient_ = false;
            try
            {
                using (var request_ = new System.Net.Http.HttpRequestMessage())
                {
                    request_.Method = new System.Net.Http.HttpMethod("GET");
                    request_.Headers.Accept.Add(System.Net.Http.Headers.MediaTypeWithQualityHeaderValue.Parse("text/plain"));

                    PrepareRequest(client_, request_, urlBuilder_);

                    var url_ = urlBuilder_.ToString();
                    request_.RequestUri = new System.Uri(url_, System.UriKind.RelativeOrAbsolute);

                    PrepareRequest(client_, request_, url_);

                    var response_ = await client_.SendAsync(request_, System.Net.Http.HttpCompletionOption.ResponseHeadersRead, cancellationToken).ConfigureAwait(false);
                    var disposeResponse_ = true;
                    try
                    {
                        var headers_ = System.Linq.Enumerable.ToDictionary(response_.Headers, h_ => h_.Key, h_ => h_.Value);
                        if (response_.Content != null && response_.Content.Headers != null)
                        {
                            foreach (var item_ in response_.Content.Headers)
                                headers_[item_.Key] = item_.Value;
                        }

                        ProcessResponse(client_, response_);

                        var status_ = (int)response_.StatusCode;
                        if (status_ == 200)
                        {
                            var objectResponse_ = await ReadObjectResponseAsync<System.Collections.Generic.ICollection<IdentityUserDto>>(response_, headers_, cancellationToken).ConfigureAwait(false);

                            //if (objectResponse_.Object == null)
                            //{
                            //    throw new ApiException("Respuesta was null which was not expected.", status_, objectResponse_.Text, headers_, null);
                            //}

                            return objectResponse_.Object;
                        }
                        else
                        if (status_ == 403)
                        {
                            var objectResponse_ = await ReadObjectResponseAsync<RemoteServiceErrorResponse>(response_, headers_, cancellationToken).ConfigureAwait(false);

                            //if (objectResponse_.Object == null)
                            //{
                            //    throw new ApiException("Respuesta was null which was not expected.", status_, objectResponse_.Text, headers_, null);
                            //}

                            throw new ApiException<RemoteServiceErrorResponse>("Forbidden", status_, objectResponse_.Text, headers_, objectResponse_.Object, null);
                        }
                        else
                        if (status_ == 401)
                        {
                            var objectResponse_ = await ReadObjectResponseAsync<RemoteServiceErrorResponse>(response_, headers_, cancellationToken).ConfigureAwait(false);

                            //if (objectResponse_.Object == null)
                            //{
                            //    throw new ApiException("Respuesta was null which was not expected.", status_, objectResponse_.Text, headers_, null);
                            //}

                            throw new ApiException<RemoteServiceErrorResponse>("Unauthorized", status_, objectResponse_.Text, headers_, objectResponse_.Object, null);
                        }
                        else
                        if (status_ == 400)
                        {
                            var objectResponse_ = await ReadObjectResponseAsync<RemoteServiceErrorResponse>(response_, headers_, cancellationToken).ConfigureAwait(false);

                            //if (objectResponse_.Object == null)
                            //{
                            //    throw new ApiException("Respuesta was null which was not expected.", status_, objectResponse_.Text, headers_, null);
                            //}

                            throw new ApiException<RemoteServiceErrorResponse>("Bad Request", status_, objectResponse_.Text, headers_, objectResponse_.Object, null);
                        }
                        else
                        if (status_ == 404)
                        {
                            var objectResponse_ = await ReadObjectResponseAsync<RemoteServiceErrorResponse>(response_, headers_, cancellationToken).ConfigureAwait(false);

                            //if (objectResponse_.Object == null)
                            //{
                            //    throw new ApiException("Respuesta was null which was not expected.", status_, objectResponse_.Text, headers_, null);
                            //}

                            throw new ApiException<RemoteServiceErrorResponse>("Not Found", status_, objectResponse_.Text, headers_, objectResponse_.Object, null);
                        }
                        else
                        if (status_ == 501)
                        {
                            var objectResponse_ = await ReadObjectResponseAsync<RemoteServiceErrorResponse>(response_, headers_, cancellationToken).ConfigureAwait(false);

                            //if (objectResponse_.Object == null)
                            //{
                            //    throw new ApiException("Respuesta was null which was not expected.", status_, objectResponse_.Text, headers_, null);
                            //}

                            throw new ApiException<RemoteServiceErrorResponse>("Server Error", status_, objectResponse_.Text, headers_, objectResponse_.Object, null);
                        }
                        else
                        if (status_ == 500)
                        {
                            var objectResponse_ = await ReadObjectResponseAsync<RemoteServiceErrorResponse>(response_, headers_, cancellationToken).ConfigureAwait(false);

                            //if (objectResponse_.Object == null)
                            //{
                            //    throw new ApiException("Respuesta was null which was not expected.", status_, objectResponse_.Text, headers_, null);
                            //}

                            throw new ApiException<RemoteServiceErrorResponse>("Server Error", status_, objectResponse_.Text, headers_, objectResponse_.Object, null);
                        }
                        else
                        {
                            var responseData_ = response_.Content == null ? null : await response_.Content.ReadAsStringAsync().ConfigureAwait(false);
                            throw new ApiException("The HTTP status code of the response was not expected (" + status_ + ").", status_, responseData_, headers_, null);
                        }
                    }
                    finally
                    {
                        if (disposeResponse_)
                            response_.Dispose();
                    }
                }
            }
            finally
            {
                if (disposeClient_)
                    client_.Dispose();
            }
        }

        protected struct ObjectResponseResult<T>
        {
            public ObjectResponseResult(T responseObject, string responseText)
            {
                this.Object = responseObject;
                this.Text = responseText;
            }

            public T Object { get; }

            public string Text { get; }
        }

        public bool ReadResponseAsString { get; set; }

        protected virtual async System.Threading.Tasks.Task<ObjectResponseResult<T>> ReadObjectResponseAsync<T>(System.Net.Http.HttpResponseMessage response, System.Collections.Generic.IReadOnlyDictionary<string, System.Collections.Generic.IEnumerable<string>> headers, System.Threading.CancellationToken cancellationToken)
        {
            if (response == null || response.Content == null)
            {
                return new ObjectResponseResult<T>(default(T), string.Empty);
            }

            if (ReadResponseAsString)
            {
                var responseText = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                try
                {
                    var typedBody = System.Text.Json.JsonSerializer.Deserialize<T>(responseText, JsonSerializerSettings);
                    return new ObjectResponseResult<T>(typedBody, responseText);
                }
                catch (Newtonsoft.Json.JsonException exception)
                {
                    var message = "Could not deserialize the response body string as " + typeof(T).FullName + ".";
                    throw new ApiException(message, (int)response.StatusCode, responseText, headers, exception);
                }
            }
            else
            {
                try
                {
                    using (var responseStream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false))
                    {
                        var typedBody = await System.Text.Json.JsonSerializer.DeserializeAsync<T>(responseStream, JsonSerializerSettings, cancellationToken).ConfigureAwait(false);
                        return new ObjectResponseResult<T>(typedBody, string.Empty);
                    }
                }
                catch (Newtonsoft.Json.JsonException exception)
                {
                    var message = "Could not deserialize the response body stream as " + typeof(T).FullName + ".";
                    throw new ApiException(message, (int)response.StatusCode, string.Empty, headers, exception);
                }
            }
        }

        private string ConvertToString(object value, System.Globalization.CultureInfo cultureInfo)
        {
            if (value == null)
            {
                return "";
            }

            if (value is System.Enum)
            {
                var name = System.Enum.GetName(value.GetType(), value);
                if (name != null)
                {
                    var field = System.Reflection.IntrospectionExtensions.GetTypeInfo(value.GetType()).GetDeclaredField(name);
                    if (field != null)
                    {
                        var attribute = System.Reflection.CustomAttributeExtensions.GetCustomAttribute(field, typeof(System.Runtime.Serialization.EnumMemberAttribute))
                            as System.Runtime.Serialization.EnumMemberAttribute;
                        if (attribute != null)
                        {
                            return attribute.Value != null ? attribute.Value : name;
                        }
                    }

                    var converted = System.Convert.ToString(System.Convert.ChangeType(value, System.Enum.GetUnderlyingType(value.GetType()), cultureInfo));
                    return converted == null ? string.Empty : converted;
                }
            }
            else if (value is bool)
            {
                return System.Convert.ToString((bool)value, cultureInfo).ToLowerInvariant();
            }
            else if (value is byte[])
            {
                return System.Convert.ToBase64String((byte[])value);
            }
            else if (value.GetType().IsArray)
            {
                var array = System.Linq.Enumerable.OfType<object>((System.Array)value);
                return string.Join(",", System.Linq.Enumerable.Select(array, o => ConvertToString(o, cultureInfo)));
            }

            var result = System.Convert.ToString(value, cultureInfo);
            return result == null ? "" : result;
        }
    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "10.5.2.0 (Newtonsoft.Json v11.0.0.0)")]
    public partial class RemoteServiceErrorResponse
    {
        [Newtonsoft.Json.JsonProperty("error", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public RemoteServiceErrorInfo Error { get; set; }


    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "10.5.2.0 (Newtonsoft.Json v11.0.0.0)")]
    public partial class RemoteServiceErrorInfo
    {
        [Newtonsoft.Json.JsonProperty("code", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string Code { get; set; }

        [Newtonsoft.Json.JsonProperty("message", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string Message { get; set; }

        [Newtonsoft.Json.JsonProperty("details", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string Details { get; set; }

        [Newtonsoft.Json.JsonProperty("data", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public System.Collections.Generic.IDictionary<string, object> Data { get; set; }

        [Newtonsoft.Json.JsonProperty("validationErrors", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public System.Collections.Generic.ICollection<RemoteServiceValidationErrorInfo> ValidationErrors { get; set; }


    }

    [System.CodeDom.Compiler.GeneratedCode("NSwag", "13.14.4.0 (NJsonSchema v10.5.2.0 (Newtonsoft.Json v11.0.0.0))")]
    public partial class ApiException : System.Exception
    {
        public int StatusCode { get; private set; }

        public string Response { get; private set; }

        public System.Collections.Generic.IReadOnlyDictionary<string, System.Collections.Generic.IEnumerable<string>> Headers { get; private set; }

        public ApiException(string message, int statusCode, string response, System.Collections.Generic.IReadOnlyDictionary<string, System.Collections.Generic.IEnumerable<string>> headers, System.Exception innerException)
            : base(message + "\n\nStatus: " + statusCode + "\nResponse: \n" + ((response == null) ? "(null)" : response.Substring(0, response.Length >= 512 ? 512 : response.Length)), innerException)
        {
            StatusCode = statusCode;
            Response = response;
            Headers = headers;
        }

        public override string ToString()
        {
            return string.Format("HTTP Response: \n\n{0}\n\n{1}", Response, base.ToString());
        }
    }

    [System.CodeDom.Compiler.GeneratedCode("NSwag", "13.14.4.0 (NJsonSchema v10.5.2.0 (Newtonsoft.Json v11.0.0.0))")]
    public partial class ApiException<TResult> : ApiException
    {
        public TResult Result { get; private set; }

        public ApiException(string message, int statusCode, string response, System.Collections.Generic.IReadOnlyDictionary<string, System.Collections.Generic.IEnumerable<string>> headers, TResult result, System.Exception innerException)
            : base(message, statusCode, response, headers, innerException)
        {
            Result = result;
        }
    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "10.5.2.0 (Newtonsoft.Json v11.0.0.0)")]
    public partial class RemoteServiceValidationErrorInfo
    {
        [Newtonsoft.Json.JsonProperty("message", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string Message { get; set; }

        [Newtonsoft.Json.JsonProperty("members", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public System.Collections.Generic.ICollection<string> Members { get; set; }


    }
}
