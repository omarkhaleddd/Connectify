using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Core;

namespace Talabat.Core.Specifications
{
    public class ReportedPostWithPostSpecs : BaseSpecifications<ReportedPost>
    {
        public ReportedPostWithPostSpecs() : base()
        {
            Includes.Add(P => P.Post);
        }
        public ReportedPostWithPostSpecs(int postId) : base(p => p.PostId == postId)
        {
            Includes.Add(P => P.Post);
        }
    }
}
