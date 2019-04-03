﻿using Base.DAL;
using Base.DAL.EF;
using Base.Forum.Entities;
using Common.Data.EF;

namespace Common.Data
{
    public class ForumConfig
    {
        public static void Init<TContext>(EntityConfigurationBuilder config) where TContext : EFContext
        {
            config.Context(EFRepositoryFactory<TContext>.Instance)
                .Entity<ForumPost>()
                .Entity<ForumSection>()
                .Entity<ForumTopic>();
        }
    }
}