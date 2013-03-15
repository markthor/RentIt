namespace RentItServer.ITU.Search
{
    internal class TernarySearchTrieNode<TV>
    {
        /// <summary>
        /// Gets or sets the character.
        /// </summary>
        /// <value>
        /// The character.
        /// </value>
        public char Character { get; internal set; }
        /// <summary>
        /// Gets or sets the left node.
        /// </summary>
        /// <value>
        /// The left node.
        /// </value>
        public TernarySearchTrieNode<TV> LeftNode { get; internal set; }
        /// <summary>
        /// Gets or sets the middle node.
        /// </summary>
        /// <value>
        /// The middle node.
        /// </value>
        public TernarySearchTrieNode<TV> MiddleNode { get; internal set; }
        /// <summary>
        /// Gets or sets the right node.
        /// </summary>
        /// <value>
        /// The right node.
        /// </value>
        public TernarySearchTrieNode<TV> RightNode { get; internal set; }
        /// <summary>
        /// Gets or sets the value associated with string.
        /// </summary>
        /// <value>
        /// The value associated with string.
        /// </value>
        public TV Value
        {
            get { return _value; }
            internal set
            {
                _value = value;
                IsValueSet = true;
            }
        }
        /// <summary>
        /// Gets a value indicating whether this instance has a value set.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is value set; otherwise, <c>false</c>.
        /// </value>
        public bool IsValueSet { get; private set; }
        /// <summary>
        /// The _value
        /// </summary>
        private TV _value;
    }
}