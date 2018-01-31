using System;
using System.Net;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;
using HtmlAgilityPack;
using Newtonsoft.Json;
namespace app.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        // GET api/values
        [HttpGet]
        public string Get(string url)
        {
            Dictionary<string, int> dictionary = new Dictionary<string, int>();
            HtmlWeb web = new HtmlWeb();
            var htmlDoc = web.Load(url);
            StringBuilder sb = new StringBuilder();
            try
            {
                // Remove script & style nodes
                htmlDoc.DocumentNode.Descendants().Where(n => n.Name == "script" || n.Name == "style").ToList().ForEach(n => n.Remove());
                foreach (HtmlNode node in htmlDoc.DocumentNode.SelectNodes("//text()[normalize-space(.) != '']"))
                {
                    string[] words = node.InnerText.Trim().Split(' ');
                    foreach (string word in words)
                    {
                        var regexItem = new Regex( @"[0-9@#$%&*+\-_(),+':;?.,![\]\s\\/]+$");
                        if (!regexItem.IsMatch(word))
                        {
                            if (dictionary.ContainsKey(word))
                            {
                                dictionary[word] += 1;
                            }
                            else
                            {
                                dictionary.Add(word, 1);
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
            }
            string json = JsonConvert.SerializeObject(dictionary.OrderByDescending(i => i.Value), Formatting.Indented);
            return json;
        }
    }
}
