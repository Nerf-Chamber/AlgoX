namespace AlgoX.Code
{
    public class Association<T>
    {
        public T Element { get; set; }
        public T AssociatedElement { get; set; }

        public Association(T element, T associatedElement)
        {
            Element = element;
            AssociatedElement = associatedElement;
        }
    }
}