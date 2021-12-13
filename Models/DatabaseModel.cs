using System.Collections.Generic;

namespace DbControlCore.Models
{
    public class DatabaseModel : ConfigModel
    {
        public List<QueryModel> Queries { get; set; }
    }
}
