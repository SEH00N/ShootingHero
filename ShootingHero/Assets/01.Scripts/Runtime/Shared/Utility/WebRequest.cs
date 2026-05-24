using System;
using Cysharp.Threading.Tasks;
using UnityEngine.Networking;
using System.Text.Json;
using UnityEngine;
using System.Threading;

namespace ShootingHero.Shared
{
    public class WebRequest<TRequest, TResponse> where TRequest : class where TResponse : class
    {
        private static readonly JsonSerializerOptions JSON_SERIALIZER_OPTIONS = new JsonSerializerOptions(JsonSerializerDefaults.Web);

        private readonly string url = null;
        private readonly TRequest request = null;

        public WebRequest(string url, TRequest request)
        {
            this.url = url;
            this.request = request;
        }

        public async UniTask<TResponse> RequestAsync(CancellationToken ct = default)
        {
            try 
            {
                if(string.IsNullOrEmpty(url) == true)
                {
                    Debug.LogError("[WebRequest::RequestAsync] Url is invalid");
                    return null;
                }

                if(request == null)
                {
                    Debug.LogError($"[WebRequest::RequestAsync] Request is null");
                    return null;
                }

                string payloadData = JsonSerializer.Serialize(request, JSON_SERIALIZER_OPTIONS);
                using (UnityWebRequest request = UnityWebRequest.Post(url, payloadData, "application/json"))
                {
                    await request.SendWebRequest().WithCancellation(ct);

                    if (request.result == UnityWebRequest.Result.Success)
                    {
                        TResponse response = JsonSerializer.Deserialize<TResponse>(request.downloadHandler.text, JSON_SERIALIZER_OPTIONS);
                        return response;
                    }
                    else
                    {
                        Debug.LogError(request.error);
                    }
                }
            }
            catch(OperationCanceledException)
            {
                throw;
            } 
            catch(Exception err) 
            {
                Debug.LogError(err);
            }

            return null;
        }
    }
}