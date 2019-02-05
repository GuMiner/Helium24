using System;
namespace H24.Logging
{
    public class JsonLogScopeObject : IDisposable
    {
        private readonly Action disposeAction;

        public JsonLogScopeObject(Action disposeAction)
        {
            this.disposeAction = disposeAction;
        }

        public void Dispose()
        {
            disposeAction();
        }
    }
}
