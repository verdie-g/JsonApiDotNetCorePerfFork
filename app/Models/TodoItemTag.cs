namespace app.Models
{
    public sealed class TodoItemTag
    {
        public int TodoItemId { get; set; }
        public TodoItem TodoItem { get; set; }

        public int TagId { get; set; }
        public Tag Tag { get; set; }
    }
}
