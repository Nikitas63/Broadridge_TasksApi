using Newtonsoft.Json;

namespace TasksApi.Pagination
{
    [JsonObject(MemberSerialization.OptIn)]
    public class PaginationContext
    {
        public PaginationContext(int page, int size, int pages, int totalRows)
        {
            Page = page;
            Size = size;
            Pages = pages;
            TotalRows = totalRows;
        }

        [JsonProperty("page")]
        public int Page { get; }

        [JsonProperty("size")]
        public int Size { get; }

        [JsonProperty("pages")]
        public int Pages { get; }

        [JsonProperty("totalRows")]
        public int TotalRows { get; set; }
    }
}
