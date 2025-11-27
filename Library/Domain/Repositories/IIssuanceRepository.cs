using Library.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Domain.Repositories
{
    public interface IIssuanceRepository
    {
        void Issue(string isbn, Guid userId);
        void Return(string isbn, Guid userId);
        IEnumerable<IssueDto> GetAllIssues(string isbn);
        IEnumerable<IssueDto> GetAllIssues(Guid userId);
    }
}
