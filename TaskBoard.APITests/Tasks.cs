using System.Text.Json.Serialization;


namespace TaskBoard.APITests
{
    internal class Tasks
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("title")]
        public string title { get; set; }

        [JsonPropertyName("board id")]
        public int board_id { get; set; }

        [JsonPropertyName("board name")]
        public string board_name { get; set; }
        [JsonPropertyName("board ")]
        public int board { get; set; }

        [JsonPropertyName("description")]
        public string description { get; set; }



    }
}