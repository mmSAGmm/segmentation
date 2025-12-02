using Segmentation.DomainModels;
using System.Net.Http;
using System.Threading;

namespace Segmentation.Web
{
    public class SegmentsApiClient(HttpClient httpClient) 
    {
        public async Task<IEnumerable<Segment>> GetSegmentPage(CancellationToken cancellationToken) 
        {
            return await httpClient.GetFromJsonAsync<IEnumerable<Segment>>($"/api/admin/v1/segment/page/{1}/{20}", cancellationToken);
        }
    }
}
