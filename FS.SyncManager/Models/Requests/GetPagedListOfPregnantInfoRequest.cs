namespace FS.SyncManager
{
    internal class GetPagedListOfPregnantInfoRequest
    {
        public GetPagedListOfPregnantInfoRequest()
        {
        }

        public string PersonName { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }
}