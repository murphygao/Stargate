using MessageQueue.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MessageQueue.Services
{
    public class DataCleaner
    {
        public MessageQueueDbContext _dbContext;
        public DataCleaner(MessageQueueDbContext _dbContext)
        {
            this._dbContext = _dbContext;
        }
        public async Task StartCleanerService()
        {
            await TimeoutCleaner.AllClean(_dbContext);
        }
    }
}