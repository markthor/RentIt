using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using RentItServer.ITU.Exceptions;

namespace RentItServer.ITU.Search
{
    /// <summary>
    /// A special trie data structure where the child nodes of a standard trie are ordered as 
    /// a binary search tree. Ternary search trees are effective for applications involving 
    /// mapping strings to other values as well as completing near-neighbor lookups and other 
    /// string-related queries. 
    /// 
    /// Ternary search trees are a more space efficient (at the cost of speed) equivalent of 
    /// typical tries. 
    /// 
    /// Ternary search trees can be used any time a hashtable would be used to store strings
    /// </summary>
    /// <typeparam name="TV">The type of the value associated with each string.</typeparam>
    public class TernarySearchTrie<TV>
    {
        /// <summary>
        /// Gets the size.
        /// </summary>
        /// <value>
        /// The size.
        /// </value>
        public int Size { get; private set; }

        /// <summary>
        /// The _root
        /// </summary>
        private TernarySearchTrieNode<TV> _root;   // root of TST

        /// <summary>
        /// The _state lock
        /// </summary>
        private readonly Object _stateLock = new Object();

        /// <summary>
        /// Determines whether the specified key contains a value.
        /// </summary>
        /// <param name="key">True if key is in the symbol table. False if not</param>
        /// <returns></returns>
        public bool Contains(String key)
        {
            try
            {
                Get(key);
            }
            catch (NullValueException)
            {
                return false;
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Gets the value associated with specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>
        /// The value
        /// </returns>
        /// <exception cref="System.Exception">illegal key</exception>
        /// <exception cref="Utilities.NullValueException">Key has no associated value</exception>
        public TV Get(String key)
        {
            if (key == null || !key.Any()) throw new Exception("illegal key");

            lock (_stateLock)
            {
                TernarySearchTrieNode<TV> x = Get(_root, key, 0);
                if (x == null) throw new NullValueException();

                return x.Value;
            }
        }

        /// <summary>
        /// Gets the subtrie for the specified key.
        /// </summary>
        /// <param name="x">The current node.</param>
        /// <param name="key">The key.</param>
        /// <param name="d">The key string index.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception">illegal key</exception>
        private TernarySearchTrieNode<TV> Get(TernarySearchTrieNode<TV> x, String key, int d)
        {
            if (key == null || !key.Any()) throw new Exception("illegal key");
            if (x == null) return null;

            lock (_stateLock)
            {
                char c = key.ElementAt(d);
                if (c < x.Character)
                    return Get(x.LeftNode, key, d);
                if (c > x.Character)
                    return Get(x.RightNode, key, d);
                if (d < key.Length - 1)
                    return Get(x.MiddleNode, key, d + 1);
                return x;
            }
        }

        /// <summary>
        /// Puts the specified s into the symbol table.
        /// </summary>
        /// <param name="s">The s.</param>
        /// <param name="val">The value associated with s.</param>
        /// <exception cref="System.ArgumentNullException">parameter s was null</exception>
        public void Put(String s, TV val)
        {
            if (s == null) throw new ArgumentNullException("parameter s was null");

            lock (_stateLock)
            {
                _root = Put(_root, s, val, 0);
            }
        }

        /// <summary>
        /// Puts the specified x.
        /// </summary>
        /// <param name="x">The node we're currently examining.</param>
        /// <param name="s">The key we want to insert.</param>
        /// <param name="val">The value for insertion.</param>
        /// <param name="d">The key string index.</param>
        /// <returns></returns>
        private TernarySearchTrieNode<TV> Put(TernarySearchTrieNode<TV> x, String s, TV val, int d)
        {

            lock (_stateLock)
            {
                char c = s.ElementAt(d);
                if (x == null)
                {
                    x = new TernarySearchTrieNode<TV>
                    {
                        Character = c
                    };
                }

                if (c < x.Character) x.LeftNode = Put(x.LeftNode, s, val, d);
                else if (c > x.Character) x.RightNode = Put(x.RightNode, s, val, d);
                else if (d < s.Length - 1) x.MiddleNode = Put(x.MiddleNode, s, val, d + 1);
                else
                {
                    x.Value = val;
                    Size++;
                }
                return x;
            }
        }

        /// <summary>
        /// Find and return longest prefix of s in the Ternary Search Treee.
        /// </summary>
        /// <param name="s">The s.</param>
        /// <returns>The longest prefix.</returns>
        public String LongestPrefixOf(String s)
        {
            if (s == null || !s.Any()) return null;
            int length = 0;

            lock (_stateLock)
            {
                TernarySearchTrieNode<TV> x = _root;
                int i = 0;
                while (x != null && i < s.Length)
                {
                    char c = s.ElementAt(i);
                    if (c < x.Character) x = x.LeftNode;
                    else if (c > x.Character) x = x.RightNode;
                    else
                    {
                        i++;
                        if (x.IsValueSet) length = i;
                        x = x.MiddleNode;
                    }
                }
                return s.Substring(0, length);
            }
        }

        /// <summary>
        /// All keys in this symbol table
        /// </summary>
        /// <returns></returns>
        public IEnumerable<String> Keys()
        {
            List<String> list = new List<String>();
            TernarySearchTrieNode<TV> snapshotRoot = _root;
            Collect(snapshotRoot, "", list);
            return list;
        }

        /// <summary>
        /// Find all keys starting with prefix.
        /// </summary>
        /// <param name="prefix">The prefix.</param>
        /// <returns>All keys starting with given prefix.</returns>
        /// <exception cref="System.ArgumentNullException">parameter prefix was null</exception>
        public IEnumerable<String> PrefixMatch(String prefix)
        {
            if (prefix == null) throw new ArgumentNullException("parameter prefix was null");

            List<String> list = new List<String>();
            TernarySearchTrieNode<TV> x = Get(_root, prefix, 0);
            if (x == null) return list;
            if (x.IsValueSet) list.Add(prefix);
            TernarySearchTrieNode<TV> snapshotRoot = x.MiddleNode;
            Collect(snapshotRoot, prefix, list);
            return new ReadOnlyCollection<string>(list);

        }

        /// <summary>
        /// Collects all keys in subtrie rooted at x with given prefix.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="prefix">The prefix.</param>
        /// <param name="list">The list containing keys.</param>
        private void Collect(TernarySearchTrieNode<TV> x, String prefix, List<String> list)
        {
            if (x == null) return;
            Collect(x.LeftNode, prefix, list);
            if (x.IsValueSet) list.Add(prefix + x.Character);
            Collect(x.MiddleNode, prefix + x.Character, list);
            Collect(x.RightNode, prefix, list);
        }

        /// <summary>
        /// Find all keys matching given wilcard pattern.
        /// </summary>
        /// <param name="pattern">The pattern.</param>
        /// <returns>All keys matching given wilcard pattern.</returns>
        /// <exception cref="System.ArgumentNullException">parameter pattern was null</exception>
        public IEnumerable<String> WildcardMatch(String pattern)
        {
            if (pattern == null) throw new ArgumentNullException("parameter pattern was null");

            List<String> list = new List<String>();
            lock (_stateLock)
            {
                Collect(_root, "", 0, pattern, list);		// Lock the entire method, since the method depends on mutable local variables
                return list;
            }
        }

        /// <summary>
        /// Collects keys that match the prefix and the temporary pattern.
        /// </summary>
        /// <param name="x">The node we're currently at.</param>
        /// <param name="prefix">The prefix we're originally searching for.</param>
        /// <param name="i">The Number of the node we're going to..</param>
        /// <param name="pattern">The temporary pattern.</param>
        /// <param name="list">A list containing the nodes collected so far..</param>
        private void Collect(TernarySearchTrieNode<TV> x, String prefix, int i, String pattern, List<String> list)
        {
            if (x == null) return;

            lock (_stateLock)
            {
                char c = pattern.ElementAt(i);
                if (c == '.' || c < x.Character) Collect(x.LeftNode, prefix, i, pattern, list);
                if (c == '.' || c == x.Character)
                {
                    if (i == pattern.Length - 1 && x.IsValueSet) list.Add(prefix + x.Character);
                    if (i < pattern.Length - 1) Collect(x.MiddleNode, prefix + x.Character, i + 1, pattern, list);
                }
                if (c == '.' || c > x.Character) Collect(x.RightNode, prefix, i, pattern, list);
            }
        }

        /// <summary>
        /// Builds a Ternary Search Trie from a Dictionary of Strings and values.
        /// </summary>
        /// <param name="dic">The dic.</param>
        /// <exception cref="System.ArgumentNullException">parameter dic was null</exception>
        public void Build(Dictionary<String, TV> dic)
        {
            if (dic == null) throw new ArgumentNullException("parameter dic was null");

            lock (_stateLock)
            {
                foreach (String name in dic.Keys.Where(s => s.Length != 0))
                {
                    Put(name, dic[name]);
                }
            }
        }
    }
}