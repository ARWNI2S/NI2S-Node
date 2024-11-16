using ARWNI2S.Infrastructure.Engine;
using System.Text;

namespace ARWNI2S.Node.Core.Diagnostics
{
    internal class DetailedError
    {
        private DetailedErrorModel _model;

        public DetailedError(DetailedErrorModel model)
        {
            _model = model;
        }

        public EngineContext Context { get; private set; }
        public IEvent Callback { get; private set; }

        internal async Task ExecuteAsync(EngineContext context)
        {
            Context = context;
            Callback = Context.Callback;
            //Response = Context.Response;
            var buffer = new MemoryStream();
            var output = new StreamWriter(buffer, Encoding.UTF8 /*UTF8NoBOM*/, 4096, leaveOpen: true);
            //await ExecuteAsync();
            await output.FlushAsync();
            //await Output.DisposeAsync();
            buffer.Seek(0, SeekOrigin.Begin);
            //await buffer.CopyToAsync(Response.Body);
        }
    }
}