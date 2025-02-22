// Disabling warnings for test double
#pragma warning disable CS1998 // This async method lacks 'await' operators and will run synchronously. Consider using the 'await' operator to await non-blocking API calls, or 'await Task.Run(...)' to do CPU-bound work on a background thread.

using ContosoSupport.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace ContosoSupport.Services
{
    public class SupportServiceInMemory : ISupportService
    {

        private static List<SupportCase> supportCases = new List<SupportCase>();
        private static object sync = new object();

        //Adds Support cases if the list is empty
        private async void Seed()
        {
            if (0 == await GetDocumentCountAsync().ConfigureAwait(false))
            {
                // Create a new SupportCase if collection is empty
                CreateAsync(new SupportCase { Title = "Support Case 1", Owner = "Shehab Fawzy", IsComplete = true, Description = "Proactively expedite parallel technology rather than wireless models. Competently syndicate 2.0 users via B2B technology. Conveniently syndicate end-to-end strategic theme areas before proactive e-business. Quickly reinvent long-term high-impact growth strategies before plug-and-play web-readiness. Synergistically extend robust growth strategies before orthogonal intellectual capital." });
                CreateAsync(new SupportCase { Title = "Support Case 2", Owner = "Devidas Gupta", IsComplete = false, Description = "Synergistically create next-generation innovation and effective innovation. Seamlessly simplify bricks-and-clicks information without high-payoff strategic theme areas. Distinctively deliver accurate vortals vis-a-vis cooperative users." });
                CreateAsync(new SupportCase { Title = "Support Case 3", Owner = "Nick Hauenstein", IsComplete = false, Description = "Collaboratively productivate performance based synergy with adaptive bandwidth. Intrinsicly communicate unique outsourcing through enterprise-wide interfaces. Interactively deliver next-generation initiatives vis-a-vis fully researched results. Assertively negotiate optimal \"outside the box\" thinking via fully researched ideas. Appropriately incentivize enterprise metrics after high-payoff convergence." });
                CreateAsync(new SupportCase { Title = "Support Case 4", Owner = "Tim Colbert", IsComplete = false, Description = "Competently network premier products whereas innovative e-tailers. Interactively leverage other's interactive products before visionary solutions. Seamlessly transition orthogonal niche markets whereas standards compliant manufactured products. Rapidiously engineer extensive solutions whereas cooperative quality vectors. Efficiently reintermediate prospective data whereas low-risk high-yield ideas." });
                CreateAsync(new SupportCase { Title = "Support Case 5", Owner = "Anne Hamilton", IsComplete = false, Description = "Continually pursue diverse content before virtual convergence. Completely supply interoperable growth strategies without progressive scenarios. Dramatically pursue cross-platform portals after functionalized niches. Holisticly orchestrate effective niches rather than turnkey paradigms. Credibly transition business." });
                CreateAsync(new SupportCase { Title = "Support Case 6", Owner = "Shehab Fawzy", IsComplete = true, Description = "Credibly synthesize accurate alignments with just in time convergence. Appropriately reconceptualize sticky resources vis-a-vis goal-oriented leadership skills. Efficiently matrix market-driven platforms rather than frictionless." });
                CreateAsync(new SupportCase { Title = "Support Case 7", Owner = "Nick Hauenstein", IsComplete = true, Description = "Seamlessly plagiarize ubiquitous expertise whereas market positioning deliverables. Efficiently unleash functional technology vis-a-vis distributed paradigms. Interactively recaptiualize state of the art mindshare after interactive niche markets. Credibly simplify virtual strategic theme areas via efficient users. Conveniently pontificate cross-platform catalysts for change through enabled ideas." });
                CreateAsync(new SupportCase { Title = "Support Case 8", Owner = "Devidas Gupta", IsComplete = false, Description = "Assertively synergize interdependent leadership skills and one-to-one outsourcing. Seamlessly engage timely functionalities after multifunctional outsourcing. Distinctively empower flexible." });
                CreateAsync(new SupportCase { Title = "Support Case 9", Owner = "Tim Colbert", IsComplete = true, Description = "Quickly myocardinate end-to-end e-tailers whereas error-free testing procedures. Assertively exploit mission-critical internal or \"organic\" sources whereas vertical resources. Synergistically facilitate client-centered web-readiness without effective supply chains." });
                CreateAsync(new SupportCase { Title = "Support Case 10", Owner = "Tim Colbert", IsComplete = false, Description = "Energistically repurpose business intellectual capital before unique sources. Competently deploy user-centric opportunities for leading-edge \"outside the box\" thinking." });
                CreateAsync(new SupportCase { Title = "Support Case 11", Owner = "Anne Hamilton", IsComplete = false, Description = "Efficiently enhance sustainable manufactured products vis-a-vis effective schemas. Progressively procrastinate one-to-one results after reliable internal or \"organic\" sources. Appropriately fabricate customized e-tailers without quality expertise. Professionally unleash visionary synergy via backend customer service. Uniquely develop enterprise-wide content rather than backward-compatible technologies." });
                CreateAsync(new SupportCase { Title = "Support Case 12", Owner = "Shehab Fawzy", IsComplete = true, Description = "Monotonectally revolutionize B2B expertise rather than front-end mindshare. Efficiently synergize stand-alone meta-services vis-a-vis interdependent functionalities. Synergistically benchmark cost effective functionalities rather than worldwide partnerships. Efficiently scale premier products whereas." });
            }
        }

        public SupportServiceInMemory(IConfiguration config)
        {
            Seed();
        }

        public async Task<long> GetDocumentCountAsync()
        {
            lock (sync)
            {
                return supportCases.Count;
            }
        }

        //pageNumber starts from 1, assumes Pages of 10 items
        public async Task<IEnumerable<SupportCase>> GetAsync(int? pageNumber = 1)
        {
            int pageNum = pageNumber.HasValue
                            ? (--pageNumber < 0 ? 0 : pageNumber).Value
                            : 1;

            lock (sync)
            {
                return supportCases.Skip(pageNum * 10).Take(10);
            }

        }

        public async Task<SupportCase> GetAsync(string id)
        {
            lock (sync)
            {
                return supportCases.FirstOrDefault(s => s.Id == id);
            }
        }

        public async void CreateAsync(SupportCase supportCase)
        {
            if (supportCase is null)
                throw new System.ArgumentNullException(nameof(supportCase));

            if (string.IsNullOrWhiteSpace(supportCase.Id) || supportCase.Id.Length != 24)
                supportCase.Id = Guid.NewGuid()
                                    .ToString("N", CultureInfo.InvariantCulture)
                                    .Substring(0, 24);

            lock (sync)
            {
                supportCases.Add(supportCase);
            }
        }

        public async void UpdateAsync(string id, SupportCase supportCase)
        {
            if (supportCase is null)
                throw new System.ArgumentNullException(nameof(supportCase));

            lock (sync)
            {
                var currentCase = supportCases.FirstOrDefault(s => s.Id == id);

                if (null == currentCase)
                {
                    CreateAsync(supportCase);
                }
                else
                {
                    currentCase.Description = supportCase.Description;
                    currentCase.IsComplete = supportCase.IsComplete;
                    currentCase.Owner = supportCase.Owner;
                    currentCase.Title = supportCase.Title;
                }
            }
        }

        public void RemoveAsync(SupportCase supportCase)
        {
            if (supportCase is null)
                throw new System.ArgumentNullException(nameof(supportCase));

            RemoveAsync(supportCase.Id);
        }

        public async void RemoveAsync(string id)
        {
            lock (sync)
            {
                var currentCase = supportCases.FirstOrDefault(s => s.Id == id);
                if (null != currentCase)
                {
                    supportCases.Remove(currentCase);
                }
            }
        }
    }
}
#pragma warning restore CS1998 // This async method lacks 'await' operators and will run synchronously. Consider using the 'await' operator to await non-blocking API calls, or 'await Task.Run(...)' to do CPU-bound work on a background thread.