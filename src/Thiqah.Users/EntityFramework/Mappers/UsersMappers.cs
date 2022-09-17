using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Thiqah.Users.Domain.Users;

namespace Thiqah.Users.EntityFramework.Mappers;

public sealed class UsersMappers : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");
        builder.HasKey(x => x.Id);
        builder.HasQueryFilter(x => !x.IsDeleted);
        builder.Ignore(x => x.Validator);
    }
}
