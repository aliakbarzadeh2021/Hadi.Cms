using Hadi.Cms.Model.Entities;
using Hadi.Cms.Repository.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Hadi.Cms.ApplicationService.Services
{
    public class ReportService
    {
        private DataContext _dataContext;

        public ReportService()
        {
            _dataContext = new DataContext();
        }

        public List<User> GetLoginUsersTopN(int N)
        {
            var hoursAgo4 = DateTime.Now.AddHours(-4);
            var result = _dataContext.LoginHistoryRepository.Where(l => l.IsLogin && l.CreateDate > hoursAgo4)
                .OrderByDescending(o => o.CreateDate).Select(x => x.User).Distinct().Take(N).ToList();

            return result;
        }
    }
}
