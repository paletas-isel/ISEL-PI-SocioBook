using System;

namespace Model
{
    public abstract class Share
    {
        private static long _masterStamp = 0;

        private readonly long _stamp;
        private readonly string _user;

        protected Share(string user)
        {
            _user = user;
            _stamp = _masterStamp++;
        }

        public long Stamp { get { return _stamp; } }

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

        public AnchorShare(string user, Uri anchor) : base(user)
        {
            _anchor = anchor;
        }

        public Uri Anchor { get { return _anchor; } }
    }

    public class VideoShare : AnchorShare
    {
        public VideoShare(string user, Uri video) : base(user, video)
        {

        }
    }
}
