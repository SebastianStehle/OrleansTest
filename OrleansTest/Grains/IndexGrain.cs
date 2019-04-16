using Orleans;
using System;
using System.Threading.Tasks;

namespace OrleansTest.Grains
{
    class IndexGrain : Grain, IIndexGrain
    {
        public async Task<bool> IndexAsync(Guid id, J<IndexData> data, bool onlyDraft)
        {
            Console.WriteLine("Index called");

            await Task.Delay(100);

            return true;
        }
    }
}
