namespace TestAPI {
public class Resp {
           public int page {get;set;}
           public int per_page {get;set;}
           public int total {get;set;}
           public int total_pages {get;set;}
            public IEnumerable<Data> data {get;set;}
}
}
