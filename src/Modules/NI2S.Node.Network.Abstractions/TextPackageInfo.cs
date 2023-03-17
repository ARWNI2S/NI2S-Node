namespace NI2S.Node.Networking
{
    public class TextPackageInfo
    {
        public string? Text { get; set; }

        public override string ToString()
        {
            return Text ?? "";
        }
    }
}
