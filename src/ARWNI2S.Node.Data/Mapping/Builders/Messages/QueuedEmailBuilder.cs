namespace ARWNI2S.Node.Data.Mapping.Builders.Messages
{
    /// <summary>
    /// Represents a queued email entity builder
    /// </summary>
    public partial class QueuedEmailBuilder : ServerEntityBuilder<QueuedEmail>
    {
        #region Methods

        /// <summary>
        /// Apply entity configuration
        /// </summary>
        /// <param name="table">Create table expression builder</param>
        public override void MapEntity(CreateTableExpressionBuilder table)
        {
            table
                .WithColumn(nameof(QueuedEmail.From)).AsString(500).NotNullable()
                .WithColumn(nameof(QueuedEmail.FromName)).AsString(500).Nullable()
                .WithColumn(nameof(QueuedEmail.To)).AsString(500).NotNullable()
                .WithColumn(nameof(QueuedEmail.ToName)).AsString(500).Nullable()
                .WithColumn(nameof(QueuedEmail.ReplyTo)).AsString(500).Nullable()
                .WithColumn(nameof(QueuedEmail.ReplyToName)).AsString(500).Nullable()
                .WithColumn(nameof(QueuedEmail.CC)).AsString(500).Nullable()
                .WithColumn(nameof(QueuedEmail.Bcc)).AsString(500).Nullable()
                .WithColumn(nameof(QueuedEmail.Subject)).AsString(1000).Nullable()
                .WithColumn(nameof(QueuedEmail.EmailAccountId)).AsInt32().ForeignKey<EmailAccount>();
        }

        #endregion
    }
}