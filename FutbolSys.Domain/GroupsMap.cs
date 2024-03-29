﻿using System.Data.Entity.ModelConfiguration;

namespace FutbolSys.Domain
{
    internal class GroupsMap : EntityTypeConfiguration<Group>
    {
        public GroupsMap()
        {
            HasRequired(o => o.Owner)
                .WithMany(m => m.UserGroups)
                .HasForeignKey(m => m.OwnerId);

        }
    }
}