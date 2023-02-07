using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Expense_Tracker_v1._0.Contracts.Services;
using Expense_Tracker_v1._0.Core.Models;

namespace Expense_Tracker_v1._0.Services;
internal class PoolDataService : IPoolDataService
{
    private Pool mainPool;
    public async Task<Pool> GetDataAsync()
    {
        Pool pool = SqliteDataService.GetPool();
        await Task.CompletedTask;
        return pool;
    }
}
