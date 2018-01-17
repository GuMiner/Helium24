﻿using Helium24.Models;
using Nancy;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Helium24.Modules
{
    /// <summary>
    /// Module for communicating with our PostgreSQL RPI database.
    /// </summary>
    public class DatabaseModule : NancyModule
    {
        private static DocumentationCache documentationCache = new DocumentationCache();

        public DatabaseModule()
            : base("/Database")
        {
            Get["/DocumentationTable"] = parameters =>
            {
                IEnumerable<string[]> allDocuments = documentationCache.GetAllDocuments().Select(document => new String[] {
                    document.Title, document.Source, document.Format,
                    document.Category, document.Association, document.License });

                return this.Response.AsJson(new { data = allDocuments });
            };

            Get["/DocumentationTable/CategorySummaries"] = parameters =>
            {
                IEnumerable<PiCachedDocument> docs = documentationCache.GetAllDocuments();
                List<string[]> summaries = new List<string[]>();

                // First, get the distinct different types of categories
                IEnumerable<string> categories = docs.Select(doc => doc.Category).Distinct();
                foreach (string category in categories)
                {
                    // Now get the count of items in each category.
                    IEnumerable<PiCachedDocument> categoryDocs = docs.Where(doc => doc.Category == category);
                    int itemCount = categoryDocs.Count();
                    string formatOne;
                    string formatTwo;
                    string licenseOne;
                    string licenseTwo;
                    GetDominantFormatsAndLicenses(categoryDocs, out formatOne, out formatTwo, out licenseOne, out licenseTwo);

                    summaries.Add(new String[] { category, itemCount.ToString(), formatOne, formatTwo, licenseOne, licenseTwo });
                }

                return this.Response.AsJson(new { data = summaries.ToArray() });
            };
        }

        /// <summary>
        /// Computes the first two main licenses and formats within the documentation library for a given category.
        /// </summary>
        /// <param name="categoryDocs">The documents for a given category.</param>
        /// <param name="formatOne">Stores the #1 format.</param>
        /// <param name="formatTwo">Stores the #2 format.</param>
        /// <param name="licenseOne">Stores the #1 license.</param>
        /// <param name="licenseTwo">Stores the #2 license.</param>
        private void GetDominantFormatsAndLicenses(IEnumerable<PiCachedDocument> categoryDocs, out string formatOne, out string formatTwo, out string licenseOne, out string licenseTwo)
        {
            this.GetFirstAndSecondMostFrequent(categoryDocs.Select(doc => doc.Format), out formatOne, out formatTwo);
            this.GetFirstAndSecondMostFrequent(categoryDocs.Select(doc => doc.License), out licenseOne, out licenseTwo);
        }

        private void GetFirstAndSecondMostFrequent(IEnumerable<string> items, out string mostFrequent, out string secondMostFrequent)
        {
            int mostFrequentCount = 0;
            int secondMostFrequentCount = 0;
            mostFrequent = string.Empty;
            secondMostFrequent = string.Empty;
            foreach (string distinctItem in items.Distinct())
            {
                int itemCount = items.Count(item => item.Equals(distinctItem, StringComparison.OrdinalIgnoreCase));
                if (itemCount > mostFrequentCount)
                {
                    // Swap down and assign the primary count to the new count.
                    secondMostFrequent = mostFrequent.ToString();

                    secondMostFrequentCount = mostFrequentCount;
                    mostFrequentCount = itemCount;

                    mostFrequent = this.FormItemSummary(distinctItem, itemCount);
                }
                else if (itemCount > secondMostFrequentCount)
                {
                    // Just assign to the secondary count
                    secondMostFrequentCount = itemCount;
                    secondMostFrequent = this.FormItemSummary(distinctItem, itemCount);
                }
            }
        }

        private string FormItemSummary(string item, int count) => $"{item} ({count})";
    }
}