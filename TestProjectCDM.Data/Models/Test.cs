using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace TestProjectCDM.Data.Models
{
    public class Test
    {
        public Test()
        {
            Steps = new List<TestStep>();
        }

        public bool AddStep(TestStep step)
        {
            step.Id = Steps.Count;
            Steps.Add(step);
            return true;
        }

        public bool DeleteLastStep()
        {
            if (Steps.Count == 0)
                return false;

            Steps.RemoveAt(Steps.Count);
            return true;
        }

        public List<int> GetWinnersId()
        {
            var result = new List<int>();
            var dict = new Dictionary<int,int>();

            foreach (var step in Steps)
            {
                if (dict.ContainsKey(step.ChosenStyleId))
                    dict[step.ChosenStyleId]++;
                else
                    dict.Add(step.ChosenStyleId,0);
            }

            int maxValue = dict.Values.Max();
            foreach (var i in dict)
            {
                if(i.Value==maxValue)
                    result.Add(i.Key);                   
            }

            return result;
        }

        public Guid Id { get; set; }
        public string Username { get; set; }
        public DateTime CompleteTime { get; set; }
        public List<TestStep> Steps { get; set; }
    }
}
