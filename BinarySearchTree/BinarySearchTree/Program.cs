using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace BinarySearchTree
{
    [Serializable]
    public class TreeNode
    {
        public TreeNode left;
        public int data;
        public TreeNode right;

        public TreeNode(int value, TreeNode l, TreeNode r)
        {
            data = value;
            left = l;
            right = r;
        }
    }

    public class BinarySearchTree
    {
        private TreeNode root = null;

        public void Add(int value)
        {
            root = InsertNode(root, value);
        }

        public void Remove(int value)
        {
            root = DeleteNode(root, value);
        }

        public bool Conatins(int value)
        {
            return ContainNode(root, value);
        }

        public void Clear()
        {
            MakeEmpty(root);
            root = null;
        }

        public void SaveToFile(string fileName)
        {
            var formatter = new BinaryFormatter();

            using (FileStream fs = File.Create(fileName))
            {
                if (root != null)
                    formatter.Serialize(fs, root);
            }
        }

        public void LoadFromFile(string fileName)
        {
            if (File.Exists(fileName))
            {
                using (FileStream fs = new FileStream(fileName, FileMode.Open))
                {
                    var formatter = new BinaryFormatter();
                    root = (TreeNode)formatter.Deserialize(fs);
                }
            }
        }

        public void InOrder()
        {
            PrintInOrder(root);
        }

        public void PreOrder()
        {
            PrintPreOrder(root);
        }

        public void PostOrder()
        {
            PrintPostOrder(root);
        }

        public void LevelOrder()
        {
            TreeNode temp;
            Queue<TreeNode> queue = new Queue<TreeNode>();

            if (root == null)
                return;

            queue.Enqueue(root);

            while (queue.Count > 0)
            {
                temp = queue.Dequeue();

                Console.Write($" {temp.data}");

                if (temp.left != null)
                    queue.Enqueue(temp.left);

                if (temp.right != null)
                    queue.Enqueue(temp.right);
            }
        }

        public void Output()
        {
            if (root == null)
            {
                Console.WriteLine("\nTree is empty\n");
                return;
            }
            Console.WriteLine("\nTree:\n");
            PrintTree(root, 0);
        }

        private bool ContainNode(TreeNode tree, int value)
        {
            if (tree == null)
                return false;
            else if (value < tree.data)
                return ContainNode(tree.left, value);
            else if (value > tree.data)
                return ContainNode(tree.right, value);
            else
                return true;
        }

        private void PrintInOrder(TreeNode tree)
        {
            if (tree != null)
            {
                PrintInOrder(tree.left);
                Console.Write($" {tree.data}");
                PrintInOrder(tree.right);
            }
        }

        private void PrintPreOrder(TreeNode tree)
        {
            if (tree != null)
            {
                Console.Write($" {tree.data}");
                PrintPreOrder(tree.left);
                PrintPreOrder(tree.right);
            }
        }

        private void PrintPostOrder(TreeNode tree)
        {
            if (tree != null)
            {
                PrintPostOrder(tree.left);
                PrintPostOrder(tree.right);
                Console.Write($" {tree.data}");
            }
        }

        private void PrintTree(TreeNode tree, int totalSpaces)
        {
            if (tree != null)
            {
                PrintTree(tree.right, totalSpaces + 5);
                Console.WriteLine(new string(' ', totalSpaces) + $"{tree.data,2}");
                PrintTree(tree.left, totalSpaces + 5);
            }
        }

        private TreeNode InsertNode(TreeNode tree, int value)
        {
            if (tree == null)
                tree = new TreeNode(value, null, null);

            else if (value < tree.data)
                tree.left = InsertNode(tree.left, value);

            else if (value > tree.data)
                tree.right = InsertNode(tree.right, value);

            //else
            //    Console.Write("dup");

            return tree;
        }

        private void MakeEmpty(TreeNode tree)
        {
            if (tree != null)
            {
                MakeEmpty(tree.left);
                MakeEmpty(tree.right);
                tree.left = tree.right = null;
            }
        }

        private TreeNode DeleteNode(TreeNode tree, int value)
        {
            if (tree == null)
                return null;

            else if (value < tree.data)
                tree.left = DeleteNode(tree.left, value);

            else if (value > tree.data)
                tree.right = DeleteNode(tree.right, value);

            else
            {
                if (tree.left == null && tree.right == null)
                    return null;

                else if (tree.left == null && tree.right != null)
                    return tree.right;

                else if (tree.left != null && tree.right == null)
                    return tree.left;

                else
                {
                    TreeNode temp = MaxValueNode(tree.left);
                    tree.data = temp.data;
                    tree.left = DeleteNode(tree.left, temp.data);
                }
            }

            return tree;
        }

        private TreeNode MaxValueNode(TreeNode tree)
        {
            while (tree.right != null)
                tree = tree.right;

            return tree;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var tree = new BinarySearchTree();

            /*
            Random rand = new Random();

            int[] arr = { 27, 13, 42, 6, 17, 33, 48 };

            //int[] arr = new int[20];

            //for(int i = 0; i < arr.Length; i++)
            //{
            //    arr[i] = rand.Next(10);
            //}

            foreach (var item in arr)
                tree.Add(item);

            Console.WriteLine("InOrder:");
            tree.InOrder();

            Console.WriteLine("\n\nPreOrder:");
            tree.PreOrder();

            Console.WriteLine("\n\nPostOrder:");
            tree.PostOrder();

            Console.WriteLine("\n\nLevelOrder:");
            tree.LevelOrder();

            Console.WriteLine("\n\nOutputTree:\n");
            tree.OutputTree();

            //Console.WriteLine("\nDelete 13");
            //tree.Delete(13);

            //Console.WriteLine("\n\nOutputTree:\n");
            //tree.OutputTree();

            Console.WriteLine("\nContains 64\n");
            Console.WriteLine(tree.Conatins(64));

            //Console.WriteLine("\nSave tree\n");
            //tree.SaveToFile("SavedTree");

            Console.WriteLine("\nClear tree\n");
            tree.Clear();
            */
            //Console.WriteLine("\nLoad tree");
            //tree.LoadFromFile("SavedTree");

            //Console.WriteLine("\n\nOutputTree:\n");
            //tree.OutputTree();

            //Console.WriteLine("\n");

            string line;
            string[] words;
            string command;

            while ((line = Console.ReadLine()) != null)
            {
                words = line.Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);

                if (words.Length > 0)
                {
                    command = words[0];

                    if (command == "+")
                    {
                        for (int i = 1; i < words.Length; i++)
                        {
                            int value;
                            if (int.TryParse(words[i], out value))
                                tree.Add(value);
                        }
                    }
                    else if (command == "-")
                    {
                        for (int i = 1; i < words.Length; i++)
                        {
                            int value;
                            if (int.TryParse(words[i], out value))
                                tree.Remove(value);
                        }
                    }
                    else if (command == "contains" && words.Length > 1)
                    {
                        int value;
                        if (int.TryParse(words[1], out value))
                            Console.WriteLine($"\n{tree.Conatins(value)}\n");
                    }
                    else if (command == "clear")
                        tree.Clear();

                    else if (command == "save" && words.Length > 1)
                        tree.SaveToFile(words[1]);

                    else if (command == "load" && words.Length > 1)
                        tree.LoadFromFile(words[1]);

                    else if (command == "print")
                        tree.Output();
                    else if (command == "quit")
                    {
                        Console.WriteLine("\n");
                        return;
                    }
                }
            }
        }
    }
}
