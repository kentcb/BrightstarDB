namespace BrightstarNotes.Model
{
    public partial class Note
    {
        public string ShortContent { get { return Body.Substring(30) + ((Body.Length > 30) ? "…" : ""); } }
    }
}
