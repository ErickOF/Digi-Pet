using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Services
{
    public interface IUserChoose
    {
        string NextWalker();

    }
    public class UserChoose : IUserChoose
    {
        private readonly object mLock = new object();
        public const string New = "new";
        public const string Old = "old";
        public string Current { get; set; }
        public UserChoose()
        {
            Current = "new";
        }
        public string NextWalker()
        {
            lock (mLock)
            {
                if (Current == New) Current = Old;
                else Current = New;

                return Current;
            }
        }
    }
}
