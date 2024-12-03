using ARWNI2S.Data.Entities.Users;
using FluentMigrator.Builders.Create.Table;

namespace ARWNI2S.Data.Mapping.Builders.Users
{
    /// <summary>
    /// Represents a user entity builder
    /// </summary>
    public partial class UserBuilder : DataEntityBuilder<User>
    {
        #region Methods

        /// <summary>
        /// Apply entity configuration
        /// </summary>
        /// <param name="table">Create table expression builder</param>
        public override void MapEntity(CreateTableExpressionBuilder table)
        {
            table
                .WithColumn(nameof(User.Username)).AsString(1000).Nullable()
                .WithColumn(nameof(User.Email)).AsString(1000).Nullable()
                .WithColumn(nameof(User.EmailToRevalidate)).AsString(1000).Nullable()
                .WithColumn(nameof(User.FirstName)).AsString(1000).Nullable()
                .WithColumn(nameof(User.LastName)).AsString(1000).Nullable()
                .WithColumn(nameof(User.Gender)).AsString(1000).Nullable()
                .WithColumn(nameof(User.Company)).AsString(1000).Nullable()
                .WithColumn(nameof(User.StreetAddress)).AsString(1000).Nullable()
                .WithColumn(nameof(User.StreetAddress2)).AsString(1000).Nullable()
                .WithColumn(nameof(User.ZipPostalCode)).AsString(1000).Nullable()
                .WithColumn(nameof(User.City)).AsString(1000).Nullable()
                .WithColumn(nameof(User.County)).AsString(1000).Nullable()
                .WithColumn(nameof(User.Phone)).AsString(1000).Nullable()
                .WithColumn(nameof(User.Fax)).AsString(1000).Nullable()
                .WithColumn(nameof(User.VatNumber)).AsString(1000).Nullable()
                .WithColumn(nameof(User.TimeZoneId)).AsString(1000).Nullable()
                .WithColumn(nameof(User.CustomUserAttributesXML)).AsString(int.MaxValue).Nullable()
                .WithColumn(nameof(User.DateOfBirth)).AsDateTime2().Nullable()
                .WithColumn(nameof(User.SystemName)).AsString(400).Nullable()
                //.WithColumn(nameof(User.CurrencyId)).AsInt32().ForeignKey<Currency>(onDelete: Rule.SetNull).Nullable()
                //.WithColumn(nameof(User.TokenId)).AsInt32().ForeignKey<Token>(onDelete: Rule.SetNull).Nullable()
                //.WithColumn(nameof(User.LanguageId)).AsInt32().ForeignKey<Language>(onDelete: Rule.SetNull).Nullable()
                //.WithColumn(NameCompatibilityManager.GetColumnName(typeof(User), nameof(User.BillingAddressId))).AsInt32().ForeignKey<Address>(onDelete: Rule.None).Nullable()
                //.WithColumn(NameCompatibilityManager.GetColumnName(typeof(User), nameof(User.ShippingAddressId))).AsInt32().ForeignKey<Address>(onDelete: Rule.None).Nullable()
                ;
        }

        #endregion
    }
}