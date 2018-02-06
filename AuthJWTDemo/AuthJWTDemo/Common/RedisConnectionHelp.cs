using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using StackExchange.Redis;
using System.Configuration;
using System.Collections.Concurrent;

namespace AuthJWTDemo.Common
{
    public class RedisConnectionHelp
    {
        //系统自定义Key前缀
        public static readonly string SysCustomKey = ConfigurationManager.AppSettings["redisKey"] ?? "";

        //"127.0.0.1:6379,allowadmin=true
        private static readonly string RedisConnectionString = ConfigurationManager.ConnectionStrings["RedisExchangeHosts"].ConnectionString;

        private static readonly object Locker = new object();
        private static ConnectionMultiplexer _instance;

        public static ConnectionMultiplexer GetConnectionMultiplexer(string connectionString)
        {
            if (!ConnectionCache.ContainsKey(connectionString))
            {
                ConnectionCache[connectionString] = GetManager(connectionString);
            }
            return ConnectionCache[connectionString];
        }

        private static readonly ConcurrentDictionary<string, ConnectionMultiplexer> ConnectionCache = new ConcurrentDictionary<string, ConnectionMultiplexer>();

        public static ConnectionMultiplexer Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (Locker)
                    {
                        if (_instance == null || !_instance.IsConnected)
                        {
                            _instance = GetManager();
                        }
                    }
                }

                return _instance;
            }

        }

        private static ConnectionMultiplexer GetManager(string connectionString = null)
        {
            connectionString = connectionString ?? RedisConnectionString;
            var connect = ConnectionMultiplexer.Connect(connectionString);

            //注册如下事件
            connect.ConnectionFailed += MuxerConnectionFailed;
            connect.ConnectionRestored += MuxerConnectionRestored;
            connect.ErrorMessage += MuxerErrorMessage;
            connect.ConfigurationChanged += MuxerConfigurationChanged;
            connect.HashSlotMoved += MuxerHashSlotMoved;
            connect.InternalError += MuxerInternalError;

            return connect;
        }

        private static void MuxerInternalError(object sender, InternalErrorEventArgs e)
        {
            throw new NotImplementedException();
        }

        private static void MuxerErrorMessage(object sender, RedisErrorEventArgs e)
        {
            throw new NotImplementedException();
        }

        private static void MuxerHashSlotMoved(object sender, HashSlotMovedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private static void MuxerConfigurationChanged(object sender, EndPointEventArgs e)
        {
            throw new NotImplementedException();
        }

        private static void MuxerConnectionRestored(object sender, ConnectionFailedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private static void MuxerConnectionFailed(object sender, ConnectionFailedEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}