using System.Threading;
using UnityEngine;

public class LinkedList
{
    LinkedListNode head;
    int count;

    public LinkedList()
    {
        head = null;
        count = 0;    
    }

    public void AddNodeToFront(int data)
    {
        LinkedListNode node = new LinkedListNode(data);
        node.next = head;
        head.prev = node;
        head = node;
        count++;
    }
    
    public void PrintList()
    {
        LinkedListNode runner = head;
        while (runner != null)
        {
            Debug.Log(runner.data.ToString());
            runner = runner.next;
        }
    }
}

public class LinkedListNode
{
    public int data;

    public LinkedListNode next;
    public LinkedListNode prev;

    public LinkedListNode(int x)
    {
        this.data = x;
        this.next = null;
        this.prev = null;
    }
}
