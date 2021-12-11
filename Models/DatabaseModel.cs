using System.Collections.Generic;

namespace DbControlCore.Models
{
    public class DatabaseModel
    {
        public string Name { get; set; }

        public string Connection { get; set; }

        public bool IsEnabled { get; set; }

        public List<QueryModel> Queries { get; set; }
    }
}
