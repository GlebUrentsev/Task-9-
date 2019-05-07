using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections;

namespace RecursionListApp
{
    public partial class MainForm : Form
    {
        DoublyLinkedList<int> linkedList;

        public MainForm()
        {
            InitializeComponent();
            linkedList = new DoublyLinkedList<int>();
        }

        private void createButton_Click(object sender, EventArgs e)
        {
            int count = 0;
            int.TryParse(nodesCountTextBox.Text, out count);
            
            //очищаем список
            linkedList.Clear();

            // добавление элементов
            // позиция добавляемого элемента в списке, число элементов в списке
            linkedList.AddRec(1, count);
            nodesCountTextBox.Text = "";
            //обновляем поле со списком
            refreshListTextBox();
        }

        private void searchButton_Click(object sender, EventArgs e)
        {
            int value = 0;
            int.TryParse(searchTextBox.Text, out value);
            if (linkedList.Head != null)
            {
                // ссылка на голову, значение для поиска, позиция элемента в списке
                int index = linkedList.SearchRec(linkedList.Head, value, 1);
                if (index == -1)
                {
                    MessageBox.Show("Элемент со значением " + value + " в списке не найден.");
                }
                else
                {
                    MessageBox.Show("Найден элемент с позицией " + index + ".");
                }
            } else
            {
                MessageBox.Show("Список пуст.");
            }
            searchTextBox.Text = "";
        }

        private void removeButton_Click(object sender, EventArgs e)
        {
            int value = 0;
            int.TryParse(removeTextBox.Text, out value);

            if (linkedList.Head != null)
            {
                // удаление элементов
                // ссылка на голову, значение для удаления, позиция элемента в списке
                int index = linkedList.RemoveRec(linkedList.Head, value, 1);

                removeTextBox.Text = "";
                //обновляем поле со списком
                refreshListTextBox();

                if (index == -1)
                {
                    MessageBox.Show("Элемент со значением " + value + " в списке не найден.");
                }
                else
                {
                    MessageBox.Show("Удален элемент с позицией " + index + ".");
                }
            }
            else
            {
                MessageBox.Show("Список пуст.");
            }
        }

        private void refreshListTextBox()
        {
            listTextBox.Text = "";
            foreach (var item in linkedList)
            {
                listTextBox.Text += item + " ";
            }
        }        
    }

    public class ListElement<Int>
    {
        public ListElement(int data)
        {
            Data = data;
        }
        public int Data { get; set; }
        public ListElement<int> Previous { get; set; }
        public ListElement<int> Next { get; set; }
    }

    public class DoublyLinkedList<Int> : IEnumerable<int>  // двусвязный список
    {
        ListElement<int> head; // головной/первый элемент
        ListElement<int> tail; // последний/хвостовой элемент
        int count;  // количество элементов в списке

        public ListElement<int> Head { get { return head; } }
        public int Count { get { return count; } }

        public void Clear()
        {
            head = null;
            tail = null;
            count = 0;
        }

        public void AddRec(int currElNumber, int maxCount)
        {
            if (currElNumber == maxCount + 1) return;
            else 
            {
                ListElement<int> node = new ListElement<int>(currElNumber);

                if (head == null) // первый элемент в начало 
                    head = node;
                else // ветка для записи послежющих элементов, которые не равны головному
                {
                    tail.Next = node;
                    node.Previous = tail;
                }
                tail = node;
                currElNumber++; // текущий элемент под запись
                count++; // счётчик элментов

                AddRec(currElNumber, maxCount);
            }            
        }

        public int SearchRec(ListElement<int> currEl, int value, int currElNumber)
        {
            //элемент найден - возвращаем позицию
            if (currEl.Data.Equals(value)) return currElNumber;
            //условие выхода из рекурсии - конец списка
            if (currEl.Next == null) return -1;
            else
            {
                currElNumber++;
                return SearchRec(currEl.Next, value, currElNumber);
            }
        }

        public int RemoveRec(ListElement<int> currEl, int value, int currElNumber)
        {            
            //найден элемент для удаления
            if (currEl.Data.Equals(value))
            {
                // если узел не последний
                if (currEl.Next != null)
                {
                    currEl.Next.Previous = currEl.Previous;
                }
                else
                {
                    // если последний, переустанавливаем tail
                    tail = currEl.Previous;
                }

                // если узел не первый
                if (currEl.Previous != null)
                {
                    currEl.Previous.Next = currEl.Next;
                }
                else
                {
                    // если первый, переустанавливаем head
                    head = currEl.Next;
                }
                count--;
                return currElNumber;
            }

            //условие выхода из рекурсии - конец списка
            if (currEl.Next == null) return -1;
            else
            {
                currElNumber++;
                return RemoveRec(currEl.Next, value, currElNumber);
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
         {
             return ((IEnumerable)this).GetEnumerator();
         }

        IEnumerator<int> IEnumerable<int>.GetEnumerator()
        {
            ListElement<int> current = head;
            while (current != null)
            {
                yield return current.Data;
                current = current.Next;
            }
        }
    }
}
