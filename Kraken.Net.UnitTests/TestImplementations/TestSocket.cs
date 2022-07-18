using System;
using System.Security.Authentication;
using System.Text;
using System.Threading.Tasks;
using CryptoExchange.Net.Interfaces;
using CryptoExchange.Net.Objects;
using Newtonsoft.Json;

namespace Kucoin.Net.UnitTests.TestImplementations
{
    public class TestSocket: IWebsocket
    {
        public bool CanConnect { get; set; }
        public bool Connected { get; set; }

#pragma warning disable 8618
        public event Action OnClose;
        public event Action<string> OnMessage;
        public event Action<Exception> OnError;
        public event Action OnOpen;
#pragma warning disable 0067
        public event Action OnReconnecting;
        public event Action OnReconnected;
#pragma warning restore 0067
#pragma warning restore 8618

        public int Id { get; }
        public bool ShouldReconnect { get; set; }
        public Func<string, string> DataInterpreterString { get; set; }
        public Func<byte[], string> DataInterpreterBytes { get; set; }
        public DateTime? DisconnectTime { get; set; }
        public string Url { get; } = "";
        public Encoding Encoding { get; set; }

        public bool IsClosed => !Connected;
        public bool IsOpen => Connected;
        public bool PingConnection { get; set; }
        public TimeSpan PingInterval { get; set; }
        public SslProtocols SSLProtocols { get; set; }
        public TimeSpan Timeout { get; set; }
        public string Origin { get; set; }
        public bool Reconnecting { get; set; }
        public int? RatelimitPerSecond { get; set; }
        public string LastSendMessage { get; set; }
        public double IncomingKbps => 0;

        public Uri Uri => new Uri("wss://test.com/ws");

        public TimeSpan KeepAliveInterval { get; set; }
        public Func<Task<Uri>> GetReconnectionUrl { get; set; }

        public Task<bool> ConnectAsync()
        {
            Connected = CanConnect;
            return Task.FromResult(CanConnect);
        }

        public void Send(string data)
        {
            if(!Connected)
                throw new Exception("Socket not connected");
            LastSendMessage = data;
        }

        public void Reset()
        {
            
        }

        public Task CloseAsync()
        {
            Connected = false;
            return Task.FromResult(0);
        }

        public void SetProxy(ApiProxy proxy)
        {
            throw new NotImplementedException();
        }
        public void Dispose()
        {
        }

        public void InvokeClose()
        {
            Connected = false;
            OnClose?.Invoke();
        }

        public void InvokeOpen()
        {
            OnOpen?.Invoke();
        }

        public void InvokeMessage(string data)
        {
            OnMessage?.Invoke(data);
        }

        public void InvokeMessage<T>(T data)
        {
            OnMessage?.Invoke(JsonConvert.SerializeObject(data));
        }

        public void InvokeError(Exception error)
        {
            OnError?.Invoke(error);
        }

        public async Task ProcessAsync()
        {
            while (Connected)
                await Task.Delay(50);
        }

        public Task ReconnectAsync()
        {
            throw new NotImplementedException();
        }
    }
}
