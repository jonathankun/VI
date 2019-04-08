using Android.Util;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace VInspection.Clases.Utils
{
    public sealed class AsyncTimer : CancellationTokenSource
    {
        public AsyncTimer(Func<bool> callback, int millisecondsDueTime)
        {
            Task.Run(async () =>
            {
                Log.Info("AsyncTimer", "Se inicia AsyncTimer");
                await Task.Delay(millisecondsDueTime, Token);
                while (!IsCancellationRequested)
                {
                    callback();
                }
            });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                Cancel();

            base.Dispose(disposing);
        }
    }
}