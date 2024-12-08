using System;
using System.IO;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace UnderworldExporter.Game
{
    sealed class Logger : MonoBehaviour
    {
        [SerializeField] private bool _enableLogging = true;
        private LogHandler _logHandler;
        private FatalExceptionsLogger _fatalExceptionsLogger;

        public static Logger Instance { get; private set; }
        
        private void Start()
        {
            DontDestroyOnLoad(this.gameObject);

            _fatalExceptionsLogger = new FatalExceptionsLogger();
            Instance = this;

            UltimaUnderworldApplication.OnGamePathSet += InitLog;
            InitLog();
        }

        private void OnDestroy()
        {
            UltimaUnderworldApplication.OnGamePathSet -= InitLog;
            _logHandler?.Dispose();
            _fatalExceptionsLogger.Dispose();
        }

        private void InitLog(string _)
        {
            InitLog();
        }
        
        public void InitLog()
        {
            if (!_enableLogging || string.IsNullOrEmpty(GameModel.CurrentModel.BasePath) || GameModel.CurrentModel.LogLevel == LogLevel.None)
            {
                return;
            }
            
            _logHandler?.Dispose();
            _fatalExceptionsLogger.StopLog();
            
            var logLevel = GameModel.CurrentModel.LogLevel;
            var logsFolder = GameModel.CurrentModel.BasePath + Path.Combine("Logs",$"{logLevel}_{DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()}");

            if (!Directory.Exists(logsFolder))
            {
                Directory.CreateDirectory(logsFolder);
            }
            
            var pathToPlayerLog= Path.Combine(logsFolder, "Player.log");
            var pathToCrashLog= Path.Combine(logsFolder, "Crash.log");

            switch (logLevel)
            {
                case LogLevel.Fatal:
                    _fatalExceptionsLogger.StartLog(pathToCrashLog);
                    break;
                case LogLevel.Error: 
                case LogLevel.Info: 
                    InitLog(pathToPlayerLog, logLevel);
                    break;
                case LogLevel.Verbose:
                    _fatalExceptionsLogger.StartLog(pathToCrashLog);
                    InitLog(pathToPlayerLog, logLevel);
                    break;
            }
        }

        private void InitLog( string pathToLog, LogLevel logLevel)
        {
            if (Application.isPlaying && Application.installMode != ApplicationInstallMode.Editor && !string.IsNullOrEmpty(pathToLog))
            {
                _logHandler = new LogHandler(pathToLog, logLevel);
                Debug.unityLogger.logHandler = _logHandler;
            }
        }
        
        private sealed class LogHandler : ILogHandler, IDisposable
        {
            private readonly LogLevel _logLevel;
            private readonly StreamWriter _streamWriter;

            public LogHandler(string filePath, LogLevel logLevel)
            {
                _streamWriter = File.CreateText(filePath);
                _streamWriter.AutoFlush = true;
                
                _streamWriter.WriteLine($"ApplicationPlatform = {Application.platform}");
                _streamWriter.WriteLine($"UnityVersion = {Application.unityVersion}");
                _streamWriter.WriteLine($"IsDebugBuild = {Debug.isDebugBuild}");
                _streamWriter.WriteLine($"OperatingSystem = {SystemInfo.operatingSystem}");
                _streamWriter.WriteLine($"DeviceName = {SystemInfo.deviceName}");
                _streamWriter.WriteLine($"DeviceModel = {SystemInfo.deviceModel}");
                _streamWriter.WriteLine($"CpuName = {SystemInfo.processorType}");
                _streamWriter.WriteLine($"CpuModel = {SystemInfo.processorModel}");
                _streamWriter.WriteLine($"CpuFrequency = {SystemInfo.processorFrequency}");
                _streamWriter.WriteLine($"CpuType = {SystemInfo.processorType}");
                _streamWriter.WriteLine($"GraphicsName = {SystemInfo.graphicsDeviceName}");
                _streamWriter.WriteLine($"SystemMemorySize = {SystemInfo.systemMemorySize}");
            }

            public void LogException(Exception exception, UnityEngine.Object context)
            {
                if (_logLevel == LogLevel.Verbose || _logLevel == LogLevel.Error)
                {
                    _streamWriter.WriteLine(exception.ToString());
                }
            }

            public void LogFormat(LogType logType, UnityEngine.Object context, string format, params object[] args)
            {
                string prefix = "";
                switch (logType)
                {
                    case LogType.Error:
                        prefix = "[Error] ";
                        break;

                    case LogType.Warning:
                        prefix = "[Warning] ";
                        break;

                    case LogType.Assert:
                        prefix = "[Assert] ";
                        break;

                    case LogType.Exception:
                        prefix = "[Exception] ";
                        break;
                }

                bool allowLogging = _logLevel == LogLevel.Verbose ||
                                    (_logLevel == LogLevel.Error && (logType == LogType.Exception || logType == LogType.Error)) ||
                                    (_logLevel == LogLevel.Info && (logType == LogType.Exception || logType == LogType.Error) || 
                                     logType == LogType.Warning);

                if (allowLogging)
                {
                    _streamWriter.WriteLine(prefix + string.Format(format, args));
                }
            }

            public void Dispose()
            {
                _streamWriter.Close();
                Debug.unityLogger.logHandler = null;
            }
        }

        private sealed class FatalExceptionsLogger : IDisposable
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            private readonly AndroidJavaObject _fatalExceptionLogger = new AndroidJavaObject("com.logger.ExceptionsLogger");
#endif
            public void StartLog(string pathToLog)
            {
#if UNITY_ANDROID && !UNITY_EDITOR
                _fatalExceptionLogger.Call("startLog", pathToLog);
#endif                
            }

            public void StopLog()
            {
#if UNITY_ANDROID && !UNITY_EDITOR
                _fatalExceptionLogger.Call("stopLog");
#endif                
            }

            public void Dispose()
            {
#if UNITY_ANDROID && !UNITY_EDITOR
                StopLog();
                _fatalExceptionLogger?.Dispose();
#endif
            }
        }
    }

    public enum LogLevel
    {
        Fatal,
        Info,
        Verbose,
        Error,
        None,
    }
}