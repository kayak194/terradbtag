using System;
using System.ComponentModel;
using System.Windows;
using terradbtag.Properties;

namespace terradbtag.Framework
{
    class Service
    {
        public void Execute(object args = null)
        {
            Error = string.Empty;
            Application.Current.Dispatcher.InvokeAsync(() =>
            {
                try
                {
                    if (ServiceAction(args))
                    {
                        OnFinished(true);
                    }
                }
                catch (Exception ex)
                {
                    Error = ex.Message;
                    OnFinished(false);
                }
                
            });
        }

        public event EventHandler<Tuple<int, int, object>> ProgressChanged;

        public event EventHandler<bool> Finished;

        private void OnProgressChanged(int current, int max, object data = null)
        {
            ProgressChanged?.Invoke(this, new Tuple<int, int, object>(current, max, data));
        }

        private void OnFinished(bool e)
        {
            Finished?.Invoke(this, e);
        }

        public string Error { get; set; }

        protected void ReportProgress(int progress, int max, object data = null)
        {
            OnProgressChanged(progress, max, data);
        }

        public object Result { get; set; }

        protected virtual bool ServiceAction(object args)
        {
            return true;
        }
    }
}
