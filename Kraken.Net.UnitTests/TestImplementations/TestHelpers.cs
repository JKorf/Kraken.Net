using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CryptoExchange.Net;
using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.Interfaces;
using CryptoExchange.Net.Logging;
using Kraken.Net.Interfaces;
using Moq;
using Newtonsoft.Json;

namespace Kraken.Net.UnitTests.TestImplementations
{
    public class TestHelpers
    {
        [ExcludeFromCodeCoverage]
        public static bool AreEqual(object self, object to, params string[] ignore)
        {
            if (self == null && to == null)
                return true;

            if ((self != null && to == null) || (self == null && to != null))
                return false;

            var type = self.GetType();
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IEnumerable<>)
                || typeof(IList).IsAssignableFrom(type))
            {
                var list = (IList) self;
                var listOther = (IList)to;
                for (int i = 0; i < list.Count; i++)
                {
                    if(!AreEqual(list[i], listOther[i]))
                        return false;
                }

                return true;
            }

            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Dictionary<,>))
            {
                var dict = (IDictionary) self;
                var other = (IDictionary) to;
                var items1 = new List<DictionaryEntry>();
                var items2 = new List<DictionaryEntry>();
                foreach (DictionaryEntry item in dict)
                    items1.Add(item);
                foreach (DictionaryEntry item in other)
                    items2.Add(item);

                for (int i = 0; i < items1.Count; i++)
                {
                    if (!AreEqual(items1[i].Key, items2[i].Key))
                        return false;
                    if (!AreEqual(items1[i].Value, items2[i].Value))
                        return false;
                }

                return true;
            }


            if (type.IsValueType || type == typeof(string))
                return Equals(self, to);

            var ignoreList = new List<string>(ignore);
            foreach (var pi in type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                if (ignoreList.Contains(pi.Name))
                    continue;

                var selfValue = type.GetProperty(pi.Name).GetValue(self, null);
                var toValue = type.GetProperty(pi.Name).GetValue(to, null);

                if (pi.PropertyType.IsValueType || pi.PropertyType == typeof(string))
                {
                    if (!Equals(selfValue, toValue))
                        return false;
                    continue;
                }

                if (pi.PropertyType.IsClass)
                {
                    if (!AreEqual(selfValue, toValue, ignore))
                        return false;
                    continue;
                }

                if (selfValue != toValue && (selfValue == null || !selfValue.Equals(toValue)))
                    return false;
            }

            return true;
        }

        public static KrakenSocketClient CreateSocketClient(IWebsocket socket, KrakenSocketClientOptions options = null)
        {
            KrakenSocketClient client;
            client = options != null ? new KrakenSocketClient(options) : new KrakenSocketClient(new KrakenSocketClientOptions() { LogVerbosity = LogVerbosity.Debug, ApiCredentials = new ApiCredentials("Test", "Test") });
            client.SocketFactory = Mock.Of<IWebsocketFactory>();
            Mock.Get(client.SocketFactory).Setup(f => f.CreateWebsocket(It.IsAny<Log>(), It.IsAny<string>())).Returns(socket);
            return client;
        }

        public static KrakenSocketClient CreateAuthenticatedSocketClient(IWebsocket socket, KrakenSocketClientOptions options = null)
        {
            KrakenSocketClient client;
            client = options != null ? new KrakenSocketClient(options) : new KrakenSocketClient(new KrakenSocketClientOptions(){ LogVerbosity = LogVerbosity.Debug, ApiCredentials = new ApiCredentials("Test", "Test")});
            client.SocketFactory = Mock.Of<IWebsocketFactory>();
            Mock.Get(client.SocketFactory).Setup(f => f.CreateWebsocket(It.IsAny<Log>(), It.IsAny<string>())).Returns(socket);
            return client;
        }

        public static IKrakenClient CreateClient(KrakenClientOptions options = null)
        {
            IKrakenClient client;
            client = options != null ? new KrakenClient(options) : new KrakenClient(new KrakenClientOptions(){LogVerbosity = LogVerbosity.Debug});
            client.RequestFactory = Mock.Of<IRequestFactory>();
            return client;
        }

        public static IKrakenClient CreateAuthResponseClient(string response, HttpStatusCode code = HttpStatusCode.OK)
        {
            var client = (KrakenClient)CreateClient(new KrakenClientOptions(){ ApiCredentials = new ApiCredentials("Test", "Test")});
            SetResponse(client, response, code);
            return client;
        }


        public static IKrakenClient CreateResponseClient(string response, KrakenClientOptions options = null)
        {
            var client = (KrakenClient)CreateClient(options);
            SetResponse(client, response);
            return client;
        }

        public static IKrakenClient CreateResponseClient<T>(T response, KrakenClientOptions options = null)
        {
            var client = (KrakenClient)CreateClient(options);
            SetResponse(client, JsonConvert.SerializeObject(response));
            return client;
        }

        public static void SetResponse(RestClient client, string responseData, HttpStatusCode code = HttpStatusCode.OK)
        {
            var expectedBytes = Encoding.UTF8.GetBytes(responseData);
            var responseStream = new MemoryStream();
            responseStream.Write(expectedBytes, 0, expectedBytes.Length);
            responseStream.Seek(0, SeekOrigin.Begin);

            var response = new Mock<IResponse>();
            response.Setup(c => c.IsSuccessStatusCode).Returns(code == HttpStatusCode.OK);
            response.Setup(c => c.StatusCode).Returns(code);
            response.Setup(c => c.GetResponseStream()).Returns(Task.FromResult((Stream)responseStream));

            var request = new Mock<IRequest>();
            request.Setup(c => c.Uri).Returns(new Uri("http://www.test.com"));
            request.Setup(c => c.GetResponse(It.IsAny<CancellationToken>())).Returns(Task.FromResult(response.Object));

            var factory = Mock.Get(client.RequestFactory);
            factory.Setup(c => c.Create(It.IsAny<HttpMethod>(), It.IsAny<string>()))
                .Returns(request.Object);
        }
        
        public static T CreateObjectWithTestParameters<T>() where T: class
        {
            var type = typeof(T);
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IEnumerable<>))
            {
                var elementType = type.GenericTypeArguments[0];
                Type listType = typeof(List<>).MakeGenericType(new[] { elementType });
                IList list = (IList)Activator.CreateInstance(listType);
                list.Add(GetTestValue(elementType, 0));
                list.Add(GetTestValue(elementType, 1));
                return (T)list;
            }
            else if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Dictionary<,>))
            {
                var result = (IDictionary)Activator.CreateInstance(type);
                result.Add(GetTestValue(type.GetGenericArguments()[0], 0), GetTestValue(type.GetGenericArguments()[1], 0));
                result.Add(GetTestValue(type.GetGenericArguments()[0], 1), GetTestValue(type.GetGenericArguments()[1], 1));
                return (T)Convert.ChangeType(result, type);
            }
            else
            {
                var obj = Activator.CreateInstance<T>();
                return FillWithTestParameters(obj);
            }
        }

        public static T FillWithTestParameters<T>(T obj) where T : class
        {

            var properties = obj.GetType().GetProperties();
            int i = 1;
            foreach (var property in properties)
            {
                var value = GetTestValue(property.PropertyType, i);
                Type t = Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType;
                object safeValue = (value == null) ? null : Convert.ChangeType(value, t);
                property.SetValue(obj, safeValue, null);
                i++;
            }

            return obj;
        }

        public static object[] CreateParametersForMethod(MethodInfo method, Dictionary<string, object> defaultValues)
        {
            var param = method.GetParameters();
            var result = new object[param.Length];
            for(int i = 0; i < param.Length; i++)
            {
                if (defaultValues.ContainsKey(param[i].Name))
                    result[i] = defaultValues[param[i].Name];
                else
                    result[i] = GetTestValue(param[i].ParameterType, i);
            }

            return result;
        }

        public static object GetTestValue(Type type, int i)
        {
            if (type == typeof(bool))
                return true;

            if (type == typeof(bool?))
                return (bool?) true;

            if (type == typeof(decimal))
                return i / 10m;

            if (type == typeof(decimal?))
                return (decimal?) (i / 10m);

            if (type == typeof(int) || type == typeof(long))
                return i;

            if (type == typeof(int?))
                return (int?) i;

            if (type == typeof(long?))
                return (long?) i;

            if (type == typeof(DateTime))
                return new DateTime(2019, 1, Math.Max(i, 1));

            if(type == typeof(DateTime?))
                return (DateTime?) new DateTime(2019, 1, Math.Max(i, 1));

            if (type == typeof(string))
                return "string" + i;

            if (type.IsEnum)
            {
                return Activator.CreateInstance(type);
            }

            if (type.IsArray)
            {
                var elementType = type.GetElementType();
                var result = Array.CreateInstance(elementType, 2);
                result.SetValue(GetTestValue(elementType, 0), 0);
                result.SetValue(GetTestValue(elementType, 1), 1);
                return result;
            }

            if (type.IsGenericType && (type.GetGenericTypeDefinition() == typeof(List<>)))
            {
                var result = (IList)Activator.CreateInstance(type);
                result.Add(GetTestValue(type.GetGenericArguments()[0], 0));
                result.Add(GetTestValue(type.GetGenericArguments()[0], 1));
                return result;
            }

            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Dictionary<,>))
            {
                var result = (IDictionary)Activator.CreateInstance(type);
                result.Add(GetTestValue(type.GetGenericArguments()[0], 0), GetTestValue(type.GetGenericArguments()[1], 0));
                result.Add(GetTestValue(type.GetGenericArguments()[0], 1), GetTestValue(type.GetGenericArguments()[1], 1));
                return Convert.ChangeType(result, type);
            }

            if (type.IsClass)
                return FillWithTestParameters(Activator.CreateInstance(type));

            return null;
        }
    }
}
