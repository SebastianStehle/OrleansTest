using Orleans;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OrleansTest.Grains
{
    public sealed class NamedContentData : Dictionary<string, object>
    {
    }

    public sealed class IndexData1
    {
        public NamedContentData Data { get; set; }
    }

    public sealed class IndexData2 : ICommand
    {
        public Guid ContentId { get; set; }

        public NamedContentData Data { get; set; }

        public bool OnlyDraft { get; set; }
    }

    public interface IIndexGrain : IGrainWithGuidKey
    {
        Task<J<object>> ExecuteAsync(J<ICommand> command);

        Task<bool> Index1Async(Guid id, J<IndexData1> data, bool onlyDraft);

        Task<bool> Index2Async(Guid id, IndexData1 data, bool onlyDraft);

        Task<bool> Index3Async(J<IndexData2> data);
    }
}
