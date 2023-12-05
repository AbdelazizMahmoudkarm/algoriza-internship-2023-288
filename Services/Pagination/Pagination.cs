namespace EF.Pagination
{
    /// <summary>
    /// Pipe which use to supply computed number or raws to page 
    /// </summary>
    /// <typeparam name="T">generic type of eneity class</typeparam>
    public static  class Pagination
    {
        public static int?  Pages { get; set; }
        public static  int CurrentPage { get; set; }
        public static  IEnumerable<T> Paginate<T>(this IQueryable<T> source, int pageIndex, int pageSize) where T : class
        {
            if (source is not null && source.Any())
            { 
                Pages ??= Convert.ToInt32(Math.Ceiling((decimal)pageSize / source.Count()));
                CurrentPage = pageIndex;
                return source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
            }
            return null;
        }
    }
}