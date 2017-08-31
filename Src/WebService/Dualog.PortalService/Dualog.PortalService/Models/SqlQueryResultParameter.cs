using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Newtonsoft.Json;

namespace Dualog.PortalService.Models
{
    public class SqlQueryResultParameter
    {
        public long Id { get; set; }
        [Required, StringLength( 100 )] public string Name { get; set; }
        [Required, StringLength( 100 )] public string ParameterType { get; set; }
        [JsonIgnore] public string ColumnName { get; set; }
        [JsonIgnore] public string TranslationScope { get; set; }
    }
}
