
namespace MasterPiece.Controllers
{
    public class HttpPostedFileBase
    {
        public int ContentLength { get; internal set; }
        public string? FileName { get; internal set; }

        internal void SaveAs(string img3Path)
        {
            throw new NotImplementedException();
        }
    }
}