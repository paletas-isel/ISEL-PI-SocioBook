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
        private readonly string _text;

        public TextShare(string user, string text) : base(user)
        {
            _text = text;
        }

        public string Text { get { return _text; } }

        #region Overrides of Share

        public override string Type
        {
            get { return "text"; }
        }

        #endregion
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
