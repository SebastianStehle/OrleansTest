using Orleans;
using System;
using System.Threading.Tasks;

namespace OrleansTest.Grains
{
    class IndexGrain : Grain, IIndexGrain
    {
        public async Task<bool> Index1Async(Guid id, J<IndexData1> data, bool onlyDraft)
        {
            Console.WriteLine("Index1 called");

            await Task.Delay(100);

            return true;
        }

        public async Task<bool> Index2Async(Guid id, IndexData1 data, bool onlyDraft)
        {
            Console.WriteLine("Index2 called");

            await Task.Delay(100);

            return true;
        }

        public async Task<bool> Index3Async(J<IndexData2> data)
        {
            Console.WriteLine("Index3 called");

            await Task.Delay(100);

            return true;
        }
    }
}
