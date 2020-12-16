﻿using Autobots.Infrastracture.Common.DBSolution;
using Autobots.Infrastracture.Common.RepositorySolution;
using ResearchAPI.CORS.Common;
using System;
using System.Collections.Generic;

namespace ResearchAPI.CORS.Repositories
{

    public class ProjectMemberRepository : Repository<ProjectMember>
    {
        public ProjectMemberRepository(DbContext context) : base(context)
        {
        }

        internal void CreateProjectMembers(List<ProjectMember> members)
        {
            foreach (var member in members)
            {
                Insert(member);
            }
        }
    }
}