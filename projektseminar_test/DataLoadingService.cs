using System.Collections.Generic;

namespace projektseminar_test
{
    class DataLoadingService: Service
    {
        public Repository Repository { get; set; }

        protected override bool ServiceAction(object args)
        {
            var list = args as IList<string>;
            if (list == null) return false;
            var count = list.Count;
            ReportProgress(0, count);
            var i = 0;
            foreach (var id in list)
            {
                ReportProgress(i++, count, Repository.Find(id));
            }
            return true;
        }
    }
}
