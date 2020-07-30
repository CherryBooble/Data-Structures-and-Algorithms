using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace AvlTree
{
    [Serializable]
    public class AvlNode
    {
        public AvlNode Left;
        public AvlNode Right;
        public string Key;
        public int Value;
        public int Height;
    }

    public class AvlTree
    {
        private AvlNode _root;

        public void Output()
        {
            if (_root == null)
            {
                Console.WriteLine("\nTree is empty\n");
                return;
            }

            Console.WriteLine("\nTree:\n");
            PrintTree(_root, 0);
            Console.WriteLine();
        }

        public void Add(string key, int value)
        {
            _root = Insert(_root, key, value);
        }

        public void Remove(string key)
        {
            _root = Delete(_root, key);
        }

        public void SaveToFile(string fileName)
        {
            var formatter = new BinaryFormatter();

            using (FileStream fs = File.Create(fileName))
            {
                if (_root != null)
                    formatter.Serialize(fs, _root);
            }
        }

        public void LoadFromFile(string fileName)
        {
            if (File.Exists(fileName))
            {
                using (FileStream fs = new FileStream(fileName, FileMode.Open))
                {
                    var formatter = new BinaryFormatter();
                    _root = (AvlNode)formatter.Deserialize(fs);
                }
            }
        }

        public bool Search(string key, out int value)
        {
            value = 0;
            return Contains(_root, key, ref value);
        }

        public void Clear()
        {
            MakeEmpty(_root);
            _root = null;
        }

        private void MakeEmpty(AvlNode tree)
        {
            if (tree != null)
            {
                MakeEmpty(tree.Left);
                MakeEmpty(tree.Right);
                tree.Left = tree.Right = null;
            }
        }

        private bool Contains(AvlNode tree, string key, ref int value)
        {
            if (tree == null)
                return false;
            else if (string.Compare(key, tree.Key) > 0)
                return Contains(tree.Right, key, ref value);
            else if (string.Compare(key, tree.Key) < 0)
                return Contains(tree.Left, key, ref value);
            else
            {
                value = tree.Value;
                return true;
            }
        }

        private AvlNode Insert(AvlNode tree, string key, int value)
        {
            if (tree == null)
                return new AvlNode { Key = key, Value = value };

            else if (string.Compare(key, tree.Key) > 0)
                tree.Right = Insert(tree.Right, key, value);

            else if (string.Compare(key, tree.Key) < 0)
                tree.Left = Insert(tree.Left, key, value);

            return Balance(tree);
        }

        private AvlNode Delete(AvlNode tree, string key)
        {
            if (tree == null)
                return null;

            if (string.Compare(key, tree.Key) > 0)
                tree.Right = Delete(tree.Right, key);

            else if (string.Compare(key, tree.Key) < 0)
                tree.Left = Delete(tree.Left, key);

            else
            {
                if (tree.Left == null && tree.Right == null)
                    return null;

                else if (tree.Left == null && tree.Right != null)
                    return tree.Right;

                else if (tree.Left != null && tree.Right == null)
                    return tree.Left;

                else
                {
                    AvlNode temp = MaxValueNode(tree.Left);
                    tree.Key = temp.Key;
                    tree.Value = temp.Value;
                    tree.Left = Delete(tree.Left, temp.Key);
                }
            }

            return Balance(tree);
        }

        private AvlNode MaxValueNode(AvlNode tree)
        {
            while (tree.Right != null)
                tree = tree.Right;

            return tree;
        }

        private int Height(AvlNode tree)
        {
            return tree == null ? -1 : tree.Height;
        }

        private void FixHeight(AvlNode tree)
        {
            tree.Height = Math.Max(Height(tree.Left), Height(tree.Right)) + 1;
        }

        private int BalanceFactor(AvlNode tree)
        {
            return Height(tree.Right) - Height(tree.Left);
        }

        private AvlNode Balance(AvlNode tree)
        {
            FixHeight(tree);
            if (BalanceFactor(tree) == 2)
            {
                if (BalanceFactor(tree.Right) < 0)
                    tree.Right = RotateRight(tree.Right);

                return RotateLeft(tree);
            }

            if (BalanceFactor(tree) == -2)
            {
                if (BalanceFactor(tree.Left) > 0)
                    tree.Left = RotateLeft(tree.Left);
                return RotateRight(tree);
            }

            return tree;
        }

        private AvlNode RotateRight(AvlNode tree)
        {
            AvlNode left = tree.Left;
            tree.Left = left.Right;
            left.Right = tree;

            FixHeight(tree);
            FixHeight(left);

            return left;
        }

        private AvlNode RotateLeft(AvlNode tree)
        {
            AvlNode right = tree.Right;
            tree.Right = right.Left;
            right.Left = tree;
            FixHeight(tree);
            FixHeight(right);

            return right;
        }

        private void PrintTree(AvlNode tree, int totalSpaces)
        {
            if (tree != null)
            {
                PrintTree(tree.Right, totalSpaces + 5);
                Console.WriteLine(new string(' ', totalSpaces) + $"{tree.Key,2}");
                PrintTree(tree.Left, totalSpaces + 5);
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            AvlTree tree = new AvlTree();

            string line;
            string[] words;
            string command;

            Console.Write(">>");

            while ((line = Console.ReadLine()) != null && line != "")
            {
                words = line.Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);

                if (words.Length > 0)
                {
                    command = words[0];

                    switch (command)
                    {
                        case "+":
                            for (int i = 2; i < words.Length; i += 2)
                            {
                                int value;
                                if (int.TryParse(words[i], out value))
                                    tree.Add(words[i - 1], value);
                            }
                            Console.WriteLine();
                            break;
                        case "-":
                            for (int i = 1; i < words.Length; i++)
                                tree.Remove(words[i]);
                            break;
                        case "save":
                            if (words.Length > 1)
                                tree.SaveToFile(words[1]);
                            break;
                        case "load":
                            if (words.Length > 1)
                                tree.LoadFromFile(words[1]);
                            break;
                        case "search":
                            if (words.Length > 1)
                            {
                                int value;
                                Console.WriteLine(tree.Search(words[1], out value) ? $"\nValue found: {value}" : "\nNot found\n"); 
                            }
                            break;
                        case "output":
                            tree.Output();
                            break;
                        case "clear":
                            tree.Clear();
                            break;
                        default:
                            Console.WriteLine("\nThere is no such command.\n");
                            break;
                    }
                }

                Console.Write("\n>>");
            }
        }
    }
}
