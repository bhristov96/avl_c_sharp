using System;
using System.Diagnostics;

namespace ConsoleApp1
{
    class BST
    {
        private class Node
        {
            private Node left;
            private Node right;
            private int data;
            private int height;

            public int Height
            {
                get { return height; }
                set { height = value; }
            }

            public int Data
            {
                get { return data; }
                set { data = value; }
            }

            public Node Left
            {
                get { return left; }
                set { left = value; }
            }

            public Node Right
            {
                get { return right; }
                set { right = value; }
            }

            public Node(int n_data)
            {
                data = n_data;
                left = null;
                right = null;
            }
        }

        private Node root;

        public BST()
        {
            root = null;
        }

        private int update_height(Node branch)
        {
            if (branch == null) return -1;

            int lh = 1 + update_height(branch.Left);
            int rh = 1 + update_height(branch.Right);

            branch.Height = (lh > rh ? lh : rh);
            return branch.Height;
        }

        private bool Insert(int x, Node branch)
        {
            if (root == null)
            {
                root = new Node(x);
                return true;
            }
            else if (branch.Left == null && branch.Data > x)
            {
                branch.Left = new Node(x);
                return true;
            }
            else if (branch.Right == null && branch.Data < x)
            {
                branch.Right = new Node(x);
                return true;
            }
            else if (branch.Data < x)
            {
                if(Insert(x, branch.Right))
                {
                    update_height(branch);
                    balance(branch);
                    return true;
                }
                return false;
            }
            else if (branch.Data > x)
            {
                if(Insert(x, branch.Left))
                {
                    update_height(branch);
                    balance(branch);
                    return true;
                }
                return false;
            }
            return false;
        }

        public void Insert(int x)
        {
            if (Insert(x, root))
            {
                System.Console.WriteLine("Inserted: " + x);
            }
            else
            {
                System.Console.WriteLine("Failed to insert: " + x);
            }
        }

        public void print()
        {
            update_height(root);
            print(root);
        }

        public void print_root()
        {
            Console.WriteLine("Root is: " + root.Data + "|| Height: " + root.Height);
        }

        private void print(Node branch)
        {
            if (branch != null)
            {
                print(branch.Left);
                System.Console.Write("<" + branch.Data + " : H " + branch.Height + ">" + " ");
                print(branch.Right);
            }
        }

        private int get_balance(Node branch)
        {
            if(branch == null)
            {
                return -1;
            }

            int lh = (branch.Left == null ? -1 : branch.Left.Height);
            int rh = (branch.Right == null ? -1 : branch.Right.Height);

            return rh - lh;
        }

        private bool at_root(Node branch)
        {
            return branch.Data == root.Data;
        }
        private bool at_root(int data)
        {
            return data == root.Data;
        }

        private Node get_parent(int data)
        {
            if(root == null || at_root(data))
            {
                return null;
            }
            return helper_get_parent(data, root);

        }

        private Node helper_get_parent(int data, Node branch)
        {
            if(branch == null)
            {
                return null;
            }
            else if(branch.Left.Data == data || branch.Right.Data == data)
            {
                return branch;
            }
            else if(branch.Data > data)
            {
                return helper_get_parent(data, branch.Left);
            }
            else 
            {
                return helper_get_parent(data, branch.Right);
            }
        }

        private void balance(Node branch)
        {
            int balance_val = get_balance(branch);

            if(balance_val > 1)
            {
                Node parent = get_parent(branch.Data);
                Node temp = branch.Right;

                if(get_balance(branch.Right) == -1) // right left
                {
                    branch.Right = temp.Left;
                    temp.Left = branch.Right.Right;
                    branch.Right.Right = temp;
                }

                // right right
                branch.Right = temp.Left;
                bool isroot = at_root(branch.Data);
                temp.Left = branch;
                if(isroot)
                {
                    root = temp;
                }
                else
                {
                    if (parent.Left == branch)
                        parent.Left = temp;
                    else
                        parent.Right = temp;
                }
            }
            else if(balance_val < -1) 
            {
                Node parent = get_parent(branch.Data);
                Node temp = branch.Left;

                if(get_balance(branch.Left) == 1) // left right
                {
                    branch.Left = temp.Right;
                    temp.Right = branch.Left.Left;
                    branch.Left.Left = temp;
                }

                // left left
                branch.Left = temp.Right;
                temp.Right = branch;
                bool isroot = at_root(branch.Data);
                if(isroot)
                {
                    root = temp;
                }
                else
                {
                    if (parent.Left == branch)
                        parent.Left = temp;
                    else
                        parent.Right = temp;
                }
            }
        }

        private void print_at_height(Node branch, int height)
        {
            if (branch != null)
            {
                print_at_height(branch.Left, height);
                if(height == branch.Height)
                    System.Console.Write("<" + branch.Data + " : H " + branch.Height + ">" + " ");
                print_at_height(branch.Right, height);
            }
        }

        public void print_at_height(int height)
        {
            print_at_height(root,height);
        }

        public void testInsert()
        {
            Insert(3);
            Insert(1);
            Insert(2);
            Insert(6);
            Insert(4);
            Insert(7);

            Debug.Assert(root.Data == 3);
            Debug.Assert(root.Left.Data == 1);
            Debug.Assert(root.Left.Right.Data == 2);
            Debug.Assert(root.Right.Data == 6);
            Debug.Assert(root.Right.Right.Data == 7);
            Debug.Assert(root.Right.Left.Data == 4);
        }

        // to be tested with using a separate object since it it directly
        // modifying root
        public void testUpdateHeight()
        {
            Insert(3);
            Insert(1);
            Insert(2);
            Insert(6);
            Insert(4);
            Insert(7);

            update_height(root);

            Debug.Assert(root.Height == 2);
            Debug.Assert(root.Left.Height == 1);
            Debug.Assert(root.Left.Right.Height == 0);
            Debug.Assert(root.Right.Height == 1);
            Debug.Assert(root.Right.Right.Height == 0);
            Debug.Assert(root.Right.Left.Height == 0);
        }

        // can be used to check if the tree's height is correct
        public void testIntegrity()
        {
            testIntegrity(root);
        }

        private void testIntegrity(Node branch)
        {
            if (branch == null)
                return;

            int lh = (branch.Left == null ? -1 : branch.Left.Data);
            int rh = (branch.Right == null ? -1 : branch.Right.Data);

            Debug.Assert((rh - lh <= 1) && ( rh - lh >= -1));
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            BST obj = new BST();
            for (int i = 0; i < 100; ++i)
            {
		obj.Insert(i);
		Console.WriteLine();
		Console.WriteLine();
		obj.print_root();
            }

            obj.print();
            obj.testIntegrity();

            for(int i = 0; i < 7; ++i)
            {
                obj.print_at_height(i);
                Console.WriteLine();
                Console.WriteLine();
            }
            Console.ReadLine();
        }
    }
}
