namespace Amerdrix.TGrid.Logging
{
    public interface ILogger
    {
        void Info(string message, params object[] obj);

        void Warning(string message, params object[] obj);
        void Error(string message, params object[] obj);
    }
}