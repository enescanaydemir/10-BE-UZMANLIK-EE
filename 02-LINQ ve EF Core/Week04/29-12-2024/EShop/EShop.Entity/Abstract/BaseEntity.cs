using System;

namespace EShop.Entity.Abstract;

public abstract class BaseEntity //abstract class olduğu için new'lenemez. Sadece miras alınabilir.
{
    public int Id { get; set; }
    public DateTime CreateDate { get; set; }=DateTime.UtcNow; //UtcNow, aynı anda farklı bölgelerdeki kullanıcılar için aynı tarih ve saat bilgisini almak için kullanılır.
    public DateTime UpdatedDate { get; set; }
    public bool IsActive { get; set; }=true; //aktifse true, değilse false. atama yapmasaydık default değeri false olacaktı.
    public bool IsDeleted { get; set; } //zaten default değeri false olacak. Bizde ilk değeri false olacak şekilde atama yapmadık.

}
