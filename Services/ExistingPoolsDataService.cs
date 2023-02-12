using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using Expense_Tracker_v1._0.Contracts.Services;
using Expense_Tracker_v1._0.Core.Models;
using Expense_Tracker_v1._0.Core.Services;

namespace Expense_Tracker_v1._0.Services;
public class ExistingPoolsDataService : IExistingPoolsDataService
{
    public static Pool current;
    private List<Pool>? _allPools; //question mark denotes nullable

    private static IEnumerable<Pool> AllPools()
    {
        return FileService.GetPoolsList();
    }
    public async Task<IEnumerable<Pool>> Populate()
    {
        _allPools = new List<Pool>(AllPools());

        await Task.CompletedTask;
        return _allPools;
    }

    public static async Task setCurrentPool()
    {
        current = SqliteDataService.GetCurrentPool();
        await Task.CompletedTask;
    }

    public static async Task updatePool()
    {
        current = SqliteDataService.GetCurrentPool();
        await Task.CompletedTask;
    }

}