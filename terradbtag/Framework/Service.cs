using System;
using System.ComponentModel;

namespace projektseminar_test.Framework
{
    class Service
    {
        private BackgroundWorker BackgroundWorker { get; } = new BackgroundWorker { WorkerReportsProgress = true };

        public Service()
        {
            BackgroundWorker.DoWork += DoWork;
            BackgroundWorker.ProgressChanged +=
                (sender, args) =>
                {
                    var state = args.UserState as Tuple<int, object>;
                    if(state == null) return;
                    OnProgressChanged(args.ProgressPercentage, state.Item1, state.Item2);
                };
            BackgroundWorker.RunWorkerCompleted += (sender, args) =>
            {
                var isSuceed = args.Result is bool && (bool)args.Result;
                if (!isSuceed) Error = args.Result.ToString();
                OnFinished(isSuceed);
            };
        }

        private void DoWork(object sender, DoWorkEventArgs doWorkEventArgs)
        {
            try
            {
                doWorkEventArgs.Result = ServiceAction(doWorkEventArgs.Argument);
            }
            catch (Exception ex)
            {
                doWorkEventArgs.Result = ex.Message;
            }
        }

        public void Execute(object args = null)
        {
            Error = string.Empty;
            BackgroundWorker.RunWorkerAsync(args);
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
            BackgroundWorker.ReportProgress(progress,new Tuple<int,object>(max, data));
        }

        public object Result { get; set; }

        protected virtual bool ServiceAction(object args)
        {
            return true;
        }
    }
}
