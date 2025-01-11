using System;
using System.Linq.Expressions;

namespace EShop.Data.Abstract;

public interface IGenericRepository<TEntity> where TEntity : class
{
    Task<TEntity> GetAsync(int id);
    //isim, sağ - tipi, sol / klasik yapı / isBool == true yapısıyla sol taraf aynı / genel generic yapı olarak kullanılıyor. tek tek metod tanımlamayacağız buna genelleri tanımlayıp kullanacağız
    Task<TEntity> GetAsync( //GetAsync overloadı. include ve bazı filtrelere göre GetAsync
        Expression<Func<TEntity, bool>> predicate, //ilk parametre filtre.
        params Func<IQueryable<TEntity>, IQueryable<TEntity>>[] includes //2. parametre kriter
                                                                         //birden fazla include gönderme ihtimalinden dolayı tipi dizi olarak tanımladık. IQueryable<TEntity>, IQueryable<TEntity> bunun amacı thenInclude ile aynı. params mantığı ise bir dizi vermesekte tek eleman vermek istediğimizde tek elemanlı alsın diziye çevirip göndermek zorunda kalmayalım diye ekledik.
    );
    //Yukarıdaki iki tane GetAsync iblrştirip id ye göre include kısmını kullanarak farklı bir metod kullanabiliriz. ek olarak id ye göre seçme yapabildiğimiz.

    //birden fazla elemanı geri döndürecek yapı;
    Task<IEnumerable<TEntity>> GetAllAsync(); //hepsini getirir(şartsız)
    Task<IEnumerable<TEntity>> GetAllAsync(
        Expression<Func<TEntity, bool>> predicate, //fiyatı 50 den düşük ürünler için predicate veriliyor. ancak filtre vermek istemediğimiz zamanlarda bura null gelebilir için = null yaptık
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
        params Func<IQueryable<TEntity>, IQueryable<TEntity>>[] includes
    );
    Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate); //var mı yok mu kontrolü yaparken kullanılır. varsa true yoksa false döner. 
    Task<int> CountAsync(); //dümdüz sayma yaparken. bir parametresi yok
    Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate); // bu da sayma yaparken bir şarta göre sayma yaptırdığımız CountAsync
    Task<TEntity> AddAsync(TEntity entity);

    //Ekleme,güncelleme,silme metodları;
    void Update(TEntity entity);
    void Delete(TEntity entity);


    //Genel buraların hepsini yazdıktan sonra buranın repositorysini yazması için concrete->repositories içinde class oluşturduk (GenericRepository)

}
