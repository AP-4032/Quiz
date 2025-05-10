using System;
using System.Collections.Generic;

namespace LibraryPublications
{
    // ----- Base class -----
    abstract class Publication
    {
        public string Title { get; set; }
        public int PageCount { get; set; }

        protected Publication(string title, int pageCount)
        {
            Title = title ?? throw new ArgumentNullException(nameof(title));
            if (pageCount < 0) throw new ArgumentOutOfRangeException(nameof(pageCount));
            PageCount = pageCount;
        }

        // Price: 1,000 Rials per page
        public virtual double CalculatePrice()
        {
            return PageCount * 1000;
        }

        public virtual string Info()
        {
            return $"{Title} - {PageCount} pages";
        }
    }

    // ----- Derived class: Book -----
    class Book : Publication
    {
        public string Genre { get; set; }

        public Book(string title, int pageCount, string genre) : base(title, pageCount)
        {
            Genre = genre ?? throw new ArgumentNullException(nameof(genre));
        }

        // Base price plus 10% tax
        public override double CalculatePrice()
        {
            return base.CalculatePrice() * 1.10;
        }

        public override string Info()
        {
            return $"{base.Info()} | Genre: {Genre}";
        }
    }

    // ----- Derived class: ComicBook -----
    class ComicBook : Book
    {
        public string Illustrator { get; set; }

        public ComicBook(string title, int pageCount, string genre, string illustrator) : base(title, pageCount, genre)
        {
            Illustrator = illustrator ?? throw new ArgumentNullException(nameof(illustrator));
        }

        // Price: 800 Rials per page (no tax)
        public override double CalculatePrice()
        {
            return PageCount * 800;
        }

        public override string Info()
        {
            return $"{base.Info()} | Illustrator: {Illustrator}";
        }
    }

    // ----- Extension methods for List<Publication> (NO LINQ) -----
    static class PublicationListExtensions
    {
        public static double AveragePrice(this List<Publication> publications)
        {
            if (publications == null || publications.Count == 0)
                throw new InvalidOperationException("Publication list is empty.");

            double totalPrice = 0;
            foreach (var p in publications)
            {
                totalPrice += p.CalculatePrice();
            }
            return totalPrice / publications.Count;
        }

        public static int TotalPages(this List<Publication> publications)
        {
            if (publications == null) throw new ArgumentNullException(nameof(publications));

            int totalPages = 0;
            foreach (var p in publications)
            {
                totalPages += p.PageCount;
            }
            return totalPages;
        }

        public static IEnumerable<Publication> FindByTitle(this List<Publication> publications, string keyword)
        {
            if (publications == null) throw new ArgumentNullException(nameof(publications));
            if (keyword == null) throw new ArgumentNullException(nameof(keyword));

            foreach (var p in publications)
            {
                if (p.Title.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    yield return p;
                }
            }
        }
    }

    // ----- Program entry point -----
    class Program
    {
        static void Main()
        {
            try
            {
                // Create sample objects
                var publications = new List<Publication>
                {
                    new Book("C# in Depth", 900, "Programming"),
                    new ComicBook("The Web-Slinger Returns", 120, "Superhero", "Alex Ross"),
                    new Book("The Little Prince", 96, "Fiction")
                };


                Console.WriteLine("=== Publication List ===");
                foreach (var pub in publications)
                {
                    Console.WriteLine($"{pub.Info()} | Price: {pub.CalculatePrice():N0} Rials");
                }

                Console.WriteLine("\n=== Extension Methods ===");
                Console.WriteLine($"Average Price: {publications.AveragePrice():N0} Rials");
                Console.WriteLine($"Total Pages: {publications.TotalPages()}");

                string searchKeyword = "the"; // Example keyword
                Console.WriteLine($"\nPublications containing \"{searchKeyword}\":");
                foreach (var pub in publications.FindByTitle(searchKeyword))
                {
                    Console.WriteLine(pub.Info());
                }
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Error: {ex.Message}");
                Console.ResetColor();
            }

            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey();
        }
    }
}
