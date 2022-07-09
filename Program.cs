using System.Text.Json;
using System.Linq;

namespace TestAPI {
public class Program {
    static void Main(string[] args){
       
        
      if(args[0] == null) throw new ArgumentNullException("Args is null");

      var limit = Convert.ToInt32(args[0]); 
   
      var articles = topArticles(limit);

      foreach(var article in articles){
        Console.WriteLine("{0}", article);
      }
        
       
    }

    //Get top articles
    static IEnumerable<string> topArticles(int limit){
    
        IEnumerable<string> titles;
        IEnumerable<Article> tempdata = Enumerable.Empty<Article>();
        int page = 1;
        try{
        
        //Get page 1, to get the total pages
        var initialData = GetSource(page);

        // do, until page is equal to total_pages
        do  {

            page++;

            var result = GetSource(page);
            
            //combine all results to form IEnumerable<Article> object
            tempdata = ( from d in result.data
            where !string.IsNullOrEmpty(d.title) ||
            !string.IsNullOrEmpty(d.story_title)
            select new Article {
                title = !string.IsNullOrEmpty(d.title) ? d.title : d.story_title, // use story_title if title is null
                num_comments = d.num_comments
            }).Union(tempdata);
             

            
           
           
        }
        while(page <= initialData.total_pages);

        //sort collection by number of comments, select the title and take N of item using limit parameter
        titles = tempdata
        .OrderByDescending(x => x.num_comments)
        .Select(x => x.title)
        .Take(limit);
        
       
        }
        catch{
            throw;
        }

        //before returning the collection sort it by title desc
        return titles.OrderByDescending(x => x);
    }

    //Get Response object from API
    static Resp GetSource(int page){

        Resp response;
        using var client = new HttpClient();

        try{

            var tempStr = "https://jsonmock.hackerrank.com/api/articles?page=" + page;
            var result = client.GetAsync(tempStr).Result;
        
            var body = result.Content.ReadAsStringAsync().Result;
      
            response =  JsonSerializer.Deserialize<Resp>(body);

       
        }
        catch{
            throw;
        }
        finally {
            client.Dispose();
        }

        return response;
    }


}
}

