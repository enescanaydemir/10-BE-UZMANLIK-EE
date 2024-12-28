/*
Generic yapılar, özellikle tip güvenliği sağlama konusunda işimizş görürler. Fakat aynı zamanda kodun YENİDEN KULLANILABİRLİĞİNİ arttırır.
DRY-Don't Repeat Yourself
*/

// Dictionary<TKey,TValue>

Dictionary<string,int> ages = new Dictionary<string, int>();
ages.Add("Ali",34);
ages.Add("Seren",19);
ages.Add("Can",26);

// foreach (var item in ages)
// {
//     System.Console.WriteLine($"{item.Key}: {item.Value}");
// }

// if(ages.ContainsKey("Seren"))
// {
//     System.Console.WriteLine(ages["Seren"]);
// }

ages.Remove("Can");

foreach (var item in ages)
{
    System.Console.WriteLine($"{item.Key}: {item.Value}");
}
// Box<int> numberBox = new Box<int>();
// numberBox.Add(5);
// System.Console.WriteLine(numberBox.Get());

// Box<string> stringBox = new Box<string>();
// stringBox.Add("Aleyna");
// System.Console.WriteLine(stringBox.Get());

class Box<T>
{
    private T content;
    public void Add(T item)
    {
        content=item;
    }
    public T Get(){
        return content;
    }
}