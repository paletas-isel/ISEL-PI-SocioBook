using System;

namespace Model
{
    public abstract class Share
    {
        private readonly string _user;

        protected Share(string user)
        {
            _user = user;
        }

        public long Stamp { get; set; }

        public string User { get { return _user; } }

        public abstract string Type { get; }
    }

    public class TextShare : Share
    {
        private readonly string _content;

        public TextShare(string user, string content) : base(user)
        {
            _content = content;
        }

        public string Content { get { return _content; } }

        #region Overrides of Share

        public override string Type
        {
            get { return "text"; }
        }

        #endregion
    }

    public class AnchorShare : Share
    {
        private readonly Uri _content;

        public AnchorShare(string user, string anchor) : base(user)
        {
            _content = new Uri(anchor);
        }

        public AnchorShare(string user, Uri content) : base(user)
        {
            _content = content;
        }

        public Uri Content { get { return _content; } }

        #region Overrides of Share

        public override string Type
        {
            get { return "anchor"; }
        }

        #endregion
    }

    public class VideoShare : AnchorShare
    {
        public VideoShare(string user, string video) : base(user, video)
        {

        }

        public VideoShare(string user, Uri video) : base(user, video)
        {

        }

        #region Overrides of Share

        public override string Type
        {
            get { return "video"; }
        }

        #endregion
    }
}
