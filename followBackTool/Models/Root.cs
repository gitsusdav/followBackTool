using Newtonsoft.Json;

namespace followBackTool.Models
{



    public class String_list_data
    {
        public string href { get; set; }
        public string value { get; set; }
        public int timestamp { get; set; }

    }
    public class Relationships_following
    {
        public string title { get; set; }
        public IList<dynamic> media_list_data { get; set; }
        public IList<String_list_data> string_list_data { get; set; }

    }
    public class Root
    {
        public IList<Relationships_following> relationships_following { get; set; }

    }
}
