using Orleans;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OrleansTest.Grains
{
    public sealed class NamedContentData : Dictionary<string, object>
    {
    }

    public sealed class IndexData
    {
        public NamedContentData Data { get; set; }
    }

    public interface IIndexGrain : IGrainWithGuidKey
    {
        Task<bool> IndexAsync(Guid id, J<IndexData> data, bool onlyDraft);
    }
}
