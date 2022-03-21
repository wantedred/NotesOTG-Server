namespace NotesOTG_Server.Models.Http.Responses.impl
{
    public class NoteResponse : BasicResponse
    {
        public string Title { get; set; }
        public string Body { get; set; }
        public string PublicId { get; set; }
        public string CreationDate { get; set; }
        public string ModifiedDate { get; set; }
        public bool CheckList { get; set; }
        public string Category { get; set; }
        public bool CustomCategory { get; set; }
    }
}
