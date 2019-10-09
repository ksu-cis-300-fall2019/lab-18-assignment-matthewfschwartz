/* Dictionary.cs
 * Author: Rod Howell
 * Modified By: Matthew Schwartz
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KansasStateUniversity.TreeViewer2;
using Ksu.Cis300.ImmutableBinaryTrees;

namespace Ksu.Cis300.NameLookup
{
    /// <summary>
    /// A generic dictionary in which keys must implement IComparable.
    /// </summary>
    /// <typeparam name="TKey">The key type.</typeparam>
    /// <typeparam name="TValue">The value type.</typeparam>
    public class Dictionary<TKey, TValue> where TKey : IComparable<TKey>
    {
        /// <summary>
        /// This method should return the result of removing the node with smallest Key from the given binary search tree
        /// </summary>
        /// <param name="t">Resulting tree of removing key value pair min</param>
        /// <param name="min">The pair removed (smallest key value pair)</param>
        /// <returns></returns>
        private static BinaryTreeNode<KeyValuePair<TKey, TValue>> RemoveMininumKey(BinaryTreeNode<KeyValuePair<TKey, TValue>> t, out KeyValuePair<TKey, TValue> min)
        {
            if (t.LeftChild == null)
            {
                min = t.Data;
                return t.RightChild;
            }
            else
            {
                BinaryTreeNode<KeyValuePair<TKey, TValue>> temp = new BinaryTreeNode<KeyValuePair<TKey, TValue>>(t.Data, RemoveMininumKey(t.LeftChild, out min), t.RightChild);
                return temp;
            }
        }

        /// <summary>
        /// Returns the result of removing the node containing the given key from the given binary search tree
        /// </summary>
        /// <param name="key">Key that we want to remove</param>
        /// <param name="t">Tree that we want to remove the node containing key from</param>
        /// <param name="removed">Whether or not the node containing key was successfully removed</param>
        /// <returns>The result of removing the node containing the key from the binary search tree t</returns>
        private static BinaryTreeNode<KeyValuePair<TKey, TValue>> Remove(TKey key, BinaryTreeNode<KeyValuePair<TKey, TValue>> t, out bool removed)
        {
            if (t == null)
            {
                removed = false;
                return t;
            }
            else
            {
                int compare = t.Data.Key.CompareTo(key);

                if (compare == 0)
                {
                    removed = true;
                    if (t.LeftChild == null)
                    {
                        return t.RightChild; // Return t.RightChild because this is the result of removing the root
                    }
                    else if (t.RightChild == null)
                    {
                        return t.LeftChild; // Return t.LeftChild because this is the result of removing the root
                    }
                    else
                    {
                        BinaryTreeNode<KeyValuePair<TKey, TValue>> findNewRightChild = RemoveMininumKey(t.RightChild, out KeyValuePair<TKey, TValue> min);
                        BinaryTreeNode<KeyValuePair<TKey, TValue>> result = new BinaryTreeNode<KeyValuePair<TKey, TValue>>(min, t.LeftChild, findNewRightChild);
                        return result;
                    }
                }
                else if (compare > 0)
                {
                    BinaryTreeNode<KeyValuePair<TKey, TValue>> removeKey = Remove(key, t.LeftChild, out removed);
                    BinaryTreeNode<KeyValuePair<TKey, TValue>> result = new BinaryTreeNode<KeyValuePair<TKey, TValue>>(t.Data, removeKey, t.RightChild);
                    return result;

                }
                else
                {
                    BinaryTreeNode<KeyValuePair<TKey, TValue>> removeKey = Remove(key, t.RightChild, out removed);
                    BinaryTreeNode<KeyValuePair<TKey, TValue>> result = new BinaryTreeNode<KeyValuePair<TKey, TValue>>(t.Data, t.LeftChild, removeKey);
                    return result;
                }
            }
        }

        /// <summary>
        /// Check to see if the key is null, Remove the key k from _elements
        /// </summary>
        /// <param name="k">Key to be removed</param>
        /// <returns>Returns whether or not the key was removed</returns>
        public bool Remove(TKey k)
        {
            CheckKey(k);
            _elements = Remove(k, _elements, out bool removed);
            return removed;
        }

        /// <summary>
        /// The keys and values in the dictionary.
        /// </summary>
        private BinaryTreeNode<KeyValuePair<TKey, TValue>> _elements = null;

        /// <summary>
        /// Gets a drawing of the underlying binary search tree.
        /// </summary>
        public TreeForm Drawing => new TreeForm(_elements, 100);

        /// <summary>
        /// Checks to see if the given key is null, and if so, throws an
        /// ArgumentNullException.
        /// </summary>
        /// <param name="key">The key to check.</param>
        private static void CheckKey(TKey key)
        {
            if (key == null)
            {
                throw new ArgumentNullException();
            }
        }

        /// <summary>
        /// Finds the given key in the given binary search tree.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="t">The binary search tree.</param>
        /// <returns>The node containing key, or null if the key is not found.</returns>
        private static BinaryTreeNode<KeyValuePair<TKey, TValue>> Find(TKey key, BinaryTreeNode<KeyValuePair<TKey, TValue>> t)
        {
            if (t == null)
            {
                return null;
            }
            else
            {
                int comp = key.CompareTo(t.Data.Key);
                if (comp == 0)
                {
                    return t;
                }
                else if (comp < 0)
                {
                    return Find(key, t.LeftChild);
                }
                else
                {
                    return Find(key, t.RightChild);
                }
            }
        }

        /// <summary>
        /// Builds the binary search tree that results from adding the given key and value to the given tree.
        /// If the tree already contains the given key, throws an ArgumentException.
        /// </summary>
        /// <param name="t">The binary search tree.</param>
        /// <param name="k">The key.</param>
        /// <param name="v">The value.</param>
        /// <returns>The binary search tree that results from adding k and v to t.</returns>
        private static BinaryTreeNode<KeyValuePair<TKey, TValue>> Add(BinaryTreeNode<KeyValuePair<TKey, TValue>> t, TKey k, TValue v)
        {
            if (t == null)
            {
                return new BinaryTreeNode<KeyValuePair<TKey, TValue>>(new KeyValuePair<TKey, TValue>(k, v), null, null);
            }
            else
            {
                int comp = k.CompareTo(t.Data.Key);
                if (comp == 0)
                {
                    throw new ArgumentException();
                }
                else if (comp < 0)
                {
                    return new BinaryTreeNode<KeyValuePair<TKey, TValue>>(t.Data, Add(t.LeftChild, k, v), t.RightChild);
                }
                else
                {
                    return new BinaryTreeNode<KeyValuePair<TKey, TValue>>(t.Data, t.LeftChild, Add(t.RightChild, k, v));
                }
            }
        }

        /// <summary>
        /// Tries to get the value associated with the given key.
        /// </summary>
        /// <param name="k">The key.</param>
        /// <param name="v">The value associated with k, or the default value if
        /// k is not in the dictionary.</param>
        /// <returns>Whether k was found as a key in the dictionary.</returns>
        public bool TryGetValue(TKey k, out TValue v)
        {
            CheckKey(k);
            BinaryTreeNode<KeyValuePair<TKey, TValue>> p = Find(k, _elements);
            if (p == null)
            {
                v = default(TValue);
                return false;
            }
            else
            {
                v = p.Data.Value;
                return true;
            }
        }

        /// <summary>
        /// Adds the given key with the given associated value.
        /// If the given key is already in the dictionary, throws an
        /// InvalidOperationException.
        /// </summary>
        /// <param name="k">The key.</param>
        /// <param name="v">The value.</param>
        public void Add(TKey k, TValue v)
        {
            CheckKey(k);
            _elements = Add(_elements, k, v);
        }
    }
}
