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
    }

    public class TextShare : Share
    {
        private readonly string _text;

        public TextShare(string user, string text) : base(user)
        {
            _text = text;
        }

        public string Text { get { return _text; } }
    }

    public class AnchorShare : Share
    {
        private readonly Uri _anchor;

        public AnchorShare(string user, string anchor) : base(user)
        {
            _anchor = new Uri(anchor);
        }

        public AnchorShare(string user, Uri anchor) : base(user)
        {
            _anchor = anchor;
        }

        public Uri Anchor { get { return _anchor; } }
    }

    public class VideoShare : AnchorShare
    {
        public VideoShare(string user, string video) : base(user, video)
        {

        }

        public VideoShare(string user, Uri video) : base(user, video)
        {

        }
    }
}
